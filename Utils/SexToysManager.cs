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
        int[] magnitudes;
        private List<SexToyTriggerDuration> sexToys = new List<SexToyTriggerDuration>();
        private IntifaceClient client;
        private List<SexToyFunction> functions;

        public SexToysManager(IntifaceClient client, List<SexToyFunction> sexToyFunctions)
        {
            this.client = client;
            this.functions = sexToyFunctions;
            foreach (SexToyFunction function in functions)
            {
                SexToyTriggerDuration baseTrigger = new SexToyTriggerDuration(function, 0, -1f);
                sexToys.Add(baseTrigger);
            }
        }

        public async void loop()
        {
            magnitudes = new int[sexToys.Count];
            debugTriggerToy();
            while (false && sexToys.Count>0)
            {
                
                sexToys.RemoveAll(st => st.Duration != -1f && st.Duration <= 0f);
                LongNameGameIntifacePlugin.Log.LogInfo($"Debug function total {sexToys.Count}");


                foreach (SexToyFunction f in functions)
                {
                    int newMagnitude = sexToys.Where(st => st.functionId == f.id).Max(st => st.Magnitude);
                    LongNameGameIntifacePlugin.Log.LogInfo($"Debug magnitude {newMagnitude} || {magnitudes[f.getDeviceID]}");
                    triggerToy(newMagnitude, f.getDeviceID);
                }

                foreach (var st in sexToys) { st.TickTime(); }
                Thread.Sleep(100);
            }
        }

        public void addToyDuration (uint id, int magnitude,  float duration)
        {
            sexToys.Add(new SexToyTriggerDuration(functions.First(f => f.getDeviceID == id), magnitude, duration));
        }

        private void triggerToy (int newMagnitude, uint idFunction)
        {
            if(newMagnitude != magnitudes[idFunction])
            {
                client.TriggerSexToy(functions[Convert.ToInt32(idFunction)], newMagnitude);
            }
        }

        private void debugTriggerToy()
        {
            client.TriggerSexToy(functions[0], 5);
        }

        public string debugString()
        {
            return "test";
        }

    }
}
