using System;

using NUnit.Framework;

using Xamarin.UITest;

namespace InvestmentDataSampleApp.UITests
{
	public abstract class BaseTest
	{
		protected IApp App;
		protected Platform Platform;

		protected OpportunitiesPage OpportunitiesPage;
		protected AddOpportunityPage AddOpportunityPage;
		protected OpportunityDetailPage OpportunityDetailPage;

		protected BaseTest(Platform platform)
		{
			Platform = platform;
		}

		[SetUp]
		public virtual void TestSetup()
		{
			App = AppInitializer.StartApp(Platform);

			OpportunitiesPage = new OpportunitiesPage(App, Platform);
			AddOpportunityPage = new AddOpportunityPage(App, Platform);
			OpportunityDetailPage = new OpportunityDetailPage(App, Platform);

			App.Screenshot("App Launched");

			if (OpportunitiesPage.IsWelcomeViewVisible)
				OpportunitiesPage.TapWelcomeViewOkButton();
		}
	}

}
