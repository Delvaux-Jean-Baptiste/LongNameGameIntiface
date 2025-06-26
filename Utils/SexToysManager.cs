using LongNameGameIntiface.Model;
using LongNameGameIntiface.WebClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LongNameGameIntiface.Utils
{
    public class SexToysManager
    {
        private int timer = 250;

        double[] magnitudes;
        private List<SexToyTriggerDuration> sexToysTriggers = new List<SexToyTriggerDuration>();
        private IntifaceClient client;
        private List<SexToyFunction> sexToysFunctions;

        public SexToysManager(IntifaceClient client, List<SexToyFunction> sexToyFunctions)
        {
            this.client = client;
            this.sexToysFunctions = sexToyFunctions;
            foreach (SexToyFunction function in sexToysFunctions)
            {
                SexToyTriggerDuration baseTrigger = new SexToyTriggerDuration(function, 0, -1f);
                sexToysTriggers.Add(baseTrigger);
            }
        }

        public async void loop()
        {
            magnitudes = new double[sexToysTriggers.Count];
            while (client.isConnected() && sexToysFunctions.Count > 0 && sexToysTriggers.Count>0)
            {
                
                sexToysTriggers.RemoveAll(st => st.Duration != -1f && st.Duration <= 0f);


                foreach (SexToyFunction f in sexToysFunctions)
                {
                    double newMagnitude = sexToysTriggers.Where(st => st.functionId == f.id).Max(st => st.Magnitude);
                    if (LongNameGameIntifacePlugin.booldebugLogs.Value)
                        LongNameGameIntifacePlugin.Log.LogInfo($"Debug magnitude pre-check {newMagnitude} || {f.name} || {f.id}");
                    triggerToy(newMagnitude, f.id);
                }

                foreach (var st in sexToysTriggers) { st.TickTime(); }
                Thread.Sleep(timer);
            }
        }

        public void addToyDuration (uint id, double magnitude,  float duration)
        {
            if (sexToysFunctions.Count > 0)
                sexToysTriggers.Add(new SexToyTriggerDuration(sexToysFunctions.First(f => f.getDeviceID == id), magnitude, duration));
        }

        public void removeToyDuration(uint id, double magnitude, float duration)
        {
            if (sexToysFunctions.Count > 0)
                sexToysTriggers.RemoveAll(stt => stt.functionId == id && stt.Magnitude == magnitude && stt.Duration == duration);
        }

        private void triggerToy (double newMagnitude, uint idFunction)
        {

            ;
            if (newMagnitude < 0 || newMagnitude > 1)
                return;

            if(newMagnitude != magnitudes[idFunction])
            {
                magnitudes[idFunction] = newMagnitude;
                if (LongNameGameIntifacePlugin.booldebugLogs.Value)
                    LongNameGameIntifacePlugin.Log.LogInfo($"Debug magnitude {newMagnitude} || {sexToysFunctions[Convert.ToInt32(idFunction)].getDeviceID}");
                client.TriggerSexToy(sexToysFunctions[Convert.ToInt32(idFunction)], newMagnitude);
            }
        }

        private void debugTriggerToy()
        {
            client.TriggerSexToy(sexToysFunctions[0], 0.5);
        }

        public string debugString()
        {
            return "test";
        }

    }
}
