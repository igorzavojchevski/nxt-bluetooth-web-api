using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Robotko_Mobile.WinPhone.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Robotko_Mobile.WinPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Robot : ResponsivePage
    {
        /// <summary>
        /// Enter URI to the WEB api
        /// </summary>
        private string WebApiUri = "http://192.168.43.193:8080/";
        //
        //
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private HttpClient client = new HttpClient();

        public Robot()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            try
            {
                await GetInfo();
                await Connect();
            }
            catch(Exception exc)
            {
                var s = exc.ToString();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region RobotInfo
        private async Task GetInfo()
        {
            string result = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WebApiUri + "api/Robot");
            request.ContinueTimeout = 4000;

            request.Credentials = CredentialCache.DefaultNetworkCredentials;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.Headers["Accept"] = "application/json";
                    using (Stream streamResponse = response.GetResponseStream())
                    {
                        using (StreamReader streamRead = new StreamReader(streamResponse))
                        {
                            result = await streamRead.ReadToEndAsync();
                            JArray arr = JArray.Parse(result);

                            robotName.Text = " " + (string)arr[0];
                            robotSStrength.Text = " " + (string)arr[1];
                            robotBatteryLevel.Text = " " + (string)arr[2];
                        }
                    }
                }
            }
        }
        #endregion

        #region RobotControls
        //GO FORWARD
        private void goForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            var clickMode = goForwardBtn.ClickMode.ToString();
            if (clickMode == "Press")
            {
                FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "W" } });
                var post = client.PostAsync(WebApiUri + "api/Robot", control);
                goForwardBtn.ClickMode = ClickMode.Release;
            }
            else
            {
                FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "Space" } });
                var post = client.PostAsync(WebApiUri + "api/Robot", control);
                goForwardBtn.ClickMode = ClickMode.Press;
            }
        }

        //GO LEFT
        private void goLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            var clickMode = goForwardBtn.ClickMode.ToString();
            if (clickMode == "Press")
            {
                FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "A" } });
                var post = client.PostAsync(WebApiUri + "api/Robot", control);
                goForwardBtn.ClickMode = ClickMode.Release;
            }
            else
            {
                FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "Space" } });
                var post = client.PostAsync(WebApiUri + "api/Robot", control);
                goForwardBtn.ClickMode = ClickMode.Press;
            }
        }

        //GO RIGHT
        private void goRightBtn_Click(object sender, RoutedEventArgs e)
        {
            var clickMode = goForwardBtn.ClickMode.ToString();
            if (clickMode == "Press")
            {
                FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "D" } });
                var post = client.PostAsync(WebApiUri + "api/Robot", control);
                goForwardBtn.ClickMode = ClickMode.Release;
            }
            else
            {
                FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "Space" } });
                var post = client.PostAsync(WebApiUri + "api/Robot", control);
                goForwardBtn.ClickMode = ClickMode.Press;
            }
        }

        //GO BACK
        private void goBackBtn_Click(object sender, RoutedEventArgs e)
        {
            var clickMode = goForwardBtn.ClickMode.ToString();
            if (clickMode == "Press")
            {
                FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "S" } });
                var post = client.PostAsync(WebApiUri + "api/Robot", control);
                goForwardBtn.ClickMode = ClickMode.Release;
            }
            else
            {
                FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "Space" } });
                var post = client.PostAsync(WebApiUri + "api/Robot", control);
                goForwardBtn.ClickMode = ClickMode.Press;
            }
        }

        //STOP
        private void StopMoving()
        {
            FormUrlEncodedContent control = new FormUrlEncodedContent(new Dictionary<string, string> { { "", "Space" } });
            var post = client.PostAsync(WebApiUri + "api/Robot", control);
        }
        #endregion

        #region SignalR
        private static readonly EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();

        public static bool IsRunningOnEmulator
        {
            get
            {
                return (deviceInfo.SystemProductName == "Virtual");
            }
        }

        public HubConnection Connection { get; set; }
        public IHubProxy Proxy { get; set; }

        private new CoreDispatcher Dispatcher
        {
            get { return CoreApplication.MainView.CoreWindow.Dispatcher; }
        }

        public async Task Connect()
        {
            Connection = new HubConnection(WebApiUri);

            Proxy = Connection.CreateHubProxy("RobotHub");

            if (IsRunningOnEmulator)
            {
                await Connection.Start(new LongPollingTransport());
            }
            else
            {
                await Connection.Start();
            }

            Proxy.On<short, ushort, ushort>("GetSensorValues", async (touchSensorResult, soundSensorResult, lightSensorResult) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    touchVal.Text = " " + touchSensorResult.ToString();
                    soundVal.Text = " " + Convert.ToString(soundSensorResult);
                    lightVal.Text = " " + Convert.ToString(lightSensorResult);

                    await Proxy.Invoke("GetSensors");
                });
            }
            );
            await Proxy.Invoke("GetSensors");
        }
        #endregion
    }
}
