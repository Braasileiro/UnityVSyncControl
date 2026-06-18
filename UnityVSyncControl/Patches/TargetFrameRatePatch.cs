using HarmonyLib;
using UnityEngine;
using static Settings;

namespace UnityVSyncControl.Patches;

internal class TargetFrameRatePatch
{
    private static int _targetFrameRate = -1;

    [HarmonyPatch(typeof(Application), nameof(Application.targetFrameRate), MethodType.Setter)]
    [HarmonyPrefix]
    public static bool TargetFrameRatePrefix(ref int value)
    {
        if (value != _targetFrameRate)
        {
            Plugin.Log.LogInfo($"Target framerate: {value} -> {iTargetFrameRate.Value}");

            value = iTargetFrameRate.Value;

            _targetFrameRate = value;

            if (bFixedDeltaTimeSyncWithLimit.Value)
            {
                FixedDeltaTimePatch.Update();
            }
        }

        return true;
    }
}
