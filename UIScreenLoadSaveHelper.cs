namespace MultiplayerModPatch;

internal class UIScreenLoadSaveHelper {
	public static class ReflectionMembers {

		public static System.Type Type { get; } = typeof(UIScreenLoadSave);

		public static T CreateGetFieldMethod<T>(
			System.Reflection.FieldInfo field
		) where T : System.Delegate {
			System.Reflection.Emit.DynamicMethod method = new(
				name: $"{field.Name}_Get",
				returnType: field.FieldType,
				parameterTypes: field.IsStatic ? null : [Type],
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
				parameterTypes: field.IsStatic ? [field.FieldType] : [Type, field.FieldType],
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


		public static System.Reflection.FieldInfo m_ActiveSave { get; } =
			Type.GetField(
				"m_ActiveSave",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
			);

		public static System.Func<UIScreenLoadSave, UISave> m_ActiveSave_Get { get; } =
			CreateGetFieldMethod<System.Func<UIScreenLoadSave, UISave>>(m_ActiveSave);

		public static System.Action<UIScreenLoadSave, UISave> m_ActiveSave_Set { get; } =
			CreateSetFieldMethod<System.Action<UIScreenLoadSave, UISave>>(m_ActiveSave);

	}
}
