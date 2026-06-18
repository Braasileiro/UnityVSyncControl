using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using static Settings;

namespace UnityVSyncControl
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        // Instances
        internal static new ManualLogSource Log;
        internal static new ConfigFile Config;
        internal static Harmony HarmonyInstance;

        public override void Load()
        {
            // Globals
            Log = base.Log;
            Config = base.Config;
            HarmonyInstance = new(MyPluginInfo.PLUGIN_GUID);

            Log.LogInfo($"{MyPluginInfo.PLUGIN_NAME} loaded.");

            // Settings
            Settings.Load();

            Log.LogInfo("Describing current game settings...");
            Log.LogInfo("------------------------");
            Log.LogInfo($"QualitySettings.vSyncCount: {QualitySettings.vSyncCount}");
            Log.LogInfo($"Application.targetFrameRate: {Application.targetFrameRate}");
            Log.LogInfo($"Time.fixedDeltaTime: {Time.fixedDeltaTime}");
            Log.LogInfo("------------------------");

            if (iVsyncCount.Value > -1)
            {
                HarmonyInstance.PatchAll(typeof(Patches.VSyncPatch));

                // Initial setter
                QualitySettings.vSyncCount = QualitySettings.vSyncCount;
            }

            if (iTargetFrameRate.Value > -1)
            {
                HarmonyInstance.PatchAll(typeof(Patches.TargetFrameRatePatch));

                // Initial setter
                Application.targetFrameRate = Application.targetFrameRate;
            }
        }
    }
}
