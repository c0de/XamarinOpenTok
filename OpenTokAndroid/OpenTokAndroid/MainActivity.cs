using System;
using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using OpenTok.Android;
using System.Security.Cryptography;
using Android;

namespace OpenTokAndroid
{
	[Activity (Label = "OpenTokAndroid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity,
		Session.ISessionListener,
		Publisher.IPublisherListener,
		Subscriber.IVideoListener
	{
		private static string LOGTAG = "test";
		private Session mSession;
		private Publisher mPublisher;
		private Subscriber mSubscriber;
		private List<Stream> mStreams;
		protected Handler mHandler = new Handler();

		private RelativeLayout mPublisherViewContainer;
		private RelativeLayout mSubscriberViewContainer;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			mPublisherViewContainer = (RelativeLayout) FindViewById(Resource.Id.publisherview);
			mSubscriberViewContainer = (RelativeLayout) FindViewById(Resource.Id.subscriberview);

			mStreams = new List<Stream>();
			SessionConnect();
		}

		private void SessionConnect() {
			if (mSession == null) {
				mSession = new Session(this, Configuration.Config.API_KEY, Configuration.Config.SESSION_ID);
				mSession.SetSessionListener(this);
				mSession.Connect(Configuration.Config.TOKEN);
			}
		}

		#region ISessionListener

		public void OnConnected (Session p0)
		{
		}

		public void OnDisconnected (Session p0)
		{
		}

		public void OnError (Session p0, OpentokError p1)
		{
		}

		public void OnStreamDropped (Session p0, Stream p1)
		{
		}

		public void OnStreamReceived (Session p0, Stream p1)
		{
		}

		#endregion ISessionListener


		#region IPublisherListener

		public void OnError (PublisherKit p0, OpentokError p1)
		{
		}

		public void OnStreamCreated (PublisherKit p0, Stream p1)
		{
		}

		public void OnStreamDestroyed (PublisherKit p0, Stream p1)
		{
		}

		#endregion IPublisherListener


		#region IVideoListener

		public void OnVideoDataReceived (SubscriberKit p0)
		{
		}

		public void OnVideoDisableWarning (SubscriberKit p0)
		{
		}

		public void OnVideoDisableWarningLifted (SubscriberKit p0)
		{
		}

		public void OnVideoDisabled (SubscriberKit p0, string p1)
		{
		}

		public void OnVideoEnabled (SubscriberKit p0, string p1)
		{
		}

		#endregion IVideoListener
	}
}


