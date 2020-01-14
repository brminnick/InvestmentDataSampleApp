using System;
using NUnit.Framework;

using Xamarin.UITest;

namespace InvestmentDataSampleApp.UITests
{
    public abstract class BaseTest
    {
        readonly Platform _platform;

        IApp? _app;
        OpportunitiesPage? _opportunitiesPage;
        AddOpportunityPage? _addOpportunityPage;
        OpportunityDetailPage? _opportunityDetailPage;

        protected BaseTest(Platform platform) => _platform = platform;

        protected IApp App => _app ?? throw new NullReferenceException();
        protected OpportunitiesPage OpportunitiesPage => _opportunitiesPage ?? throw new NullReferenceException();
        protected AddOpportunityPage AddOpportunityPage => _addOpportunityPage ?? throw new NullReferenceException();
        protected OpportunityDetailPage OpportunityDetailPage => _opportunityDetailPage ?? throw new NullReferenceException();

        [SetUp]
        public virtual void TestSetup()
        {
            _app = AppInitializer.StartApp(_platform);

            _opportunitiesPage = new OpportunitiesPage(App);
            _addOpportunityPage = new AddOpportunityPage(App);
            _opportunityDetailPage = new OpportunityDetailPage(App);

            App.Screenshot("App Launched");

            if (OpportunitiesPage.IsWelcomeViewVisible)
            {
                OpportunitiesPage.TapWelcomeViewOkButton();
                OpportunitiesPage.WaitForNoWelcomeView();
            }
        }
    }
}
