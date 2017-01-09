using System;
using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
	public class OpportunitiesPage : BaseContentPage<OpportunitiesViewModel>
	{
		#region Constant Fields
		readonly RelativeLayout _mainLayout;
		readonly SearchBar _searchBar;
		#endregion

		#region Fields
		ListView _listView;
		ToolbarItem _addButtonToolBar;
		WelcomeView _welcomeView;
		#endregion

		#region Constructors
		public OpportunitiesPage()
		{
			#region Create the ListView
			_listView = new ListView
			{
				ItemTemplate = new DataTemplate(typeof(OpportunitiesViewCell)),
				RowHeight = 75
			};
			_listView.IsPullToRefreshEnabled = true;
			_listView.SetBinding<OpportunitiesViewModel>(ListView.ItemsSourceProperty, vm => vm.ViewableOpportunitiesData);
			#endregion

			#region Initialize the Toolbar Add Button
			_addButtonToolBar = new ToolbarItem
			{
				Icon = "Add",
				AutomationId = AutomationIdConstants.AddOpportunityButton
			};
			ToolbarItems.Add(_addButtonToolBar);
			#endregion

			#region Create Searchbar
			_searchBar = new SearchBar
			{
				AutomationId = AutomationIdConstants.OpportunitySearchBar
			};
			_searchBar.SetBinding<OpportunitiesViewModel>(SearchBar.TextProperty, vm => vm.SearchBarText);
			#endregion

			#region Create Stack
			var listSearchStack = new StackLayout
			{
				Padding = 0,
				Spacing = 0,
				Children = {
					_searchBar,
					_listView
				}
			};
			#endregion

			_mainLayout = new RelativeLayout();
			_mainLayout.Children.Add(listSearchStack,
				Constraint.Constant(0),
				Constraint.Constant(0),
			 	Constraint.RelativeToParent(parent => parent.Width),
			 	Constraint.RelativeToParent(parent => parent.Height)
			);

			Title = PageTitleConstants.OpportunitiesPageTitle;

			Content = _mainLayout;

			NavigationPage.SetBackButtonTitle(this, "");

			DisplayWelcomeView();
		}

		#endregion

		#region Methods
		protected override void OnAppearing()
		{
			base.OnAppearing();

			ViewModel?.RefreshAllDataCommand?.Execute(false);
		}

		protected override void SubscribeEventHandlers()
		{
			if (AreEventHandlersSubscribed)
				return;

			ViewModel.PullToRefreshDataCompleted += HandlePullToRefreshDataCompleted;
			ViewModel.OkButtonTappedEvent += HandleWelcomeViewDisappearing;
			_listView.Refreshing += HandleListViewRefreshing;
			_listView.ItemSelected += HandleListViewItemSelected;
			_addButtonToolBar.Clicked += HandleAddButtonClicked;

			AreEventHandlersSubscribed = true;
		}

		protected override void UnsubscribeEventHandlers()
		{
			ViewModel.PullToRefreshDataCompleted -= HandlePullToRefreshDataCompleted;
			ViewModel.OkButtonTappedEvent -= HandleWelcomeViewDisappearing;
			_listView.Refreshing -= HandleListViewRefreshing;
			_listView.ItemSelected -= HandleListViewItemSelected;
			_addButtonToolBar.Clicked -= HandleAddButtonClicked;

			AreEventHandlersSubscribed = false;
		}

		void HandleListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var itemSelected = e?.SelectedItem as OpportunityModel;

			Device.BeginInvokeOnMainThread(async () => await Navigation?.PushAsync(new OpportunityDetailPage(itemSelected)));
		}

		void HandlePullToRefreshDataCompleted(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				_listView.IsRefreshing = false;
				_listView.SelectedItem = null;
			});
		}

		void HandleListViewRefreshing(object sender, EventArgs e)
		{
			ViewModel?.RefreshAllDataCommand?.Execute(true);
		}

		async void HandleAddButtonClicked(object sender, EventArgs e)
		{
			await Navigation?.PushModalAsync(new NavigationPage(new AddOpportunityPage()));
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
			if (!(Settings.ShouldShowWelcomeView))
				return;

			Device.BeginInvokeOnMainThread(() =>
			{
				_welcomeView = new WelcomeView();

				_mainLayout?.Children?.Add(_welcomeView,
				   Constraint.Constant(0),
				   Constraint.Constant(0)
				);

				_welcomeView?.DisplayView();
			});
		}
		#endregion
	}
}

