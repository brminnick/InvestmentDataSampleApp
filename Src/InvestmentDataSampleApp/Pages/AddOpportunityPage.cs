using System;
using System.Text;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
    public class AddOpportunityPage : BaseContentPage<AddOpportunityViewModel>
    {
        const string _saveToolBarItemText = "Save";
        const string _cancelToolBarItemText = "Cancel";
        const int _relativeLayoutSpacing = 5;
        readonly ToolbarItem _cancelButtonToolBarItem;
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


            var mainLayout = new RelativeLayout();
            mainLayout.Children.Add(topicLabel,
                   Constraint.Constant(0),
                   Constraint.Constant(0));
            mainLayout.Children.Add(_topicEntry,
                Constraint.Constant(0),
                Constraint.RelativeToView(topicLabel, (parent, view) => view.Y + view.Height),
                Constraint.RelativeToParent((parent) => parent.Width));
            mainLayout.Children.Add(companyLabel,
                Constraint.Constant(0),
                Constraint.RelativeToView(_topicEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing));
            mainLayout.Children.Add(_companyEntry,
                Constraint.Constant(0),
                Constraint.RelativeToView(companyLabel, (parent, view) => view.Y + view.Height),
                Constraint.RelativeToParent((parent) => parent.Width));
            mainLayout.Children.Add(leaseAmountLabel,
                Constraint.Constant(0),
                Constraint.RelativeToView(_companyEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing));
            mainLayout.Children.Add(_leaseAmountEntry,
                Constraint.Constant(0),
                Constraint.RelativeToView(leaseAmountLabel, (parent, view) => view.Y + view.Height),
                Constraint.RelativeToParent((parent) => parent.Width));
            mainLayout.Children.Add(ownerLabel,
                Constraint.Constant(0),
                Constraint.RelativeToView(_leaseAmountEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing));
            mainLayout.Children.Add(_ownerEntry,
                Constraint.Constant(0),
                Constraint.RelativeToView(ownerLabel, (parent, view) => view.Y + view.Height),
                Constraint.RelativeToParent((parent) => parent.Width));
            mainLayout.Children.Add(dbaLabel,
                Constraint.Constant(0),
                Constraint.RelativeToView(_ownerEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing));
            mainLayout.Children.Add(_dbaEntry,
                Constraint.Constant(0),
                Constraint.RelativeToView(dbaLabel, (parent, view) => view.Y + view.Height),
                Constraint.RelativeToParent((parent) => parent.Width));


            var saveButtonToolBar = new ToolbarItem
            {
                Text = _saveToolBarItemText,
                Priority = 0,
                AutomationId = AutomationIdConstants.SaveButton,
            };
            saveButtonToolBar.SetBinding(ToolbarItem.CommandProperty, nameof(AddOpportunityViewModel.SaveButtonTapped));
            ToolbarItems.Add(saveButtonToolBar);


            _cancelButtonToolBarItem = new ToolbarItem
            {
                Text = _cancelToolBarItemText,
                Priority = 1,
                AutomationId = AutomationIdConstants.CancelButton
            };
            _cancelButtonToolBarItem.Clicked += HandleCancelButtonTapped;
            ToolbarItems.Add(_cancelButtonToolBarItem);

            Title = PageTitleConstants.AddOpportunityPage;

            Padding = new Thickness(20, 10, 20, 0);

            Content = mainLayout;
        }


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
                ReturnType = returnType;
                AutomationId = automationId;
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing;
            }
        }

        class AddOpportunityLabel : Label
        {
            public AddOpportunityLabel(string text) => Text = text;
        }
    }
}

