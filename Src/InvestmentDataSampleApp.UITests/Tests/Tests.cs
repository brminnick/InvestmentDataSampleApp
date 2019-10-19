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
        const string _topicText = "715023 / Investment Data Corp";
        const string _companyText = "Test Company";
        const int _leaseAmount = 123456789;
        const string _ownerText = "Test Owner";
        const string _dbaText = "Test DBA";

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

        [Ignore("ContextActions not currently available in Xamarin.Forms.CollectionView: https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/collectionview/introduction#move-from-listview-to-collectionview")]
        [TestCase(_topicText)]
        public void DeleteOpportunity(string opportunityTopic)
        {
            //Arrange

            //Act
            OpportunitiesPage.DeleteViewCell(opportunityTopic);

            //Assert
            Assert.IsFalse(OpportunitiesPage.DoesViewCellExist(opportunityTopic));

            //Act
            OpportunitiesPage.TriggerPullToRefresh();

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

        [TestCase(true, _topicText, _companyText, _leaseAmount, _ownerText, _dbaText)]
        [TestCase(false, _topicText, _companyText, _leaseAmount, _ownerText, _dbaText)]
        public async Task AddNewOpportunity(bool shouldUseKeyboardReturnButton, string topicText, string companyText, int leaseAmount, string ownerText, string dbaText)
        {
            //Arrange

            //Act
            OpportunitiesPage.TapAddOpportunityButton();

            AddOpportunityPage.PopulateAllFields(topicText, companyText, leaseAmount, ownerText, dbaText, shouldUseKeyboardReturnButton);

            if (!shouldUseKeyboardReturnButton)
                AddOpportunityPage.TapSaveButton();

            OpportunitiesPage.TriggerPullToRefresh();
            await OpportunitiesPage.WaitForNoActivityIndicator().ConfigureAwait(false);

            OpportunitiesPage.TapOpportunityViewCell(topicText);

            OpportunityDetailPage.WaitForPageToAppear();

            //Assert
            Assert.AreEqual(PageTitleConstants.OpportunityDetailPage, OpportunityDetailPage.Title);
        }

        [TestCase(_topicText, _companyText, _leaseAmount, _ownerText, _dbaText)]
        public void CancelAddNewOpportunity(string topicText, string companyText, int leaseAmount, string ownerText, string dbaText)
        {
            //Arrange

            //Act
            OpportunitiesPage.TapAddOpportunityButton();

            AddOpportunityPage.PopulateAllFields(topicText, companyText, leaseAmount, ownerText, dbaText, false);
            AddOpportunityPage.TapCancelButton();

            //Assert
            Assert.IsFalse(OpportunitiesPage.DoesViewCellExist(topicText));
        }
    }
}

