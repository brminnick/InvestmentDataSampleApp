using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
	public class AddOpportunityPage : ContentPage
	{
		const string _saveToolBarItemText = "Save";
		const string _cancelToolBarItemText = "Cancel";

		const int relativeLayoutSpacing = 5;

		AddOpportunityViewModel _viewModel;

		public AddOpportunityPage()
		{
			_viewModel = new AddOpportunityViewModel();
			BindingContext = _viewModel;

			#region Create Topic Stack
			var topicLabel = new Label
			{
				Text = "Topic"
			};

			var topicEntry = new Entry
			{
				AutomationId = AutomationIdConstants.TopicEntry
			};
			topicEntry.SetBinding(Entry.TextProperty, "Topic");

			var topicStack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					topicLabel,
					topicEntry
				}
			};
			#endregion

			#region Create Company Stack
			var companyLabel = new Label
			{
				Text = "Company"
			};

			var companyEntry = new Entry
			{
				AutomationId = AutomationIdConstants.CompanyEntry
			};
			companyEntry.SetBinding(Entry.TextProperty, "Company");

			var companyStack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					companyLabel,
					companyEntry
				}
			};
			#endregion

			#region Create DBA Stack
			var dbaLabel = new Label
			{
				Text = "DBA"
			};

			var dbaEntry = new Entry
			{
				AutomationId = AutomationIdConstants.DBAEntry
			};
			dbaEntry.SetBinding(Entry.TextProperty, "DBA");

			var dbaStack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					dbaLabel,
					dbaEntry
				}
			};
			#endregion

			#region Create LeaseAmount Stack
			var leaseAmountLabel = new Label
			{
				Text = "Lease Amount"
			};

			var leaseAmountEntry = new Entry
			{
				AutomationId = AutomationIdConstants.LeaseAmountEntry,
				Keyboard = Keyboard.Numeric,
				Placeholder = "0"
			};
			leaseAmountEntry.SetBinding(Entry.TextProperty, "LeaseAmount");

			var leaseAmountStack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					leaseAmountLabel,
					leaseAmountEntry
				}
			};
			#endregion

			#region Create Owner Entry
			var ownerLabel = new Label
			{
				Text = "Owner"
			};

			var ownerEntry = new Entry
			{
				AutomationId = AutomationIdConstants.OwnerEntry
			};
			ownerEntry.SetBinding(Entry.TextProperty, "Owner");

			var ownerStack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					ownerLabel,
					ownerEntry
				}
			};
			#endregion

			#region create the TableView
			var mainLayout = new RelativeLayout();
			mainLayout.Children.Add(topicLabel,
								   Constraint.Constant(0),
								   Constraint.Constant(0)
								   );
			mainLayout.Children.Add(topicEntry,
									Constraint.Constant(0),
									Constraint.RelativeToView(topicLabel, (parent, view) => view.Y + view.Height),
									Constraint.RelativeToParent((parent) => parent.Width)
								   );
			mainLayout.Children.Add(companyLabel,
									Constraint.Constant(0),
									Constraint.RelativeToView(topicEntry, (parent, view) => view.Y + view.Height + relativeLayoutSpacing)
								   );
			mainLayout.Children.Add(companyEntry,
									Constraint.Constant(0),
									Constraint.RelativeToView(companyLabel, (parent, view) => view.Y + view.Height),
									Constraint.RelativeToParent((parent) => parent.Width)
								   );
			mainLayout.Children.Add(leaseAmountLabel,
									Constraint.Constant(0),
									Constraint.RelativeToView(companyEntry, (parent, view) => view.Y + view.Height + relativeLayoutSpacing)
								   );
			mainLayout.Children.Add(leaseAmountEntry,
									Constraint.Constant(0),
									Constraint.RelativeToView(leaseAmountLabel, (parent, view) => view.Y + view.Height),
									Constraint.RelativeToParent((parent) => parent.Width)
								   );
			mainLayout.Children.Add(ownerLabel,
									Constraint.Constant(0),
									Constraint.RelativeToView(leaseAmountEntry, (parent, view) => view.Y + view.Height + relativeLayoutSpacing)
								   );
			mainLayout.Children.Add(ownerEntry,
									Constraint.Constant(0),
									Constraint.RelativeToView(ownerLabel, (parent, view) => view.Y + view.Height),
									Constraint.RelativeToParent((parent) => parent.Width)
								   );
			mainLayout.Children.Add(dbaLabel,
									Constraint.Constant(0),
									Constraint.RelativeToView(ownerEntry, (parent, view) => view.Y + view.Height + relativeLayoutSpacing)
								   );
			mainLayout.Children.Add(dbaEntry,
									Constraint.Constant(0),
									Constraint.RelativeToView(dbaLabel, (parent, view) => view.Y + view.Height),
									Constraint.RelativeToParent((parent) => parent.Width)
								   );
			#endregion

			#region Create Save Button
			var saveButtonToolBar = new ToolbarItem
			{
				Text = _saveToolBarItemText,
				Priority = 0,
				AutomationId = AutomationIdConstants.SaveButton
			};
			saveButtonToolBar.SetBinding(ToolbarItem.CommandProperty, "SaveButtonTapped");
			ToolbarItems.Add(saveButtonToolBar);
			#endregion

			#region Create Cancel Button
			var cancelButtonToolBar = new ToolbarItem
			{
				Text = _cancelToolBarItemText,
				Priority = 1,
				AutomationId = AutomationIdConstants.CancelButton
			};
			cancelButtonToolBar.Clicked += HandleCancelButtonTapped;
			ToolbarItems.Add(cancelButtonToolBar);
			#endregion

			Title = "Add Opportunity";

			Padding = new Thickness(20, 5, 20, 0);

			Content = mainLayout;

			_viewModel.SaveError += HandleSaveError;

			_viewModel.SaveToDatabaseCompleted += HandleCancelButtonTapped;
		}

		public void HandleSaveError(object sender, EventArgs e)
		{
			var opportunityModel = sender as AddOpportunityViewModel;
			var blankFieldsString = "\n";

			if (opportunityModel.Topic == null || opportunityModel.Topic == "")
				blankFieldsString += "Topic\n";
			if (opportunityModel.Company == null || opportunityModel.Company == "")
				blankFieldsString += "Company\n";
			if (opportunityModel.LeaseAmount == 0)
				blankFieldsString += "Lease Amount\n";
			if (opportunityModel.Owner == null || opportunityModel.Owner == "")
				blankFieldsString += "Owner\n";
			if (opportunityModel.DBA == null || opportunityModel.DBA == "")
				blankFieldsString += "DBA";

			DisplayAlert("Error: Missing Data", $"The following fields are empty: {blankFieldsString}", "OK");
		}

		public async Task PopModalAsync(bool isAnimated)
		{
			await Navigation.PopModalAsync(isAnimated);

			while (ToolbarItems.Count > 0)
			{
				if (string.Equals(ToolbarItems[0]?.Text, _cancelToolBarItemText))
					ToolbarItems[0].Clicked -= HandleCancelButtonTapped;

				ToolbarItems.RemoveAt(0);
			}

			_viewModel.SaveToDatabaseCompleted -= HandleCancelButtonTapped;
			_viewModel.SaveError -= HandleSaveError;
			_viewModel = null;
		}

		async void HandleCancelButtonTapped(object sender, EventArgs e)
		{
			await PopModalAsync(true);
		}
	}
}

