﻿using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InvestmentDataSampleApp
{
    abstract class OpportunityModelDatabase : BaseDatabase
    {
        public static async Task<IList<OpportunityModel>> GetAllOpportunityData_OldestToNewest()
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<OpportunityModel>().Where(x => x.ID > 0).OrderBy(x => x.Topic).ToListAsync()).ConfigureAwait(false);
        }

        public static async Task<IList<OpportunityModel>> GetAllOpportunityData_NewestToOldest()
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<OpportunityModel>().Where(x => x.ID > 0).OrderByDescending(x => x.Topic).ToListAsync()).ConfigureAwait(false);
        }

        public static async Task<OpportunityModel> GetOpportunityByID(int id)
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<OpportunityModel>().FirstOrDefaultAsync(x => x.ID.Equals(id))).ConfigureAwait(false);
        }

        public static async Task<OpportunityModel> GetOpportunityByTopic(string topic)
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<OpportunityModel>().FirstOrDefaultAsync(x => x.Topic.Equals(topic))).ConfigureAwait(false);
        }

        public static async Task<int> SaveOpportunity(OpportunityModel opportunity)
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.InsertOrReplaceAsync(opportunity)).ConfigureAwait(false);
        }

        public static async Task<int> DeleteItem(OpportunityModel opportunity)
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.DeleteAsync(opportunity)).ConfigureAwait(false);
        }

        public static async Task<OpportunityModel> GetNewestOpportunity()
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<OpportunityModel>().OrderByDescending(x => x.ID).FirstOrDefaultAsync()).ConfigureAwait(false);
        }

        public static async Task<int> GetNumberOfRows()
        {
            var databaseConnection = await GetDatabaseConnection<OpportunityModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<OpportunityModel>().CountAsync()).ConfigureAwait(false);
        }
    }
}

