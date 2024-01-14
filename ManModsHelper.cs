namespace MultiplayerModPatch;

internal class ManModsHelper(ManMods instance) {

	public ManMods InstanceForHelper { get; set; } = instance;

	public System.Collections.Generic.Dictionary<string, ModContainer> m_Mods {
		get => ReflectionMembers.m_Mods_Get(InstanceForHelper);
		set => ReflectionMembers.m_Mods_Set(InstanceForHelper, value);
	}

	public ModSessionInfo m_CurrentLobbySession {
		get => ReflectionMembers.m_CurrentLobbySession_Get(InstanceForHelper);
		set => ReflectionMembers.m_CurrentLobbySession_Set(InstanceForHelper, value);
	}

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

		public static T CreateGetFieldMethod<T>(
			System.Reflection.FieldInfo field
		) where T : System.Delegate {
			System.Reflection.Emit.DynamicMethod method = new(
				name: $"{field.Name}_Get",
				returnType: field.FieldType,
				parameterTypes: field.IsStatic ?
					null :
					[typeof(ManMods)],
				restrictedSkipVisibility: true
			);
			var generator = method.GetILGenerator();
			if (field.IsStatic) {
				generator.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field);
			} else {
				generator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
				generator.Emit(System.Reflection.Emit.OpCodes.Ldfld, field);
			}
			generator.Emit(System.Reflection.Emit.OpCodes.Ret);
			return (T)method.CreateDelegate(typeof(T));
		}

		public static T CreateSetFieldMethod<T>(
			System.Reflection.FieldInfo field
		) where T : System.Delegate {
			System.Reflection.Emit.DynamicMethod method = new(
				name: $"{field.Name}_Set",
				returnType: null,
				parameterTypes: field.IsStatic ?
					[field.FieldType] :
					[typeof(ManMods), field.FieldType],
				restrictedSkipVisibility: true
			);
			var generator = method.GetILGenerator();
			if (field.IsStatic) {
				generator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
				generator.Emit(System.Reflection.Emit.OpCodes.Stsfld, field);
			} else {
				generator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
				generator.Emit(System.Reflection.Emit.OpCodes.Ldarg_1);
				generator.Emit(System.Reflection.Emit.OpCodes.Stfld, field);
			}
			generator.Emit(System.Reflection.Emit.OpCodes.Ret);
			return (T)method.CreateDelegate(typeof(T));
		}


		public static System.Reflection.FieldInfo m_Mods { get; } =
			typeof(ManMods).GetField(
				"m_Mods",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
			);

		public static System.Func<ManMods, System.Collections.Generic.Dictionary<string, ModContainer>> m_Mods_Get { get; } =
			CreateGetFieldMethod<System.Func<ManMods, System.Collections.Generic.Dictionary<string, ModContainer>>>(m_Mods);

		public static System.Action<ManMods, System.Collections.Generic.Dictionary<string, ModContainer>> m_Mods_Set { get; } =
			CreateSetFieldMethod<System.Action<ManMods, System.Collections.Generic.Dictionary<string, ModContainer>>>(m_Mods);

		public static System.Reflection.FieldInfo m_CurrentLobbySession { get; } =
			typeof(ManMods).GetField(
				"m_CurrentLobbySession",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
			);

		public static System.Func<ManMods, ModSessionInfo> m_CurrentLobbySession_Get { get; } =
			CreateGetFieldMethod<System.Func<ManMods, ModSessionInfo>>(m_CurrentLobbySession);

		public static System.Action<ManMods, ModSessionInfo> m_CurrentLobbySession_Set { get; } =
			CreateSetFieldMethod<System.Action<ManMods, ModSessionInfo>>(m_CurrentLobbySession);

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
