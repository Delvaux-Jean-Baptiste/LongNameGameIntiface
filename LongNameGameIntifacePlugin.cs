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

        //Grope
        public static string IntGropeToyFunctionKey = "Grope Sex Toy Function";
        public static string DoubleGropeMultiplierKey = "Grope multiplier";
        public static string FloatGropeDurationKey = "Grope Duration (sec)";

        //Hit
        public static string IntHitToyFunctionKey = "Hit Sex Toy Function";
        public static string DoubleHitMultiplierKey = "Hit multiplier";
        public static string FloatHitDurationKey = "Hit Duration (sec)";

        //Sex
        public static string IntSexToyFunctionKey = "Sex Sex Toy Function";
        public static string DoubleSexMultiplierKey = "Sex multiplier";

        //Capture
        public static string IntCaptureToyFunctionKey = "Capture Sex Toy Function";
        public static string DoubleCaptureMultiplierKey = "Capture multiplier";

        public static string KeyboardConnectIntifaceKey = "Connect Intiface";
        public static string KeyboardStartIntifaceKey = "StartFollowingToys";
        public static string KeyboardTestKey = "Test";
        public static string booldebugLogsKey = "Enable logs debug";

        public static IntifaceClient intifaceClient = new IntifaceClient();

        // Configuration entries. Static, so can be accessed directly elsewhere in code via
        // e.g.
        // float myFloat = LongNameGameIntifacePlugin.FloatExample.Value;
        
        //Grope
        public static ConfigEntry<int> IntGropeToyFunction;
        public static ConfigEntry<double> DoubleGropeMultiplier;
        public static ConfigEntry<float> FloatGropeDuration;

        //Hit
        public static ConfigEntry<int> IntHitToyFunction;
        public static ConfigEntry<double> DoubleHitMultiplier;
        public static ConfigEntry<float> FloatHitDuration;

        //Sex
        public static ConfigEntry<int> IntSexToyFunction;
        public static ConfigEntry<double> DoubleSexMultiplier;

        //Capture
        public static ConfigEntry<int> IntCaptureToyFunction;
        public static ConfigEntry<double> DoubleCaptureMultiplier;

        public static ConfigEntry<bool> booldebugLogs;
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

            //Grope
            IntGropeToyFunction = Config.Bind("Grope",
                IntGropeToyFunctionKey,
                0,
                new ConfigDescription("Id of toy function",
                    new AcceptableValueRange<int>(0, 10)));

            DoubleGropeMultiplier = Config.Bind("Grope",
                DoubleGropeMultiplierKey,
                0.25,
                new ConfigDescription("Sex Toy Power",
                    new AcceptableValueRange<double>(0f, 10f)));

            FloatGropeDuration = Config.Bind("Grope",    // The section under which the option is shown
                FloatGropeDurationKey,                            // The key of the configuration option
                    2.0f,                            // The default value
                    new ConfigDescription("Example float configuration setting.",         // Description that appears in Configuration Manager
                        new AcceptableValueRange<float>(0.0f, 10.0f)));     // Acceptable range, enabled slider and validation in Configuration Manager

            //Hit
            IntHitToyFunction = Config.Bind("Hit",
                IntHitToyFunctionKey,
                0,
                new ConfigDescription("Id of toy function",
                    new AcceptableValueRange<int>(0, 10)));

            DoubleHitMultiplier = Config.Bind("Hit",
                DoubleHitMultiplierKey,
                0.25,
                new ConfigDescription("Sex Toy Power",
                    new AcceptableValueRange<double>(0f, 10f)));

            FloatHitDuration = Config.Bind("Hit",    // The section under which the option is shown
                FloatHitDurationKey,                            // The key of the configuration option
                    2.0f,
                    new ConfigDescription("Example float configuration setting.",         // Description that appears in Configuration Manager
                        new AcceptableValueRange<float>(0.0f, 10.0f)));     // Acceptable range, enabled slider and validation in Configuration Manager


            //Sex
            IntSexToyFunction = Config.Bind("Sex",
                IntSexToyFunctionKey,
                0,
                new ConfigDescription("Id of toy function",
                    new AcceptableValueRange<int>(0, 10)));

            DoubleSexMultiplier = Config.Bind("Sex",
                DoubleSexMultiplierKey,
                0.25,
                new ConfigDescription("Sex Toy Power",
                    new AcceptableValueRange<double>(0f, 10f)));

            //Capture
            IntCaptureToyFunction = Config.Bind("Capture",
                IntCaptureToyFunctionKey,
                0,
                new ConfigDescription("Id of toy function",
                    new AcceptableValueRange<int>(0, 10)));

            DoubleCaptureMultiplier = Config.Bind("Capture",
                DoubleCaptureMultiplierKey,
                0.25,
                new ConfigDescription("Sex Toy Power",
                    new AcceptableValueRange<double>(0f, 10f)));

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

            booldebugLogs = Config.Bind("General",
                booldebugLogsKey,
                false,
                new ConfigDescription("debug logs"));

            // Add listeners methods to run if and when settings are changed by the player.
            // TODO Change this code or remove the code if not required.
            IntGropeToyFunction.SettingChanged += ConfigSettingChanged;
            DoubleGropeMultiplier.SettingChanged += ConfigSettingChanged;
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
                Logger.LogInfo($"Intiface Connection attempt");
                intifaceClient.ConnectIntiface(); //Proceed properly
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
                Logger.LogInfo($"Start detecting");
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
