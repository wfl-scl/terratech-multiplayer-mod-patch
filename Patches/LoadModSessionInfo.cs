using HarmonyLib;
using System.Collections.Generic;

namespace MultiplayerModPatch.Patches;

[HarmonyPatch(typeof(ManMods), nameof(ManMods.PreLobbyCreated))]
internal class LoadModSessionInfo {

	[HarmonyPriority(Priority.Last)]
	public static void Postfix(
		ModSessionInfo ___m_CurrentLobbySession,
		Dictionary<string, ModContainer> ___m_Mods
	) {
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
				var activeSave = Traverse.Create(loadSave).Field<UISave>("m_ActiveSave").Value;

				var gameType = activeSave.SaveInfo.m_GameType;
				savePath = ManSaveGame.CreateGameSaveFilePath(gameType, saveName);
			}
			var saveData = ManSaveGame.LoadSaveData(savePath);

			if (saveData.State.GetSaveData<ModSessionInfo>(ManSaveGame.SaveDataJSONType.ManMods, out var modSessionInfo)) {
				copyIDs(source: modSessionInfo, destination: ___m_CurrentLobbySession, overwrite: false);
			}
		}

		updateLobbySession(___m_CurrentLobbySession, ___m_Mods);
	}

	private static void copyIDs(ModSessionInfo source, ModSessionInfo destination, bool overwrite) {
		if (overwrite) {
			destination.CorpIDs.Clear();
			destination.SkinIDsByCorp.Clear();
			destination.BlockIDs.Clear();
		}
		// Corps
		foreach (var corp in source.CorpIDs) {
			if (!destination.CorpIDs.ContainsKey(corp.Key)) {
				destination.CorpIDs.Add(corp.Key, corp.Value);
			}
		}
		// Skins
		foreach (var corp in source.SkinIDsByCorp) {
			if (destination.SkinIDsByCorp.TryGetValue(corp.Key, out var skinIDs)) {
				if (skinIDs != null) {
					foreach (var skin in corp.Value) {
						if (!skinIDs.ContainsKey(skin.Key)) {
							skinIDs.Add(skin.Key, skin.Value);
						}
					}
				} else {
					destination.SkinIDsByCorp[corp.Key] = corp.Value;
				}
			} else {
				destination.SkinIDsByCorp.Add(corp.Key, corp.Value);
			}
		}
		// Blocks
		foreach (var block in source.BlockIDs) {
			if (!destination.BlockIDs.ContainsKey(block.Key)) {
				destination.BlockIDs.Add(block.Key, block.Value);
			}
		}
	}

	private static void updateLobbySession(ModSessionInfo sessionInfo, Dictionary<string, ModContainer> mods) {
		Dictionary<string, string> corpModIds = [];
		Dictionary<string, List<string>> skinsToAssign = [];
		List<string> blocksToAssign = [];
		foreach (var mod in mods) {
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

		Traverse.Create(ManMods.inst)
			.Method(
				name: "AutoAssignIDs",
				paramTypes: [
					typeof(ModSessionInfo),
					typeof(List<string>),
					typeof(Dictionary<string, List<string>>),
					typeof(List<string>)
				]
			)
			.GetValue(sessionInfo, corpsToAssign, skinsToAssign, blocksToAssign);
	}

}
