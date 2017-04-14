using System.Collections.Generic;
using System.Reflection;

namespace Lib.Input.Helper
{
	/// <summary>
	/// Helps to work with reflection
	/// </summary>
	internal static class ReflectionHelper
	{
		/// <summary>
		/// Gets all properties with their related instance of a given object recursive to a given level
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="recursionLevel"></param>
		/// <returns></returns>
		public static Dictionary<PropertyInfo, object> GetPropertiesOfInstanceRecursive(object instance, int recursionLevel)
		{
			var result = new Dictionary<PropertyInfo, object>();

			if (recursionLevel < 0) return result;

			foreach (var propItem in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (!propItem.PropertyType.IsPrimitive)
				{
					var propertyInstance = propItem.GetValue(instance);
					var subPropInfos = GetPropertiesOfInstanceRecursive(propertyInstance, recursionLevel - 1);

					foreach (var subPropItem in subPropInfos)
					{
						if (!result.ContainsKey(subPropItem.Key))
							result.Add(subPropItem.Key, subPropItem.Value);
					}
				}

				result.Add(propItem, instance);
			}

			return result;
		}
	}
}
