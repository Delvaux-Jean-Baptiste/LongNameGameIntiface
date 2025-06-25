using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LongNameGameIntiface.Model;
using LongNameGameIntiface.Utils;
using LongNameGameIntiface.WebClient;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace LongNameGameIntiface
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class LongNameGameIntifacePlugin : BaseUnityPlugin
    {
        // Mod specific details. MyGUID should be unique, and follow the reverse domain pattern
        // e.g.
        // com.mynameororg.pluginname
        // Version should be a valid version string.
        // e.g.
        // 1.0.0
        private const string MyGUID = "com.Elgate.LongNameGameIntiface";
        private const string PluginName = "LongNameGameIntiface";
        private const string VersionString = "1.0.0";

        // Config entry key strings
        // These will appear in the config file created by BepInEx and can also be used
        // by the OnSettingsChange event to determine which setting has changed.
        public static string IntGropeToyFunctionKey = "Grope Sex Toy Function";
        public static string IntGropeMultiplierKey = "Grope multiplier";
        public static string FloatGropeDurationKey = "Grope Duration (sec)";
        public static string KeyboardConnectIntifaceKey = "Connect Intiface";
        public static string KeyboardStartIntifaceKey = "StartFollowingToys";
        public static string KeyboardTestKey = "Test";

        public static IntifaceClient intifaceClient = new IntifaceClient();

        // Configuration entries. Static, so can be accessed directly elsewhere in code via
        // e.g.
        // float myFloat = LongNameGameIntifacePlugin.FloatExample.Value;
        // TODO Change this code or remove the code if not required.
        public static ConfigEntry<int> IntGropeToyFunction;
        public static ConfigEntry<int> IntGropeMultiplier;
        public static ConfigEntry<float> FloatGropeDuration;
        public static ConfigEntry<KeyboardShortcut> KeyboardConnectIntiface;
        public static ConfigEntry<KeyboardShortcut> KeyboardStartIntiface;
        public static ConfigEntry<KeyboardShortcut> KeyboardTest;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);
        public static SexToysManager stManager;

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {

            IntGropeToyFunction = Config.Bind("Grope",
                IntGropeToyFunctionKey,
                0,
                new ConfigDescription("Id of toy function",
                    new AcceptableValueRange<int>(0, 10)));

            IntGropeMultiplier = Config.Bind("Grope",
                IntGropeMultiplierKey,
                5,
                new ConfigDescription("Sex Toy Power",
                    new AcceptableValueRange<int>(0, 20)));

            FloatGropeDuration = Config.Bind("Grope",    // The section under which the option is shown
                FloatGropeDurationKey,                            // The key of the configuration option
                    1.0f,                            // The default value
                    new ConfigDescription("Example float configuration setting.",         // Description that appears in Configuration Manager
                        new AcceptableValueRange<float>(0.0f, 10.0f)));     // Acceptable range, enabled slider and validation in Configuration Manager

            // Keyboard shortcut setting example
            // TODO Change this code or remove the code if not required.
            KeyboardConnectIntiface = Config.Bind("General",
                KeyboardConnectIntifaceKey,
                new KeyboardShortcut(KeyCode.A, KeyCode.LeftControl));

            KeyboardStartIntiface = Config.Bind("General",
                KeyboardStartIntifaceKey,
                new KeyboardShortcut(KeyCode.Z, KeyCode.LeftControl));

            KeyboardTest = Config.Bind("General",
                KeyboardTestKey,
                new KeyboardShortcut(KeyCode.E, KeyCode.LeftControl));

            // Add listeners methods to run if and when settings are changed by the player.
            // TODO Change this code or remove the code if not required.
            IntGropeToyFunction.SettingChanged += ConfigSettingChanged;
            IntGropeMultiplier.SettingChanged += ConfigSettingChanged;
            FloatGropeDuration.SettingChanged += ConfigSettingChanged;
            KeyboardConnectIntiface.SettingChanged += ConfigSettingChanged;

            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            // Set logger
            Log = Logger;

        }

        /// <summary>
        /// Code executed every frame. See below for an example use case
        /// to detect keypress via custom configuration.
        /// </summary>
        // TODO - Add your code here or remove this section if not required.
        private void Update()
        {
            if (LongNameGameIntifacePlugin.KeyboardConnectIntiface.Value.IsDown())
            {
                // Code here to do something on keypress
                intifaceClient.ConnectIntiface(); //Proceed properly
                Logger.LogInfo($"Keypress detected!");
            }

            if (LongNameGameIntifacePlugin.KeyboardTest.Value.IsDown())
            {
                Logger.LogInfo(intifaceClient == null);
                if(intifaceClient != null)
                {

                    Logger.LogInfo(intifaceClient.sexToyFunctions == null);
                }
                Logger.LogInfo($"Test {intifaceClient.sexToyFunctions.Count()}");
            }

            if (LongNameGameIntifacePlugin.KeyboardStartIntiface.Value.IsDown())
            {
                Logger.LogInfo(stManager);
                stManager = new SexToysManager(intifaceClient, intifaceClient.sexToyFunctions.Cast<SexToyFunction>().ToList());
                Thread thr = new Thread(stManager.loop);
                thr.Start();
            }
        }

        /// <summary>
        /// Method to handle changes to configuration made by the player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            // Check if null and return
            if (settingChangedEventArgs == null)
            {
                return;
            }

            // Example Float Shortcut setting changed handler
            //if (settingChangedEventArgs.ChangedSetting.Definition.Key == FloatExampleKey)
            //{
            //    // TODO - Add your code here or remove this section if not required.
            //    // Code here to do something with the new value
            //}

            // Example Int Shortcut setting changed handler
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == IntGropeToyFunctionKey)
            {
                // TODO - Add your code here or remove this section if not required.
                // Code here to do something with the new value
            }

            // Example Keyboard Shortcut setting changed handler
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KeyboardConnectIntifaceKey)
            {
                KeyboardShortcut newValue = (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;

                // TODO - Add your code here or remove this section if not required.
                // Code here to do something with the new value
            }
        }
    }

    
}
