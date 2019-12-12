using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
    public class WelcomeView : OverlayContentView
    {
        public WelcomeView() : base(true)
        {
            const string titleText = "Welcome";
            const string bodyText = "Enjoy InvestmentDataSampleApp";
            const string okButtonText = "Ok, thanks!";

            var titleLabel = new Label
            {
                TextColor = Color.DarkSlateGray,
                FontAttributes = FontAttributes.Bold,
                Text = titleText,
                HorizontalTextAlignment = TextAlignment.Center,
                AutomationId = AutomationIdConstants.WelcomeViewTitleLabel
            };

            var bodyLabel = new Label
            {
                Text = bodyText,
                HorizontalTextAlignment = TextAlignment.Center,
                AutomationId = AutomationIdConstants.WelcomeViewBodyLabel
            };

            var okButton = new Button
            {
                BackgroundColor = Color.DarkSlateGray,
                BorderWidth = 1,
                BorderColor = Color.DarkSlateGray,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(5),
                Text = okButtonText,
                AutomationId = AutomationIdConstants.WelcomeViewOkButton
            };
            okButton.SetBinding(Button.CommandProperty, nameof(OpportunitiesViewModel.OkButtonTappedCommand));

            var textAndButtonStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 20,
                Children = {
                    titleLabel,
                    bodyLabel,
                    okButton
                }
            };

            OverlayContent = textAndButtonStack;
        }
    }
}


