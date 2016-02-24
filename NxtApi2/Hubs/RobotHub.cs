using Microsoft.AspNet.SignalR;
using NxtApi2.Controllers;
using NxtNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxtApi2
{
    class RobotHub : Hub
    {
        public void GetSensors()
        {
            short touchSensorResult = TouchSensorCheck();
            ushort soundSensorResult = SoundSensorCheck();
            ushort lightSensorResult = LightSensorCheck();
            Clients.Caller.GetSensorValues(touchSensorResult,soundSensorResult,lightSensorResult);
        }
        #region TouchSensor
        public short TouchSensorCheck()
        {
            var nxt = RobotController.nxt;
            nxt.SetInputMode(SensorPort.Port1, SensorType.Switch, SensorMode.Boolean);
            SensorState state = nxt.GetInputValues(SensorPort.Port1);
            return state.ScaledValue;
        }
        #endregion

        #region SoundSensor
        public ushort SoundSensorCheck()
        {
            var nxt = RobotController.nxt;
            nxt.SetInputMode(SensorPort.Port2, SensorType.SoundDB, SensorMode.FullScale);
            SensorState state = nxt.GetInputValues(SensorPort.Port2);
            return state.NormalizedValue;
        }
        #endregion

        #region LightSensor
        public ushort LightSensorCheck()
        {
            var nxt = RobotController.nxt;
            nxt.SetInputMode(SensorPort.Port3, SensorType.LightInactive, SensorMode.FullScale);
            SensorState state = nxt.GetInputValues(SensorPort.Port3);
            return state.NormalizedValue;
        }
        #endregion
    }
}
