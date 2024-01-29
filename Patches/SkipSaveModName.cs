using HarmonyLib;
using System.Linq;

namespace MultiplayerModPatch.Patches;

[HarmonyPatch(typeof(ManSaveGame.SaveInfo), nameof(ManSaveGame.SaveInfo.UpdateSaveInfo))]
internal class SkipSaveModName {

	public static void Postfix(ref string ___m_ModNames) {
		var currentModSession = Traverse.Create(ManMods.inst).Field<ModSessionInfo>("m_CurrentSession").Value;
		var contents = Mod.GetContents(currentModSession);
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
