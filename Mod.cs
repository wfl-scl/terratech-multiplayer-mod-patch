using HarmonyLib;

namespace MultiplayerModPatch;

public class Mod : ModBase {

	internal static ManModsHelper ManModsHelper {
		get {
			manModsHelper ??= new(ManMods.inst);
			return manModsHelper;
		}
	}

	private const string modId = "com.snocream.terratech.mpmodpatch";

	private static readonly Harmony harmony = new(modId);

	private static ManModsHelper? manModsHelper;


	public override bool HasEarlyInit() => true;

	public override void EarlyInit() {
		base.EarlyInit();
		harmony.PatchAll();
	}

	public override void Init() { }

	public override void DeInit() { }

}
