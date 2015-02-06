using System;
using System.Drawing;

using OpenTok.Binding.Ios;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using CoreGraphics;

[assembly: Xamarin.Forms.ExportRenderer (typeof (OpenTokForms.OpenTokView), typeof (OpenTokForms.iOS.OpenTokViewRenderer))]
namespace OpenTokForms.iOS
{
	public class OpenTokViewRenderer : Xamarin.Forms.Platform.iOS.ViewRenderer
	{
		SessionDelegate _sessionDelegate;
		PublisherDelegate _publisherDelegate;
		SubscriberDelegate _subscriberDelegate;

		OTSession _session;
		OTPublisher _publisher;
		OTSubscriber _subscriber;

		public EventHandler<OnErrorEventArgs> OnError;

		protected override void OnElementChanged (Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged (e);

			var width = (float)UIScreen.MainScreen.Bounds.Size.Width;
			var height = (float)UIScreen.MainScreen.Bounds.Size.Height - 20;
			var view = new UIView (new RectangleF (0, 0, width, height));
			this.SetNativeControl (view);

			DoConnect();
		}

		private void DoConnect()
		{
			OTError error;

			_sessionDelegate = new SessionDelegate (this);
			_session = new OTSession(Configuration.Config.API_KEY, Configuration.Config.SESSION_ID, _sessionDelegate);
			_session.ConnectWithToken (Configuration.Config.TOKEN, out error);
		}

		public void DoPublish()
		{
			var _publisherDelegate = new PublisherDelegate (this);
			_publisher = new OTPublisher(_publisherDelegate);

			OTError error;

			_session.Publish(_publisher, out error);

			if (error != null)
			{
				this.RaiseOnError(error.Description);
			}

			var pubView = _publisher.View;
			pubView.TranslatesAutoresizingMaskIntoConstraints = false;
			pubView.Frame = new RectangleF(0, 0, 100, 100);
			pubView.Layer.CornerRadius = 50;
			pubView.Layer.MasksToBounds = true;

			this.AddSubview (pubView);
			this.BringSubviewToFront (pubView);

			this.AddConstraints (new NSLayoutConstraint[] {
				NSLayoutConstraint.Create (pubView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1f, 0),
				NSLayoutConstraint.Create (pubView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1f, 0),
				NSLayoutConstraint.Create (pubView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, pubView.Frame.Height),
				NSLayoutConstraint.Create (pubView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1f, pubView.Frame.Width)
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
			private OpenTokViewRenderer _this;

			public SessionDelegate(OpenTokViewRenderer This)
			{
				_this = This;
			}

			public override void DidConnect(OTSession session)
			{
				InvokeOnMainThread (_this.DoPublish);
			}

			public override void DidFailWithError(OTSession session, OTError error)
			{
				var msg = "SessionDelegate:DidFailWithError: " + session.SessionId;
				_this.RaiseOnError(msg);
			}

			public override void DidDisconnect(OTSession session)
			{
				// disconnect
			}

			public override void ConnectionCreated(OTSession session, OTConnection connection)
			{
				// connection created
			}

			public override void ConnectionDestroyed(OTSession session, OTConnection connection)
			{
				InvokeOnMainThread (() => _this.CleanupSubscriber());
			}

			public override void StreamCreated(OTSession session, OTStream stream)
			{
				if(_this._subscriber == null)
				{
					_this.DoSubscribe(stream);
				}
			}

			public override void StreamDestroyed(OTSession session, OTStream stream)
			{
				_this.CleanupSubscriber();
			}
		}

		#endregion SessionDelegate

		#region SubscriberDelegate

		private class SubscriberDelegate : OTSubscriberKitDelegate
		{
			private OpenTokViewRenderer _this;

			public SubscriberDelegate(OpenTokViewRenderer This)
			{
				_this = This;
			}

			public override void DidConnectToStream(OTSubscriber subscriber)
			{
				InvokeOnMainThread (() => {
					var subView = _this._subscriber.View;
					subView.Frame = new RectangleF (0, 0, 1, 1);

					_this.AddSubview (subView);
					subView.TranslatesAutoresizingMaskIntoConstraints = false;
					_this.AddConstraints(new NSLayoutConstraint[] {
						NSLayoutConstraint.Create(subView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, _this, NSLayoutAttribute.Top, 1, 0),
						NSLayoutConstraint.Create(subView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, _this, NSLayoutAttribute.Bottom, 1, 0),
						NSLayoutConstraint.Create(subView, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, _this, NSLayoutAttribute.Leading, 1, 0),
						NSLayoutConstraint.Create(subView, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, _this, NSLayoutAttribute.Trailing, 1, 0)
					});
					_this.BringSubviewToFront (subView);
					_this.BringSubviewToFront(_this._publisher.View);
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
			private OpenTokViewRenderer _this;

			public PublisherDelegate(OpenTokViewRenderer This)
			{
				_this = This;
			}

			public override void DidFailWithError(OTPublisher publisher, OTError error)
			{
				var msg = String.Format("PublisherDelegate:DidFailWithError: Error: {0}", error.Description);

				_this.RaiseOnError(msg);
				_this.CleanupPublisher();
			}


			public override void StreamCreated(OTPublisher publisher, OTStream stream)
			{
				if(_this._subscriber == null)
				{
					InvokeOnMainThread (() => _this.DoSubscribe (stream));
				}
			}

			public override void StreamDestroyed(OTPublisher publisher, OTStream stream)
			{
				InvokeOnMainThread (() => {
					_this.CleanupSubscriber();
					_this.CleanupPublisher();
				});
			}
		}

		#endregion PublisherDelegate
	}
}

