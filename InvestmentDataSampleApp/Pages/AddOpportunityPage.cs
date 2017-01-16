using System;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
	public class AddOpportunityPage : BaseContentPage<AddOpportunityViewModel>
	{
		#region Constant Fields
		const string _saveToolBarItemText = "Save";
		const string _cancelToolBarItemText = "Cancel";
		const int _relativeLayoutSpacing = 5;
		readonly ToolbarItem _cancelButtonToolBarItem;
		readonly EntryWithCustomKeyboardReturnButton _topicEntry, _companyEntry, _dbaEntry, _leaseAmountEntry, _ownerEntry;
		#endregion

		#region Constructors
		public AddOpportunityPage()
		{
			#region Create Topic Controls
			var topicLabel = new Label
			{
				Text = "Topic"
			};

			_topicEntry = new EntryWithCustomKeyboardReturnButton
			{
				ReturnType = ReturnType.Next,
				AutomationId = AutomationIdConstants.TopicEntry
			};
			_topicEntry.SetBinding<AddOpportunityViewModel>(Entry.TextProperty, vm => vm.Topic);
			#endregion

			#region Create Company Controls
			var companyLabel = new Label
			{
				Text = "Company"
			};

			_companyEntry = new EntryWithCustomKeyboardReturnButton
			{
				ReturnType = ReturnType.Next,
				AutomationId = AutomationIdConstants.CompanyEntry
			};
			_companyEntry.SetBinding<AddOpportunityViewModel>(Entry.TextProperty, vm => vm.Company);
			#endregion

			#region Create DBA Controls
			var dbaLabel = new Label
			{
				Text = "DBA"
			};

			_dbaEntry = new EntryWithCustomKeyboardReturnButton
			{
				ReturnType = ReturnType.Done,
				AutomationId = AutomationIdConstants.DBAEntry
			};
			_dbaEntry.SetBinding<AddOpportunityViewModel>(Entry.TextProperty, vm => vm.DBA);
			#endregion

			#region Create LeaseAmount Controls
			var leaseAmountLabel = new Label
			{
				Text = "Lease Amount"
			};

			_leaseAmountEntry = new EntryWithCustomKeyboardReturnButton
			{
				ReturnType = ReturnType.Next,
				AutomationId = AutomationIdConstants.LeaseAmountEntry,
				Keyboard = Keyboard.Numeric,
				Placeholder = "0"
			};
			_leaseAmountEntry.SetBinding<AddOpportunityViewModel>(Entry.TextProperty, vm => vm.LeaseAmount);
			#endregion

			#region Create Owner Controls
			var ownerLabel = new Label
			{
				Text = "Owner"
			};

			_ownerEntry = new EntryWithCustomKeyboardReturnButton
			{
				ReturnType = ReturnType.Next,
				AutomationId = AutomationIdConstants.OwnerEntry
			};
			_ownerEntry.SetBinding<AddOpportunityViewModel>(Entry.TextProperty, vm => vm.Owner);
			#endregion

			#region create the Relative Layout
			var mainLayout = new RelativeLayout();
			mainLayout.Children.Add(topicLabel,
			   	Constraint.Constant(0),
			   	Constraint.Constant(0)
		   	);
			mainLayout.Children.Add(_topicEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView(topicLabel, (parent, view) => view.Y + view.Height),
				Constraint.RelativeToParent((parent) => parent.Width)
		   	);
			mainLayout.Children.Add(companyLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(_topicEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing)
		   	);
			mainLayout.Children.Add(_companyEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView(companyLabel, (parent, view) => view.Y + view.Height),
				Constraint.RelativeToParent((parent) => parent.Width)
		   	);
			mainLayout.Children.Add(leaseAmountLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(_companyEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing)
		   	);
			mainLayout.Children.Add(_leaseAmountEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView(leaseAmountLabel, (parent, view) => view.Y + view.Height),
				Constraint.RelativeToParent((parent) => parent.Width)
		   	);
			mainLayout.Children.Add(ownerLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(_leaseAmountEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing)
		  	);
			mainLayout.Children.Add(_ownerEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView(ownerLabel, (parent, view) => view.Y + view.Height),
				Constraint.RelativeToParent((parent) => parent.Width)
		   	);
			mainLayout.Children.Add(dbaLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(_ownerEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing)
		   	);
			mainLayout.Children.Add(_dbaEntry,
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
			saveButtonToolBar.SetBinding<AddOpportunityViewModel>(ToolbarItem.CommandProperty, vm => vm.SaveButtonTapped);
			ToolbarItems.Add(saveButtonToolBar);
			#endregion

			#region Create Cancel Button
			_cancelButtonToolBarItem = new ToolbarItem
			{
				Text = _cancelToolBarItemText,
				Priority = 1,
				AutomationId = AutomationIdConstants.CancelButton
			};
			ToolbarItems.Add(_cancelButtonToolBarItem);
			#endregion

			Title = PageTitleConstants.AddOpportunityPageTitle;

			Padding = new Thickness(20, 10, 20, 0);

			Content = mainLayout;
		}
		#endregion

		#region Methods
		protected override void SubscribeEventHandlers()
		{
			if (AreEventHandlersSubscribed)
				return;

			_dbaEntry.Completed += HandleDbaEntryCompleted;
			_topicEntry.Completed += HandleTopicEntryCompleted;
			_ownerEntry.Completed += HandleOwnerEntryCompleted;
			_companyEntry.Completed += HandleCompanyEntryCompleted;
			_leaseAmountEntry.Completed += HandleLeaseAmountEntryCompleted;

			_cancelButtonToolBarItem.Clicked += HandleCancelButtonTapped;
			ViewModel.SaveError += HandleSaveError;
			ViewModel.SaveToDatabaseCompleted += HandleCancelButtonTapped;
			
			AreEventHandlersSubscribed = true;
		}

		protected override void UnsubscribeEventHandlers()
		{
			_cancelButtonToolBarItem.Clicked -= HandleCancelButtonTapped;
			ViewModel.SaveError -= HandleSaveError;
			ViewModel.SaveToDatabaseCompleted -= HandleCancelButtonTapped;
			
			AreEventHandlersSubscribed = false;
		}
		void HandleSaveError(object sender, EventArgs e)
		{
			var opportunityModel = sender as AddOpportunityViewModel;
			var blankFieldsString = new StringBuilder();
			blankFieldsString.AppendLine();

			if (string.IsNullOrEmpty(opportunityModel?.Topic))
				blankFieldsString.AppendLine("Topic");
			if (string.IsNullOrEmpty(opportunityModel?.Company))
				blankFieldsString.AppendLine("Company");
			if (opportunityModel?.LeaseAmount == 0)
				blankFieldsString.AppendLine("Lease Amount");
			if (string.IsNullOrEmpty(opportunityModel?.Owner))
				blankFieldsString.AppendLine("Owner");
			if (string.IsNullOrEmpty(opportunityModel?.DBA))
				blankFieldsString.Append("DBA");

			Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error: Missing Data", $"The following fields are empty: {blankFieldsString}", "OK"));
		}

		void HandleTopicEntryCompleted(object sender, EventArgs e)
		{
			_companyEntry.Focus();
		}

		void HandleCompanyEntryCompleted(object sender, EventArgs e)
		{
			_leaseAmountEntry.Focus();
		}

		void HandleLeaseAmountEntryCompleted(object sender, EventArgs e)
		{
			_ownerEntry.Focus();
		}

		void HandleOwnerEntryCompleted(object sender, EventArgs e)
		{
			_dbaEntry.Focus();
		}

		void HandleDbaEntryCompleted(object sender, EventArgs e)
		{
			ViewModel?.SaveButtonTapped?.Execute(null);
		}

		async void HandleCancelButtonTapped(object sender, EventArgs e)
		{
			await PopModalAsync(true);
		}

		async Task PopModalAsync(bool isAnimated)
		{
			await Navigation.PopModalAsync(isAnimated);
		}

		#endregion
	}
}

