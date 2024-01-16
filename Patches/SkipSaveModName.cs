using HarmonyLib;
using System.Linq;

namespace MultiplayerModPatch.Patches;

[HarmonyPatch(typeof(ManSaveGame.SaveInfo), nameof(ManSaveGame.SaveInfo.UpdateSaveInfo))]
internal class SkipSaveModName {

	public static void Postfix(ref string ___m_ModNames) {
		var contents = Mod.GetContents(Mod.ManModsHelper.m_CurrentSession);
		if (contents != null) {
			// セーブデータには保存されないようにする
			___m_ModNames = string.Join(
				",",
				___m_ModNames
					.Split(',')
					.Where(x => x != $"[{contents.ModName}:{contents.m_WorkshopId}]")
			);
		}
	}

}
