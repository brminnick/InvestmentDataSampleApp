using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesDataTemplate : DataTemplate
    {
        readonly static int _rowHeight = Device.Idiom is TargetIdiom.Phone ? 50 : 150;

        public OpportunitiesDataTemplate() : base(LoadTemplate)
        {

        }

        static Grid LoadTemplate()
        {
            var beaconFundingImage = new Image
            {
                Source = "beaconfundingicon",
                HeightRequest = _rowHeight
            };

            var topicTitleLabel = new Label
            {
                Text = "Topic",
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.End
            };

            var topicDescriptionLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Start
            };
            topicDescriptionLabel.SetBinding(Label.TextProperty, nameof(OpportunityModel.Topic));

            var companyTitleLabel = new Label
            {
                Text = "Company",
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.End
            };

            var companyDescriptionLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Start
            };
            companyDescriptionLabel.SetBinding(Label.TextProperty, nameof(OpportunityModel.Company));

            var leaseAmountTitleLabel = new Label
            {
                Text = "Lease Amount",
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.End
            };

            var leaseAmountDescriptionLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Start
            };
            leaseAmountDescriptionLabel.SetBinding(Label.TextProperty, nameof(OpportunityModel.LeaseAmountAsCurrency));

            var ownerTitleLabel = new Label
            {
                Text = "Owner",
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.End
            };

            var ownerDescriptionLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Start
            };
            ownerDescriptionLabel.SetBinding(Label.TextProperty, nameof(OpportunityModel.Owner));

            var grid = new Grid
            {
                ColumnSpacing = 20
            };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_rowHeight / 3, GridUnitType.Absolute) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            grid.Children.Add(beaconFundingImage, 0, 0);
            Grid.SetRowSpan(beaconFundingImage, 2);

            grid.Children.Add(topicTitleLabel, 1, 0);
            grid.Children.Add(topicDescriptionLabel, 1, 1);

            if (Device.Idiom != TargetIdiom.Phone)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


                grid.Children.Add(companyTitleLabel, 2, 0);
                grid.Children.Add(companyDescriptionLabel, 2, 1);

                grid.Children.Add(leaseAmountTitleLabel, 3, 0);
                grid.Children.Add(leaseAmountDescriptionLabel, 3, 1);

                grid.Children.Add(ownerTitleLabel, 4, 0);
                grid.Children.Add(ownerDescriptionLabel, 4, 1);
            }

            var gridLeftMargin = Device.RuntimePlatform is Device.Android ? 5 : 0;
            grid.Margin = new Thickness(gridLeftMargin, 5, 0, 15);

            return grid;
        }
    }
}

