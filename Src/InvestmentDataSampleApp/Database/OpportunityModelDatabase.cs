using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InvestmentDataSampleApp
{
    public abstract class OpportunityModelDatabase : BaseDatabase
    {
        public static async Task<IList<OpportunityModel>> GetAllOpportunityData_OldestToNewest_Filter(string filter)
        {
            var filterAsLowerCase = filter.ToLower();
            var allOpportunityData = await GetAllOpportunityData_OldestToNewest().ConfigureAwait(false);

            return allOpportunityData.Where(x => x != null &&
                (x.Company.ToLower().Contains(filterAsLowerCase) ||
                    x.CreatedAt.ToString().ToLower().Contains(filterAsLowerCase) ||
                    x.DBA.ToLower().Contains(filterAsLowerCase) ||
                    x.LeaseAmountAsCurrency.ToLower().Contains(filterAsLowerCase) ||
                    x.Owner.ToLower().Contains(filterAsLowerCase) ||
                    x.SalesStage.ToString().ToLower().Contains(filterAsLowerCase) ||
                    x.Topic.ToLower().Contains(filterAsLowerCase))).ToList();
        }

        public static async Task<IList<OpportunityModel>> GetAllOpportunityData_OldestToNewest()
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await ExecuteDatabaseFunction(() => databaseConnection.Table<OpportunityModel>().Where(x => x.ID > 0).OrderBy(x => x.Topic).ToListAsync()).ConfigureAwait(false);
        }

        public static async Task<IList<OpportunityModel>> GetAllOpportunityData_NewestToOldest()
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await ExecuteDatabaseFunction(() => databaseConnection.Table<OpportunityModel>().Where(x => x.ID > 0).OrderByDescending(x => x.Topic).ToListAsync()).ConfigureAwait(false);
        }

        public static async Task<OpportunityModel> GetOpportunityByID(int id)
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await ExecuteDatabaseFunction(() => databaseConnection.Table<OpportunityModel>().FirstOrDefaultAsync(x => x.ID.Equals(id))).ConfigureAwait(false);
        }

        public static async Task<OpportunityModel> GetOpportunityByTopic(string topic)
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await ExecuteDatabaseFunction(() => databaseConnection.Table<OpportunityModel>().FirstOrDefaultAsync(x => x.Topic.Equals(topic))).ConfigureAwait(false);
        }

        public static async Task<int> SaveOpportunity(OpportunityModel opportunity)
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await ExecuteDatabaseFunction(() => databaseConnection.InsertOrReplaceAsync(opportunity)).ConfigureAwait(false);
        }

        public static async Task<int> DeleteItem(OpportunityModel opportunity)
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await ExecuteDatabaseFunction(() => databaseConnection.DeleteAsync(opportunity)).ConfigureAwait(false);
        }

        public static async Task<OpportunityModel> GetNewestOpportunity()
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await ExecuteDatabaseFunction(() => databaseConnection.Table<OpportunityModel>().OrderByDescending(x => x.ID).FirstOrDefaultAsync()).ConfigureAwait(false);
        }

        public static async Task<int> GetNumberOfRows()
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await ExecuteDatabaseFunction(() => databaseConnection.Table<OpportunityModel>().CountAsync()).ConfigureAwait(false);
        }
    }
}

