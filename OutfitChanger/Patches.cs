using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using HarmonyLib;
using UnityEngine;

namespace OutfitChanger;

[HarmonyPatch]

public class Patches
{
	static List<Customization> Outfits = [];

	static int CurrentPage = 0;

	[HarmonyPatch(typeof(PlayerCustomizationMenu), nameof(PlayerCustomizationMenu.Start))]
	[HarmonyPostfix]

	public static void Start()
	{
		Outfits = [];
		var files = Directory.GetFiles(OutfitChangerPlugin.OutfitFolder.Value, "*.amogus");

		foreach (var file in files)
		{
			var json = File.ReadAllText(file);

			var root = JsonSerializer.Deserialize<Root>(json);

			Outfits.Add(root.customization);
		}
	}

	[HarmonyPatch(typeof(PlayerCustomizationMenu), nameof(PlayerCustomizationMenu.Update))]

	public static void Postfix(PlayerCustomizationMenu __instance)
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			CurrentPage--;
			CurrentPage = CurrentPage < 0 ? Outfits.Count - 1 : CurrentPage;

			SetOutfit(Outfits[CurrentPage]);

			__instance.PreviewArea.UpdateFromLocalPlayer(PlayerMaterial.MaskType.None);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			CurrentPage++;
			CurrentPage = CurrentPage > Outfits.Count - 1 ? 0 : CurrentPage;

			SetOutfit(Outfits[CurrentPage]);

			__instance.PreviewArea.UpdateFromLocalPlayer(PlayerMaterial.MaskType.None);
		}
	}

	[HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
	[HarmonyPostfix]

	public static void Postfix(ModManager __instance)
	{
		__instance.ShowModStamp();
	}

	public static void SetOutfit(Customization outfit)
	{
		PlayerControl.LocalPlayer.CmdCheckColor((byte)outfit.colorID);
		PlayerControl.LocalPlayer.RpcSetHat(outfit.hat ?? "hat_NoHat");
		PlayerControl.LocalPlayer.RpcSetPet(outfit.pet ?? "pet_EmptyPet");
		PlayerControl.LocalPlayer.RpcSetSkin(outfit.skin ?? "skin_None");
		PlayerControl.LocalPlayer.RpcSetVisor(outfit.visor ?? "visor_EmptyVisor");
		PlayerControl.LocalPlayer.RpcSetNamePlate(outfit.namePlate ?? "nameplate_NoPlate");
		PlayerControl.LocalPlayer.CmdCheckName(outfit.name ?? "where name");
	}
}

public class Customization
{
	public string name { get; set; }

	public int colorID { get; set; }

	public string pet { get; set; }

	public string hat { get; set; }

	public string skin { get; set; }

	public string visor { get; set; }

	public string namePlate { get; set; }
}

public class Root
{
	public Customization customization { get; set; }
}
