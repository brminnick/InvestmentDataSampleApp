using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesViewModel : BaseViewModel
    {
        readonly WeakEventManager _okButtonTappedEventManager = new WeakEventManager();
    
        bool _isListViewRefreshing;
        string _searchBarText;
        IList<OpportunityModel> _allOpportunitiesData, _viewableOpportunitiesData;
        ICommand _refreshDataCommand,  _okButtonTappedCommand, _filterTextEnteredCommand;

        public event EventHandler OkButtonTapped
        {
            add => _okButtonTappedEventManager.AddEventHandler(value);
            remove => _okButtonTappedEventManager.RemoveEventHandler(value);
        }

        public ICommand OkButtonTappedCommand => _okButtonTappedCommand ??
            (_okButtonTappedCommand = new Command(ExecuteOkButtonTapped));

        public ICommand FilterTextEnteredCommand => _filterTextEnteredCommand ??
            (_filterTextEnteredCommand = new Command<string>(ExecuteFilterTextEnteredCommand));

        public ICommand RefreshDataCommand => _refreshDataCommand ??
            (_refreshDataCommand = new AsyncCommand(ExecuteRefreshDataCommand));

        public string SearchBarText
        {
            get => _searchBarText;
            set => SetProperty(ref _searchBarText, value, () => FilterList(value));
        }

        public IList<OpportunityModel> AllOpportunitiesData
        {
            get => _allOpportunitiesData;
            set => SetProperty(ref _allOpportunitiesData, value, () => FilterList(SearchBarText));
        }

        public IList<OpportunityModel> ViewableOpportunitiesData
        {
            get => _viewableOpportunitiesData;
            set => SetProperty(ref _viewableOpportunitiesData, value);
        }

        public bool IsListViewRefreshing
        {
            get => _isListViewRefreshing;
            set => SetProperty(ref _isListViewRefreshing, value);
        }

        void FilterList(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                ViewableOpportunitiesData = AllOpportunitiesData;
            }
            else
            {
                var upperCaseFilter = filter.ToUpper();

                ViewableOpportunitiesData = AllOpportunitiesData.Where(x =>
                   (x?.Company?.ToUpper().Contains(upperCaseFilter) ?? false) ||
                   (x?.CreatedAt.ToString().ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.DBA?.ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.LeaseAmountAsCurrency?.ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.Owner?.ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.SalesStage.ToString()?.ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.Topic?.ToUpper()?.Contains(upperCaseFilter) ?? false)
                 ).ToList();
            }
        }

        async Task InitializeDataInDatabaseAsync(int numberOfOpportunityModelsToGenerate = 20)
        {
            for (int i = 0; i < numberOfOpportunityModelsToGenerate; i++)
            {
                var newOpportunity = new OpportunityModel();

                var rnd = new Random();
                var companyIndex = rnd.Next(50);
                var dbaIndex = rnd.Next(50);
                var leaseAmount = rnd.Next(1000000);
                var ownerIndex = rnd.Next(50);
                var dayIndex = rnd.Next(1, 28);
                var monthIndex = rnd.Next(1, 12);
                var yearIndex = rnd.Next(2000, 2015);
                var salesStage = (SalesStages)rnd.Next(2);

                newOpportunity.Topic = $"{i + 715003} / Investment Data Corp";
                newOpportunity.Company = $"{LoremIpsumConstants.LoremIpsum.Substring(companyIndex, 10)}";
                newOpportunity.DBA = $"{LoremIpsumConstants.LoremIpsum.Substring(dbaIndex, 10)}";
                newOpportunity.LeaseAmount = leaseAmount;
                newOpportunity.SalesStage = salesStage;
                newOpportunity.Owner = $"{LoremIpsumConstants.LoremIpsum.Substring(ownerIndex, 10)}";
                newOpportunity.CreatedAt = new DateTimeOffset(yearIndex, monthIndex, dayIndex, 0, 0, 0, default);

                await OpportunityModelDatabase.SaveOpportunity(newOpportunity).ConfigureAwait(false);
            }
        }

        async Task ExecuteRefreshDataCommand()
        {
            try
            {
                var opportunityModelsFromDatabase = await OpportunityModelDatabase.GetAllOpportunityData_OldestToNewest().ConfigureAwait(false);

                // If the database is empty, initialize the database with dummy data
                if (!opportunityModelsFromDatabase.Any())
                {
                    await InitializeDataInDatabaseAsync().ConfigureAwait(false);
                    opportunityModelsFromDatabase = await OpportunityModelDatabase.GetAllOpportunityData_OldestToNewest().ConfigureAwait(false);
                }

                AllOpportunitiesData = opportunityModelsFromDatabase;
            }
            finally
            {
                IsListViewRefreshing = false;
            }
        }

        void ExecuteFilterTextEnteredCommand(string filterText) => FilterList(filterText);

        void ExecuteOkButtonTapped()
        {
            OnOkButtonTapped();
            Settings.ShouldShowWelcomeView = false;
        }

        void OnOkButtonTapped() =>
             _okButtonTappedEventManager.HandleEvent(this, EventArgs.Empty, nameof(OkButtonTapped));
    }
}

