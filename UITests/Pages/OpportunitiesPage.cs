using System;

using Xamarin.UITest;

using InvestmentDataSampleApp.Constants;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace InvestmentDataSampleApp.UITests
{
	public class OpportunitiesPage : BasePage
	{
		#region Constant Fields
		readonly Query AddOpportunityButton;
		readonly Query OpportunitySearchBar;
		readonly Query WelcomeViewOkButton;
		#endregion

		#region Constructors
		public OpportunitiesPage(IApp app) : base(app)
		{
			AddOpportunityButton = x => x.Marked(AutomationIdConstants.AddOpportunityButton);
			OpportunitySearchBar = x => x.Marked(AutomationIdConstants.OpportunitySearchBar);
			WelcomeViewOkButton = x => x.Marked(AutomationIdConstants.WelcomeViewOkButton);
		}
		#endregion

		#region Properties
		public bool IsWelcomeViewVisible => IsWelcomeViewOnScreen();
		#endregion

		#region Methods
		public void TapAddOpportunityButton()
		{
			if (OniOS)
				App.Tap(AddOpportunityButton);
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

			if (OniOS)
				App.SwipeRightToLeft(topic);
			else
				App.TouchAndHold(topic);

			App.Tap("Delete");
		}

		public void TapWelcomeViewOkButton()
		{
			App.Tap(WelcomeViewOkButton);
			App.Screenshot("Welcome View Ok Button Tapped");
		}

		public void Search(string searchString)
		{
			App.Tap(OpportunitySearchBar);
			App.EnterText(searchString);
			App.DismissKeyboard();
			App.Screenshot($"Entered {searchString} into Search Bar");
		}

		public bool DoesViewCellExist(string topic, int timeoutInSeconds = 10)
		{
			try
			{
				App.ScrollDownTo(topic, timeout: TimeSpan.FromSeconds(timeoutInSeconds));
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		bool IsWelcomeViewOnScreen(int timeoutInSeconds = 10)
		{
			try
			{
				App.WaitForElement(WelcomeViewOkButton, timeout: TimeSpan.FromSeconds(timeoutInSeconds));
			}
			catch
			{
				return false;
			}

			return true;
		}
		#endregion
	}
}
