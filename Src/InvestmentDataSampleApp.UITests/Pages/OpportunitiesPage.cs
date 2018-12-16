using System;

using Xamarin.UITest;
using Xamarin.UITest.iOS;

using InvestmentDataSampleApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace InvestmentDataSampleApp.UITests
{
    public class OpportunitiesPage : BasePage
    {
        #region Constant Fields
        readonly Query _addOpportunityButton, _opportunitySearchBar, _welcomeViewOkButton;
        #endregion

        #region Constructors
        public OpportunitiesPage(IApp app) : base(app)
        {
            _addOpportunityButton = x => x.Marked(AutomationIdConstants.AddOpportunityButton);
            _opportunitySearchBar = x => x.Marked(AutomationIdConstants.OpportunitySearchBar);
            _welcomeViewOkButton = x => x.Marked(AutomationIdConstants.WelcomeViewOkButton);
        }
        #endregion

        #region Properties
        public bool IsWelcomeViewVisible => IsWelcomeViewOnScreen();
        #endregion

        #region Methods
        public void TapAddOpportunityButton()
        {
            if (App is iOSApp)
                App.Tap(_addOpportunityButton);
            else
                App.Tap(x => x.Class("ActionMenuItemView"));
            App.Screenshot("Tapped Add Opportunity Button");
        }

        public void TapOpportunityViewCell(string topic)
        {
            App.ScrollDownTo(topic);
            App.Tap(topic);
            App.Screenshot($"Tapped ${topic} View Cell");
        }

        public void DeleteViewCell(string topic)
        {
            App.ScrollDownTo(topic);

            if (App is iOSApp)
                App.SwipeRightToLeft(topic);
            else
                App.TouchAndHold(topic);

            App.Tap("Delete");
        }

        public void TapWelcomeViewOkButton()
        {
            App.Tap(_welcomeViewOkButton);
            App.Screenshot("Welcome View Ok Button Tapped");
        }

        public void Search(string searchString)
        {
            App.Tap(_opportunitySearchBar);
            App.EnterText(searchString);
            App.DismissKeyboard();
            App.Screenshot($"Entered {searchString} into Search Bar");
        }

        public bool DoesViewCellExist(string topic, int timeoutInSeconds = 10)
        {
            try
            {
                App.ScrollDownTo(topic, timeout: TimeSpan.FromSeconds(timeoutInSeconds));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void WaitForNoWelcomeView() => App.WaitForNoElement(_welcomeViewOkButton);

        bool IsWelcomeViewOnScreen(int timeoutInSeconds = 10)
        {
            try
            {
                App.WaitForElement(_welcomeViewOkButton, timeout: TimeSpan.FromSeconds(timeoutInSeconds));
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
