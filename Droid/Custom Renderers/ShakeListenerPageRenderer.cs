using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using InvestmentDataSampleApp;
using InvestmentDataSampleApp.Droid;

[assembly: ExportRenderer(typeof(ShakeListenerNavigationPage), typeof(ShakeListenerNavigationPageRenderer))]
namespace InvestmentDataSampleApp.Droid
{
	public class ShakeListenerNavigationPageRenderer : PageRenderer
	{
		static ShakeListenerNavigationPage _page;

		const int ShakeDetectionTimeLapse = 250;
		const double ShakeThreshold = 800;

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
				_page = e.NewElement as ShakeListenerNavigationPage;
		}

		public static void HandleDeviceShake()
		{
			_page?.HandleShake(_page, new EventArgs());

		}
	}
}