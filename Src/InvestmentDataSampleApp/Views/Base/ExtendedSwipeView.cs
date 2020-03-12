using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class ExtendedSwipeView : SwipeView
    {
        public ExtendedSwipeView(ICommand tappedCommand, object? tappedCommandProperty)
        {
            CloseRequested += OnCloseRequested;
            SwipeEnded += OnSwipeEnded;

            TappedCommand = tappedCommand;
            TappedCommandProperty = tappedCommandProperty;

            var tappedGestureRecognizer = new TapGestureRecognizer();
            tappedGestureRecognizer.Tapped += HandleTapped;

            GestureRecognizers.Add(tappedGestureRecognizer);
        }

        public bool IsSwiped { get; private set; }

        public ICommand TappedCommand { get; }
        public object? TappedCommandProperty { get; }

        void OnCloseRequested(object sender, EventArgs e) => IsSwiped = false;

        void OnSwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            if (GetSwipeMode(e.SwipeDirection) is SwipeMode.Reveal)
                IsSwiped = true;
        }

        void HandleTapped(object sender, EventArgs e)
        {
            if (!IsSwiped)
                TappedCommand.Execute(TappedCommandProperty);
            else
                IsSwiped = false;
        }

        SwipeMode GetSwipeMode(SwipeDirection swipeDirection) => swipeDirection switch
        {
            SwipeDirection.Down => TopItems.Mode,
            SwipeDirection.Left => RightItems.Mode,
            SwipeDirection.Up => BottomItems.Mode,
            SwipeDirection.Right => LeftItems.Mode,
            _ => throw new NotSupportedException()
        };
    }
}
