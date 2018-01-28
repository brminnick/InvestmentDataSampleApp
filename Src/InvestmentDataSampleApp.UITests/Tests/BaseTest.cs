using NUnit.Framework;

using Xamarin.UITest;

namespace InvestmentDataSampleApp.UITests
{
    public abstract class BaseTest
    {
        #region Constant Fields
        readonly Platform _platform;
        #endregion

        #region Constructors
        protected BaseTest(Platform platform) => _platform = platform;
        #endregion

        #region Properties
        protected IApp App { get; private set; }
        protected OpportunitiesPage OpportunitiesPage { get; private set; }
        protected AddOpportunityPage AddOpportunityPage { get; private set; }
        protected OpportunityDetailPage OpportunityDetailPage { get; private set; }
        #endregion

        #region Methods
        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(_platform);

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
