using System;
using System.Text;
using InvestmentDataSampleApp.Shared;
using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class AddOpportunityPage : BaseContentPage<AddOpportunityViewModel>
    {
        readonly Button _cancelButton, _saveButton;
        readonly Entry _topicEntry, _companyEntry, _leaseAmountEntry, _ownerEntry, _dbaEntry;

        public AddOpportunityPage()
        {
            ViewModel.SaveError += HandleSaveError;
            ViewModel.SaveToDatabaseCompleted += HandleCancelButtonTapped;

            var topicLabel = new AddOpportunityLabel("Topic");

            _topicEntry = new AddOpportunityEntry(ReturnType.Next, AutomationIdConstants.TopicEntry);
            _topicEntry.SetBinding(Entry.TextProperty, nameof(AddOpportunityViewModel.Topic));


            var companyLabel = new AddOpportunityLabel("Company");

            _companyEntry = new AddOpportunityEntry(ReturnType.Next, AutomationIdConstants.CompanyEntry);
            _companyEntry.SetBinding(Entry.TextProperty, nameof(AddOpportunityViewModel.Company));


            var dbaLabel = new AddOpportunityLabel("DBA");

            _dbaEntry = new AddOpportunityEntry(ReturnType.Go, AutomationIdConstants.DBAEntry);
            _dbaEntry.SetBinding(Entry.TextProperty, nameof(AddOpportunityViewModel.DBA));
            _dbaEntry.SetBinding(Entry.ReturnCommandProperty, nameof(AddOpportunityViewModel.SaveButtonTapped));


            var leaseAmountLabel = new AddOpportunityLabel("Lease Amount");

            _leaseAmountEntry = new AddOpportunityEntry(ReturnType.Next, AutomationIdConstants.LeaseAmountEntry)
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = "0"
            };
            _leaseAmountEntry.SetBinding(Entry.TextProperty, nameof(AddOpportunityViewModel.LeaseAmount));


            var ownerLabel = new AddOpportunityLabel("Owner");

            _ownerEntry = new AddOpportunityEntry(ReturnType.Next, AutomationIdConstants.OwnerEntry);
            _ownerEntry.SetBinding(Entry.TextProperty, nameof(AddOpportunityViewModel.Owner));

            _saveButton = new Button
            {
                Text = "Save",
                AutomationId = AutomationIdConstants.SaveButton,
            };
            _saveButton.SetBinding(Button.CommandProperty, nameof(AddOpportunityViewModel.SaveButtonTapped));

            _cancelButton = new Button
            {
                Text = "Cancel",
                AutomationId = AutomationIdConstants.CancelButton
            };
            _cancelButton.Clicked += HandleCancelButtonClicked;

            const int rowHeight = 35;
            var grid = new Grid
            {
                RowSpacing = 10,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(rowHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                }
            };

            grid.Children.Add(topicLabel, 0, 1);
            grid.Children.Add(_topicEntry, 1, 1);

            grid.Children.Add(companyLabel, 0, 2);
            grid.Children.Add(_companyEntry, 1, 2);

            grid.Children.Add(dbaLabel, 0, 3);
            grid.Children.Add(_dbaEntry, 1, 3);

            grid.Children.Add(leaseAmountLabel, 0, 4);
            grid.Children.Add(_leaseAmountEntry, 1, 4);

            grid.Children.Add(ownerLabel, 0, 5);
            grid.Children.Add(_ownerEntry, 1, 5);

            grid.Children.Add(_saveButton, 0, 6);
            Grid.SetColumnSpan(_saveButton, 2);

            //Only display Cancel button on Android because iOS users can swipe away the modal page
            if (Device.RuntimePlatform is Device.Android)
            {
                grid.Children.Add(_cancelButton, 0, 6);
                Grid.SetColumnSpan(_cancelButton, 2);
            }

            Title = PageTitleConstants.AddOpportunityPage;

            Padding = new Thickness(20, 10, 20, 0);

            Content = grid;
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
        }

        async void HandleCancelButtonClicked(object sender, EventArgs e) =>
            await Navigation.PopModalAsync();

        void HandleSaveError(object sender, EventArgs e)
        {
            var opportunityViewModel = (AddOpportunityViewModel)sender;
            var blankFieldsString = new StringBuilder();

            if (string.IsNullOrWhiteSpace(opportunityViewModel?.Topic))
                blankFieldsString.AppendLine("Topic");
            if (string.IsNullOrWhiteSpace(opportunityViewModel?.Company))
                blankFieldsString.AppendLine("Company");
            if (opportunityViewModel?.LeaseAmount == 0)
                blankFieldsString.AppendLine("Lease Amount");
            if (string.IsNullOrWhiteSpace(opportunityViewModel?.Owner))
                blankFieldsString.AppendLine("Owner");
            if (string.IsNullOrWhiteSpace(opportunityViewModel?.DBA))
                blankFieldsString.Append("DBA");

            blankFieldsString.Remove(blankFieldsString.Length - 1, 1);

            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error: Missing Data", $"The following fields are empty: \n{blankFieldsString}", "OK"));
        }

        void HandleCancelButtonTapped(object sender, EventArgs e) => Device.BeginInvokeOnMainThread(() => Navigation.PopModalAsync());

        class AddOpportunityEntry : Entry
        {
            public AddOpportunityEntry(ReturnType returnType, string automationId)
            {
                VerticalTextAlignment = TextAlignment.Center;
                HorizontalTextAlignment = TextAlignment.End;
                TextColor = Color.Black;
                BackgroundColor = Color.White;
                ReturnType = returnType;
                AutomationId = automationId;
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing;
            }
        }

        class AddOpportunityLabel : Label
        {
            public AddOpportunityLabel(string text)
            {
                HorizontalOptions = LayoutOptions.Start;
                VerticalTextAlignment = TextAlignment.Center;
                HorizontalTextAlignment = TextAlignment.Start;
                Text = text;
            }
        }
    }
}

