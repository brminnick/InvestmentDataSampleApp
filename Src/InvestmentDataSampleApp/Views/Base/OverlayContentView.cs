using System.Threading.Tasks;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public abstract class OverlayContentView : ContentView
    {
        readonly BoxView _backgroundOverlayBoxView;
        readonly Frame _overlayFrame;
        readonly RelativeLayout _relativeLayout;

        View _overlayContent;

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

            _relativeLayout.Children.Add(_backgroundOverlayBoxView,
                Constraint.Constant(-10),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Width + 20),
                Constraint.RelativeToParent(parent => parent.Height));

            _relativeLayout.Children.Add(_overlayFrame,
                Constraint.RelativeToParent(parent =>
                {
                    var preferredX = parent.Width / 2 - getOverlayContentWidth(parent) / 2 - 25;
                    return preferredX < 10 ? 10 : preferredX;
                }),
                Constraint.RelativeToParent(parent =>
                {
                    if (isChildOfNavigationPage)
                        return parent.Height / 4 - getOverlayContentHeight(parent) / 2;

                    return parent.Height / 2 - getOverlayContentHeight(parent) / 2 - 10;
                }),
                Constraint.RelativeToParent(parent =>
                {
                    var preferedWidth = getOverlayContentWidth(parent) + 50;
                    return preferedWidth > parent.Width ? parent.Width - 20 : preferedWidth;
                }),
                Constraint.RelativeToParent(parent => getOverlayContentHeight(parent) + 40));

            double getOverlayContentHeight(RelativeLayout p) => OverlayContent.Measure(p.Width, p.Height).Request.Height;
            double getOverlayContentWidth(RelativeLayout p) => OverlayContent.Measure(p.Width, p.Height).Request.Width;
        }

        public View OverlayContent
        {
            get => _overlayContent;
            set
            {
                _overlayContent = value;
                _overlayContent.Scale = 0;

                _overlayFrame.Content = _overlayContent;

                Content = _relativeLayout;
            }
        }

        public Task ShowView()
        {
            const uint overlayContentViewAnimationTime = 300;
            const double overlayContentViewMaxSize = 1.05;
            const double overlayContentViewNormalSize = 1;

            return Device.InvokeOnMainThreadAsync(async () =>
            {
                IsVisible = true;
                _backgroundOverlayBoxView.Opacity = 1;

                await Task.WhenAll(OverlayContent?.ScaleTo(overlayContentViewMaxSize, overlayContentViewAnimationTime, Easing.CubicOut),
                                    _overlayFrame?.ScaleTo(overlayContentViewMaxSize, overlayContentViewAnimationTime, Easing.CubicOut));

                await Task.WhenAll(OverlayContent?.ScaleTo(overlayContentViewNormalSize, overlayContentViewAnimationTime, Easing.CubicOut),
                                    _overlayFrame?.ScaleTo(overlayContentViewNormalSize, overlayContentViewAnimationTime, Easing.CubicOut));
            });
        }

        public Task HideView()
        {
            return Device.InvokeOnMainThreadAsync(async () =>
            {
                await this.FadeTo(0, 1000, Easing.CubicIn);

                IsVisible = false;
                InputTransparent = true;
                Opacity = 1;

                _backgroundOverlayBoxView.Opacity = 0;
                OverlayContent.Scale = 0;
                _overlayFrame.Scale = 0;
            });
        }
    }
}



