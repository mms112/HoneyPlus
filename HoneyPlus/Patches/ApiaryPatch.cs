using HarmonyLib;

namespace HoneyPlus
{
    [HarmonyPatch(typeof(CraftingStation), nameof(CraftingStation.CheckUsable))]
    public static class CraftingStation_CheckUsable_Patch
    {
        private static void Postfix(CraftingStation __instance, ref bool __result, bool showMessage)
        {
            if (!__result)
                return;

            if (__instance.m_name == "$custom_piece_apiary")
            {
                if ((Heightmap.FindBiome(__instance.transform.position) & (Heightmap.Biome.Meadows | Heightmap.Biome.BlackForest | Heightmap.Biome.Plains)) == 0)
                {
                    if (showMessage)
                    {
                        Player.m_localPlayer.Message(MessageHud.MessageType.Center, "$piece_beehive_area");
                    }
                    __result = false;
                    return;
                }

                Cover.GetCoverForPoint(__instance.m_roofCheckPoint.position, out var coverPercentage, out var _);
                if (coverPercentage >= 0.30f)
                {
                    if (showMessage)
                    {
                        Player.m_localPlayer.Message(MessageHud.MessageType.Center, "$piece_beehive_freespace");
                    }
                    __result = false;
                    return;
                }

                if (!EnvMan.IsDaylight())
                {
                    if (showMessage)
                    {
                        Player.m_localPlayer.Message(MessageHud.MessageType.Center, "$piece_beehive_sleep");
                    }
                    __result = false;
                    return;
                }
            }
        }
    }
}