using System.Reflection;
using HarmonyLib;

namespace LevelDisplay.patch
{

    [HarmonyPatch(typeof(PlayerControl), "Update")]
    public static class TargetNamePatch
    {
        
        [HarmonyPostfix]
        public static void Postfix(PlayerControl __instance)
        {
            var currentTarget = __instance.CurrentTarget;
            
            if (currentTarget != null && 
                currentTarget.transform.name != "Player" && 
                NamePlatePatch.IsMob(currentTarget.MyNPC))
            {
                var myStatsField = typeof(NPC).GetField("MyStats", BindingFlags.NonPublic | BindingFlags.Instance);

                if (myStatsField == null)
                {
                    return;
                }
                
                var myStats = myStatsField.GetValue(currentTarget.MyNPC) as Stats;

                if (myStats == null)
                {
                    return;
                }
                
                __instance.TargetName.text = __instance.CurrentTarget.transform.name + " (" + myStats.Level + ")";
            }
        }
    }
}