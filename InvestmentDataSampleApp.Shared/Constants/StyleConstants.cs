using Xamarin.Forms;

namespace InvestmentDataSampleApp.Shared
{
	public static class StyleConstants
	{
		public static Style WelcomeViewButtonStyle = new Style(typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = ColorConstants.BlackWith75Opacity },
				new Setter { Property = Button.TextColorProperty, Value = Color.White },
				new Setter { Property = Button.BorderWidthProperty, Value = 1 },
				new Setter { Property = Button.BorderColorProperty, Value = ColorConstants.BlackWith75Opacity },
			}
		};
	}
}

