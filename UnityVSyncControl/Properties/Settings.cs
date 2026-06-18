using UnityVSyncControl;
using BepInEx.Configuration;

internal class Settings
{
    public static ConfigEntry<int> iVsyncCount;
    public static ConfigEntry<int> iTargetFrameRate;
    public static ConfigEntry<bool> bFixedDeltaTimeSyncWithLimit;
    public static ConfigEntry<int> iFixedDeltaTimeClampDenominator;

    public static void Load()
    {
        iVsyncCount = Plugin.Config.Bind(
            "VSync",
            "VsyncCount",
            -1,
            new ConfigDescription(
                "Set up a VSync mode.\n" +
                "Setting this to '-1' will use the game setting.\n" +
                "Setting this to '0' will disable VSync.\n" +
                "Setting this to above zero will enable fractional VSync.",
            new AcceptableValueRange<int>(-1, 4))
        );

        iTargetFrameRate = Plugin.Config.Bind(
            "VSync",
            "TargetFrameRate",
            -1,
            "Set an arbitrary framerate limit.\n" +
            "Setting this to '-1' will use the game setting.\n" +
            "Setting this to '0' effectively unlocks the framerate (when 'VsyncCount' is 0 or disabled ingame)."
        );

        bFixedDeltaTimeSyncWithLimit = Plugin.Config.Bind(
            "VSync",
            "FixedDeltaTimeSyncWithLimit",
            false,
            "Synchronizes with the 'TargetFrameRate' if configured. If not, uses the current refresh rate.\n" +
            "Changing 'Time.fixedDeltaTime' can introduce bugs and also increase CPU consumption and affect performance.\n" +
            "Setting this to 'false' will use the game setting."
        );

        iFixedDeltaTimeClampDenominator = Plugin.Config.Bind(
            "VSync",
            "FixedDeltaTimeClampDenominator",
            0,
            "The maximum limit for the denominator.\n" +
            "If the denominator (framerate limit or current refresh rate) exceeds the value set here, it will be set to the lowest possible multiplier.\n" +
            "Divide by 2 until the result is less than or equal to the clamped denominator.\n" +
            "This option is designed to improve performance when the refresh rate or frame rate limit is very high.\n" +
            "The higher the denominator, the tighter the timing.\n" +
            "Setting this to '0' will use the current refresh rate or the 'TargetFrameRate' value (if set).\n" +
            "This setting only applies if 'FixedDeltaTimeSyncWithLimit' is enabled."
        );

        Plugin.Log.LogInfo("Describing loaded settings...");
        Plugin.Log.LogInfo("------------------------");
        Plugin.Log.LogInfo($"VsyncCount: {iVsyncCount.Value}");
        Plugin.Log.LogInfo($"TargetFrameRate: {iTargetFrameRate.Value}");
        Plugin.Log.LogInfo($"FixedDeltaTimeSyncWithLimit: {bFixedDeltaTimeSyncWithLimit.Value}");
        Plugin.Log.LogInfo($"FixedDeltaTimeClampDenominator: {iFixedDeltaTimeClampDenominator.Value}");
        Plugin.Log.LogInfo("------------------------");
    }
}
