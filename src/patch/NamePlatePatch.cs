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
        public static void Postfix(NPC __instance)
        {
            var myStats = typeof(NPC).GetField("MyStats", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(__instance) as Stats;
            var character = typeof(NPC).GetField("Myself", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(__instance) as Character;
            var namePlate = __instance.NamePlate?.GetComponent<TextMeshPro>();

            if (myStats == null || character == null || namePlate == null)
                return;
                
            if (LevelDisplayPlugin.DisplaySimPlayerLevelAboveHead.Value && character.MyNPC.SimPlayer)
            {
                var playerLevel = GameData.PlayerControl.Myself.MyStats.Level;
                var displayText = LevelDisplayPlugin.DisplaySimLevelAsOffset.Value 
                    ? (myStats.Level - playerLevel) : myStats.Level;
                    
                namePlate.text = __instance.NPCName + " [" + displayText + "]";
                        
                if (__instance.GuildName != "")
                {
                    namePlate.text = namePlate.text + "\n<" + __instance.GuildName + ">";
                }

                return;
            }

            if (LevelDisplayPlugin.DisplayMobLevelAboveHead.Value && IsMob(__instance))
            {
                var playerLevel = GameData.PlayerControl.Myself.MyStats.Level;
                var displayText = LevelDisplayPlugin.DisplayMobLevelAsOffset.Value 
                    ? (myStats.Level - playerLevel) : myStats.Level;
                    
                namePlate.text = __instance.NPCName + " [" + displayText + "]";
            }
        }

        public static bool IsMob(NPC npc)
        {
            var characterField = typeof(NPC).GetField("Myself", BindingFlags.NonPublic | BindingFlags.Instance);

            if (characterField == null)
                return false;
                        
            var character = characterField.GetValue(npc) as Character;

            if (character == null)
                return false;
                        
            var npcDialogueManager = character.GetComponent<NPCDialogManager>();
                
            if (npc.SimPlayer || 
                npcDialogueManager != null || 
                character.isVendor ||
                _bankNpcs.Contains(character.MyNPC.NPCName) ||
                _otherNpcs.Contains(character.MyNPC.NPCName) ||
                character.MiningNode)
            {
                return false;
            }

            return true;
        }
    }
}