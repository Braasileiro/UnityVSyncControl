using UnityEngine;
using static Settings;

namespace UnityVSyncControl.Patches;

internal class FixedDeltaTimePatch
{
    private static float _lastFixedDeltaTime = -1;

    public static void Update()
    {
        // Apply only if changed
        if (Time.fixedDeltaTime != _lastFixedDeltaTime)
        {
            float targetRate = (iTargetFrameRate.Value > 0)
                ? iTargetFrameRate.Value
                : Screen.currentResolution.refreshRate;

            float physicsRate = GetClampedPhysicsRate(targetRate, iFixedDeltaTimeClampDenominator.Value);

            if (physicsRate == -1)
            {
                Plugin.Log.LogWarning($"The clamped physics rate of '{physicsRate}' is less than or equal to 0. Time.fixedDeltaTime will not be set.");
            }

            Time.fixedDeltaTime = 1f / physicsRate;

            _lastFixedDeltaTime = Time.fixedDeltaTime;

            Plugin.Log.LogInfo($"Physics rate: Target {targetRate}Hz -> {physicsRate}Hz (Delta: {Time.fixedDeltaTime})");
        }
    }

    private static float GetClampedPhysicsRate(float targetRate, float maxAllowedRate)
    {
        // Safety check
        if (targetRate <= 0)
        {
            return -1;
        }

        float denominator = targetRate;

        if (maxAllowedRate > 0)
        {
            while (denominator > maxAllowedRate)
            {
                denominator /= 2f;
            }
        }

        return denominator;
    }
}
