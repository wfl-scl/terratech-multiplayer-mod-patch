using HarmonyLib;
using System.Collections.Generic;

namespace MultiplayerModPatch.Patches;

[HarmonyPatch(typeof(ManMods), nameof(ManMods.WriteLobbySession))]
internal class LoadModSessionInfo {

	public static void Prefix() {
		var nextModeSetting = ManGameMode.inst.NextModeSetting;

		// セーブデータはまだ読まれてないので読む必要がある
		if (
			nextModeSetting.GetModeInitSetting("SaveName", out var saveNameObject) &&
			saveNameObject is string saveName
		) {
			string savePath;
			if (
				nextModeSetting.GetModeInitSetting("SaveWorkshopPath", out var saveWorkshopPathObject) &&
				saveWorkshopPathObject is string saveWorkshopPath
			) {
				savePath = saveWorkshopPath;
			} else {
				var loadSave = (UIScreenLoadSave)ManUI.inst.GetScreen(ManUI.ScreenType.LoadSave);
				var activeSave = UIScreenLoadSaveHelper.ReflectionMembers.m_ActiveSave_Get(loadSave);
				var gameType = activeSave.SaveInfo.m_GameType;
				savePath = ManSaveGame.CreateGameSaveFilePath(gameType, saveName);
			}
			var saveData = ManSaveGame.LoadSaveData(savePath);

			if (saveData.State.GetSaveData<ModSessionInfo>(ManSaveGame.SaveDataJSONType.ManMods, out var modSessionInfo)) {
				Mod.ManModsHelper.m_CurrentLobbySession = modSessionInfo;
			}
		}

		UpdateLobbySession(Mod.ManModsHelper.m_CurrentLobbySession);
	}

	public static void UpdateLobbySession(ModSessionInfo sessionInfo) {
		Dictionary<string, string> corpModIds = [];
		Dictionary<string, List<string>> skinsToAssign = [];
		List<string> blocksToAssign = [];
		foreach (var mod in Mod.ManModsHelper.m_Mods) {
			var modId = mod.Key;
			if (!mod.Value.IsRemote && sessionInfo.Mods.ContainsKey(modId)) {
				foreach (var corp in mod.Value.Contents.m_Corps) {
					if (!corpModIds.ContainsKey(corp.name)) {
						corpModIds.Add(corp.name, modId);
					} else {
						d.LogWarningFormat(
							"[Warning] Failed to add duplicate corp {0} from mod {1} because we already have one from mod {2}",
							corp.name,
							modId,
							corpModIds[corp.name]
						);
					}
				}
				foreach (var skin in mod.Value.Contents.m_Skins) {
					if (!skinsToAssign.ContainsKey(skin.m_Corporation)) {
						skinsToAssign[skin.m_Corporation] = [];
					}
					skinsToAssign[skin.m_Corporation].Add(ModUtils.CreateCompoundId(modId, skin.name));
				}
				foreach (var block in mod.Value.Contents.m_Blocks) {
					blocksToAssign.Add(ModUtils.CreateCompoundId(modId, block.name));
				}
			}
		}
		List<string> corpsToAssign = new(corpModIds.Count);
		foreach (var item in corpModIds) {
			corpsToAssign.Add(ModUtils.CreateCompoundId(item.Value, item.Key));
		}
		Mod.ManModsHelper.AutoAssignIDs(sessionInfo, corpsToAssign, skinsToAssign, blocksToAssign);
	}

}
