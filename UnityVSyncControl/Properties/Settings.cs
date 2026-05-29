using UnityVSyncControl;
using BepInEx.Configuration;

internal class Settings
{
    public static ConfigEntry<int> iVsyncCount;
    public static ConfigEntry<int> iFrameRateLimit;
    public static ConfigEntry<bool> bSyncWithLimit;
    public static ConfigEntry<int> iClampDenominator;

    public static void Load()
    {
        // VSync
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

        iFrameRateLimit = Plugin.Config.Bind(
            "VSync",
            "FrameRateLimit",
            -1,
            "Set an arbitrary framerate limit.\n" +
            "Setting this to '-1' will use the game setting.\n" +
            "Setting this to '0' effectively unlocks the framerate (when 'VsyncCount' is 0 or disabled ingame)."
        );

        // Time.fixedDeltaTime
        bSyncWithLimit = Plugin.Config.Bind(
            "Time.fixedDeltaTime",
            "SyncWithLimit",
            false,
            "Synchronizes with the 'FrameRateLimit' if configured. If not, uses the current refresh rate.\n" +
            "Changing 'Time.fixedDeltaTime' can introduce bugs and also increase CPU consumption and affect performance.\n" +
            "Setting this to 'false' will use the game setting."
        );

        iClampDenominator = Plugin.Config.Bind(
            "Time.fixedDeltaTime",
            "ClampDenominator",
            0,
            "The maximum limit for the denominator.\n" +
            "If the denominator (framerate limit or current refresh rate) exceeds the value set here, it will be set to the lowest possible multiplier.\n" +
            "Divide by 2 until the result is less than or equal to the clamped denominator.\n" +
            "This option is designed to improve performance when the refresh rate or frame rate limit is very high.\n" +
            "The higher the denominator, the tighter the timing.\n" +
            "Setting this to '0' will use the current refresh rate or the 'FrameRateLimit' value (if set).\n" +
            "This setting only applies if 'SyncWithLimit' is enabled."
        );

        Plugin.Log.LogInfo("Describing loaded settings...");
        Plugin.Log.LogInfo("------------------------");
        Plugin.Log.LogInfo($"VsyncCount: {iVsyncCount.Value}");
        Plugin.Log.LogInfo($"FrameRateLimit: {iFrameRateLimit.Value}");
        Plugin.Log.LogInfo($"SyncWithLimit: {bSyncWithLimit.Value}");
        Plugin.Log.LogInfo($"ClampDenominator: {iClampDenominator.Value}");
        Plugin.Log.LogInfo("------------------------");
    }
}
