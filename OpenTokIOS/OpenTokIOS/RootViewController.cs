using System;
using CoreGraphics;
using Foundation;
using UIKit;
using OpenTok.Binding.Ios;

namespace OpenTokIOS
{
	public partial class RootViewController : UIViewController
	{
		OTSession _session;
		OTPublisher _publisher;
		OTSubscriber _subscriber;

		static readonly float widgetHeight = 240;
		static readonly float widgetWidth = 320;

		public RootViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_session = new OTSession(Configuration.Config.API_KEY, Configuration.Config.SESSION_ID, new SessionDelegate(this));
		}

		private class SessionDelegate : OTSessionDelegate
		{
			private RootViewController _this;
			public SessionDelegate(RootViewController controller)
			{
				_this = controller;
			}

			#region OTSessionDelegate implementation

			public void DidConnect (OTSession session)
			{
			}

			public void DidDisconnect (OTSession session)
			{
			}

			public void DidFailWithError (OTSession session, OTError error)
			{
			}

			public void StreamCreated (OTSession session, OTStream stream)
			{
			}

			public void StreamDestroyed (OTSession session, OTStream stream)
			{
			}

			public void ConnectionCreated (OTSession session, OTConnection connection)
			{
			}

			public void ConnectionDestroyed (OTSession session, OTConnection connection)
			{
			}

			public void ReceivedSignalType (OTSession session, string type, OTConnection connection, string data)
			{
			}

			#endregion
		}
	}
}

