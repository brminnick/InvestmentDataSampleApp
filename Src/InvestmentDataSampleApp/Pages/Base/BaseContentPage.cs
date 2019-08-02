using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public abstract class BaseContentPage<T> : ContentPage where T : BaseViewModel, new()
    {
        protected BaseContentPage() => BindingContext = ViewModel;

        protected T ViewModel { get; } = new T();
    }
}
