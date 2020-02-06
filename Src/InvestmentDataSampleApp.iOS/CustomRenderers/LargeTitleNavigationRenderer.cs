using InvestmentDataSampleApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(LargeTitleNavigationRenderer))]
namespace InvestmentDataSampleApp.iOS
{
    public class LargeTitleNavigationRenderer : NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                var navigationPage = (NavigationPage)Element;
                var navigationPageBackgroundColor = navigationPage.BarBackgroundColor;

                NavigationBar.StandardAppearance.BackgroundColor = navigationPageBackgroundColor == Color.Default
                    ? UINavigationBar.Appearance.BarTintColor
                    : navigationPageBackgroundColor.ToUIColor();

                if (NavigationBar.TitleTextAttributes != null)
                    NavigationBar.StandardAppearance.TitleTextAttributes = NavigationBar.TitleTextAttributes;

                if (NavigationBar.LargeTitleTextAttributes != null)
                    NavigationBar.StandardAppearance.LargeTitleTextAttributes = NavigationBar.LargeTitleTextAttributes;

                NavigationBar.ScrollEdgeAppearance = NavigationBar.StandardAppearance;
            }
        }
    }
}
