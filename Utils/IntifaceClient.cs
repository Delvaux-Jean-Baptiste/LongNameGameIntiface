using Buttplug.Client;
using Buttplug.Core.Messages;
using LongNameGameIntiface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LongNameGameIntiface.WebClient
{

    public class IntifaceClient
    {
        private const int sleepTimer = 250;

        public SexToyFunction[] sexToyFunctions;
        public ButtplugClient client = new ButtplugClient("ButtplugClient");

        public SexToyFunction vibratorStf;
        public SexToyFunction pistonStf;

        public async Task<string> ConnectIntiface()
        {

            if (!client.Connected)
            {
                var connector = new ButtplugWebsocketConnector(new Uri("ws://127.0.0.1:12345"));
                try
                {
                    await client.ConnectAsync(connector);
                }
                catch (ButtplugClientConnectorException ex)
                {
                    Console.WriteLine(
                        $"Can't connect, exiting! Message: {ex.InnerException.Message}");
                    return "Not Connected";
                }
                LongNameGameIntifacePlugin.Log.LogInfo("Connected to Intiface");
            }


            var devices = await scanDevicesIntifaceClient();

            List<SexToyFunction> listFunction = new List<SexToyFunction>();

            foreach (var d in devices)
            {

                
                //Function loop
                foreach (GenericDeviceMessageAttributes s in d.MessageAttributes.ScalarCmd)
                {
                    
                    string name = d.Name + " " + s.ActuatorType.ToString();
                    SexToyFunction sexToyFunction = new SexToyFunction(d, s.Index, name, s.ActuatorType, Convert.ToInt32(s.StepCount));
                    listFunction.Add(sexToyFunction);
                }
            }

            sexToyFunctions = listFunction.ToArray();

            LongNameGameIntifacePlugin.Log.LogInfo($"Debug function total {sexToyFunctions.Length}");

            return "Connected";
        }

        public async Task<ButtplugClientDevice[]> scanDevicesIntifaceClient()
        {
            if (!client.Connected)
                return null;

            client.DeviceAdded += (aObj, aDeviceEventArgs) =>
                Console.WriteLine($"Device {aDeviceEventArgs.Device.Name} Connected!");

            client.DeviceRemoved += (aObj, aDeviceEventArgs) =>
                Console.WriteLine($"Device {aDeviceEventArgs.Device.Name} Removed!");

            await client.StartScanningAsync();

            List<SexToyFunction> listSexToysFunction = new List<SexToyFunction>();

            return client.Devices;

        }

        public bool isConnected()
        {
            return client.Connected;
        }

        public async void TriggerToys()
        {
            


            if (vibratorStf == null && pistonStf == null)
                return;

            while (true)
            {
                

                
                if (vibratorStf != null)
                {
                    int width = 15;
                    int height = 5;

                        

                    double scalar = 0;

                    TriggerSexToy(vibratorStf, scalar);
                }
                Thread.Sleep(sleepTimer);
            }
        }

        public void DisconnectIntiface()
        {
            client.DisconnectAsync();
        }

        public async void TriggerSexToy(SexToyFunction stf, double scalar)
        {
            await stf.device.ScalarAsync(new ScalarCmd.ScalarSubcommand(stf.id, scalar, stf.type));
        }

        //public string ConnectSexToys(DataGridView sexToysGrid, DataGridView pistonToysGrid)
        //{
        //    Int32 selectedSexToyRowCount =
        //    sexToysGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);

        //    Int32 selectedPistonToyRowCount =
        //    pistonToysGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);

        //    var sexToyRow = sexToysGrid.SelectedRows[0];
        //    var pistonToyRow = pistonToysGrid.SelectedRows[0];

        //    var returnString = new StringBuilder();
        //    string sexToyStatus = "unlinked";
        //    string pistonToysStatus = "unlinked";

        //    if (selectedSexToyRowCount > 0)
        //    {
        //        vibratorStf = sexToyFunctions[sexToyRow.Index];
        //        sexToyStatus = "linked";
        //    }

        //    if (selectedPistonToyRowCount > 0)
        //    {
        //        pistonStf = sexToyFunctions[pistonToyRow.Index];
        //        pistonToysStatus = "linked";
        //    }

        //    returnString.Append($"Sex Toy Status: {sexToyStatus}");
        //    returnString.AppendLine($"Sex Toy Status: {pistonToysStatus}");

        //    return returnString.ToString();
        //}

    }
}
