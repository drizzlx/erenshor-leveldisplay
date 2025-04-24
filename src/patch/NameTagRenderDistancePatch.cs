using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace LevelDisplay.patch
{

    [HarmonyPatch(typeof(NPC), "HandleNameTag")]
    public static class NameTagRenderDistancePatch
    {
        
        [HarmonyPostfix]
        public static void Postfix(NPC __instance)
        {
            if (!__instance.SimPlayer)
            {
                switch (GameData.ShowNPCName)
                {
                    case 0:
                        if (!NamePlatePatch.IsMob(__instance))
                        {
                            return;
                        }
                        
                        if (!__instance.NamePlate.gameObject.activeSelf && 
                            Vector3.Distance(__instance.transform.position, GameData.GameCamPos.position) < 25.0 * LevelDisplayPlugin.DisplayNamePlateRangeMultiplier.Value)
                        {
                            __instance.NamePlate.transform.LookAt(GameData.GameCamPos);
                            __instance.NamePlate.gameObject.SetActive(true);
                            
                            break;
                        }
                        
                        if (!__instance.NamePlate.gameObject.activeSelf || 
                            Vector3.Distance(__instance.transform.position, GameData.GameCamPos.position) <= 25.0 * LevelDisplayPlugin.DisplayNamePlateRangeMultiplier.Value)
                            break;
                        
                        __instance.NamePlate.transform.LookAt(GameData.GameCamPos);
                        __instance.NamePlate.gameObject.SetActive(false);
                        
                        break;
                }
            }
        }
    }
}