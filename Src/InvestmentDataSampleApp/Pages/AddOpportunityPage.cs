using System;
using System.Text;
using InvestmentDataSampleApp.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static InvestmentDataSampleApp.MarkupExtensions;
using static Xamarin.Forms.Markup.GridRowsColumns;

namespace InvestmentDataSampleApp
{
    public class AddOpportunityPage : BaseContentPage<AddOpportunityViewModel>
    {
        public AddOpportunityPage()
        {
            ViewModel.SaveError += HandleSaveError;
            ViewModel.SaveToDatabaseCompleted += HandleCancelButtonTapped;

            int rowHeight = Device.RuntimePlatform switch
            {
                Device.Android => 50,
                Device.iOS => 35,
                _ => throw new NotSupportedException()
            };

            BackgroundColor = Color.White;

            Title = PageTitleConstants.AddOpportunityPage;

            Padding = new Thickness(20, 10, 20, 0);

            Content = new Grid
            {
                RowSpacing = 10,
                RowDefinitions = Rows.Define(
                    (Row.Title, Absolute(Device.RuntimePlatform is Device.iOS ? rowHeight * 1.5 : 0)),
                    (Row.Topic, Absolute(rowHeight)),
                    (Row.Company, Absolute(rowHeight)),
                    (Row.Owner, Absolute(rowHeight)),
                    (Row.LeaseAmount, Absolute(rowHeight)),
                    (Row.DBA, Absolute(rowHeight)),
                    (Row.Save, Absolute(rowHeight)),
                    (Row.Cancel, Stars(1))),

                ColumnDefinitions = Columns.Define(
                    (Column.Description, Stars(1)),
                    (Column.Entry, Stars(2))),

                Children =
                {
                    new TitleLabel()
                        .Row(Row.Title).ColumnSpan(All<Column>())
                        .Bind(IsVisibleProperty,nameof(AddOpportunityViewModel.IsTitleVisible)),

                    new AddOpportunityLabel("Topic")
                        .Row(Row.Topic).Column(Column.Description),

                    new AddOpportunityEntry(ReturnType.Next, AutomationIdConstants.TopicEntry, 0)
                        .Row(Row.Topic).Column(Column.Entry)
                        .Bind(Entry.TextProperty, nameof(AddOpportunityViewModel.Topic)),

                    new AddOpportunityLabel("Company")
                        .Row(Row.Company).Column(Column.Description),

                    new AddOpportunityEntry(ReturnType.Next, AutomationIdConstants.CompanyEntry, 1).Assign(out Entry companyEntry)
                        .Row(Row.Company).Column(Column.Entry)
                        .Bind(Entry.TextProperty, nameof(AddOpportunityViewModel.Company)),

                    new AddOpportunityLabel("Owner")
                        .Row(Row.Owner).Column(Column.Description),

                    new AddOpportunityEntry(ReturnType.Next, AutomationIdConstants.OwnerEntry, 2).Assign(out Entry ownerEntry)
                        .Row(Row.Owner).Column(Column.Entry)
                        .Bind(Entry.TextProperty, nameof(AddOpportunityViewModel.Owner)),

                    new AddOpportunityLabel("Lease Amount")
                        .Row(Row.LeaseAmount).Column(Column.Description),

                    new AddOpportunityEntry(ReturnType.Next, AutomationIdConstants.LeaseAmountEntry, 3) { Keyboard = Keyboard.Numeric }.Assign(out Entry leaseAmountEntry)
                        .Row(Row.LeaseAmount).Column(Column.Entry)
                        .Bind(Entry.TextProperty, nameof(AddOpportunityViewModel.LeaseAmount)),

                    new AddOpportunityLabel("DBA")
                        .Row(Row.DBA).Column(Column.Description),

                    new AddOpportunityEntry(ReturnType.Go, AutomationIdConstants.DBAEntry, 4).Assign(out Entry dbaEntry)
                        .Row(Row.DBA).Column(Column.Entry)
                        .Bind(Entry.TextProperty, nameof(AddOpportunityViewModel.DBA))
                        .Bind(Entry.ReturnCommandProperty, nameof(AddOpportunityViewModel.SaveButtonTapped)),

                    new Button { Text = "Save", AutomationId = AutomationIdConstants.SaveButton }.CenterHorizontal()
                        .Row(Row.Save).ColumnSpan(All<Column>())
                        .Bind(Button.CommandProperty, nameof(AddOpportunityViewModel.SaveButtonTapped)),

                    new Button { Text = "Cancel", AutomationId = AutomationIdConstants.CancelButton }.CenterHorizontal().Top()
                        .Row(Row.Cancel).ColumnSpan(All<Column>())
                        .Bind(IsVisibleProperty, nameof(AddOpportunityViewModel.IsCancelButtonVisible))
                        .Invoke(cancelButton => cancelButton.Clicked += HandleCancelButtonClicked)
                }
            };
        }

        enum Row { Title, Topic, Company, Owner, LeaseAmount, DBA, Save, Cancel }
        enum Column { Description, Entry }

        async void HandleCancelButtonClicked(object sender, EventArgs e) => await Navigation.PopModalAsync();

        void HandleSaveError(object sender, EventArgs e)
        {
            var opportunityViewModel = (AddOpportunityViewModel)sender;
            var blankFieldsString = new StringBuilder();

            if (string.IsNullOrWhiteSpace(opportunityViewModel.Topic))
                blankFieldsString.AppendLine("Topic");
            if (string.IsNullOrWhiteSpace(opportunityViewModel.Company))
                blankFieldsString.AppendLine("Company");
            if (opportunityViewModel.LeaseAmount is 0)
                blankFieldsString.AppendLine("Lease Amount");
            if (string.IsNullOrWhiteSpace(opportunityViewModel.Owner))
                blankFieldsString.AppendLine("Owner");
            if (string.IsNullOrWhiteSpace(opportunityViewModel.DBA))
                blankFieldsString.Append("DBA");

            blankFieldsString.Remove(blankFieldsString.Length - 1, 1);

            MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("Error: Missing Data", $"The following fields are empty:\n{blankFieldsString}", "OK"));
        }

        void HandleCancelButtonTapped(object sender, EventArgs e) => MainThread.BeginInvokeOnMainThread(() => Navigation.PopModalAsync());

        class AddOpportunityEntry : Entry
        {
            public AddOpportunityEntry(in ReturnType returnType, in string automationId, in int tabIndex)
            {
                TabIndex = tabIndex;
                ReturnType = returnType;
                AutomationId = automationId;

                TextColor = Color.Black;
                BackgroundColor = Color.White;
                VerticalTextAlignment = TextAlignment.Center;
                HorizontalTextAlignment = TextAlignment.Start;
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing;
            }
        }

        class AddOpportunityLabel : Label
        {
            public AddOpportunityLabel(in string text)
            {
                Text = text;
                HorizontalOptions = LayoutOptions.Start;
                VerticalTextAlignment = TextAlignment.Center;
                HorizontalTextAlignment = TextAlignment.Start;
            }
        }

        class TitleLabel : Label
        {
            public TitleLabel()
            {
                FontSize = 24;
                FontAttributes = FontAttributes.Bold;
                Margin = new Thickness(0, 10, 0, 10);
                Text = PageTitleConstants.AddOpportunityPage;
            }
        }
    }
}

