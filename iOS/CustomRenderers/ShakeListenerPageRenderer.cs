using System;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using InvestmentDataSampleApp;
using InvestmentDataSampleApp.iOS;

[assembly: ExportRenderer(typeof(ShakeListenerNavigationPage), typeof(ShakeListenerNavigationPageRenderer))]
namespace InvestmentDataSampleApp.iOS
{
	public class ShakeListenerNavigationPageRenderer : PageRenderer
	{
        ShakeListenerNavigationPage _page;

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
				_page = e.NewElement as ShakeListenerNavigationPage;
		}
		public override void MotionEnded(UIEventSubtype motion, UIEvent evt)
		{
			base.MotionEnded(motion, evt);

			if (motion == UIEventSubtype.MotionShake)
			{
				_page.HandleShake(_page, EventArgs.Empty);
			}
		}
	}
}