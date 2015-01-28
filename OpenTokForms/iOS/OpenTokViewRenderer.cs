using System;
using System.Drawing;

using OpenTok.Binding.Ios;
using Xamarin.Forms.Platform.iOS;

namespace OpenTokForms.iOS
{
	public class OpenTokViewRenderer : Xamarin.Forms.Platform.iOS.ViewRenderer
	{
		private OpenTokForms.OpenTokView _openTokView;
		OTSession _session;
		OTPublisher _publisher;
		OTSubscriber _subscriber;

		public EventHandler<OnErrorEventArgs> OnError;

		protected override void OnElementChanged (Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged (e);

			_openTokView = e.NewElement as OpenTokView;

			DoConnect();
		}

		private void DoConnect()
		{
			OTError error;

			_session = new OTSession(Configuration.Config.API_KEY, Configuration.Config.SESSION_ID, new SessionDelegate(this));
			_session.ConnectWithToken (Configuration.Config.TOKEN, out error);
		}

		public void DoPublish()
		{
			_publisher = new OTPublisher(new PublisherDelegate(this));

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

			//Xamarin.Forms.View view = _publisher.View as Xamarin.Forms.View;
			//_openTokView.PublisherViewContainer.Children.Add(view);
		}

		private void DoSubscribe(OTStream stream)
		{
			_subscriber = new OTSubscriber(stream, new SubscriberDelegate(this));

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
				_this.DoPublish();
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
				// connection destroyed

				_this.CleanupSubscriber();
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
				//Xamarin.Forms.View view = _subscriber.View as Xamarin.Forms.View;
				//_openTokView.SubscriberViewContainer.Children.Add(view);

				/*
				_this._subscriber.View.Frame = new RectangleF(0, 0, (float)_this.View.Frame.Width, (float)_this.View.Frame.Height);
				_openTokView.SubscriberViewContainer.Children.Add (_subscriber.View);
				*/
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
				if (_this._subscriber == null)
				{
					_this.DoSubscribe(stream);
				}
			}

			public override void StreamDestroyed(OTPublisher publisher, OTStream stream)
			{
				_this.CleanupSubscriber();
				_this.CleanupPublisher();
			}
		}

		#endregion PublisherDelegate
	}
}

