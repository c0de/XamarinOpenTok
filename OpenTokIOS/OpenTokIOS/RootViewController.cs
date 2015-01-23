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

			#region implemented abstract members of OTSessionDelegate
			public override void DidFailWithError (OTSession session, OTError error)
			{
				throw new NotImplementedException ();
			}
			public override void DidConnect (OTSession session)
			{
				throw new NotImplementedException ();
			}
			public override void DidDisconnect (OTSession session)
			{
				throw new NotImplementedException ();
			}
			public override void StreamCreated (OTSession session, OTStream stream)
			{
				throw new NotImplementedException ();
			}
			public override void StreamDestroyed (OTSession session, OTStream stream)
			{
				throw new NotImplementedException ();
			}
			#endregion
		}
	}
}

