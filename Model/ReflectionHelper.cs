// Ignore Spelling: Tdest Tsource Tprop dest Tret

using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nano.Electric {
    public static class EnumConverter<TEnum> where TEnum : Enum {
        private static readonly bool _isFlagsEnum;
        private static readonly uint _allflags;
        private static string[] _descriptions;
        private static string[] _enumNames;
        private static TEnum[] _enumValues;
        static EnumConverter() {
            bool isFlags = EnumConverter<TEnum>._isFlagsEnum = typeof(TEnum).GetCustomAttribute<FlagsAttribute>() is not null;
            InitStaticFields();
            if (isFlags) {
                _allflags = 0;
                foreach (var item in _enumValues!) {
                    _allflags += ToUint(item);
                }
            }
            else {
                _allflags = ToUint(_enumValues[_enumValues.Length - 1]);
            }
        }

        private static void InitStaticFields() {
            GetEnumData(out string[] enumNames, out TEnum[] enumValues);
            EnumConverter<TEnum>._enumNames = enumNames;
            EnumConverter<TEnum>._enumValues = enumValues;
            _descriptions = new string[enumNames.Length];
            for (int i = 0; i < enumNames.Length; i++) {
                var name = enumNames[i];
                var memberInfo = typeof(TEnum).GetMember(name);
                var descriptionAttribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                var value = (TEnum)Enum.Parse(typeof(TEnum), name);
                _descriptions[i] = descriptionAttribute?.Description ?? string.Empty;
            }
        }

        public static string GetDescription(TEnum value) {
            if (_isFlagsEnum) {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                for (; i < _enumValues.Length - 1; i++) {
                    if ((ToUint(_enumValues[i]) & ToUint(value)) == ToUint(_enumValues[i])) {
                        if (sb.Length > 0) {
                            sb.Append("; ");
                        }
                        sb.Append(GetDescriptionSingleValue(_enumValues[i]));
                    }
                }
                if ((ToUint(_enumValues[i]) & ToUint(value)) == ToUint(_enumValues[i])) {
                    if (sb.Length > 0) {
                        sb.Append("; ");
                    }
                    sb.Append(sb.Append(GetDescriptionSingleValue(_enumValues[i])));
                }
                return sb.ToString();
            }
            else {
                return GetDescriptionSingleValue(value);
            }
        }
        public static bool IsDefineValue(TEnum value) {
            if (_isFlagsEnum) {
                var uintValue = ToUint(value);
                return ((~_allflags) & uintValue) == 0;
            }
            else {
                int i = Array.BinarySearch(_enumValues, value);
                return i >= 0;
            }
        }
        public static bool IsDefineValue(string value) {

            foreach (var item in _enumNames) {
                if (item == value)
                    return true;
            }
            return false;
        }
        private static void GetEnumData(out string[] enumNames, out TEnum[] enumValues) {
            TEnum[] array = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
            Array.Sort(array);
            string[] array2 = new string[array.Length];
            for (int i = 0; i < array2.Length; i++) {
                array2[i] = array[i].ToString();
            }
            enumNames = array2;
            enumValues = array;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ToUint(TEnum val) {
            return Unsafe.As<TEnum, uint>(ref val);
        }
        private static string GetDescriptionSingleValue(TEnum value) {
            int i = Array.BinarySearch(_enumValues, value);
            if (i >= 0) {
                return _descriptions[i];
            }
            return string.Empty;
        }
    }
    internal static class ReflectionHelper {
        private static readonly MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
        /// <summary>
        /// Формирует перечень публичных свойств типа <typeparamref name="Tret"/> для типа <typeparamref name="T"/>
        /// </summary>
        public static List<(string PropertyName, Func<T, Tret> Getter)> GetPropertiesWithGetters<T, Tret>() {
            var cacheKey = $"GetPropertiesWithGetters_{typeof(T).FullName}_{typeof(Tret).FullName}";

            if (memoryCache.TryGetValue(cacheKey, out List<(string PropertyName, Func<T, Tret> Getter)> properties)) {
                return properties ?? throw new Exception($"Invalid cache key {cacheKey}");
            }

            properties = new List<(string PropertyName, Func<T, Tret> Getter)>();
            var type = typeof(T);
            var allProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (var property in allProperties) {
                if (property.PropertyType == typeof(Tret)) {
                    var getter = (Func<T, Tret>)Delegate.CreateDelegate(typeof(Func<T, Tret>), property.GetGetMethod());
                    properties.Add((property.Name, getter));
                }
            }

            memoryCache.Set(cacheKey, properties, TimeSpan.FromDays(5));
            return properties;
        }
        /// <summary>
        /// Формирует перечень публичных свойств типа <typeparamref name="Tprop"/>, для которых возможна запись, для типа <typeparamref name="T"/>
        /// </summary>
        public static List<(string PropertyName, Action<T, Tprop> Setter)> GetPropertiesWithSetters<T, Tprop>() {
            var cacheKey = $"GetPropertiesWithSetters_{typeof(T).FullName}_{typeof(Tprop).FullName}";

            if (memoryCache.TryGetValue(cacheKey, out List<(string PropertyName, Action<T, Tprop> Setter)> properties)) {
                return properties ?? throw new Exception($"Invalid cache key {cacheKey}");
            }

            properties = new List<(string PropertyName, Action<T, Tprop> Setter)>();
            var type = typeof(T);
            var allProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (var property in allProperties) {
                if (property.PropertyType == typeof(Tprop) && property.CanWrite) {
                    var setter = (Action<T, Tprop>)Delegate.CreateDelegate(typeof(Action<T, Tprop>), property.GetSetMethod());
                    properties.Add((property.Name, setter));
                }
            }

            memoryCache.Set(cacheKey, properties, TimeSpan.FromDays(5));
            return properties;
        }


        public static string GetDescription<TEnum>(this TEnum enumValue) where TEnum : Enum {
            return EnumConverter<TEnum>.GetDescription(enumValue);
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
                }
                ))
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





    }
}
