using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InvestmentDataSampleApp
{
	public abstract class OpportunityModelDatabase : BaseDatabase
	{
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
				(x?.Topic?.ToLower()?.Contains(filterAsLowerCase) ?? false))?.ToList();
		}

		public static async Task<IList<OpportunityModel>> GetAllOpportunityDataAsync_OldestToNewest()
		{
			var databaseConnection = await GetDatabaseConnectionAsync();

			return await databaseConnection.Table<OpportunityModel>().OrderBy(x => x.Topic).Where(x => x.ID > 0).ToListAsync();
		}

		public static async Task<IList<OpportunityModel>> GetAllOpportunityDataAsync_NewestToOldest()
		{
			var databaseConnection = await GetDatabaseConnectionAsync();

			return await databaseConnection.Table<OpportunityModel>().OrderByDescending(x => x.Topic).Where(x => x.ID > 0).ToListAsync();
		}

		public static async Task<OpportunityModel> GetOpportunityByIDAsync(int id)
		{
			var databaseConnection = await GetDatabaseConnectionAsync();

			return await databaseConnection.Table<OpportunityModel>().Where(x => x.ID.Equals(id)).FirstOrDefaultAsync();
		}

		public static async Task<OpportunityModel> GetOpportunityByTopicAsync(string topic)
		{
			var databaseConnection = await GetDatabaseConnectionAsync();

			return await databaseConnection.Table<OpportunityModel>().Where(x => x.Topic.Equals(topic)).FirstOrDefaultAsync();
		}

		public static async Task<int> SaveOpportunityAsync(OpportunityModel opportunity)
		{
            var databaseConnection = await GetDatabaseConnectionAsync();
			
			var isOpportunityInDatabase = await GetOpportunityByTopicAsync(opportunity.Topic) != null;

			if (isOpportunityInDatabase)
				return await databaseConnection.UpdateAsync(opportunity);

			return await databaseConnection.InsertAsync(opportunity);
		}

		public static async Task<int> DeleteItemAsync(OpportunityModel opportunity)
		{
			var databaseConnection = await GetDatabaseConnectionAsync();

			return await databaseConnection.DeleteAsync(opportunity);
		}

		public static async Task<OpportunityModel> GetNewestOpportunityAsync()
		{
			var databaseConnection = await GetDatabaseConnectionAsync();

			return await databaseConnection.Table<OpportunityModel>().OrderByDescending(x => x.ID).FirstOrDefaultAsync();
		}

		public static async Task<int> GetNumberOfRowsAsync()
		{
            var databaseConnection = await GetDatabaseConnectionAsync();	

			return await databaseConnection.Table<OpportunityModel>().CountAsync();
		}

		
		#endregion
	}
}

