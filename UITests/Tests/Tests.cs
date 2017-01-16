using System.Threading.Tasks;

using NUnit.Framework;

using Xamarin.UITest;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests : BaseTest
	{
		public Tests(Platform platform) : base(platform)
		{
		}

		[Test]
		public void SmokeTest()
		{
			App.Screenshot("First screen");
		}

		[TestCase("715005 / Investment Data Corp")]
		[Test]
		public void ViewOpportunity(string opportunityTopic)
		{
			//Arrange

			//Act
			OpportunitiesPage.Search("05");
			OpportunitiesPage.TapOpportunityViewCell(opportunityTopic);
			OpportunityDetailPage.WaitForPageToAppear();

			//Assert
			Assert.AreEqual(PageTitleConstants.OpportunityDetailPageTitle, OpportunityDetailPage.Title);
		}

		[TestCase("715003 / Investment Data Corp")]
		[Test]
		public async Task DeleteOpportunity(string opportunityTopic)
		{
			//Arrange

			//Act
			OpportunitiesPage.DeleteViewCell(opportunityTopic);
			await Task.Delay(1000);

			//Assert
			Assert.IsFalse(OpportunitiesPage.DoesViewCellExist(opportunityTopic));
		}

		[Test]
		public void AddNewOpportunityEmptyFields()
		{
			//Arrange

			//Act
			OpportunitiesPage.TapAddOpportunityButton();

			AddOpportunityPage.TapSaveButton();

			//Assert
			Assert.IsTrue(AddOpportunityPage.IsErrorMessageDisplayed);
		}

		[TestCase(true)]
		[TestCase(false)]
		[Test]
		public void AddNewOpportunity(bool shouldUseKeyboardReturnButton)
		{
			//Arrange
			var topicText = "714999 / Investment Data Corp";
			var companyText = "Test Company";
			var leaseAmount = 123456789;
			var ownerText = "Test Owner";
			var dbaText = "Test DBA";

			//Act
			OpportunitiesPage.TapAddOpportunityButton();

			AddOpportunityPage.PopulateAllFields(topicText, companyText, leaseAmount, ownerText, dbaText, shouldUseKeyboardReturnButton);

			if (!shouldUseKeyboardReturnButton)
				AddOpportunityPage.TapSaveButton();

			OpportunitiesPage.TapOpportunityViewCell(topicText);

			OpportunityDetailPage.WaitForPageToAppear();

			//Assert
			Assert.AreEqual(PageTitleConstants.OpportunityDetailPageTitle, OpportunityDetailPage.Title);
		}

		[Test]
		public void CancelAddNewOpportunity()
		{
			//Arrange
			var topicText = "Test Topic";
			var companyText = "Test Company";
			var leaseAmount = 123456789;
			var ownerText = "Test Owner";
			var dbaText = "Test DBA";

			//Act
			OpportunitiesPage.TapAddOpportunityButton();

			AddOpportunityPage.PopulateAllFields(topicText, companyText, leaseAmount, ownerText, dbaText, false);
			AddOpportunityPage.TapCancelButton();

			//Assert
			Assert.IsFalse(OpportunitiesPage.DoesViewCellExist(topicText));
		}
	}
}

