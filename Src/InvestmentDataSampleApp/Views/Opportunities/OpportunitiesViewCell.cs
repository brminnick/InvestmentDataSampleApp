using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesViewCell : ViewCell
    {
        readonly MenuItem _deleteAction;

        public OpportunitiesViewCell()
        {
            #region Create Image
            var beaconFundingImage = new Image { Source = "beaconfundingicon" };
            #endregion

            #region Create Topic Stack
            var topicLabel = new Label
            {
                Text = "Topic",
                FontAttributes = FontAttributes.Bold
            };

            var topic = new Label();
            topic.SetBinding(Label.TextProperty, nameof(OpportunityModel.Topic));

            var topicStack = new StackLayout
            {
                Children = {
                    topicLabel,
                    topic
                }
            };
            #endregion

            #region Create Company
            var companyLabel = new Label
            {
                Text = "Company",
                FontAttributes = FontAttributes.Bold
            };

            var company = new Label();
            company.SetBinding(Label.TextProperty, nameof(OpportunityModel.Company));

            var companyStack = new StackLayout
            {
                Children = {
                    companyLabel,
                    company
                }
            };
            #endregion

            #region Create LeaseAmount
            var leaseAmountLabel = new Label
            {
                Text = "LeaseAmount",
                FontAttributes = FontAttributes.Bold
            };

            var leaseAmount = new Label();
            leaseAmount.SetBinding(Label.TextProperty, nameof(OpportunityModel.LeaseAmountAsCurrency));

            var leaseAmountStack = new StackLayout
            {
                Children = {
                    leaseAmountLabel,
                    leaseAmount
                }
            };
            #endregion

            #region Create LeaseAmount 
            var ownerLabel = new Label
            {
                Text = "Owner",
                FontAttributes = FontAttributes.Bold
            };

            var owner = new Label();
            owner.SetBinding(Label.TextProperty, nameof(OpportunityModel.Owner));

            var ownerStack = new StackLayout
            {
                Children = {
                    ownerLabel,
                    owner
                }
            };
            #endregion

            #region Create MenuItem
            _deleteAction = new MenuItem
            {
                Text = "Delete",
                IsDestructive = true
            };
            _deleteAction.Clicked += HandleDeleteActionClicked;
            ContextActions.Add(_deleteAction);
            #endregion

            StackLayout cellStack;
            // Create Cell Horizontal StackLayout for Phone, or Tablet/Desktop
            if (Device.Idiom is TargetIdiom.Phone)
            {
                topic.LineBreakMode = LineBreakMode.NoWrap;
                cellStack = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    Padding = 10,
                    Spacing = 20,
                    Orientation = StackOrientation.Horizontal,
                    Children = {
                        beaconFundingImage,
                        topicStack
                    }
                };
            }
            else
            {
                cellStack = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    Padding = 10,
                    Spacing = 20,
                    Orientation = StackOrientation.Horizontal,
                    Children = {
                        beaconFundingImage,
                        topicStack,
                        companyStack,
                        leaseAmountStack,
                        ownerStack
                    }
                };
            }

            View = cellStack;
        }

        async void HandleDeleteActionClicked(object sender, EventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.BindingContext is OpportunityModel selectedModel)
            {
                await OpportunityModelDatabase.DeleteItemAsync(selectedModel).ConfigureAwait(false);

                //Wait for the iOS animation to finish
                if (Device.RuntimePlatform is Device.iOS)
                    await Task.Delay(250).ConfigureAwait(false);

                if (Application.Current.MainPage is NavigationPage navigationPage
                    && navigationPage.CurrentPage is OpportunitiesPage opportunitiesPage
                    && opportunitiesPage.BindingContext is OpportunitiesViewModel opportunitiesViewModel)
                {
                    opportunitiesViewModel?.RefreshAllDataCommand?.Execute(null);
                }
            }
        }
    }
}

