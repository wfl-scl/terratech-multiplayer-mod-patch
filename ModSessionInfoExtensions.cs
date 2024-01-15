namespace MultiplayerModPatch;

public static class ModSessionInfoExtensions {

	public static void AddIDsFrom(this ModSessionInfo modSessionInfo, ModSessionInfo source) {
		if (source.CorpIDs != null) {
			foreach (var corp in source.CorpIDs) {
				if (!modSessionInfo.CorpIDs.ContainsKey(corp.Key)) {
					modSessionInfo.CorpIDs.Add(corp.Key, corp.Value);
				}
			}
		}
		if (source.SkinIDsByCorp != null) {
			foreach (var corp in source.SkinIDsByCorp) {
				if (modSessionInfo.SkinIDsByCorp.TryGetValue(corp.Key, out var skinIDs)) {
					if (skinIDs == null) {
						modSessionInfo.SkinIDsByCorp[corp.Key] = corp.Value;
					} else {
						foreach (var skin in corp.Value) {
							if (!skinIDs.ContainsKey(skin.Key)) {
								skinIDs.Add(skin.Key, skin.Value);
							}
						}
					}
				} else {
					modSessionInfo.SkinIDsByCorp.Add(corp.Key, corp.Value);
				}
			}
		}
		if (source.BlockIDs != null) {
			foreach (var block in source.BlockIDs) {
				if (!modSessionInfo.BlockIDs.ContainsKey(block.Key)) {
					modSessionInfo.BlockIDs.Add(block.Key, block.Value);
				}
			}
		}
	}

}
