using System;
using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
	public class OpportunitiesPage : ContentPage
	{
		ListView _listView;
		OpportunitiesViewModel _opportunitiesViewModel;
		ToolbarItem _addButtonToolBar;
		bool _areEventHandlersSubscribed;

		RelativeLayout _mainLayout;

		WelcomeView _welcomeView;

		public OpportunitiesPage()
		{
			_opportunitiesViewModel = new OpportunitiesViewModel();
			BindingContext = _opportunitiesViewModel;

			#region Create the ListView
			_listView = new ListView()
			{
				ItemTemplate = new DataTemplate(typeof(OpportunitiesViewCell)),
				RowHeight = 75
			};

			_listView.IsPullToRefreshEnabled = true;
			_listView.Refreshing += (async (sender, e) =>
			{
				await _opportunitiesViewModel.RefreshOpportunitiesDataAsync();
				_listView.EndRefresh();
			});

			_listView.ItemSelected += (sender, e) =>
			{
				Navigation.PushAsync(new CreditBuilderCarouselPage());
			};

			_listView.SetBinding(ListView.ItemsSourceProperty, "AllOpportunitiesData");
			#endregion

			Title = $"Opportunities";

			#region Initialize the Toolbar Add Button
			_addButtonToolBar = new ToolbarItem();
			_addButtonToolBar.Icon = "Add";
			_addButtonToolBar.AutomationId = AutomationIdConstants.AddOpportunityButton;

			ToolbarItems.Add(_addButtonToolBar);
			#endregion

			#region Create Searchbar
			var searchBar = new SearchBar();
			searchBar.AutomationId = AutomationIdConstants.OpportunitySearchBar;
			searchBar.TextChanged += (sender, e) => _opportunitiesViewModel.FilterLocations(searchBar.Text);
			#endregion

			#region Create Stack
			var listSearchStack = new StackLayout
			{
				Padding = 0,
				Spacing = 0,
				Children = {
					searchBar,
					_listView
				}
			};
			#endregion

			_mainLayout = new RelativeLayout();
			_mainLayout.Children.Add(listSearchStack,
				Constraint.Constant(0),
				Constraint.Constant(0)
			);

			SubscribeEventHandlers();

			Content = _mainLayout;

			DisplayWelcomeView();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			await _opportunitiesViewModel.RefreshOpportunitiesDataAsync();
			SubscribeEventHandlers();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			UnsubscribeEventHandlers();
		}

		void SubscribeEventHandlers()
		{
			if (_areEventHandlersSubscribed)
				return;

			_opportunitiesViewModel.OkButtonTappedEvent += HandleWelcomeViewDisappearing;
			_addButtonToolBar.Clicked += HandleAddButtonClicked;
			_areEventHandlersSubscribed = true;

		}

		void UnsubscribeEventHandlers()
		{
			_opportunitiesViewModel.OkButtonTappedEvent -= HandleWelcomeViewDisappearing;
			_addButtonToolBar.Clicked -= HandleAddButtonClicked;
			_areEventHandlersSubscribed = false;
		}

		void HandleAddButtonClicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new NavigationPage(new AddOpportunityPage()));
		}

		void HandleWelcomeViewDisappearing(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await _welcomeView?.FadeTo(0);
				_welcomeView.IsVisible = false;
				_welcomeView.InputTransparent = true;
			});
		}

		void DisplayWelcomeView()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				_welcomeView = new WelcomeView();

				_mainLayout.Children.Add(_welcomeView,
				   Constraint.Constant(0),
				   Constraint.Constant(0)
				);

				_welcomeView.DisplayView();

			});
		}
	}
}

