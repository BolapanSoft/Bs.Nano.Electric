// Ignore Spelling: Tdest Tsource Tprop dest Tret

using Nano.Electric;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Nano.Electric {
    internal static class ReflectionHelper {

        /// <summary>
        /// Для типа <typeparamref name="Type"/> формирует перечень публичных свойств типа <typeparamref name="Tret"/>
        /// </summary>
        /// <typeparam name="Tret"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<(string PropertyName, Func<T, Tret> Getter)> GetPropertiesWithGetters<T, Tret>() {
            var properties = new List<(string PropertyName, Func<T, Tret> Getter)>();
            var type = typeof(T);
            // Get all properties declared in the given type and its base types
            var allProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (var property in allProperties) {
                if (property.PropertyType == typeof(Tret)) {
                    // Create a getter function for the property
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
                    Func<T, Tret> getter = (o) => (Tret)property.GetValue(o);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                    // Add the property name and getter to the list
                    properties.Add((property.Name, getter));
                }
            }
            return properties;
        }
        /// <summary>
        /// Для типа <typeparamref name="T"/> формирует перечень публичных свойств типа <typeparamref name="Tprop"/>, для которых возможна запись.
        /// </summary>
        /// <typeparam name="Tprop"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<(string PropertyName, Action<T, Tprop> Setter)> GetPropertiesWithSetters<T, Tprop>() {
            var properties = new List<(string PropertyName, Action<T, Tprop> Setter)>();
            var type = typeof(T);
            // Get all properties declared in the given type and its base types
            var allProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (var property in allProperties) {
                if (property.PropertyType == typeof(Tprop) && property.CanWrite) {
                    // Create a getter function for the property
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
                    Action<T, Tprop> setter = (o, val) => property.SetValue(o, val);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                    // Add the property name and getter to the list
                    properties.Add((property.Name, setter));
                }
            }
            return properties;
        }


        public static string GetDescription<MyEnum>(this MyEnum value) where MyEnum : Enum {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field is null) {
                throw new InvalidOperationException($"For value = \"{value}\" of type {typeof(MyEnum)} not defined DescriptionAttribute.");
            }
            DescriptionAttribute? attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute?.Description ?? value.ToString();
        }
        /// <summary>
        /// Возвращает список геттеров экземпляра класса для свойств простого типа (string, double, int, bool).
        /// </summary>
        /// <typeparam name="Tsource">Тип, для которого формируется список.</typeparam>
        /// <returns></returns>
        public static List<(string PropertyName, Func<Tsource, string> Getter)> GetColumnGetters<Tsource>() where Tsource : class {
            var columnGetters = new List<(string PropertyName, Func<Tsource, string> Getter)>();
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, string>()
                .Select<(string PropertyName, Func<Tsource, string> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
                vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc); }))
                );
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, double>()
       .Select<(string PropertyName, Func<Tsource, double> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
       vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc).ToString(CultureInfo.InvariantCulture); }))
       );
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, double?>()
                .Select<(string PropertyName, Func<Tsource, double?> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
                selector: vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { 
                    double? value = vp.Getter(sgc);
                    return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                }))
                );
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, int>()
                 .Select<(string PropertyName, Func<Tsource, int> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
                 vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc).ToString(); }))
                 );
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, int?>()
                .Select<(string PropertyName, Func<Tsource, int?> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
                vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc).ToString(); }))
                );

            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, bool>()
            .Select<(string PropertyName, Func<Tsource, bool> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
            vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc).ToString(); }))
            );
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, bool?>()
                .Select<(string PropertyName, Func<Tsource, bool?> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
                vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc).ToString(); }))
                );

            return columnGetters;
        }
        
        public static List<(string PropertyName, Action<Tsource, string> Setter)> GenerateColumnSetters<Tsource>() where Tsource : class {
            var columnSetters = new List<(string PropertyName, Action<Tsource, string> Setter)>();
            columnSetters.AddRange(ReflectionHelper.GetPropertiesWithSetters<Tsource, string>()
                .Select<(string PropertyName, Action<Tsource, string> Setter), (string PropertyName, Action<Tsource, string> Setter)>(
                vp => vp));
            columnSetters.AddRange(ReflectionHelper.GetPropertiesWithSetters<Tsource, double>()
                .Select<(string PropertyName, Action<Tsource, double> Setter), (string PropertyName, Action<Tsource, string> Setter)>(
                vp => (PropertyName: vp.PropertyName, Setter: (sgc, val) => { vp.Setter(sgc, double.Parse(val, CultureInfo.InvariantCulture)); }))
                );
            columnSetters.AddRange(ReflectionHelper.GetPropertiesWithSetters<Tsource, double?>()
                .Select<(string PropertyName, Action<Tsource, double?> Setter), (string PropertyName, Action<Tsource, string> Setter)>(
                vp => (PropertyName: vp.PropertyName, Setter: (sgc, val) => {
                    if (string.IsNullOrEmpty(val)) {
                        vp.Setter(sgc, null);
                    }
                    else {
                        vp.Setter(sgc, double.Parse(val, CultureInfo.InvariantCulture));
                    }
                }
                ))
                );
            columnSetters.AddRange(ReflectionHelper.GetPropertiesWithSetters<Tsource, int>()
                            .Select<(string PropertyName, Action<Tsource, int> Setter), (string PropertyName, Action<Tsource, string> Setter)>(
                            vp => (PropertyName: vp.PropertyName, Setter: (sgc, val) => { vp.Setter(sgc, int.Parse(val, CultureInfo.InvariantCulture)); }))
                            );
            columnSetters.AddRange(ReflectionHelper.GetPropertiesWithSetters<Tsource, int?>()
                .Select<(string PropertyName, Action<Tsource, int?> Setter), (string PropertyName, Action<Tsource, string> Setter)>(
                vp => (PropertyName: vp.PropertyName, Setter: (sgc, val) => {
                    if (string.IsNullOrEmpty(val)) {
                        vp.Setter(sgc, null);
                    }
                    else {
                        vp.Setter(sgc, int.Parse(val, CultureInfo.InvariantCulture));
                    }
                }
                ))
                );
            columnSetters.AddRange(ReflectionHelper.GetPropertiesWithSetters<Tsource, bool>()
                            .Select<(string PropertyName, Action<Tsource, bool> Setter), (string PropertyName, Action<Tsource, string> Setter)>(
                            vp => (PropertyName: vp.PropertyName, Setter: (sgc, val) => { vp.Setter(sgc, bool.Parse(val)); }))
                            );
            columnSetters.AddRange(ReflectionHelper.GetPropertiesWithSetters<Tsource, bool?>()
                .Select<(string PropertyName, Action<Tsource, bool?> Setter), (string PropertyName, Action<Tsource, string> Setter)>(
                vp => (PropertyName: vp.PropertyName, Setter: (sgc, val) => {
                    if (string.IsNullOrEmpty(val)) {
                        vp.Setter(sgc, null);
                    }
                    else {
                        vp.Setter(sgc, bool.Parse(val));
                    }
                }
                ))
                );
            return columnSetters;
        }

        internal static bool IsNullableEnum(PropertyInfo piSource) {
            return (piSource.PropertyType.IsGenericType &&
                                                piSource.PropertyType.GenericTypeArguments.Length == 1
                                                && piSource.PropertyType.GenericTypeArguments[0].IsEnum
                                            );
        }

        internal static bool IsSimpleType(Type type) {
            return type.IsPrimitive || type == typeof(string) || type.IsEnum || IsNullableFromSimple(type);
        }
        internal static bool IsNullableFromSimple(Type type) {
            return type.IsConstructedGenericType
                && (type.GenericTypeArguments[0].IsPrimitive || type.GenericTypeArguments[0] == typeof(string) || type.GenericTypeArguments[0].IsEnum);
        }

        //internal static bool GetConverter<Tenum>(Type targetType, string errMessage, out Func<object, object?> convert)
        //    where Tenum : Enum {
        //    if (targetType ==typeof(int?)) {
        //        convert = (o) => {
        //            if (o is null)
        //                return null;
        //            IConvertible convertible = (IConvertible)o;
        //            int value = convertible.ToInt32(CultureInfo.InvariantCulture);
        //            return value;
        //        };
        //        return true;
        //    }
        //    else if (targetType.IsAssignableFrom(typeof(int))) {
        //        convert = (o) => {
        //            if (o is null)
        //                return 0;
        //            IConvertible convertible = (IConvertible)o;
        //            int value = convertible.ToInt32(CultureInfo.InvariantCulture);
        //            return value;
        //        };
        //        return true;
        //    }
        //    else {
        //        convert = (o) => throw new InvalidCastException(errMessage);
        //        return false;
        //    }
        //}
        private static Dictionary<Type, Func<string, object>> enumConverters = new();
        internal static bool GetConverter(Type type, out Func<string, object> convert) {
            if (enumConverters.ContainsKey(type)) {
                convert= enumConverters[type];
                return true;
            }
            if (type.IsEnum) {
                MethodInfo method = typeof(ReflectionHelper).GetMethod(nameof(Convert), BindingFlags.Public | BindingFlags.Static);
               var generic = method.MakeGenericMethod(type);
                convert = (s) => {
                    var obj = generic.Invoke(null, new object[] { s });
                    return System.Convert.ChangeType(obj, type);
                };
                enumConverters[type] = convert;
                return true;
            }
            // если type является Nullable<Enum>
            else if (Nullable.GetUnderlyingType(type)?.IsEnum == true) {
                MethodInfo method = typeof(ReflectionHelper).GetMethod(nameof(Convert), BindingFlags.Public | BindingFlags.Static);
                var generic = method.MakeGenericMethod(Nullable.GetUnderlyingType(type));
                convert = (s) => {
                    if (string.IsNullOrEmpty(s)) {
                        return null;
                    }
                    var obj = generic.Invoke(null, new object[] { s });
                    return System.Convert.ChangeType(obj, type);
                };
                enumConverters[type] = convert;
                return true;
            }
            else {
                throw new InvalidOperationException($"Can't make converter for {type}.");
            }
        }
        public static TEnum Convert<TEnum>(string description) where TEnum : Enum {
            var enumValues = typeof(TEnum).GetEnumValues();

            foreach (var enumValue in enumValues) {
                var memberInfo = typeof(TEnum).GetMember(enumValue.ToString());
                var descriptionAttribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                if (descriptionAttribute != null && descriptionAttribute.Description == description) {
                    return (TEnum)enumValue;
                }
            }

            throw new ArgumentException($"No enum value found with the description '{description}' for type '{typeof(TEnum).Name}'.");
        }
        
    }
}
