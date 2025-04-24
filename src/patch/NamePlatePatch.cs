using System.Linq;
using System.Reflection;
using HarmonyLib;
using TMPro;

namespace LevelDisplay.patch
{
	[HarmonyPatch(typeof(NPC), "Start")]
	public static class NamePlatePatch
	{
		// NPC List
		private static readonly string[] _bankNpcs = { "Prestigio Valusha", "Validus Greencent", "Comstock Retalio", "Summoned: Pocket Rift" };
		private static readonly string[] _otherNpcs = { "Thella Steepleton", "Goldie Retalio" };

		[HarmonyPostfix]
		public static void Postfix(NPC __instance) {
			var myStats = typeof(NPC).GetField("MyStats", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(__instance) as Stats;
			var character = typeof(NPC).GetField("Myself", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(__instance) as Character;
			var namePlate = __instance.NamePlate?.GetComponent<TextMeshPro>();

			if (myStats == null || character == null || namePlate == null)
				return;

			var playerLevel = GameData.PlayerControl.Myself.MyStats.Level;

			if (character.MyNPC.SimPlayer || IsMob(__instance)) {
				if (LevelDisplayPlugin.DisplaySimPlayerLevelAboveHead.Value) {
					var num = LevelDisplayPlugin.DisplayLevelAsOffset.Value ? (myStats.Level - playerLevel) : myStats.Level;
					namePlate.text = __instance.NPCName + " [" + num + "]";

					if (character.MyNPC.SimPlayer && __instance.GuildName != "") {
						namePlate.text = namePlate.text + "\n<" + __instance.GuildName + ">";
					}
				}

				return;
			}
		}

		public static bool IsMob(NPC npc)
		{
			var character = typeof(NPC).GetField("Myself", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(npc) as Character;

			if (character == null)
				return false;

			return !(npc.SimPlayer
				|| character.TryGetComponent<NPCDialogManager>(out _)
				|| character.isVendor
				|| _bankNpcs.Contains(character.MyNPC.NPCName)
				|| _otherNpcs.Contains(character.MyNPC.NPCName)
				|| character.MiningNode);
		}
	}
}