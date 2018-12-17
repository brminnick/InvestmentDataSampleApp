using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesViewModel : BaseViewModel
    {
        #region Constant Fields
        readonly WeakEventManager _okButtonTappedEventManager = new WeakEventManager();
        #endregion

        #region Fields
        bool _isListViewRefreshing;
        string _searchBarText;
        IList<OpportunityModel> _allOpportunitiesData, _viewableOpportunitiesData;
        ICommand _refreshAllDataCommand, _okButtonTappedCommand, _filterTextEnteredCommand;
        #endregion

        #region Constructors
        public OpportunitiesViewModel()
        {
            Task.Run(async () =>
            {
                // If the database is empty, initialize the database with dummy data
                if (await OpportunityModelDatabase.GetNumberOfRowsAsync().ConfigureAwait(false) < 20)
                    await InitializeDataInDatabaseAsync().ConfigureAwait(false);

                await RefreshOpportunitiesDataAsync().ConfigureAwait(false);
            });
        }
        #endregion

        #region Events
        public event EventHandler OkButtonTapped
        {
            add => _okButtonTappedEventManager.AddEventHandler(value);
            remove => _okButtonTappedEventManager.RemoveEventHandler(value);
        }
        #endregion

        #region Properties
        public ICommand OkButtonTappedCommand => _okButtonTappedCommand ??
            (_okButtonTappedCommand = new Command(ExecuteOkButtonTapped));

        public ICommand FilterTextEnteredCommand => _filterTextEnteredCommand ??
            (_filterTextEnteredCommand = new Command<string>(ExecuteFilterTextEnteredCommand));

        public ICommand RefreshAllDataCommand => _refreshAllDataCommand ??
            (_refreshAllDataCommand = new AsyncCommand(ExecuteRefreshAllDataCommand, continueOnCapturedContext: false));

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
        #endregion

        #region Methods
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

                var salesStageNumber = rnd.Next(2);
                SalesStages salesStage;
                switch (salesStageNumber)
                {
                    case 0:
                        salesStage = SalesStages.New;
                        break;
                    case 1:
                        salesStage = SalesStages.Pending;
                        break;
                    default:
                        salesStage = SalesStages.Closed;
                        break;
                }

                newOpportunity.Topic = $"{i + 715003} / Investment Data Corp";
                newOpportunity.Company = $"{LoremIpsumConstants.LoremIpsum.Substring(companyIndex, 10)}";
                newOpportunity.DBA = $"{LoremIpsumConstants.LoremIpsum.Substring(dbaIndex, 10)}";
                newOpportunity.LeaseAmount = leaseAmount;
                newOpportunity.SalesStage = salesStage;
                newOpportunity.Owner = $"{LoremIpsumConstants.LoremIpsum.Substring(ownerIndex, 10)}";
                newOpportunity.CreatedAt = new DateTimeOffset(yearIndex, monthIndex, dayIndex, 0, 0, 0, default(TimeSpan));

                await OpportunityModelDatabase.SaveOpportunityAsync(newOpportunity).ConfigureAwait(false);
            }
        }

        async Task ExecuteRefreshAllDataCommand()
        {
            IsListViewRefreshing = true;

            try
            {
                var minimumRefreshTimeTask = Task.Delay(1000);

                await RefreshOpportunitiesDataAsync().ConfigureAwait(false);

                await minimumRefreshTimeTask;
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

        async Task RefreshOpportunitiesDataAsync() =>
            AllOpportunitiesData = await OpportunityModelDatabase.GetAllOpportunityDataAsync_OldestToNewest().ConfigureAwait(false);

        void OnOkButtonTapped() =>
             _okButtonTappedEventManager?.HandleEvent(this, EventArgs.Empty, nameof(OkButtonTapped));
        #endregion
    }
}

