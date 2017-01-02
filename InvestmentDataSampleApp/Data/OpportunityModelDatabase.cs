using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SQLite;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
	public class OpportunityModelDatabase
	{
		#region Constant Fields
		readonly static object _locker = new object();
		readonly SQLiteConnection _database;
		#endregion

		#region Constructors
		public OpportunityModelDatabase()
		{
			_database = DependencyService.Get<ISQLite>().GetConnection();
			_database.CreateTable<OpportunityModel>();
		}
		#endregion

		#region Methods
		public async Task<IList<OpportunityModel>> GetAllOpportunityDataAsync_OldestToNewest_Filter(string filter)
		{
			var filterAsLowerCase = filter.ToLower();
			var allOpportunityData = await GetAllOpportunityDataAsync_OldestToNewest();

			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return allOpportunityData.Where(x =>
						(x?.Company?.ToLower()?.Contains(filterAsLowerCase) ?? false) ||
						(x?.DateCreated.ToString()?.ToLower()?.Contains(filterAsLowerCase) ?? false) ||
						(x?.DBA?.ToLower()?.Contains(filterAsLowerCase) ?? false) ||
						(x?.LeaseAmountAsCurrency?.ToLower()?.Contains(filterAsLowerCase) ?? false) ||
						(x?.Owner?.ToLower()?.Contains(filterAsLowerCase) ?? false) ||
						(x?.SalesStage.ToString()?.ToLower()?.Contains(filterAsLowerCase) ?? false) ||
						(x?.Topic?.ToLower()?.Contains(filterAsLowerCase) ?? false)
						)?.ToList();
				}
			});
		}

		public async Task<IList<OpportunityModel>> GetAllOpportunityDataAsync_OldestToNewest()
		{
			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return _database.Table<OpportunityModel>().OrderBy(x => x.Topic).Where(x => x.ID > 0).ToList();
				}
			});
		}

		public async Task<IList<OpportunityModel>> GetAllOpportunityDataAsync_NewestToOldest()
		{
			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return _database.Table<OpportunityModel>().OrderByDescending(x => x.Topic).Where(x => x.ID > 0).ToList();
				}
			});
		}

		public async Task<OpportunityModel> GetOpportunityByIDAsync(int id)
		{
			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return _database.Table<OpportunityModel>().FirstOrDefault(x => x.ID == id);
				}
			});
		}

		public async Task<OpportunityModel> GetOpportunityByTopicAsync(string topic)
		{
			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return _database.Table<OpportunityModel>().FirstOrDefault(x => x.Topic == topic);
				}
			});
		}

		public async Task<int> SaveOpportunityAsync(OpportunityModel opportunity)
		{
			var isOpportunityInDatabase = await GetOpportunityByTopicAsync(opportunity.Topic) != null;

			return await Task.Run(() =>
			{
				if (isOpportunityInDatabase)
				{
					lock (_locker)
					{
						_database.Update(opportunity);
					}
					return opportunity.ID;
				}

				lock (_locker)
				{
					return _database.Insert(opportunity);
				}
			});
		}

		public async Task<int> DeleteItemAsync(int id)
		{
			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return _database.Delete<OpportunityModel>(id);
				}
			});
		}

		public async Task<OpportunityModel> GetNewestOpportunityAsync()
		{
			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return _database.Table<OpportunityModel>().OrderByDescending(x => x.ID).FirstOrDefault();
				}
			});
		}

		public async Task<int> GetNumberOfRowsAsync()
		{
			return await Task.Run(() =>
			{
				return _database.Table<OpportunityModel>().Count();
			});
		}
		#endregion
	}
}

