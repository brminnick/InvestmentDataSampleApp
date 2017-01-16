using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using InvestmentDataSampleApp;
using InvestmentDataSampleApp.iOS;

[assembly: ExportRenderer(typeof(EntryWithCustomKeyboardReturnButton), typeof(EntryWithCustomKeyboardReturnButtonCustomRenderer))]
namespace InvestmentDataSampleApp.iOS
{
	public class EntryWithCustomKeyboardReturnButtonCustomRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var customEntry = Element as EntryWithCustomKeyboardReturnButton;

			if (Control != null && customEntry != null)
			{
				SetKeyboardButtonType(customEntry.ReturnType);

				Control.ShouldReturn += (UITextField tf) =>
				{
					customEntry?.InvokeCompleted();
					return true;
				};
			}
		}

		void SetKeyboardButtonType(ReturnType returnType)
		{
			switch (returnType)
			{
				case ReturnType.Go:
					Control.ReturnKeyType = UIReturnKeyType.Go;
					break;
				case ReturnType.Next:
					Control.ReturnKeyType = UIReturnKeyType.Next;
					break;
				case ReturnType.Send:
					Control.ReturnKeyType = UIReturnKeyType.Send;
					break;
				case ReturnType.Search:
					Control.ReturnKeyType = UIReturnKeyType.Search;
					break;
				case ReturnType.Done:
					Control.ReturnKeyType = UIReturnKeyType.Done;
					break;
				default:
					Control.ReturnKeyType = UIReturnKeyType.Default;
					break;
			}
		}
	}
}
