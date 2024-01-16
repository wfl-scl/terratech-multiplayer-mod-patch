using HarmonyLib;

namespace MultiplayerModPatch.Patches;

[HarmonyPatch(typeof(ManMods), nameof(ManMods.Save))]
internal class SkipSaveModSession {

	public static void Prefix(ModSessionInfo ___m_CurrentSession, out ModContents? __state) {
		var contents = Mod.GetContents(___m_CurrentSession);
		if (
			contents != null &&
			___m_CurrentSession.Mods.TryGetValue(contents.ModName, out var workshopId) == true &&
			workshopId == contents.m_WorkshopId.m_PublishedFileId
		) {
			// セーブデータには保存されないようにする
			___m_CurrentSession.Mods.Remove(contents.ModName);
			__state = contents;
		} else {
			__state = null;
		}
	}

	public static void Postfix(ModSessionInfo ___m_CurrentSession, ModContents? __state) {
		if (__state != null) {
			___m_CurrentSession.Mods.Add(__state.ModName, __state.m_WorkshopId.m_PublishedFileId);
		}
	}

}
