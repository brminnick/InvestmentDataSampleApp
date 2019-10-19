using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesViewModel : BaseViewModel
    {
        readonly WeakEventManager _okButtonTappedEventManager = new WeakEventManager();

        bool _isCollectionRefreshing;
        string _searchBarText = string.Empty;
        IReadOnlyList<OpportunityModel> _allOpportunitiesList = Enumerable.Empty<OpportunityModel>().ToList();
        ICommand? _refreshDataCommand, _okButtonTappedCommand, _filterTextEnteredCommand;

        public OpportunitiesViewModel()
        {
            BindingBase.EnableCollectionSynchronization(VisibleOpportunitiesCollection, null, ObservableCollectionCallback);
        }

        public event EventHandler OkButtonTapped
        {
            add => _okButtonTappedEventManager.AddEventHandler(value);
            remove => _okButtonTappedEventManager.RemoveEventHandler(value);
        }

        public ICommand OkButtonTappedCommand => _okButtonTappedCommand ??= new Command(ExecuteOkButtonTapped);
        public ICommand FilterTextEnteredCommand => _filterTextEnteredCommand ??= new Command<string>(ExecuteFilterTextEnteredCommand);
        public ICommand RefreshDataCommand => _refreshDataCommand ??= new AsyncCommand(ExecuteRefreshDataCommand);

        public ObservableCollection<OpportunityModel> VisibleOpportunitiesCollection { get; } = new ObservableCollection<OpportunityModel>();

        public string SearchBarText
        {
            get => _searchBarText;
            set => SetProperty(ref _searchBarText, value, () => FilterList(value));
        }

        public bool IsCollectionRefreshing
        {
            get => _isCollectionRefreshing;
            set => SetProperty(ref _isCollectionRefreshing, value);
        }

        void FilterList(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                VisibleOpportunitiesCollection.Clear();

                foreach (var opportunity in _allOpportunitiesList)
                    VisibleOpportunitiesCollection.Add(opportunity);
            }
            else
            {
                var upperCaseFilter = filter.ToUpper();

                var filteredOpportunitiesList = _allOpportunitiesList.Where(x =>
                   (x?.Company?.ToUpper().Contains(upperCaseFilter) ?? false) ||
                   (x?.CreatedAt.ToString().ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.DBA?.ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.LeaseAmountAsCurrency?.ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.Owner?.ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.SalesStage.ToString()?.ToUpper()?.Contains(upperCaseFilter) ?? false) ||
                   (x?.Topic?.ToUpper()?.Contains(upperCaseFilter) ?? false));

                VisibleOpportunitiesCollection.Clear();

                foreach (var opportunity in filteredOpportunitiesList)
                    VisibleOpportunitiesCollection.Add(opportunity);
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
                var opportunityModelsFromDatabase = await OpportunityModelDatabase.GetAllOpportunityData_OldestToNewest();

                // If the database is empty, initialize the database with dummy data
                if (!opportunityModelsFromDatabase.Any())
                {
                    await InitializeDataInDatabaseAsync();
                    opportunityModelsFromDatabase = await OpportunityModelDatabase.GetAllOpportunityData_OldestToNewest();
                }

                _allOpportunitiesList = opportunityModelsFromDatabase.ToList();

                foreach (var opportunity in _allOpportunitiesList)
                    VisibleOpportunitiesCollection.Add(opportunity);

            }
            finally
            {
                IsCollectionRefreshing = false;
            }
        }

        void ExecuteFilterTextEnteredCommand(string filterText) => FilterList(filterText);

        void ExecuteOkButtonTapped()
        {
            OnOkButtonTapped();
            Settings.ShouldShowWelcomeView = false;
        }

        void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            lock (collection)
            {
                accessMethod?.Invoke();
            }
        }

        void OnOkButtonTapped() =>
             _okButtonTappedEventManager.HandleEvent(this, EventArgs.Empty, nameof(OkButtonTapped));
    }
}

