using NUnit.Framework;

using Xamarin.UITest;

namespace InvestmentDataSampleApp.UITests
{
    public abstract class BaseTest
    {
        readonly Platform _platform;

        protected BaseTest(Platform platform) => _platform = platform;

        protected IApp App { get; private set; }
        protected OpportunitiesPage OpportunitiesPage { get; private set; }
        protected AddOpportunityPage AddOpportunityPage { get; private set; }
        protected OpportunityDetailPage OpportunityDetailPage { get; private set; }

        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(_platform);

            OpportunitiesPage = new OpportunitiesPage(App);
            AddOpportunityPage = new AddOpportunityPage(App);
            OpportunityDetailPage = new OpportunityDetailPage(App);

            App.Screenshot("App Launched");

            if (OpportunitiesPage.IsWelcomeViewVisible)
            {
                OpportunitiesPage.TapWelcomeViewOkButton();
                OpportunitiesPage.WaitForNoWelcomeView();
            }
        }
    }
}
