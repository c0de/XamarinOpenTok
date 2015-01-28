using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.OS;
using Android.App;
using Android.Widget;
using OpenTok.Android;

[assembly:Xamarin.Forms.ExportRenderer(typeof(OpenTokForms.ConferencePage), typeof(OpenTokForms.Droid.ConferencePageRenderer))]
namespace OpenTokForms.Droid
{
	public class ConferencePageRenderer : Xamarin.Forms.Platform.Android.PageRenderer,
		Session.ISessionListener,
		Publisher.IPublisherListener,
		Subscriber.IVideoListener
	{
		private Activity mActivity;
		private Session mSession;
		private Publisher mPublisher;
		private Subscriber mSubscriber;
		private List<Stream> mStreams;
		protected Handler mHandler = new Handler();

		private RelativeLayout mPublisherViewContainer;
		private RelativeLayout mSubscriberViewContainer;

		protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.Page> e)
		{
			base.OnElementChanged (e);

			mActivity = this.Context as Activity;
			mActivity.SetContentView (Resource.Layout.ConferencePageLayout);

			mPublisherViewContainer = (RelativeLayout) mActivity.FindViewById(Resource.Id.publisherview);
			mSubscriberViewContainer = (RelativeLayout) mActivity.FindViewById(Resource.Id.subscriberview);

			mStreams = new List<Stream>();
			SessionConnect();

			/*
			var activity = this.Context as Activity;
			var conferenceActivity = new Intent (activity, typeof (ConferenceActivity));
			activity.StartActivity (conferenceActivity);
			*/
		}

		private void SessionConnect() {
			if (mSession == null) {
				mSession = new Session(mActivity, Configuration.Config.API_KEY, Configuration.Config.SESSION_ID);
				mSession.SetSessionListener(this);
				mSession.Connect(Configuration.Config.TOKEN);
			}
		}

		/**
     * Converts dp to real pixels, according to the screen density.
     *
     * @param dp A number of density-independent pixels.
     * @return The equivalent number of real pixels.
     */
		private int DpToPx(int dp) {
			double screenDensity = this.Resources.DisplayMetrics.Density;
			return (int) (screenDensity * (double) dp);
		}

		private void AttachSubscriberView(Subscriber subscriber) {
			RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams (Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
			mSubscriberViewContainer.RemoveView(mSubscriber.View);
			mSubscriberViewContainer.AddView(mSubscriber.View, layoutParams);
			subscriber.SetStyle(BaseVideoRenderer.StyleVideoScale, BaseVideoRenderer.StyleVideoFill);
		}

		private void AttachPublisherView(Publisher publisher) {
			mPublisher.SetStyle(BaseVideoRenderer.StyleVideoScale, BaseVideoRenderer.StyleVideoFill);
			RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(320, 240);
			layoutParams.AddRule(LayoutRules.AlignParentBottom, (int)LayoutRules.True);
			layoutParams.AddRule(LayoutRules.AlignParentRight, (int)LayoutRules.True);
			layoutParams.BottomMargin = DpToPx(8);
			layoutParams.RightMargin = DpToPx(8);
			mPublisherViewContainer.AddView(mPublisher.View, layoutParams);
		}

		private void UnsubscribeFromStream(Stream stream) {
			mStreams.Remove(stream);
			if (mSubscriber.Stream.Equals(stream)) {
				mSubscriberViewContainer.RemoveView(mSubscriber.View);
				mSubscriber = null;
				if (mStreams.Count > 0) {
					SubscribeToStream(mStreams.First());
				}
			}
		}

		private void SubscribeToStream(Stream stream) {
			mSubscriber = new Subscriber(mActivity, stream);
			mSubscriber.SetVideoListener(this);
			mSession.Subscribe(mSubscriber);
		}

		#region ISessionListener

		public void OnConnected (Session p0)
		{
			if (mPublisher == null) {
				mPublisher = new Publisher(mActivity, "publisher");
				mPublisher.SetPublisherListener(this);
				AttachPublisherView(mPublisher);
				mSession.Publish(mPublisher);
			}
		}

		public void OnDisconnected (Session p0)
		{
			if (mPublisher != null) {
				mPublisherViewContainer.RemoveView(mPublisher.View);
			}

			if (mSubscriber != null) {
				mSubscriberViewContainer.RemoveView(mSubscriber.View);
			}

			mPublisher = null;
			mSubscriber = null;
			mStreams.Clear();
			mSession = null;
		}

		public void OnError (Session session, OpentokError error)
		{
			// error
		}

		public void OnStreamDropped (Session session, Stream stream)
		{
			if (mSubscriber != null) {
				UnsubscribeFromStream(stream);
			}
		}

		public void OnStreamReceived (Session session, Stream stream)
		{
			mStreams.Add(stream);
			if (mSubscriber == null) {
				SubscribeToStream(stream);
			}
		}

		#endregion ISessionListener

		#region IPublisherListener

		public void OnError (PublisherKit publisher, OpentokError exception)
		{
			// error
		}

		public void OnStreamCreated (PublisherKit publisher, Stream stream)
		{
			mStreams.Add(stream);
			if (mSubscriber == null) {
				SubscribeToStream(stream);
			}
		}

		public void OnStreamDestroyed (PublisherKit publisher, Stream stream)
		{
			UnsubscribeFromStream (stream);
		}

		#endregion IPublisherListener

		#region IVideoListener

		public void OnVideoDataReceived (SubscriberKit subscriber)
		{
			AttachSubscriberView(mSubscriber);
		}

		public void OnVideoDisableWarning (SubscriberKit subscriber)
		{
			// warning
		}

		public void OnVideoDisableWarningLifted (SubscriberKit subscriber)
		{
			// warning
		}

		public void OnVideoDisabled (SubscriberKit subscriber, string reason)
		{
			// video disabled
		}

		public void OnVideoEnabled (SubscriberKit subscriber, string reason)
		{
			// video enabled
		}

		#endregion IVideoListener
	}
}

