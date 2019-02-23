using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
	class OpportunityDetailsPage : ContentPage
	{
		#region Constant Fields
		const int _relativeLayoutSpacing = 5;
		#endregion

		#region Constructors
		public OpportunityDetailsPage(OpportunityModel opportunityModelSelected)
		{
			BindingContext = opportunityModelSelected;

			#region Create Topic Controls
			var topicLabel = new OpportunitiesDetailsTextLabel
			{
				Text = "Topic"
			};

			var topicEntry = new Label();
			topicEntry.SetBinding(Label.TextProperty, nameof(opportunityModelSelected.Topic));
			#endregion

			#region Create Company Controls
			var companyLabel = new OpportunitiesDetailsTextLabel
			{
				Text = "Company"
			};

			var companyEntry = new Label();
			companyEntry.SetBinding(Label.TextProperty, nameof(opportunityModelSelected.Company));
			#endregion

			#region Create DBA Controls
			var dbaLabel = new OpportunitiesDetailsTextLabel
			{
				Text = "DBA"
			};

			var dbaEntry = new Label();
			dbaEntry.SetBinding(Label.TextProperty, nameof(opportunityModelSelected.DBA));
			#endregion

			#region Create LeaseAmount Controls
			var leaseAmountLabel = new OpportunitiesDetailsTextLabel
			{
				Text = "Lease Amount"
			};

			var leaseAmountEntry = new Label();
			leaseAmountEntry.SetBinding(Label.TextProperty, nameof(opportunityModelSelected.LeaseAmount));
			#endregion

			#region Create Owner Controls
			var ownerLabel = new OpportunitiesDetailsTextLabel
			{
				Text = "Owner"
			};

			var ownerEntry = new Label();
			ownerEntry.SetBinding(Label.TextProperty, nameof(opportunityModelSelected.Owner));
			#endregion

			#region create the Relative Layout
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
				Constraint.RelativeToView(topicEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing)
		   	);
			mainLayout.Children.Add(companyEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView(companyLabel, (parent, view) => view.Y + view.Height),
				Constraint.RelativeToParent((parent) => parent.Width)
		   	);
			mainLayout.Children.Add(leaseAmountLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(companyEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing)
		   	);
			mainLayout.Children.Add(leaseAmountEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView(leaseAmountLabel, (parent, view) => view.Y + view.Height),
				Constraint.RelativeToParent((parent) => parent.Width)
		   	);
			mainLayout.Children.Add(ownerLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(leaseAmountEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing)
		  	);
			mainLayout.Children.Add(ownerEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView(ownerLabel, (parent, view) => view.Y + view.Height),
				Constraint.RelativeToParent((parent) => parent.Width)
		   	);
			mainLayout.Children.Add(dbaLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(ownerEntry, (parent, view) => view.Y + view.Height + _relativeLayoutSpacing)
		   	);
			mainLayout.Children.Add(dbaEntry,
				Constraint.Constant(0),
				Constraint.RelativeToView(dbaLabel, (parent, view) => view.Y + view.Height),
				Constraint.RelativeToParent((parent) => parent.Width)
		   	);
			#endregion

			Title = PageTitleConstants.OpportunityDetailPageTitle;

			var termsScrollView = new ScrollView
			{
				Content = mainLayout
			};

			Padding = new Thickness(10, 0, 10, 0);

			Content = termsScrollView;
		}
		#endregion
	}
}

