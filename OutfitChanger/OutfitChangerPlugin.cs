using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace OutfitChanger;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]

public partial class OutfitChangerPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public static ConfigEntry<string> OutfitFolder { get; private set; }

    public override void Load()
    {
        OutfitFolder = this.Config.Bind("General", "OutfitFolder", $"{Application.persistentDataPath}\\Outfits", "The folder where outfits are loaded from.");
        Harmony.PatchAll();
    }
}
