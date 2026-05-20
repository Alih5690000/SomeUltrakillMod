using BepInEx;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using PluginConfig;
using PluginConfig.API;
using PluginConfig.API.Fields;


[BepInPlugin("me.fastshotgun", "Fast Shotgun", "1.0")]
public class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> FastShotgunEnabled;
    public static PluginConfigurator config;
    public BoolField isEnabled;
    void Awake()
    {
        FastShotgunEnabled=Config.Bind(
            "General", 
            "Enabled", true, 
            "Whether the mod is enabled or not."
        );
        config=PluginConfigurator.Create(
            "Fast Shotgun", 
            "me.fastshotgun"
        );
        isEnabled=new BoolField(
            config.rootPanel,
            "Enabled", 
            "field.isenabled", FastShotgunEnabled.Value, 
            true
        );
        isEnabled.onValueChange += (value) => {
            FastShotgunEnabled.Value=value.value;
        };
        isEnabled.value=FastShotgunEnabled.Value;
        Logger.LogInfo("Mod loaded!");
        var harmony = new Harmony("me.fastshotgun");
        harmony.PatchAll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Logger.LogInfo("Pressed F1");
        }
    }
};

[HarmonyPatch(typeof(Shotgun), "Update")]
class Shotgun_Patch
{
    static void Prefix(Shotgun __instance)
    {
        if (!Plugin.FastShotgunEnabled.Value) return;
        Traverse.Create(__instance)
            .Field("gunReady")
            .SetValue(true);
    }
}