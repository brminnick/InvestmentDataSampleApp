using System;
namespace InvestmentDataSampleApp
{
    public interface ISearchPage
    {
        void OnSearchBarTextChanged(in string text);
        event EventHandler<string> SearchBarTextChanged;
    }
}
