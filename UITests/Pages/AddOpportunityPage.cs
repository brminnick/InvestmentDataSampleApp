using Xamarin.UITest;

using InvestmentDataSampleApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace InvestmentDataSampleApp.UITests
{
	public class AddOpportunityPage : BasePage
	{
		#region Constant Fields
		const int _entryCellXOffset = 200;

		readonly Query TopicEntry;
		readonly Query CompanyEntry;
		readonly Query LeaseAmountEntry;
		readonly Query OwnerEntry;
		readonly Query DBAEntry;

		readonly Query SaveButton;
		readonly Query CancelButton;
		#endregion

		#region Construvtors
		public AddOpportunityPage(IApp app, Platform platform) : base(app, platform)
		{
			TopicEntry = x => x.Marked(AutomationIdConstants.TopicEntry);
			CompanyEntry = x => x.Marked(AutomationIdConstants.CompanyEntry);
			LeaseAmountEntry = x => x.Marked(AutomationIdConstants.LeaseAmountEntry);
			OwnerEntry = x => x.Marked(AutomationIdConstants.OwnerEntry);
			DBAEntry = x => x.Marked(AutomationIdConstants.DBAEntry);

			SaveButton = x => x.Marked(AutomationIdConstants.SaveButton);
			CancelButton = x => x.Marked(AutomationIdConstants.CancelButton);
		}
		#endregion

		#region Properties
		public bool IsErrorMessageDisplayed => app.Query("OK").Length > 0;
		#endregion

		#region Methods
		public void PopulateAllFields(string topicText, string companyText, int leaseAmount, string ownerText, string dbaText)
		{
			EnterTopicText(topicText);
			EnterCompanyText(companyText);
			EnterLeaseAmountText(leaseAmount);
			EnterOwnerText(ownerText);
			EnterDBAText(dbaText);
		}

		public void TapSaveButton()
		{
			if (OniOS)
				app.Tap(SaveButton);
			else
				app.Tap(x => x.Marked("Save"));

			app.Screenshot("Tapped Save Button");
		}

		public void TapCancelButton()
		{
			if (OniOS)
				app.Tap(CancelButton);
			else
				app.Tap(x => x.Marked("Cancel"));

			app.Screenshot("Tapped Cancel Button");
		}

		void EnterTopicText(string topicText)
		{
			app.Tap(TopicEntry);
			app.ClearText();
			app.EnterText(topicText);
			app.DismissKeyboard();
			app.Screenshot($"Entered {topicText} into Topic Entry");
		}

		void EnterCompanyText(string companyText)
		{
			app.Tap(CompanyEntry);
			app.ClearText();
			app.EnterText(companyText);
			app.DismissKeyboard();
			app.Screenshot($"Entered {companyText} into Company Entry");
		}

		void EnterLeaseAmountText(int leaseText)
		{
			app.Tap(LeaseAmountEntry);
			app.ClearText();
			app.EnterText(leaseText.ToString());
			app.DismissKeyboard();
			app.Screenshot($"Entered {leaseText} into Lease Amount Entry");
		}

		void EnterOwnerText(string ownerText)
		{
			app.Tap(OwnerEntry);
			app.ClearText();
			app.EnterText(ownerText);
			app.DismissKeyboard();
			app.Screenshot($"Entered {ownerText} into Owner Entry");
		}

		void EnterDBAText(string dbaText)
		{
			app.Tap(DBAEntry);
			app.ClearText();
			app.EnterText(dbaText);
			app.DismissKeyboard();
			app.Screenshot($"Entered {dbaText} into DBA Entry");
		}
		#endregion
	}
}
