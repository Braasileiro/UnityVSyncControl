using HarmonyLib;
using UnityEngine;

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
            Plugin.Log.LogInfo($"VSync count: {value} -> {Settings.iVsyncCount.Value}");

            value = Settings.iVsyncCount.Value;

            _vSyncCount = value;

            if (Settings.bSyncWithLimit.Value)
            {
                FixedDeltaTimePatch.Update();
            }
        }

        return true;
    }
}
