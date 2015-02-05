using System;
using CoreGraphics;
using Foundation;
using UIKit;
using OpenTok.Binding.Ios;
using Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Threading.Tasks;
using System.Threading;
using CoreFoundation;

namespace OpenTokIOS
{
	public partial class RootViewController : UIViewController
	{
		OTSession _session;
		OTPublisher _publisher;
		OTSubscriber _subscriber;

		SessionDelegate _sesionDelegate;
		PublisherDelegate _publisherDelegate;
		SubscriberDelegate _subscriberDelegate;

		static readonly float widgetHeight = 240;
		static readonly float widgetWidth = 320;

		public EventHandler<OnErrorEventArgs> OnError;

		public RootViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			DoConnect();
		}

		private void DoConnect()
		{
			OTError error;

			_sesionDelegate = new SessionDelegate (this);
			_session = new OTSession(Configuration.Config.API_KEY,
				Configuration.Config.SESSION_ID,
				_sesionDelegate);
			_session.ConnectWithToken (Configuration.Config.TOKEN, out error);
		}

		public void DoPublish()
		{
			_publisherDelegate = new PublisherDelegate (this);
			_publisher = new OTPublisher(_publisherDelegate);

			OTError error;

			_session.Publish(_publisher, out error);

			if (error != null)
			{
				this.RaiseOnError(error.Description);
			}

			// Show the Video in the View In Round Mode
			_publisher.View.Frame = new RectangleF(0, 0, 100, 100);
			_publisher.View.Layer.CornerRadius = 50;
			_publisher.View.Layer.MasksToBounds = true;

			this.PublisherView.AddSubview (_publisher.View);

			// Schedule a periodic task to send a broadcast signal to all
			// peers on the session
			Task.Run (() => {
				while(true) {
					InvokeOnMainThread( () => {
						OTError signalerror;
						_session.SignalWithType("BroadcastSignal", 
							DateTime.Now.ToString(), 
							null, // This is the connection of the peer you are sending the signal
								  // Leave it null and it will send to all members of the session.
							out signalerror);
					});
					Thread.Sleep(10000);
				}
			});
		}

		private void DoSubscribe(OTStream stream)
		{
			_subscriberDelegate = new SubscriberDelegate (this);
			_subscriber = new OTSubscriber(stream, _subscriberDelegate);

			OTError error;

			_session.Subscribe(_subscriber, out error);

			if (error != null)
			{
				this.RaiseOnError(error.Description);
			}
		}

		private void CleanupSubscriber()
		{
			if (_subscriber != null)
			{
				_subscriber.View.RemoveFromSuperview();
				_subscriber.Delegate = null;
				_subscriber.Dispose();
				_subscriber = null;
				_subscriberDelegate = null;
			}
		}

		// Beware the SDK publisher and subscriber is not inmediatly destroyed after called
		// Session's disconnect so it is not safe to call this method right after disconnect.
		// You should wait until publisher/subscriber delegate "*destroyed" method is called to
		// dispose de references.
		private void CleanupPublisher()
		{
			if (_publisher != null)
			{
				_publisher.View.RemoveFromSuperview();
				_publisher.Delegate = null;
				_publisher.Dispose();
				_publisher = null;
				_publisherDelegate = null;
			}
		}

		private void RaiseOnError(string message)
		{
			OnErrorEventArgs e = new OnErrorEventArgs(message);

			this.OnError(this, e);
		}

		public class OnErrorEventArgs : EventArgs
		{
			public OnErrorEventArgs(string s)
			{
				message = s;
			}
			private string message;

			public string Message
			{
				get { return message; }
				set { message = value; }
			}
		}

		#region SessionDelegate

		private class SessionDelegate : OTSessionDelegate
		{
			private RootViewController _this;

			public SessionDelegate(RootViewController This)
			{
				_this = This;
			}

			// Although SDK callbacks are called in the main thread,
			// it might be safer to ensure that other method are called in the Main Thread.
			public override void DidConnect(OTSession session)
			{
				Debug.WriteLine("SessionDelegate:DidConnect: " + session.SessionId);
				InvokeOnMainThread (_this.DoPublish);
			}

			public override void DidFailWithError(OTSession session, OTError error)
			{
				var msg = "SessionDelegate:DidFailWithError: " + session.SessionId;

				Debug.WriteLine(msg);

				_this.RaiseOnError(msg);
			}

			public override void DidDisconnect(OTSession session)
			{
				var msg = "SessionDelegate:DidDisconnect: " + session.SessionId;

				Debug.WriteLine(msg);
			}

			public override void ConnectionCreated(OTSession session, OTConnection connection)
			{
				Debug.WriteLine("SessionDelegate:ConnectionCreated: " + connection.ConnectionId);
			}

			public override void ConnectionDestroyed(OTSession session, OTConnection connection)
			{
				Debug.WriteLine("SessionDelegate:ConnectionDestroyed: " + connection.ConnectionId);

				InvokeOnMainThread (() => _this.CleanupSubscriber());
			}

			public override void StreamCreated(OTSession session, OTStream stream)
			{
				Debug.WriteLine("SessionDelegate:StreamCreated: " + stream.StreamId);

				if(_this._subscriber == null)
				{
					InvokeOnMainThread (() => _this.DoSubscribe (stream));
				}
			}

			public override void StreamDestroyed(OTSession session, OTStream stream)
			{
				Debug.WriteLine("SessionDelegate:StreamDestroyed: " + stream.StreamId);

				InvokeOnMainThread (_this.CleanupSubscriber);
			}

			public override void ReceivedSignalType(OTSession session, string type, 
				OTConnection connection, string stringParam)
			{
				Console.WriteLine ("Signal Received: {0}, {1}", type, stringParam);
			}
		}

		#endregion SessionDelegate

		#region SubscriberDelegate

		private class SubscriberDelegate : OTSubscriberKitDelegate
		{
			private RootViewController _this;

			public SubscriberDelegate(RootViewController This)
			{
				_this = This;
			}

			public override void DidConnectToStream(OTSubscriber subscriber)
			{
				InvokeOnMainThread (() => {
					_this._subscriber.View.Frame = new RectangleF (0, 0, (float)_this.View.Frame.Width, (float)_this.View.Frame.Height);
					_this.SubscriberView.AddSubview (_this._subscriber.View);

					// In this case, we are sending a signal to a specific peer
					// using its connection as the connection parameter.
					OTError error;
					_this._session.SignalWithType("PrivateSignal",
						"Hello Subscriber",
						subscriber.Stream.Connection,
						out error);
				});
			}

			public override void DidFailWithError(OTSubscriber subscriber, OTError error)
			{
				var msg = String.Format("SubscriberDelegate:DidFailWithError: Stream {0}, Error: {1}", subscriber.Stream.StreamId, error.Description);
				_this.RaiseOnError(msg);
			}
		}

		#endregion SubscriberDelegate

		#region PublisherDelegate

		private class PublisherDelegate : OTPublisherKitDelegate
		{
			private RootViewController _this;

			public PublisherDelegate(RootViewController This)
			{
				_this = This;
			}

			public override void DidFailWithError(OTPublisher publisher, OTError error)
			{
				var msg = String.Format("PublisherDelegate:DidFailWithError: Error: {0}", error.Description);

				_this.RaiseOnError(msg);
				InvokeOnMainThread(_this.CleanupPublisher);
			}


			public override void StreamCreated(OTPublisher publisher, OTStream stream)
			{
				if (_this._subscriber == null)
				{
					InvokeOnMainThread(() => _this.DoSubscribe(stream));
				}
			}

			public override void StreamDestroyed(OTPublisher publisher, OTStream stream)
			{
				InvokeOnMainThread (() => {
					_this.CleanupSubscriber ();
					_this.CleanupPublisher ();
				});
			}
		}

		#endregion PublisherDelegate
	}
}

