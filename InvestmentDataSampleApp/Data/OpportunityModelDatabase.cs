using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SQLite;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
	public static class OpportunityModelDatabase
	{
		#region Constant Fields
		static readonly SQLiteAsyncConnection _database = DependencyService.Get<ISQLite>().GetConnection();
		#endregion

		#region Fields
		static bool _isInitialized;
		#endregion

		#region Methods
		public static async Task<IList<OpportunityModel>> GetAllOpportunityDataAsync_OldestToNewest_Filter(string filter)
		{
			var filterAsLowerCase = filter.ToLower();
			var allOpportunityData = await GetAllOpportunityDataAsync_OldestToNewest();

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

		public static async Task<IList<OpportunityModel>> GetAllOpportunityDataAsync_OldestToNewest()
		{
			if (!_isInitialized)
				await Initialize();
			
			return await _database.Table<OpportunityModel>().OrderBy(x => x.Topic).Where(x => x.ID > 0).ToListAsync();
		}

		public static async Task<IList<OpportunityModel>> GetAllOpportunityDataAsync_NewestToOldest()
		{
			if (!_isInitialized)
				await Initialize();
			
			return await _database.Table<OpportunityModel>().OrderByDescending(x => x.Topic).Where(x => x.ID > 0).ToListAsync();
		}

		public static async Task<OpportunityModel> GetOpportunityByIDAsync(int id)
		{
			if (!_isInitialized)
				await Initialize();
			
			return await _database.Table<OpportunityModel>().Where(x => x.ID.Equals(id)).FirstOrDefaultAsync();
		}

		public static async Task<OpportunityModel> GetOpportunityByTopicAsync(string topic)
		{
			if (!_isInitialized)
				await Initialize();
			
			return await _database.Table<OpportunityModel>().Where(x => x.Topic.Equals(topic)).FirstOrDefaultAsync();
		}

		public static async Task<int> SaveOpportunityAsync(OpportunityModel opportunity)
		{
			if (!_isInitialized)
				await Initialize();
			
			var isOpportunityInDatabase = await GetOpportunityByTopicAsync(opportunity.Topic) != null;

			if (isOpportunityInDatabase)
			{
				return await _database.UpdateAsync(opportunity);
			}

			return await _database.InsertAsync(opportunity);
		}

		public static async Task<int> DeleteItemAsync(OpportunityModel opportunity)
		{
			if (!_isInitialized)
				await Initialize();
			
			return await _database.DeleteAsync(opportunity);
		}

		public static async Task<OpportunityModel> GetNewestOpportunityAsync()
		{
			if (!_isInitialized)
				await Initialize();
			
			return await _database.Table<OpportunityModel>().OrderByDescending(x => x.ID).FirstOrDefaultAsync();
		}

		public static async Task<int> GetNumberOfRowsAsync()
		{
			if (!_isInitialized)
				await Initialize();
			
			return await _database.Table<OpportunityModel>().CountAsync();
		}

		static async Task Initialize()
		{
			await _database.CreateTableAsync<OpportunityModel>();
			_isInitialized = true;
		}
		#endregion
	}
}

