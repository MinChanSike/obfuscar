﻿using Mono.Cecil;

namespace Obfuscar
{
	internal static class TypeDefinitionExtensions
	{
		static public bool IsTypePublic (this TypeDefinition type)
		{
			if (type.DeclaringType == null)
				return type.IsPublic;

			if (type.IsNestedFamily || type.IsNestedFamilyOrAssembly || type.IsNestedPublic)
				return IsTypePublic (type.DeclaringType);

			return false;
		}

		public static bool? MarkedToRename (this ICustomAttributeProvider type, bool member = false)
		{
			var obfuscarObfuscate = typeof(ObfuscateAttribute).FullName;
			var reflectionObfuscate = typeof(System.Reflection.ObfuscationAttribute).FullName;

			foreach (CustomAttribute attr in type.CustomAttributes) {
				var attrFullName = attr.Constructor.DeclaringType.FullName;
				if (attrFullName == obfuscarObfuscate)
					return (bool)(Helper.GetAttributePropertyByName (attr, "ShouldObfuscate") ?? true);

				if (attrFullName == reflectionObfuscate) {
					var applyToMembers = (bool)(Helper.GetAttributePropertyByName (attr, "ApplyToMembers") ?? true);
					var rename = !(bool)(Helper.GetAttributePropertyByName (attr, "Exclude") ?? false);

					if (member && !applyToMembers)
						return !rename;

					return rename;
				}
			}

			// no attribute found.
			return null;
		}
	}
}
