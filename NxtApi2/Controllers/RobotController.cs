using NxtNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NxtApi2.Controllers
{
    public class RobotController : ApiController
    {
        public static Nxt nxt = new Nxt();
        bool isConnected;
        private MotorPort motorC = MotorPort.PortC;
        private MotorPort motorB = MotorPort.PortB;

        // GET: api/Robot
        public IEnumerable<string> Get()
        {
            DeviceInfo deviceInfo = nxt.GetDeviceInfo();
            ushort batteryLevel = nxt.GetBatteryLevel();
            ulong keepAliveTime = nxt.KeepAlive();
            return new string[] { deviceInfo.Name, deviceInfo.SignalStrength.ToString(), batteryLevel.ToString() };
        }

        // POST: api/Robot
        public void Post([FromBody]string key)
        {
          MoveRobot(key);
        }

        #region Connect
        public void Connect(string port)
        {
            // Connecting to the NXT.
            nxt = new Nxt();
            // check for null reference
            if (nxt != null)
            {
                try
                {
                    // Connecting via the selected serial port for communication
                    nxt.Connect(port);
                    // play a tone if there was a successfull connection
                    nxt.PlayTone(700, 200);
                    isConnected = true;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
            //UpdateStatusControls();
        }
        #endregion

        #region Move
        public void MoveRobot(string key)
        {
            if (key == "W" || key == "Up")
            {
                nxt.SetOutputState(motorB, 100, MotorModes.On, MotorRegulationMode.Speed, 0, MotorRunState.Running, 0);
                nxt.SetOutputState(motorC, 100, MotorModes.On, MotorRegulationMode.Speed, 0, MotorRunState.Running, 0);
            }
            if (key == "S" || key == "Down")
            {
                nxt.SetOutputState(motorB, -100, MotorModes.On, MotorRegulationMode.Speed, 50, MotorRunState.Running, 0);
                nxt.SetOutputState(motorC, -100, MotorModes.On, MotorRegulationMode.Speed, 50, MotorRunState.Running, 0);
            }
            if (key == "Space")
            {
                nxt.SetOutputState(motorB, 0, MotorModes.Brake, MotorRegulationMode.Idle, 0, MotorRunState.Idle, 0);
                nxt.SetOutputState(motorC, 0, MotorModes.Brake, MotorRegulationMode.Idle, 0, MotorRunState.Idle, 0);
            }
            if (key == "A" || key == "Left")
            {
                nxt.SetOutputState(motorC, 0, MotorModes.Brake, MotorRegulationMode.Idle, 50, MotorRunState.Idle, 0);
                nxt.SetOutputState(motorB, 100, MotorModes.On, MotorRegulationMode.Speed, 50, MotorRunState.Running, 0);
            }
            if (key == "D" || key == "Right")
            {
                nxt.SetOutputState(motorB, 0, MotorModes.Brake, MotorRegulationMode.Idle, 50, MotorRunState.Idle, 0);
                nxt.SetOutputState(motorC, 100, MotorModes.On, MotorRegulationMode.Speed, 50, MotorRunState.Running, 0);
            }
        }
        #endregion
    }
}
