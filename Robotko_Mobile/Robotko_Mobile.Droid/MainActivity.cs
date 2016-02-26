using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;

namespace Robotko_Mobile.Droid
{
	[Activity (Label = "Robotko_Mobile.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        private string WebApiUri = "http://192.168.1.66:8080/";
        private readonly HubConnection _connection;
        private readonly IHubProxy _proxy;
        private Button goForward;
        private Button goBack;
        private Button goLeft;
        private Button goRight;
        private TextView robotName;
        private TextView robotSignal;
        private TextView robotBattery;
        private TextView touchVal;
        private TextView soundVal;
        private TextView lightVal;

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            base.RequestWindowFeature(WindowFeatures.NoTitle);

			SetContentView (Resource.Layout.Main);

            try
            {
                //await GetInfo();
                await Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //Buttons
            goForward = FindViewById<Button>(Resource.Id.goForward);
            goBack = FindViewById<Button>(Resource.Id.goBack);
            goLeft = FindViewById<Button>(Resource.Id.goLeft);
            goRight = FindViewById<Button>(Resource.Id.goRight);

            //TextViews
            robotName = FindViewById<TextView>(Resource.Id.nameTxt);
            robotSignal = FindViewById<TextView>(Resource.Id.signalTxt);
            robotBattery = FindViewById<TextView>(Resource.Id.batteryTxt);
            touchVal = FindViewById<TextView>(Resource.Id.touchVal);
            soundVal = FindViewById<TextView>(Resource.Id.soundVal);
            lightVal = FindViewById<TextView>(Resource.Id.lightVal);
        }

        #region RobotInfo
        private async Task GetInfo()
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WebApiUri + "api/Robot");
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

                            robotName.Text = "Име на роботот: " + (string)arr[0];
                            robotSignal.Text = "Сигнал: " + (string)arr[1];
                            robotBattery.Text = "Батерија: " + (string)arr[2];
                        }
                    }
                }
            }
        }
        #endregion

        #region SignalR
        public MainActivity()
        {
            _connection = new HubConnection(WebApiUri);
            _proxy = _connection.CreateHubProxy("RobotHub");
        }

        public async Task Connect()
        {
            await _connection.Start();

            if (_connection.ConnectionId != null)
            {
                _proxy.On<short, ushort, ushort>("GetSensorValues", (touchSensorResult, soundSensorResult, lightSensorResult) =>
                {
                    RunOnUiThread(async () =>
                    {
                        touchVal.Text = " " + touchSensorResult.ToString();
                        soundVal.Text = " " + Convert.ToString(soundSensorResult);
                        lightVal.Text = " " + Convert.ToString(lightSensorResult);

                        await _proxy.Invoke("GetSensors");
                    });

                });

                //await _proxy.Invoke("GetSensors");


            }
            else
            {
                try
                {
                    await Connect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        #endregion
    }
}


