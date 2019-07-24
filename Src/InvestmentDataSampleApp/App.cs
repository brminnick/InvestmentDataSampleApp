using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class App : Application
    {
        public App() => MainPage = new ShakeListenerNavigationPage(new OpportunitiesPage());
    }
}

