using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using InvestmentDataSampleApp.Shared;
using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesPage : BaseContentPage<OpportunitiesViewModel>, ISearchPage
    {
        readonly WeakEventManager<string> _searchBarChangedEventManager = new WeakEventManager<string>();

        readonly RefreshView _refreshView;
        readonly RelativeLayout _mainLayout;

        readonly WelcomeView _welcomeView = new WelcomeView();

        public OpportunitiesPage()
        {
            SearchBarTextChanged += HandleSearchBarTextChanged;
            ViewModel.OkButtonTapped += HandleWelcomeViewDisappearing;

            var collectionView = new CollectionView
            {
                ItemTemplate = new OpportunitiesDataTemplate(),
                SelectionMode = SelectionMode.Single,
            };
            collectionView.SelectionChanged += HandleSelectionChanged;
            collectionView.SetBinding(CollectionView.ItemsSourceProperty, nameof(OpportunitiesViewModel.VisibleOpportunitiesCollection));

            _refreshView = new RefreshView { Content = collectionView };
            _refreshView.SetBinding(RefreshView.CommandProperty, nameof(OpportunitiesViewModel.RefreshDataCommand));
            _refreshView.SetBinding(RefreshView.IsRefreshingProperty, nameof(OpportunitiesViewModel.IsCollectionRefreshing));

            var addButtonToolBar = new ToolbarItem
            {
                IconImageSource = "Add",
                Text = "Add",
                AutomationId = AutomationIdConstants.AddOpportunityButton,
                Order = Device.RuntimePlatform is Device.Android ? ToolbarItemOrder.Secondary : ToolbarItemOrder.Default
            };
            addButtonToolBar.Clicked += HandleAddButtonClicked;
            ToolbarItems.Add(addButtonToolBar);

            _mainLayout = new RelativeLayout();

            _mainLayout.Children.Add(_refreshView,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height));

            Title = PageTitleConstants.OpportunitiesPage;

            NavigationPage.SetBackButtonTitle(this, "");

            Content = _mainLayout;
        }

        public event EventHandler<string> SearchBarTextChanged
        {
            add => _searchBarChangedEventManager.AddEventHandler(value);
            remove => _searchBarChangedEventManager.RemoveEventHandler(value);
        }

        public void OnSearchBarTextChanged(in string text) =>
            _searchBarChangedEventManager.HandleEvent(this, text, nameof(SearchBarTextChanged));

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await DisplayWelcomeView();

            if (_refreshView.Content is CollectionView collectionView
                && collectionView.ItemsSource is ICollection<OpportunityModel> collectionItemSource
                && !collectionItemSource.Any())
            {
                _refreshView.IsRefreshing = true;
            }
        }

        void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (e.CurrentSelection.FirstOrDefault() is OpportunityModel itemTapped)
                    await Navigation.PushAsync(new OpportunityDetailsPage(itemTapped));

                if (sender is CollectionView collectionView)
                    collectionView.SelectedItem = null;
            });
        }

        async void HandleAddButtonClicked(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform is Device.iOS)
                await Navigation.PushModalAsync(new AddOpportunityPage());
            else
                await Navigation.PushModalAsync(new NavigationPage(new AddOpportunityPage()));
        }


        async void HandleWelcomeViewDisappearing(object sender, EventArgs e)
        {
            await _welcomeView.HideView();

            await Device.InvokeOnMainThreadAsync(() =>
            {
                if (_mainLayout.Children.Contains(_welcomeView))
                    _mainLayout.Children.Remove(_welcomeView);
            });
        }

        async Task DisplayWelcomeView()
        {
            if (Settings.ShouldShowWelcomeView)
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    _mainLayout.Children.Add(_welcomeView,
                       Constraint.Constant(0),
                       Constraint.Constant(0));

                    return _welcomeView.ShowView();
                });
            }
        }

        void HandleSearchBarTextChanged(object sender, string text) => ViewModel.FilterTextCommand.Execute(text);
    }
}

