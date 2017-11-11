using System;

using NUnit.Framework;

using Xamarin.UITest;

namespace InvestmentDataSampleApp.UITests
{
    public abstract class BaseTest
    {
        #region Constructors
        protected BaseTest(Platform platform) => Platform = platform;
        #endregion

        #region Properties
        protected IApp App { get; private set; }
        protected Platform Platform { get; private set; }
        protected OpportunitiesPage OpportunitiesPage { get; private set; }
        protected AddOpportunityPage AddOpportunityPage { get; private set; }
        protected OpportunityDetailPage OpportunityDetailPage { get; private set; }
        #endregion

        #region Methods
        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(Platform);

            OpportunitiesPage = new OpportunitiesPage(App);
            AddOpportunityPage = new AddOpportunityPage(App);
            OpportunityDetailPage = new OpportunityDetailPage(App);

            App.Screenshot("App Launched");

            if (OpportunitiesPage.IsWelcomeViewVisible)
                OpportunitiesPage.TapWelcomeViewOkButton();
        }
        #endregion
    }

}
