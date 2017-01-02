using System;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
	public class App : Application
	{
		static OpportunityModelDatabase _database;

		public App()
		{
            var customNavigationPage = new ShakeListenerNavigationPage(new OpportunitiesPage());

            MainPage = customNavigationPage;
		}

		public static OpportunityModelDatabase Database=>
			_database ?? (_database = new OpportunityModelDatabase());
	}
}

