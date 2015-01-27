// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace OpenTokIOS
{
	[Register ("RootViewController")]
	partial class RootViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView PublisherView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView SubscriberView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (PublisherView != null) {
				PublisherView.Dispose ();
				PublisherView = null;
			}
			if (SubscriberView != null) {
				SubscriberView.Dispose ();
				SubscriberView = null;
			}
		}
	}
}
