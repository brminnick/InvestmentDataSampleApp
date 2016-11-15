using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
	public class WelcomeView : ContentView
	{
		readonly BoxView _backgroundOverlayBoxView;
		readonly Frame _overlayFrame;
		readonly StackLayout _textAndButtonStack;

		readonly RelativeLayout _relativeLayout;

		public WelcomeView()
		{
			const string titleText = "Welcome!";
			const string bodyText = "Enjoy InvestmentDataSampleApp";

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

			var titleLabel = new Label
			{
				Style = StyleConstants.LabelStyle,
				FontAttributes = FontAttributes.Bold,
				Text = titleText,
				HorizontalTextAlignment = TextAlignment.Center,
				AutomationId = AutomationIdConstants.WelcomeViewTitleLabel
			};

			var bodyLabel = new Label
			{
				Style = StyleConstants.LabelStyle,
				Text = bodyText,
				HorizontalTextAlignment = TextAlignment.Center,
				AutomationId = AutomationIdConstants.WelcomeViewBodyLabel
			};

			var okButton = new Button
			{
				Text = "Ok",
				AutomationId = AutomationIdConstants.WelcomeViewOkButton
			};
			okButton.SetBinding(Button.CommandProperty, "OkButtonTapped");

			var buttonStack = new StackLayout
			{
				Spacing = 20,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Children =
				{
					okButton
				}
			};

			_textAndButtonStack = new StackLayout
			{
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Spacing = 20,
				Children = {
					titleLabel,
					bodyLabel,
					buttonStack
				}
			};
			_textAndButtonStack.Scale = 0;

			_relativeLayout = new RelativeLayout();
			Func<RelativeLayout, double> gettextAndButtonStackHeight = (p) => _textAndButtonStack.Measure(_relativeLayout.Width, _relativeLayout.Height).Request.Height;
			Func<RelativeLayout, double> gettextAndButtonStackWidth = (p) => _textAndButtonStack.Measure(_relativeLayout.Width, _relativeLayout.Height).Request.Width;


			_relativeLayout.Children.Add(_backgroundOverlayBoxView,
			   	Constraint.Constant(-10),
			   	Constraint.Constant(0),
			  	Constraint.RelativeToParent(parent => parent.Width + 20),
				Constraint.RelativeToParent(parent => parent.Height)
		   	);
			_relativeLayout.Children.Add(_overlayFrame,
				Constraint.RelativeToParent(parent => parent.Width / 2 - gettextAndButtonStackWidth(parent) / 2 - 20),
				Constraint.RelativeToParent(parent => parent.Height / 2 - gettextAndButtonStackHeight(parent) / 2 - 10),
			   	Constraint.RelativeToParent(parent => gettextAndButtonStackWidth(parent) + 30),
				Constraint.RelativeToParent(parent => gettextAndButtonStackHeight(parent) + 30)
		  	);

			_relativeLayout.Children.Add(_textAndButtonStack,
				 Constraint.RelativeToView(_overlayFrame, (parent, view) => view.X + 15),
				 Constraint.RelativeToView(_overlayFrame, (parent, view) => view.Y + 15)
			);

			if (Device.OS == TargetPlatform.Android)
			{
				_overlayFrame.IsVisible = false;
				_textAndButtonStack.BackgroundColor = ColorConstants.WhiteWith90Opacity;
			}

			Content = _relativeLayout;
		}

		public void DisplayView()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				var animationList = new List<Task>
				{
					_backgroundOverlayBoxView.FadeTo(1,AnimationConstants.WelcomeViewAnimationTime),
					_textAndButtonStack.ScaleTo(AnimationConstants.WelcomeViewMaxSize, AnimationConstants.WelcomeViewAnimationTime),
				 	_overlayFrame.ScaleTo(AnimationConstants.WelcomeViewMaxSize,AnimationConstants.WelcomeViewAnimationTime)
				};
				await Task.WhenAll(animationList);

				animationList = new List<Task>
				{
					_textAndButtonStack.ScaleTo(AnimationConstants.WelcomeViewNormalSize, AnimationConstants.WelcomeViewAnimationTime),
					_overlayFrame.ScaleTo(AnimationConstants.WelcomeViewNormalSize, AnimationConstants.WelcomeViewAnimationTime)
				};
				await Task.WhenAll(animationList);
			});
		}
	}
}


