using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace InvestmentDataSampleApp
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            var navigationPage = new ShakeListenerNavigationPage(new OpportunitiesPage());
            navigationPage.On<iOS>().SetPrefersLargeTitles(true);

            MainPage = navigationPage;
        }
    }
}

