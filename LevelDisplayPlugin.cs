using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace LevelDisplay;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class LevelDisplayPlugin : BaseUnityPlugin
{
    private Harmony _harmony;
    
    public static ConfigEntry<bool> DisplaySimPlayerLevelAboveHead;
    public static ConfigEntry<bool> DisplayMobLevelAboveHead;
    public static ConfigEntry<bool> DisplayMobLevelAsOffset;
    public static ConfigEntry<bool> DisplaySimLevelAsOffset;
    public static ConfigEntry<float> DisplayNamePlateRangeMultiplier;
        
    private void Awake()
    {
        InitializeConfigurations();
        
        // Apply all Patches
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        
        _harmony.PatchAll();
    }

    private void InitializeConfigurations()
    {
        // Bind config entry (section, key, default value, description)
        DisplaySimPlayerLevelAboveHead = Config.Bind(
            "Display",
            "DisplaySimPlayerLevelAboveHead",
            false,
            "Whether to display levels for SimPlayer NPCs."
        );
        
        DisplaySimLevelAsOffset = Config.Bind(
            "Display",
            "DisplaySimLevelAsOffset",
            false,
            "Instead of showing the actual sim level, show an offset from the player's level."
        );
        
        DisplayMobLevelAboveHead = Config.Bind(
            "Display",
            "DisplayMobLevelAboveHead",
            true,
            "Whether to display levels for Mobs."
        );
        
        DisplayMobLevelAsOffset = Config.Bind(
            "Display",
            "DisplayMobLevelAsOffset",
            false,
            "Instead of showing the actual mob level, show an offset from the player's level."
        );
        
        DisplayNamePlateRangeMultiplier = Config.Bind(
            "Display",
            "DisplayNameTagRangeMultiplier",
            1.5f,
            "Multiplier for distance to render mob name tags."
        );
    }
}
