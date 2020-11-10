using System;

using SQLite;

namespace InvestmentDataSampleApp
{
    public record OpportunityModel
    {
        public string LeaseAmountAsCurrency => $"{LeaseAmount:C}";

        [PrimaryKey, AutoIncrement]
        public int? ID { get; init; }
        public string Topic { get; init; } = string.Empty;
        public string Company { get; init; } = string.Empty;
        public string DBA { get; init; } = string.Empty;
        public long LeaseAmount { get; init; }
        public SalesStages SalesStage { get; init; } = SalesStages.New;
        public string Owner { get; init; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    }

    public enum SalesStages { New, Pending, Closed }
}

#warning .NET 5 workaround https://stackoverflow.com/a/62656145/5953643
namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}