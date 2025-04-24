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
        
        DisplayMobLevelAboveHead = Config.Bind(
            "Display",
            "DisplayMobLevelAboveHead",
            true,
            "Whether to display levels for Mobs."
        );
        
        DisplayNamePlateRangeMultiplier = Config.Bind(
            "Display",
            "DisplayNameTagRangeMultiplier",
            1.5f,
            "Multiplier for distance to render mob name tags."
        );
    }
}
