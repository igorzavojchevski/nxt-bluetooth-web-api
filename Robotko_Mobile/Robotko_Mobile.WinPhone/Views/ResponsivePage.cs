using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Robotko_Mobile.WinPhone.Views
{
    /// <summary>
    /// A simple page which 
    /// </summary>
    public class ResponsivePage : Page
    {
        public ResponsivePage()
        {
            SizeChanged += ResponsivePage_SizeChanged;
            Unloaded += ResponsivePage_Unloaded;
        }

        private void ResponsivePage_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= ResponsivePage_SizeChanged;
            Unloaded -= ResponsivePage_Unloaded;
        }

        private void ResponsivePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try {
                // In some scenarios you may wish to determine if the app is being used in Full Screen mode (likely touch),
                // the commented code below can achieve that but gets a little complex

                //var fullScreen = ScreenSizeHelper.isFullScreen(e);
                //if (fullScreen != null)
                //{
                //    if (fullScreen == Windows.UI.ViewManagement.ApplicationViewOrientation.Landscape)
                //    {
                //        VisualStateManager.GoToState(this, "FullScreen", true);
                //    }
                //    else
                //    {
                //        VisualStateManager.GoToState(this, "FullScreenPortrait", true);
                //    }
                //}
                //else
                if (e.NewSize.Width > 1170)
                {
                    VisualStateManager.GoToState(this, "Large", true);
                }
                else if (e.NewSize.Width > 970)
                {
                    VisualStateManager.GoToState(this, "Medium", true);
                }
                else if (e.NewSize.Width > 650)
                {
                    VisualStateManager.GoToState(this, "Small", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "ExtraSmall", true);
                }
            }
            catch(Exception)
            { }
        }
    }
}
