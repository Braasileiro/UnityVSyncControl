using HarmonyLib;
using UnityEngine;
using static Settings;

namespace UnityVSyncControl.Patches;

internal class VSyncPatch
{
    private static int _vSyncCount = -1;

    [HarmonyPatch(typeof(QualitySettings), nameof(QualitySettings.vSyncCount), MethodType.Setter)]
    [HarmonyPrefix]
    public static bool VSyncCountPrefix(ref int value)
    {
        if (value != _vSyncCount)
        {
            Plugin.Log.LogInfo($"VSync count: {value} -> {iVsyncCount.Value}");

            value = iVsyncCount.Value;

            _vSyncCount = value;

            if (bFixedDeltaTimeSyncWithLimit.Value)
            {
                FixedDeltaTimePatch.Update();
            }
        }

        return true;
    }
}
