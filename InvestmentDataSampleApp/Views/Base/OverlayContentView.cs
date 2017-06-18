using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
	public abstract class OverlayContentView : ContentView
	{
		#region Constant Fields
		readonly BoxView _backgroundOverlayBoxView;
		readonly Frame _overlayFrame;
		readonly RelativeLayout _relativeLayout;
		#endregion

		#region Fields
		View _overlayContent;
		#endregion

		#region Constructors
		protected OverlayContentView(bool isChildOfNavigationPage)
		{
			_backgroundOverlayBoxView = new BoxView
			{
				BackgroundColor = ColorConstants.WhiteWith75Opacity
			};
			_backgroundOverlayBoxView.Opacity = 0;

			_overlayFrame = new Frame
			{
				HasShadow = true,
				BackgroundColor = Color.White
			};
			_overlayFrame.Scale = 0;

			_relativeLayout = new RelativeLayout();
			Func<RelativeLayout, double> getOverlayContentHeight = (p) => OverlayContent.Measure(p.Width, p.Height).Request.Height;
			Func<RelativeLayout, double> getOverlayContentWidth = (p) => OverlayContent.Measure(p.Width, p.Height).Request.Width;

			_relativeLayout.Children.Add(_backgroundOverlayBoxView,
			   	Constraint.Constant(-10),
			   	Constraint.Constant(0),
			  	Constraint.RelativeToParent(parent => parent.Width + 20),
				Constraint.RelativeToParent(parent => parent.Height)
		   	);
			_relativeLayout.Children.Add(_overlayFrame,
				Constraint.RelativeToParent(parent => parent.Width / 2 - getOverlayContentWidth(parent) / 2 - 25),
				Constraint.RelativeToParent(parent =>
					{
						switch (isChildOfNavigationPage)
						{
							case true:
								return parent.Height / 4 - getOverlayContentHeight(parent) / 2;

							default:
								return parent.Height / 2 - getOverlayContentHeight(parent) / 2 - 10;
						}
					}),

				Constraint.RelativeToParent(parent => getOverlayContentWidth(parent) + 50),
				Constraint.RelativeToParent(parent => getOverlayContentHeight(parent) + 40)
		  	);
		}
		#endregion

		#region Properties
		public View OverlayContent
		{
			get { return _overlayContent; }
			set
			{
				_overlayContent = value;
				_overlayContent.Scale = 0;

				_overlayFrame.Content = _overlayContent;

				Content = _relativeLayout;
			}
		}
		#endregion

		#region Methods
		public void ShowView()
		{
			const uint overlayContentViewAnimationTime = 300;
			const double overlayContentViewMaxSize = 1.05;
			const double overlayContentViewNormalSize = 1;

			Device.BeginInvokeOnMainThread(async () =>
			{
				IsVisible = true;
				_backgroundOverlayBoxView.Opacity = 1;

				await Task.WhenAll(OverlayContent?.ScaleTo(overlayContentViewMaxSize, overlayContentViewAnimationTime, Easing.CubicOut),
									_overlayFrame?.ScaleTo(overlayContentViewMaxSize, overlayContentViewAnimationTime, Easing.CubicOut));

				await Task.WhenAll(OverlayContent?.ScaleTo(overlayContentViewNormalSize, overlayContentViewAnimationTime, Easing.CubicOut),
									_overlayFrame?.ScaleTo(overlayContentViewNormalSize, overlayContentViewAnimationTime, Easing.CubicOut));
			});
		}

		public void HideView()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await this.FadeTo(0);

				IsVisible = false;
				InputTransparent = true;
				Opacity = 1;

				_backgroundOverlayBoxView.Opacity = 0;
				OverlayContent.Scale = 0;
				_overlayFrame.Scale = 0;
			});
		}
		#endregion
	}
}



