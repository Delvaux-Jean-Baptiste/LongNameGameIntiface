using HarmonyLib;
using System;

namespace LongNameGameIntiface.Patches
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required

    /// <summary>
    /// Sample Harmony Patch class. Suggestion is to use one file per patched class
    /// though you can include multiple patch classes in one file.
    /// Below is included as an example, and should be replaced by classes and methods
    /// for your mod.
    /// </summary>
    [HarmonyPatch(typeof(GirlsScript))]
    internal class GirlsScriptPatches
    {

        private static int girlFuckedNow = 0;
        private static int girlCapturedNow = 0;
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(GirlsScript.ChangeIsHavingSex))]
        [HarmonyPrefix]
        public static void ChangeIsHavingSex_Prefix(GirlsScript __instance,bool which, EnemiesEnum thisEnemy)
        {
            if (isSTManagerInitiated())
            {
                LongNameGameIntifacePlugin.Log.LogInfo($"In ChangeIsHavingSex ${__instance.GetGirlName()}");
                if (which)
                {
                    if (!(girlFuckedNow >= 3))
                        girlFuckedNow++;
                    if (girlFuckedNow == 1)
                    {
                        uint func = Convert.ToUInt32(LongNameGameIntifacePlugin.IntSexToyFunction.Value);
                        double magnitude = LongNameGameIntifacePlugin.DoubleSexMultiplier.Value;
                        LongNameGameIntifacePlugin.stManager.addToyDuration(func, magnitude, -1f);
                        LongNameGameIntifacePlugin.Log.LogInfo($"Trigger Toy now {LongNameGameIntifacePlugin.DoubleSexMultiplier.Value} for Infinite seconds. Girl Fucked {girlFuckedNow}");
                    }
                }
                else
                {
                    if (!(girlFuckedNow <= 0))
                        girlFuckedNow--;

                    if (girlFuckedNow == 0)
                    {
                        uint func = Convert.ToUInt32(LongNameGameIntifacePlugin.IntSexToyFunction.Value);
                        double magnitude = LongNameGameIntifacePlugin.DoubleSexMultiplier.Value;
                        LongNameGameIntifacePlugin.stManager.removeToyDuration(func, magnitude, -1f);
                        LongNameGameIntifacePlugin.Log.LogInfo($"Trigger Toy now {LongNameGameIntifacePlugin.DoubleSexMultiplier.Value} for Infinite seconds. Girl Fucked {girlFuckedNow}");
                    }
                }
            }
        }


        [HarmonyPatch(nameof(GirlsScript.ChangeIsCaptured))]
        [HarmonyPrefix]
        public static void ChangeIsCaptured_Prefix(GirlsScript __instance, bool which)
        {
            if (isSTManagerInitiated())
            {
                LongNameGameIntifacePlugin.Log.LogInfo($"In ChangeIsHavingCapture ${__instance.GetGirlName()}");
                if (which)
                {
                    if (!(girlCapturedNow >= 3))
                        girlCapturedNow++;
                    if (girlCapturedNow == 1)
                    {
                        uint func = Convert.ToUInt32(LongNameGameIntifacePlugin.IntCaptureToyFunction.Value);
                        double magnitude = LongNameGameIntifacePlugin.DoubleCaptureMultiplier.Value;
                        LongNameGameIntifacePlugin.stManager.addToyDuration(func, magnitude, -1f);
                        LongNameGameIntifacePlugin.Log.LogInfo($"Trigger Toy now {LongNameGameIntifacePlugin.DoubleCaptureMultiplier.Value} for Infinite seconds. Girl Fucked {girlFuckedNow}");
                    }
                }
                else
                {
                    if (!(girlCapturedNow <= 0))
                        girlCapturedNow--;

                    if (girlCapturedNow == 0)
                    {
                        uint func = Convert.ToUInt32(LongNameGameIntifacePlugin.IntCaptureToyFunction.Value);
                        double magnitude = LongNameGameIntifacePlugin.DoubleCaptureMultiplier.Value;
                        LongNameGameIntifacePlugin.stManager.removeToyDuration(func, magnitude, -1f);
                        LongNameGameIntifacePlugin.Log.LogInfo($"Trigger Toy now {LongNameGameIntifacePlugin.DoubleCaptureMultiplier.Value} for Infinite seconds. Girl Fucked {girlFuckedNow}");
                    }
                }
            }
        }

        [HarmonyPatch(nameof(GirlsScript.ChangeStamina))]
        [HarmonyPrefix]
        public static void ChangeStamina_Prefix(float amount)
        {

            LongNameGameIntifacePlugin.Log.LogInfo($"Test 1");
            //TODO Check if sex Toy is connected
            if (isSTManagerInitiated()) 
            {

                LongNameGameIntifacePlugin.Log.LogInfo($"Test 2");
                if (amount < 0f)
                {

                    LongNameGameIntifacePlugin.Log.LogInfo($"Test 3");
                    uint func = Convert.ToUInt32(LongNameGameIntifacePlugin.IntHitToyFunction.Value);
                    double magnitude = LongNameGameIntifacePlugin.DoubleHitMultiplier.Value;
                    float duration = LongNameGameIntifacePlugin.FloatHitDuration.Value;
                    LongNameGameIntifacePlugin.stManager.addToyDuration(func, magnitude, duration);
                    LongNameGameIntifacePlugin.Log.LogInfo($"Trigger Toy now {LongNameGameIntifacePlugin.DoubleHitMultiplier.Value} for {LongNameGameIntifacePlugin.FloatHitDuration.Value} seconds");
                }
            }
        }

        [HarmonyPatch(nameof(GirlsScript.ChangeSexResistance))]
        [HarmonyPrefix]
        public static void ChangeSexResistance_Prefix(float amount)
        {
            //TODO Check if sex Toy is connected

            LongNameGameIntifacePlugin.Log.LogInfo($"Test 4");
            if (isSTManagerInitiated())
            {

                LongNameGameIntifacePlugin.Log.LogInfo($"Test 5");
                if (amount < 0f)
                {

                    LongNameGameIntifacePlugin.Log.LogInfo($"Test 6");
                    uint func = Convert.ToUInt32(LongNameGameIntifacePlugin.IntGropeToyFunction.Value);
                    double magnitude = LongNameGameIntifacePlugin.DoubleGropeMultiplier.Value;
                    float duration = LongNameGameIntifacePlugin.FloatGropeDuration.Value;
                    LongNameGameIntifacePlugin.stManager.addToyDuration(func, magnitude, duration);
                    LongNameGameIntifacePlugin.Log.LogInfo($"Trigger Toy now {LongNameGameIntifacePlugin.DoubleGropeMultiplier.Value} for {LongNameGameIntifacePlugin.FloatGropeDuration.Value} seconds");
                }
            }
        }

        private static bool isSTManagerInitiated()
        {
            return LongNameGameIntifacePlugin.stManager != null;
        }

        /// <summary>
        /// Patches the Player Awake method with postfix code.
        /// </summary>
        /// <param name="__instance"></param>
        //[HarmonyPatch(nameof(PlayerScript.Awake))]
        //[HarmonyPostfix]
        //public static void Awake_Postfix(PlayerScript __instance)
        //{
        //    LongNameGameIntifacePlugin.Log.LogInfo("In Player Awake method Postfix.");
        //}
    }
}