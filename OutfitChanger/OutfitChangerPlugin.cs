using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
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

    public static ManualLogSource Logger { get; set; }

    public override void Load()
    {
        Logger = Log;

        var path = Path.GetFullPath("Outfits", Application.persistentDataPath);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        OutfitFolder = Config.Bind("General", "OutfitFolder", path, "The folder where outfits are loaded from.");
        Harmony.PatchAll();
    }
}
