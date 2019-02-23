using System;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesPage : BaseContentPage<OpportunitiesViewModel>
    {
        #region Constant Fields
        readonly RelativeLayout _mainLayout;
        readonly ListView _listView;
        #endregion

        #region Fields
        WelcomeView _welcomeView;
        #endregion

        #region Constructors
        public OpportunitiesPage()
        {
            ViewModel.OkButtonTapped += HandleWelcomeViewDisappearing;

            #region Create the ListView
            _listView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(OpportunitiesViewCell)),
                RowHeight = 75,
                IsPullToRefreshEnabled = true
            };
            _listView.ItemTapped += HandleListViewItemTapped;
            _listView.SetBinding(ListView.ItemsSourceProperty, nameof(ViewModel.ViewableOpportunitiesData));
            _listView.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.RefreshAllDataCommand));
            _listView.SetBinding(ListView.IsRefreshingProperty, nameof(ViewModel.IsListViewRefreshing));
            #endregion

            #region Initialize the Toolbar Add Button
            var addButtonToolBar = new ToolbarItem
            {
                Icon = "Add",
                AutomationId = AutomationIdConstants.AddOpportunityButton
            };
            addButtonToolBar.Clicked += HandleAddButtonClicked;
            ToolbarItems.Add(addButtonToolBar);
            #endregion

            #region Create Searchbar
            var searchBar = new SearchBar
            {
                AutomationId = AutomationIdConstants.OpportunitySearchBar
            };
            searchBar.SetBinding(SearchBar.TextProperty, nameof(ViewModel.SearchBarText));
            #endregion

            _mainLayout = new RelativeLayout();

            _mainLayout.Children.Add(searchBar,
                Constraint.Constant(0),
                Constraint.Constant(0),
                 Constraint.RelativeToParent(parent => parent.Width));
            _mainLayout.Children.Add(_listView,
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => getSearchBarHeight(parent)),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height - getSearchBarHeight(parent)));

            Title = PageTitleConstants.OpportunitiesPageTitle;

            NavigationPage.SetBackButtonTitle(this, "");

            Content = _mainLayout;

            DisplayWelcomeView();

            double getSearchBarHeight(RelativeLayout p) => searchBar.Measure(p.Width, p.Height).Request.Height;
        }

        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _listView.BeginRefresh();
        }

        void HandleListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (e.Item is OpportunityModel itemTapped)
                    await Navigation?.PushAsync(new OpportunityDetailsPage(itemTapped));

                _listView.SelectedItem = null;
            });
        }

        async void HandleAddButtonClicked(object sender, EventArgs e) =>
            await Navigation?.PushModalAsync(new NavigationPage(new AddOpportunityPage()));

        void HandleWelcomeViewDisappearing(object sender, EventArgs e) => _welcomeView?.HideView();

        void DisplayWelcomeView()
        {
            if (!Settings.ShouldShowWelcomeView)
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                _welcomeView = new WelcomeView();

                _mainLayout?.Children?.Add(_welcomeView,
                   Constraint.Constant(0),
                   Constraint.Constant(0));

                _welcomeView?.ShowView(true);
            });
        }
        #endregion
    }
}

