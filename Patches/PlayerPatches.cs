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
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(GirlsScript.ChangeIsHavingSex))]
        [HarmonyPrefix]
        public static void ChangeIsHavingSex_Prefix(GirlsScript __instance,bool which, EnemiesEnum thisEnemy)
        {
            LongNameGameIntifacePlugin.Log.LogInfo($"In ChangeIsHavingSex ${__instance.GetGirlName()}");
        }

        [HarmonyPatch(nameof(GirlsScript.ChangeStamina))]
        [HarmonyPrefix]
        public static void ChangeStamina_Prefix(float amount)
        {
            //TODO Check if sex Toy is connected
            if (true) 
            {
                if(amount < 0f)
                {
                    LongNameGameIntifacePlugin.Log.LogInfo($"Trigger Toy now");
                }
            }
        }

        [HarmonyPatch(nameof(GirlsScript.ChangeSexResistance))]
        [HarmonyPrefix]
        public static void ChangeSexResistance_Prefix(float amount)
        {
            //TODO Check if sex Toy is connected
            if (true)
            {
                if (amount < 0f)
                {
                    uint func = Convert.ToUInt32(LongNameGameIntifacePlugin.IntGropeToyFunction.Value);
                    int magnitude = LongNameGameIntifacePlugin.IntGropeMultiplier.Value;
                    float duration = LongNameGameIntifacePlugin.FloatGropeDuration.Value;
                    LongNameGameIntifacePlugin.stManager.addToyDuration(func, magnitude, duration);
                    LongNameGameIntifacePlugin.Log.LogInfo($"Trigger Toy now {LongNameGameIntifacePlugin.IntGropeMultiplier.Value} for {LongNameGameIntifacePlugin.FloatGropeDuration.Value} seconds");
                }
            }
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