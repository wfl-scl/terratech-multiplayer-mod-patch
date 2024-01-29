namespace MultiplayerModPatch;

internal class ManModsHelper(ManMods instance) {

	public ManMods InstanceForHelper { get; set; } = instance;

	public void AutoAssignIDs(
		ModSessionInfo sessionInfo,
		System.Collections.Generic.List<string> corpsToAssign,
		System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> skinsToAssign,
		System.Collections.Generic.List<string> blocksToAssign
	) {
		ReflectionMembers.AutoAssignIDs_1_Method(
			InstanceForHelper,
			sessionInfo,
			corpsToAssign,
			skinsToAssign,
			blocksToAssign
		);
	}

	public static class ReflectionMembers {

		public static System.Reflection.MethodInfo AutoAssignIDs_1 { get; } =
			typeof(ManMods).GetMethod(
				"AutoAssignIDs",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
				binder: null,
				types: [
					typeof(ModSessionInfo),
					typeof(System.Collections.Generic.List<string>),
					typeof(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>),
					typeof(System.Collections.Generic.List<string>)
				],
				modifiers: null
			);

		public delegate void AutoAssignIDs_1_Delegate(
			ManMods instance,
			ModSessionInfo sessionInfo,
			System.Collections.Generic.List<string> corpsToAssign,
			System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> skinsToAssign,
			System.Collections.Generic.List<string> blocksToAssign
		);

		public static AutoAssignIDs_1_Delegate AutoAssignIDs_1_Method { get; } =
			(AutoAssignIDs_1_Delegate)AutoAssignIDs_1.CreateDelegate(typeof(AutoAssignIDs_1_Delegate));

	}

}
