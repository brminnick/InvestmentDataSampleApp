using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.iOS;

using InvestmentDataSampleApp.Constants;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace InvestmentDataSampleApp.UITests
{
    public class AddOpportunityPage : BasePage
    {
        #region Constant Fields
        const int _entryCellXOffset = 200;

        readonly Query _topicEntry, _companyEntry, _leaseAmountEntry, _ownerEntry,
            _dbaEntry, _saveButton, _cancelButton;
        #endregion

        #region Construvtors
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
        #endregion

        #region Properties
        public bool IsErrorMessageDisplayed => App.Query("OK").Any();
        #endregion

        #region Methods
        public void PopulateAllFields(string topicText, string companyText, int leaseAmount, string ownerText, string dbaText, bool shouldUseKeyboardReturnButton)
        {
            if (shouldUseKeyboardReturnButton)
                PopulateAllFieldsByUsingKeyboardReturnButton(topicText, companyText, leaseAmount, ownerText, dbaText);
            else
                PopulateAllFieldsByTappingEachEntry(topicText, companyText, leaseAmount, ownerText, dbaText);
        }

        public void TapSaveButton()
        {
            Query saveButtonQuery;

            if (App is iOSApp)
                saveButtonQuery = _saveButton;
            else
                saveButtonQuery = x => x.Marked("Save");

            App.WaitForElement(saveButtonQuery);
            App.Tap(saveButtonQuery);

            App.Screenshot("Tapped Save Button");
        }

        public void TapCancelButton()
        {
            if (App is iOSApp)
                App.Tap(_cancelButton);
            else
                App.Tap(x => x.Marked("Cancel"));

            App.Screenshot("Tapped Cancel Button");
        }

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
        #endregion
    }
}
