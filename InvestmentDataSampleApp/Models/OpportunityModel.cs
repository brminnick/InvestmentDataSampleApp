using System;

using SQLite;

namespace InvestmentDataSampleApp
{
    public class OpportunityModel
    {
        #region Constructors
        public OpportunityModel()
        {
            Topic = "";
            Company = "";
            DBA = "";
            LeaseAmount = 0;
            SalesStage = SalesStages.New;
            Owner = "";
            CreatedAt = DateTime.Now;
        }
        #endregion

        #region Properties
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }
        public string Topic { get; set; }
        public string Company { get; set; }
        public string DBA { get; set; }
        public long LeaseAmount { get; set; }
        public string LeaseAmountAsCurrency => $"{LeaseAmount:C}";
        public SalesStages SalesStage { get; set; }
        public string Owner { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        #endregion
    }

    public enum SalesStages { New, Pending, Closed }
}

