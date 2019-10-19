using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace InvestmentDataSampleApp
{
    public abstract class BaseContentPage<T> : ContentPage where T : BaseViewModel, new()
    {
        protected BaseContentPage()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
            BindingContext = ViewModel;
            BackgroundColor = Color.White;
        }

        protected T ViewModel { get; } = new T();
    }
}
