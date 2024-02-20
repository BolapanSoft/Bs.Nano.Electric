using System.Collections.Generic;
using System.Reflection;

namespace Cadwise.Data
{
	internal static class ReflectionHelper
	{
		private static readonly Dictionary<PropertyInfo, DatabaseSerializableAttribute[]> _kitAttributesCache = new Dictionary<PropertyInfo, DatabaseSerializableAttribute[]>();

		private static readonly Dictionary<IDataType, IDataFieldPrimitive> MainFields = new Dictionary<IDataType, IDataFieldPrimitive>();

		internal static DatabaseSerializableAttribute[] GetDatabaseSerializableAttribute(this PropertyInfo prop)
		{
			if (!ReflectionHelper._kitAttributesCache.TryGetValue(prop, out var value))
			{
				value = (DatabaseSerializableAttribute[])prop.GetCustomAttributes(typeof(DatabaseSerializableAttribute), false);
				ReflectionHelper._kitAttributesCache.Add(prop, value);
			}
			return value;
		}

		internal static IDataFieldPrimitive GetMainField(this IDataType dataType)
		{
			if (!ReflectionHelper.MainFields.TryGetValue(dataType, out var value))
			{
				foreach (IDataFieldPrimitive item in dataType.DataFieldsPrimitive)
				{
					if (item.Main)
					{
						ReflectionHelper.MainFields.Add(dataType, item);
						return value;
					}
				}
				return value;
			}
			return value;
		}
	}
}
