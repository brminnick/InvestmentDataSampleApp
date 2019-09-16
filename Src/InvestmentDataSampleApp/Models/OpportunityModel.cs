using System;

using SQLite;

namespace InvestmentDataSampleApp
{
    public class OpportunityModel
    {
        public OpportunityModel()
        {
            Topic = "";
            Company = "";
            DBA = "";
            LeaseAmount = 0;
            SalesStage = SalesStages.New;
            Owner = "";
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public string LeaseAmountAsCurrency => $"{LeaseAmount:C}";

        [PrimaryKey, AutoIncrement]
        public int? ID { get; set; }
        public string Topic { get; set; }
        public string Company { get; set; }
        public string DBA { get; set; }
        public long LeaseAmount { get; set; }
        public SalesStages SalesStage { get; set; }
        public string Owner { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public enum SalesStages { New, Pending, Closed }
}

