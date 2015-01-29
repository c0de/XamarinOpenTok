using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Widget;
using Android.App;
using OpenTok.Android;
using Xamarin.Forms.Platform.Android;
using System.Reflection.Emit;

[assembly: Xamarin.Forms.ExportRenderer (typeof (OpenTokForms.OpenTokView), typeof (OpenTokForms.Droid.OpenTokViewRenderer))]
namespace OpenTokForms.Droid
{
	public class OpenTokViewRenderer : Xamarin.Forms.Platform.Android.ViewRenderer,
		Session.ISessionListener,
		Publisher.IPublisherListener,
		Subscriber.IVideoListener
	{
		private Activity _activity;
		private OpenTokForms.OpenTokView _openTokView;
		private Session _session;
		private Publisher _publisher;
		private Subscriber _subscriber;
		private List<Stream> _streams;
		protected Handler _handler = new Handler();

		private RelativeLayout _layout;

		protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged (e);

			_activity = this.Context as Activity;
			_openTokView = e.NewElement as OpenTokView;
			_streams = new List<Stream>();

			_layout = new RelativeLayout (this.Context);
			_layout.SetMinimumHeight (Resources.DisplayMetrics.HeightPixels);
			//_layout.SetMinimumWidth (Resources.DisplayMetrics.WidthPixels);

			SetNativeControl (_layout);

			SessionConnect();
		}

		private void SessionConnect() {
			if (_session == null) {
				_session = new Session(_activity, Configuration.Config.API_KEY, Configuration.Config.SESSION_ID);
				_session.SetSessionListener(this);
				_session.Connect(Configuration.Config.TOKEN);
			}
		}

		private void AttachSubscriberView(Subscriber subscriber) {
			RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams (Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
			_layout.RemoveView(_subscriber.View);
			_layout.AddView(_subscriber.View, layoutParams);
			subscriber.SetStyle(BaseVideoRenderer.StyleVideoScale, BaseVideoRenderer.StyleVideoFill);
		}

		private void AttachPublisherView(Publisher publisher) {
			_publisher.SetStyle(BaseVideoRenderer.StyleVideoScale, BaseVideoRenderer.StyleVideoFill);
			RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(320, 240);
			layoutParams.AddRule(LayoutRules.AlignParentBottom, (int)LayoutRules.True);
			layoutParams.AddRule(LayoutRules.AlignParentRight, (int)LayoutRules.True);
			layoutParams.BottomMargin = DpToPx(8);
			layoutParams.RightMargin = DpToPx(8);
			_layout.AddView(_publisher.View, layoutParams);
		}

		private void UnsubscribeFromStream(Stream stream) {
			_streams.Remove(stream);
			if (_subscriber.Stream.Equals(stream)) {
				_layout.RemoveView(_subscriber.View);
				_subscriber = null;
				if (_streams.Count > 0) {
					SubscribeToStream(_streams.First());
				}
			}
		}

		private void SubscribeToStream(Stream stream) {
			_subscriber = new Subscriber(_activity, stream);
			_subscriber.SetVideoListener(this);
			_session.Subscribe(_subscriber);
		}

		private int DpToPx(int dp) {
			double screenDensity = this.Resources.DisplayMetrics.Density;
			return (int) (screenDensity * (double) dp);
		}

		#region ISessionListener

		public void OnConnected (Session p0)
		{
			if (_publisher == null) {
				_publisher = new Publisher(_activity, "publisher");
				_publisher.SetPublisherListener(this);
				AttachPublisherView(_publisher);
				_session.Publish(_publisher);
			}
		}

		public void OnDisconnected (Session p0)
		{
			if (_publisher != null) {
				_layout.RemoveView(_publisher.View);
			}

			if (_subscriber != null) {
				_layout.RemoveView(_subscriber.View);
			}

			_publisher = null;
			_subscriber = null;
			_streams.Clear();
			_session = null;
		}

		public void OnError (Session session, OpentokError error)
		{
			// TODO: handle error
		}

		public void OnStreamDropped (Session session, Stream stream)
		{
			if (_subscriber != null) {
				UnsubscribeFromStream(stream);
			}
		}

		public void OnStreamReceived (Session session, Stream stream)
		{
			_streams.Add(stream);
			if (_subscriber == null) {
				SubscribeToStream(stream);
			}
		}

		#endregion ISessionListener

		#region IPublisherListener

		public void OnError (PublisherKit publisher, OpentokError exception)
		{
			// TODO: handle error
		}

		public void OnStreamCreated (PublisherKit publisher, Stream stream)
		{
			_streams.Add(stream);
			if (_subscriber == null) {
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
			AttachSubscriberView(_subscriber);
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

