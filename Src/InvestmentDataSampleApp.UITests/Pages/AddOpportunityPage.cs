using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.iOS;

using InvestmentDataSampleApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using Xamarin.UITest.Android;
using System;
using System.Runtime.InteropServices;

namespace InvestmentDataSampleApp.UITests
{
    public class AddOpportunityPage : BasePage
    {
        const int _entryCellXOffset = 200;

        readonly Query _topicEntry, _companyEntry, _leaseAmountEntry, _ownerEntry,
            _dbaEntry, _saveButton, _cancelButton;

        public AddOpportunityPage(IApp app) : base(app)
        {
            _topicEntry = x => x.Marked(AutomationIdConstants.TopicEntry);
            _companyEntry = x => x.Marked(AutomationIdConstants.CompanyEntry);
            _leaseAmountEntry = x => x.Marked(AutomationIdConstants.LeaseAmountEntry);
            _ownerEntry = x => x.Marked(AutomationIdConstants.OwnerEntry);
            _dbaEntry = x => x.Marked(AutomationIdConstants.DBAEntry);

            _saveButton = x => x.Marked(AutomationIdConstants.SaveButton);
            _cancelButton = x => x.Marked(AutomationIdConstants.CancelButton);
        }

        public bool IsErrorMessageDisplayed => App.Query("OK").Any();

        public void PopulateAllFields(string topicText, string companyText, int leaseAmount, string ownerText, string dbaText, bool shouldUseKeyboardReturnButton)
        {
            if (shouldUseKeyboardReturnButton)
                PopulateAllFieldsByUsingKeyboardReturnButton(topicText, companyText, leaseAmount, ownerText, dbaText);
            else
                PopulateAllFieldsByTappingEachEntry(topicText, companyText, leaseAmount, ownerText, dbaText);
        }

        public void TapSaveButton()
        {
            App.Tap(_saveButton);
            App.Screenshot("Tapped Save Button");
        }

        public void TapCancelButton()
        {
            switch (App)
            {
                case AndroidApp androidApp:
                    App.Tap(_cancelButton);
                    App.Screenshot("Tapped Cancel Button");
                    break;

                case iOSApp iOSApp:
                    var screenHeight = App.Query().First().Rect.Height;
                    App.DragCoordinates(0, 100, 0, screenHeight);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        public void WaitForErrorMessage() => App.WaitForElement("OK");

        void PopulateAllFieldsByTappingEachEntry(string topicText, string companyText, int leaseAmount, string ownerText, string dbaText)
        {
            EnterTextThenDismissKeyboard(_topicEntry, topicText);
            EnterTextThenDismissKeyboard(_companyEntry, companyText);
            EnterTextThenDismissKeyboard(_leaseAmountEntry, leaseAmount.ToString());
            EnterTextThenDismissKeyboard(_ownerEntry, ownerText);
            EnterTextThenDismissKeyboard(_dbaEntry, dbaText);
        }

        void PopulateAllFieldsByUsingKeyboardReturnButton(string topicText, string companyText, int leaseAmount, string ownerText, string dbaText)
        {
            EnterTextThenPressEnter(_topicEntry, topicText);
            EnterTextThenPressEnter(_companyEntry, companyText);
            EnterTextThenPressEnter(_leaseAmountEntry, leaseAmount.ToString());
            EnterTextThenPressEnter(_ownerEntry, ownerText);
            EnterTextThenPressEnter(_dbaEntry, dbaText);
        }

        void EnterTextThenDismissKeyboard(Query entryQuery, string text)
        {
            App.EnterText(entryQuery, text);
            App.DismissKeyboard();

            App.Screenshot($"Entered {text} into {nameof(entryQuery)}");
        }

        void EnterTextThenPressEnter(Query entryQuery, string text)
        {
            App.EnterText(entryQuery, text);
            App.PressEnter();

            App.Screenshot($"Entered {text}");
        }
    }
}
