using System.Linq;
using System.Reflection;
using HarmonyLib;
using TMPro;

namespace MobLevelDisplay.patch
{

    [HarmonyPatch(typeof(NPC), "Start")]
    public static class PlayerCombatPatch
    {
        // NPC List
        private static readonly string[] _bankNpcs = { "Prestigio Valusha", "Validus Greencent", "Comstock Retalio", "Summoned: Pocket Rift" };
        private static readonly string[] _otherNpcs = { "Thella Steepleton", "Goldie Retalio" };
        
        [HarmonyPostfix]
        public static void Postfix(NPC __instance)
        {
            var myStatsField = typeof(NPC).GetField("MyStats", BindingFlags.NonPublic | BindingFlags.Instance);
            var characterField = typeof(NPC).GetField("Myself", BindingFlags.NonPublic | BindingFlags.Instance);

            if (myStatsField != null && characterField != null)
            {
                var myStats = myStatsField.GetValue(__instance) as Stats;
                var character = characterField.GetValue(__instance) as Character;
                var namePlate = __instance.NamePlate?.GetComponent<TextMeshPro>();
                
                if (myStats == null || character == null || namePlate == null)
                    return;
                
                if (character.MyNPC.SimPlayer)
                {
                    namePlate.text = __instance.NPCName + " [" + myStats.Level + "]";

                    return;
                }
                
                var npcDialogueManager = character.GetComponent<NPCDialogManager>();

                if (npcDialogueManager != null)
                    return;
                
                if (!character.isVendor &&
                    !_bankNpcs.Contains(character.MyNPC.NPCName) &&
                    !_otherNpcs.Contains(character.MyNPC.NPCName) && 
                    !character.MiningNode)
                {
                    // Mobs
                    namePlate.text = __instance.NPCName + " [" + myStats.Level + "]";
                }
            }
        }
    }
}