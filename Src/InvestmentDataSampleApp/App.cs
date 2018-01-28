using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
	public class App : Application
	{
		public App()
		{
            var customNavigationPage = new ShakeListenerNavigationPage(new OpportunitiesPage());

            MainPage = customNavigationPage;
		}
	}
}

