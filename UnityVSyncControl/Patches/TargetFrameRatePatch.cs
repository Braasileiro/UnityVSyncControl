using HarmonyLib;
using UnityEngine;

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
            Plugin.Log.LogInfo($"Target framerate: {value} -> {Settings.iFrameRateLimit.Value}");

            value = Settings.iFrameRateLimit.Value;

            _targetFrameRate = value;

            if (Settings.bSyncWithLimit.Value)
            {
                FixedDeltaTimePatch.Update();
            }
        }

        return true;
    }
}
