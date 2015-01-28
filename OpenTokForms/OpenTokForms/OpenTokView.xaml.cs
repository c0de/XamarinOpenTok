using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OpenTokForms
{
	public partial class OpenTokView : View
	{
		public RelativeLayout SubscriberViewContainer { get { return subscriberView; } }
		public RelativeLayout PublisherViewContainer { get { return publisherView; } }

		public OpenTokView ()
		{
			InitializeComponent ();
		}
	}
}

