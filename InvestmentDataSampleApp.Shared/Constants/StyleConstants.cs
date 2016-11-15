using Xamarin.Forms;

namespace InvestmentDataSampleApp.Shared
{
	public static class StyleConstants
	{
		public static Style LabelStyle = new Style(typeof(Label))
		{
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Color.Black}
			}
		};

		public static Style AboutPageLabelStyle = new Style(typeof(Label))
		{
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = ColorConstants.BlackWith75Opacity}
			}
		};

		public static Style ButtonStyle = new Style(typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = Color.Transparent },
				new Setter { Property = Button.BorderColorProperty, Value = Color.Transparent }
			}
		};

		public static Style StoreRequestButtonStyle = new Style(typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.BackgroundColorProperty, Value = Color.White },
				new Setter { Property = Button.TextColorProperty, Value = ColorConstants.BlackWith75Opacity },
				new Setter { Property = Button.BorderWidthProperty, Value = 1 },
				new Setter { Property = Button.BorderColorProperty, Value = ColorConstants.BlackWith75Opacity },
			}
		};

		public static Style EntryStyle = new Style(typeof(Entry))
		{
			Setters = {
				new Setter { Property = Entry.BackgroundColorProperty, Value = Color.Transparent},
			}
		};

		public static Style PickerStyle = new Style(typeof(Picker))
		{
			Setters = {
				new Setter { Property = Picker.BackgroundColorProperty, Value = Color.Transparent}
			}
		};

		public static Style ImageStyle = new Style(typeof(Image))
		{
			Setters = {
				new Setter { Property = Image.BackgroundColorProperty, Value = Color.Transparent}
			}
		};

		public static Style ActivityIndicatorStyle = new Style(typeof(ActivityIndicator))
		{
			Setters = {
				new Setter { Property = ActivityIndicator.ColorProperty, Value = Color.Gray}
			}
		};
	}
}

