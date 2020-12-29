using InvestmentDataSampleApp.Shared;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Markup;

namespace InvestmentDataSampleApp
{
    public class WelcomeView : OverlayContentView
    {
        public WelcomeView() : base(true)
        {
            OverlayContent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 20,
                Children =
                {
                    new TitleLabel(),
                    new BodyLabel(),
                    new OkButton()
                        .Bind(Button.CommandProperty, nameof(OpportunitiesViewModel.OkButtonTappedCommand))
                }
            };
        }

        class TitleLabel : Label
        {
            public TitleLabel()
            {
                Text = "Welcome";
                TextColor = Color.DarkSlateGray;
                FontAttributes = FontAttributes.Bold;
                HorizontalTextAlignment = TextAlignment.Center;
                AutomationId = AutomationIdConstants.WelcomeViewTitleLabel;
            }
        }

        class BodyLabel : Label
        {
            public BodyLabel()
            {
                Text = $"Enjoy {nameof(InvestmentDataSampleApp)}";
                HorizontalTextAlignment = TextAlignment.Center;
                AutomationId = AutomationIdConstants.WelcomeViewBodyLabel;
            }
        }

        class OkButton : Button
        {
            public OkButton()
            {
                Text = "Ok, thanks!";
                BorderWidth = 1;
                TextColor = Color.White;
                Margin = new Thickness(5);
                BorderColor = Color.DarkSlateGray;
                BackgroundColor = Color.DarkSlateGray;
                FontAttributes = FontAttributes.Bold;
                AutomationId = AutomationIdConstants.WelcomeViewOkButton;
            }
        }
    }
}


