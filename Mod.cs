using HarmonyLib;

namespace MultiplayerModPatch;

public class Mod : ModBase {

	private const string modId = "com.snocream.terratech.mpmodpatch";

	private static readonly Harmony harmony = new(modId);


	public override bool HasEarlyInit() => true;

	public override void EarlyInit() {
		base.EarlyInit();
		harmony.PatchAll();
	}

	public override void Init() { }

	public override void DeInit() { }

	public static ModContents? GetContents(ModSessionInfo? modSessionInfo) {
		if (modSessionInfo == null) {
			return null;
		}
		foreach (var modContainer in modSessionInfo) {
			if (modContainer?.Script is Mod) {
				return modContainer.Contents;
			}
		}
		return null;
	}

}
