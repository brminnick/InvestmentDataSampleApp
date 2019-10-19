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
        public void AppLaunches() => App.Screenshot("First screen");

        [Test]
        public void ViewOpportunity()
        {
            //Arrange
            const string searchString = "05";
            const string opportunityTopic = "715005 / Investment Data Corp";

            //Act
            OpportunitiesPage.Search(searchString);
            OpportunitiesPage.TapOpportunityViewCell(opportunityTopic);
            OpportunityDetailPage.WaitForPageToAppear();

            //Assert
            Assert.AreEqual(PageTitleConstants.OpportunityDetailPage, OpportunityDetailPage.Title);
        }

        [Ignore("ContextActions not currently available in Xamarin.Forms.CollectionView: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/collectionview/introduction#move-from-listview-to-collectionview")]
        [Test]
        public async Task DeleteOpportunity()
        {
            //Arrange
            const string opportunityTopic = "715003 / Investment Data Corp";

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
            AddOpportunityPage.WaitForErrorMessage();
            //Assert

            Assert.IsTrue(AddOpportunityPage.IsErrorMessageDisplayed);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddNewOpportunity(bool shouldUseKeyboardReturnButton)
        {
            //Arrange
            const string topicText = "715023 / Investment Data Corp";
            const string companyText = "Test Company";
            const int leaseAmount = 123456789;
            const string ownerText = "Test Owner";
            const string dbaText = "Test DBA";

            //Act
            OpportunitiesPage.TapAddOpportunityButton();

            AddOpportunityPage.PopulateAllFields(topicText, companyText, leaseAmount, ownerText, dbaText, shouldUseKeyboardReturnButton);

            if (!shouldUseKeyboardReturnButton)
                AddOpportunityPage.TapSaveButton();

            OpportunitiesPage.TapOpportunityViewCell(topicText);

            OpportunityDetailPage.WaitForPageToAppear();

            //Assert
            Assert.AreEqual(PageTitleConstants.OpportunityDetailPage, OpportunityDetailPage.Title);
        }

        [Test]
        public void CancelAddNewOpportunity()
        {
            //Arrange
            const string topicText = "Test Topic";
            const string companyText = "Test Company";
            const int leaseAmount = 123456789;
            const string ownerText = "Test Owner";
            const string dbaText = "Test DBA";

            //Act
            OpportunitiesPage.TapAddOpportunityButton();

            AddOpportunityPage.PopulateAllFields(topicText, companyText, leaseAmount, ownerText, dbaText, false);
            AddOpportunityPage.TapCancelButton();

            //Assert
            Assert.IsFalse(OpportunitiesPage.DoesViewCellExist(topicText));
        }
    }
}

