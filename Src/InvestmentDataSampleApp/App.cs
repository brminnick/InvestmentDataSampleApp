using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace InvestmentDataSampleApp
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            Xamarin.Forms.Device.SetFlags(new[] { "Markup_Experimental", "SwipeView_Experimental" });

            var navigationPage = new ShakeListenerNavigationPage(new OpportunitiesPage());
            navigationPage.On<iOS>().SetPrefersLargeTitles(true);

            MainPage = navigationPage;
        }
    }

    static class MarkupExtensions
    {
        public static GridLength Stars(double value) => new GridLength(value, GridUnitType.Star);
        public static GridLength Absolute(double value) => new GridLength(value, GridUnitType.Absolute);
    }
}

