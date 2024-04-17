using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Nano.Electric {
    internal class FieldBinder : Binder {
        public static readonly FieldBinder Instance = new FieldBinder();
        private FieldBinder() : base() {
        }
        private class BinderState {
            public object[] args;
        }
        public override FieldInfo BindToField(
            BindingFlags bindingAttr,
            FieldInfo[] match,
            object value,
            CultureInfo culture
            ) {
            if (match == null)
                throw new ArgumentNullException("match");
            // Get a field for which the value parameter can be converted to the specified field type.
            for (int i = 0; i < match.Length; i++)
                if (ChangeType(value, match[i].FieldType, culture) != null)
                    return match[i];
            return null;
        }
        public override MethodBase BindToMethod(
            BindingFlags bindingAttr,
            MethodBase[] match,
            ref object[] args,
            ParameterModifier[] modifiers,
            CultureInfo culture,
            string[] names,
            out object state
            ) {
            // Store the arguments to the method in a state object.
            BinderState myBinderState = new BinderState();
            object[] arguments = new Object[args.Length];
            args.CopyTo(arguments, 0);
            myBinderState.args = arguments;
            state = myBinderState;
            if (match == null)
                throw new ArgumentNullException();
            // Find a method that has the same parameters as those of the args parameter.
            for (int i = 0; i < match.Length; i++) {
                // Count the number of parameters that match.
                int count = 0;
                ParameterInfo[] parameters = match[i].GetParameters();
                // Go on to the next method if the number of parameters do not match.
                if (args.Length != parameters.Length)
                    continue;
                // Match each of the parameters that the user expects the method to have.
                for (int j = 0; j < args.Length; j++) {
                    // If the names parameter is not null, then reorder args.
                    if (names != null) {
                        if (names.Length != args.Length)
                            throw new ArgumentException("names and args must have the same number of elements.");
                        for (int k = 0; k < names.Length; k++)
                            if (String.Compare(parameters[j].Name, names[k].ToString()) == 0)
                                args[j] = myBinderState.args[k];
                    }
                    // Determine whether the types specified by the user can be converted to the parameter type.
                    if (ChangeType(args[j], parameters[j].ParameterType, culture) != null)
                        count += 1;
                    else
                        break;
                }
                // Determine whether the method has been found.
                if (count == args.Length)
                    return match[i];
            }
            return null;
        }
        public override object ChangeType(
            object value,
            Type myChangeType,
            CultureInfo culture
            ) {
            // Determine whether the value parameter can be converted to a value of type myType.
            if (CanConvertFrom(value.GetType(), myChangeType))
                // Return the converted object.
                return ChangeTypeRelease(value, myChangeType, culture);
            else
                throw new NotImplementedException();
        }

        private static object ChangeTypeRelease(object value, Type myChangeType, CultureInfo culture) {
            var type1 = value.GetType();
            var type2 = myChangeType;
            TypeCode typeCode1 = Type.GetTypeCode(type1);
            TypeCode typeCode2 = Type.GetTypeCode(type2);
            // If both type1 and type2 have the same type, return true.
            if (typeCode1 == typeCode2)
                return value;
            if (typeCode1 == TypeCode.String) {
                if (Nullable.GetUnderlyingType(type2) is not null) { // type2 is Nullable
                    if (value is null) {
                        return value!;
                    }
                    if ((string)value == "null") {
                        return null!;
                    }
                    type2 = Nullable.GetUnderlyingType(type2);
                    typeCode2 = Type.GetTypeCode(type2);
                }
                if (type2.IsEnum) {
                    if (IsFlagsEnum(type2)) {
                        var flagValues= ((string)value).Split(';').Select(s=>s.TrimStart());
                        ulong val = flagValues.Aggregate(0ul, (current, item) => current | Convert.ToUInt64(ParseEnum(type2, item)));
                        return Enum.ToObject(type2, val);
                    }
                    else {
                        object enumValue = ParseEnum(type2, (string)value);
                        return enumValue;
                    }
                }
                else {
                    switch (typeCode2) {
                        case TypeCode.Boolean: {
                                var strValue = (string)value;
                                if (culture == CultureInfo.GetCultureInfo("Ru-ru")) {
                                    if (string.Compare("Да", strValue, ignoreCase: true, culture: culture) == 0) {
                                        strValue = "True";
                                    }
                                    else if (string.Compare("Нет", strValue, ignoreCase: true, culture: culture) == 0) {
                                        strValue = "False";
                                    }
                                    else if (string.Compare("1", strValue, ignoreCase: true, culture: culture) == 0) {
                                        strValue = "True";
                                    }
                                    else if (string.Compare("0", strValue, ignoreCase: true, culture: culture) == 0) {
                                        strValue = "False";
                                    }
                                    else if (string.Compare("Истина", strValue, ignoreCase: true, culture: culture) == 0) {
                                        strValue = "True";
                                    }
                                    else if (string.Compare("Ложь", strValue, ignoreCase: true, culture: culture) == 0) {
                                        strValue = "False";
                                    }
                                }
                                return Convert.ToBoolean(strValue);
                            }
                        case TypeCode.SByte:
                            return Convert.ToSByte(value);
                        case TypeCode.Byte:
                            return Convert.ToByte(value);
                        case TypeCode.Int16:
                            return Convert.ToInt16(value);
                        case TypeCode.UInt16:
                            return Convert.ToUInt16(value);
                        case TypeCode.Int32:
                            return Convert.ToInt32(value);
                        case TypeCode.UInt32:
                            return Convert.ToUInt32(value);
                        case TypeCode.Int64:
                            return Convert.ToInt64(value);
                        case TypeCode.UInt64:
                            return Convert.ToUInt64(value);
                        case TypeCode.Double: {
                                var strValue = (string)value;
                                if (double.TryParse(strValue, NumberStyles.Float, culture, out double result)) {
                                    return result;
                                }
                                else if (double.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result)) {
                                    return result;
                                }
                                throw new FormatException($"Значение {strValue} не удается преобразовать в double.");
                            }
                        case TypeCode.Single: {
                                var strValue = (string)value;
                                if (Single.TryParse(strValue, NumberStyles.Float, culture, out Single result)) {
                                    return result;
                                }
                                else if (Single.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result)) {
                                    return result;
                                }
                                throw new FormatException($"Значение {strValue} не удается преобразовать в System.Single.");
                            }
                        case TypeCode.Decimal: {
                                var strValue = (string)value;
                                if (decimal.TryParse(strValue, NumberStyles.Number, culture, out decimal result)) {
                                    return result;
                                }
                                else if (decimal.TryParse(strValue, NumberStyles.Number, CultureInfo.InvariantCulture, out result)) {
                                    return result;
                                }
                                throw new FormatException($"Значение {strValue} не удается преобразовать в decimal.");
                            }
                        case TypeCode.Empty:
                        case TypeCode.Object:
                        case TypeCode.DBNull:
                        case TypeCode.Char:
                        case TypeCode.DateTime: {
                                var strValue = (string)value;
                                if (DateTime.TryParse(strValue, culture, DateTimeStyles.AssumeUniversal, out DateTime result)) {
                                    return result;
                                }
                                else if (DateTime.TryParse(strValue, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out result)) {
                                    return result;
                                }
                                throw new FormatException($"Значение {strValue} не удается преобразовать в DateTime.");
                            }
                        case TypeCode.String:
                            return value;
                        default:
                            throw new NotImplementedException($"Преобразование из System.String к {typeCode2} не реализовано.");
                    }
                }
            }
            else {
                try {
                    return Convert.ChangeType(typeCode1, typeCode2, culture);
                }
                catch (Exception) {
                    return Convert.ChangeType(typeCode1, typeCode2, CultureInfo.InvariantCulture);
                }
            }
        }

        public override void ReorderArgumentArray(ref object[] args, object state) {
            // Return the args that had been reordered by BindToMethod.
            ((BinderState)state).args.CopyTo(args, 0);
        }
        public override MethodBase SelectMethod(
            BindingFlags bindingAttr,
            MethodBase[] match,
            Type[] types,
            ParameterModifier[] modifiers
            ) {
            if (match == null)
                throw new ArgumentNullException("match");
            for (int i = 0; i < match.Length; i++) {
                // Count the number of parameters that match.
                int count = 0;
                ParameterInfo[] parameters = match[i].GetParameters();
                // Go on to the next method if the number of parameters do not match.
                if (types.Length != parameters.Length)
                    continue;
                // Match each of the parameters that the user expects the method to have.
                for (int j = 0; j < types.Length; j++)
                    // Determine whether the types specified by the user can be converted to parameter type.
                    if (CanConvertFrom(types[j], parameters[j].ParameterType))
                        count += 1;
                    else
                        break;
                // Determine whether the method has been found.
                if (count == types.Length)
                    return match[i];
            }
            return null;
        }
        public override PropertyInfo SelectProperty(
            BindingFlags bindingAttr,
            PropertyInfo[] match,
            Type returnType,
            Type[] indexes,
            ParameterModifier[] modifiers
            ) {
            if (match == null)
                throw new ArgumentNullException("match");
            for (int i = 0; i < match.Length; i++) {
                // Count the number of indexes that match.
                int count = 0;
                ParameterInfo[] parameters = match[i].GetIndexParameters();
                // Go on to the next property if the number of indexes do not match.
                if (indexes.Length != parameters.Length)
                    continue;
                // Match each of the indexes that the user expects the property to have.
                for (int j = 0; j < indexes.Length; j++)
                    // Determine whether the types specified by the user can be converted to index type.
                    if (CanConvertFrom(indexes[j], parameters[j].ParameterType))
                        count += 1;
                    else
                        break;
                // Determine whether the property has been found.
                if (count == indexes.Length)
                    // Determine whether the return type can be converted to the properties type.
                    if (CanConvertFrom(returnType, match[i].PropertyType))
                        return match[i];
                    else
                        continue;
            }
            return null;
        }
        // Determines whether type1 can be converted to type2. Check only for primitive types.
        private static bool CanConvertFrom(Type type1, Type type2) {

            TypeCode typeCode1 = Type.GetTypeCode(type1);
            TypeCode typeCode2 = Type.GetTypeCode(type2);
            // If both type1 and type2 have the same type, return true.
            if (typeCode1 == typeCode2)
                return true;
            if (typeCode1 == TypeCode.String) {
                if (Nullable.GetUnderlyingType(type2) is not null) { // type2 is Nullable
                    type2 = Nullable.GetUnderlyingType(type2);
                    typeCode2 = Type.GetTypeCode(type2);
                }
                if (type2.IsEnum) {
                    type2 = Enum.GetUnderlyingType(type2);// usually, Int32
                    typeCode2 = Type.GetTypeCode(type2);
                }

                switch (typeCode2) {
                    case TypeCode.Empty:
                        return false;
                    case TypeCode.Object:
                        return false;
                    case TypeCode.DBNull:
                        return false;
                    case TypeCode.Boolean:
                        return true;
                    case TypeCode.Char:
                        return false;
                    case TypeCode.SByte:
                        return true;
                    case TypeCode.Byte:
                        return true;
                    case TypeCode.Int16:
                        return true;
                    case TypeCode.UInt16:
                        return true;
                    case TypeCode.Int32:
                        return true;
                    case TypeCode.UInt32:
                        return true;
                    case TypeCode.Int64:
                        return true;
                    case TypeCode.UInt64:
                        return true;
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    case TypeCode.Decimal:
                        return true;
                    case TypeCode.DateTime:
                        return true;
                    case TypeCode.String:
                        return false;
                    default:
                        return false;
                }
            }
            // Possible conversions from Char follow.
            if (typeCode1 == TypeCode.Char)
                switch (typeCode2) {
                    case TypeCode.UInt16:
                        return true;
                    case TypeCode.UInt32:
                        return true;
                    case TypeCode.Int32:
                        return true;
                    case TypeCode.UInt64:
                        return true;
                    case TypeCode.Int64:
                        return true;
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from Byte follow.
            if (typeCode1 == TypeCode.Byte)
                switch (typeCode2) {
                    case TypeCode.Char:
                        return true;
                    case TypeCode.UInt16:
                        return true;
                    case TypeCode.Int16:
                        return true;
                    case TypeCode.UInt32:
                        return true;
                    case TypeCode.Int32:
                        return true;
                    case TypeCode.UInt64:
                        return true;
                    case TypeCode.Int64:
                        return true;
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from SByte follow.
            if (typeCode1 == TypeCode.SByte)
                switch (typeCode2) {
                    case TypeCode.Int16:
                        return true;
                    case TypeCode.Int32:
                        return true;
                    case TypeCode.Int64:
                        return true;
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from UInt16 follow.
            if (typeCode1 == TypeCode.UInt16)
                switch (typeCode2) {
                    case TypeCode.UInt32:
                        return true;
                    case TypeCode.Int32:
                        return true;
                    case TypeCode.UInt64:
                        return true;
                    case TypeCode.Int64:
                        return true;
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from Int16 follow.
            if (typeCode1 == TypeCode.Int16)
                switch (typeCode2) {
                    case TypeCode.Int32:
                        return true;
                    case TypeCode.Int64:
                        return true;
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from UInt32 follow.
            if (typeCode1 == TypeCode.UInt32)
                switch (typeCode2) {
                    case TypeCode.UInt64:
                        return true;
                    case TypeCode.Int64:
                        return true;
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from Int32 follow.
            if (typeCode1 == TypeCode.Int32)
                switch (typeCode2) {
                    case TypeCode.Int64:
                        return true;
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from UInt64 follow.
            if (typeCode1 == TypeCode.UInt64)
                switch (typeCode2) {
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from Int64 follow.
            if (typeCode1 == TypeCode.Int64)
                switch (typeCode2) {
                    case TypeCode.Single:
                        return true;
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }
            // Possible conversions from Single follow.
            if (typeCode1 == TypeCode.Single)
                switch (typeCode2) {
                    case TypeCode.Double:
                        return true;
                    default:
                        return false;
                }

            return false;
        }
        private static object ParseEnum(Type enumType, string description) {
            var knownValues = GetCachedValues(enumType);
            if (knownValues.TryGetValue(description, out object value)) {
                return value;
            }
            throw new InvalidCastException($"значение \"{description}\" не удается привести к типу {enumType.Name}.");
        }
        private static Dictionary<Type, Dictionary<string, object>> cacheEnumValues = new();
        private static Dictionary<string, object> GetCachedValues(Type typeEnum) {
            if (!cacheEnumValues.TryGetValue(typeEnum, out Dictionary<string, object> values)) {
                var enumValues = typeEnum.GetEnumValues();
                values = new Dictionary<string, object>();
                foreach (var enumValue in enumValues) {
                    string enumName = enumValue.ToString();
                    values[enumName] = enumValue;
                    MemberInfo[] memberInfo = typeEnum.GetMember(enumName);
                    DescriptionAttribute? descriptionAttribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                    if (descriptionAttribute is null) {
                        throw new ArgumentException($"Для одного или нескольких членов перечисления {typeEnum.Name} не определен атрибут [Description].");
                    }
                    values[descriptionAttribute.Description] = enumValue;
                }
                cacheEnumValues.Add(typeEnum, values);
            }
            return values;
        }
        private static bool IsFlagsEnum(Type typeEnum) {
            return typeEnum.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
        }
    }
}
