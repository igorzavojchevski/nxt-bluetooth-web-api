using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Sistemko.WinPhone
{
    public static class ScreenSizeHelper
    {
        private static double ScreenWidth;
        private static double ScreenHeight;
        private static ApplicationViewOrientation InitOrientation;
        private static bool initiated = false;

        /// <summary>
        /// Determines the current orientation and state of full screen by recording the first values called when the page is loaded
        /// Note: this method works due to all apps initially launching as full screen even if they are then immediately reduced, thus the
        /// first page MUST utilise this code.
        /// </summary>
        /// <param name="args">Event args from a Size Changed Event</param>
        /// <returns>null if not fullscreen, else the current orientation</returns>
        public static ApplicationViewOrientation? isFullScreen(SizeChangedEventArgs args)
        {
            if (!initiated)
            {
                ScreenHeight = args.NewSize.Height;
                ScreenWidth = args.NewSize.Width;
                InitOrientation = ApplicationView.GetForCurrentView().Orientation;
                initiated = true;
            }

            if (ApplicationView.GetForCurrentView().Orientation == InitOrientation)
            {
                if (args.NewSize.Height == ScreenHeight && args.NewSize.Width == ScreenWidth)
                    return InitOrientation;
            }
            else
            {
                if (args.NewSize.Height == ScreenWidth && args.NewSize.Width == ScreenHeight)
                {
                    if (InitOrientation == ApplicationViewOrientation.Landscape)
                        return ApplicationViewOrientation.Portrait;
                    else
                        return ApplicationViewOrientation.Landscape;
                }
            }
            return null;
        }
    }
}
