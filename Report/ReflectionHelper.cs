using Bs.Nano.Electric.Report;
using Bs.XML.SpreadSheet;
using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
//using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace Bs.Nano.Electric.Report {
    public static class ReflectionHelper {
        
        private struct CacheConverterValue { public bool HasConverter; public Func<object?, object?> Convert; }
        private static class EnumConverter<TEnum> where TEnum : struct, Enum {
            private static readonly TEnum[] enumValues;
            private static readonly string[] descriptions;
            private static readonly (string Description, TEnum Value)[] sortedByDescription;

            static EnumConverter() {
                enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
                descriptions = new string[enumValues.Length];

                for (int i = 0; i < enumValues.Length; i++) {
                    var enumValue = enumValues[i];
                    var memberInfo = typeof(TEnum).GetMember(enumValue.ToString())[0];
                    var descriptionAttribute = memberInfo
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .Cast<DescriptionAttribute>()
                        .FirstOrDefault();

                    descriptions[i] = descriptionAttribute?.Description ?? string.Empty;
                }

                // Подготовим массив для бинарного поиска по описанию
                sortedByDescription = enumValues
                    .Select((val, i) => (Description: descriptions[i], Value: val))
                    .OrderBy(x => x.Description, StringComparer.Ordinal)
                    .ToArray();
            }

            internal static TEnum Convert(string description) {
                if (TryConvert(description, out var value))
                    return value;

                throw new ArgumentOutOfRangeException(nameof(description));
            }

            internal static bool TryConvert(string description, out TEnum value) {
                int index = Array.BinarySearch(
                    sortedByDescription,
                    (description, default(TEnum)),
                    Comparer<(string Description, TEnum Value)>.Create(
                        (a, b) => StringComparer.Ordinal.Compare(a.Description, b.Description))
                );

                if (index >= 0) {
                    value = sortedByDescription[index].Value;
                    return true;
                }

                value = default;
                return false;
            }

            internal static string GetDescription(TEnum value) {
                // Поиск по исходному массиву
                for (int i = 0; i < enumValues.Length; i++) {
                    if (EqualityComparer<TEnum>.Default.Equals(enumValues[i], value))
                        return descriptions[i];
                }
                return string.Empty;
            }
        }
        private static readonly IMemoryCache cacheConverters = new MemoryCache(new MemoryCacheOptions());
        private static readonly IMemoryCache cacheColumnSetters = new MemoryCache(new MemoryCacheOptions());


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
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (T item in collection) {
                action(item);
            }
        }
        public static string GetDescription<MyEnum>(this MyEnum value) where MyEnum : struct, Enum {
            return EnumConverter<MyEnum>.GetDescription(value);
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
                vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc)?.ToString(CultureInfo.InvariantCulture); }))
                );
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, int>()
                 .Select<(string PropertyName, Func<Tsource, int> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
                 vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc).ToString(CultureInfo.InvariantCulture); }))
                 );
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, int?>()
                .Select<(string PropertyName, Func<Tsource, int?> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
                vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc)?.ToString(CultureInfo.InvariantCulture); }))
                );

            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, bool>()
            .Select<(string PropertyName, Func<Tsource, bool> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
            vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc).ToString(CultureInfo.InvariantCulture); }))
            );
            columnGetters.AddRange(ReflectionHelper.GetPropertiesWithGetters<Tsource, bool?>()
                .Select<(string PropertyName, Func<Tsource, bool?> Getter), (string PropertyName, Func<Tsource, string> Getter)>(
                vp => (PropertyName: vp.PropertyName, Getter: (sgc) => { return vp.Getter(sgc)?.ToString(CultureInfo.InvariantCulture); }))
                );

            return columnGetters;
        }
        /// <summary>
        /// Возвращает список сеттеров экземпляра класса для свойств простого типа (string, double, int, bool).
        /// </summary>
        /// <typeparam name="Tsource">Тип, для которого формируется список.</typeparam>
        /// <returns></returns>
        public static List<(string PropertyName, Action<Tsource, string> Setter)> GetColumnSetters<Tsource>() where Tsource : class {
            string cacheKey = $"{typeof(Tsource).FullName}::ColumnSetters";
            List<(string PropertyName, Action<Tsource, string> Setter)> setters;
            if (cacheColumnSetters.TryGetValue(cacheKey, out  setters) && (setters is not null)) {
                // Return the cached result if available
                return setters;
            }
            // If not cached, generate the column setters
            setters = GenerateColumnSetters<Tsource>();

            // Add the generated setters to the cache
            cacheColumnSetters.Set(cacheKey, setters);

            return setters;
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
        /// <summary>
        /// Заполняет свойства <paramref name="dest"/> из образца <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="Tsource"></typeparam>
        /// <typeparam name="Tdest"></typeparam>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns>Список свойств, имеющих общее наименование, для которых 
        /// процедура преобразования типа свойства источника в тип свойства назначения не определена
        /// или свойство с совпадающим именем не найдено.</returns>
        /// <remarks>Для типа <typeparamref name="Tdest"/> производится поиск свойств простого типа, допускающих установку значения, включая типы Enum.
        /// Затем производится сравнение наименований и типов свойств со свойствами <typeparamref name="Tsource"/>.
        /// Если имя и тип совпадают, значения свойства в <paramref name="dest"/>
        /// заполняется значением из <paramref name="source"/>.</remarks>
        public static List<string> FillCommonProperties<Tsource, Tdest>(Tsource source, Tdest dest) {
            List<string> propNames = new List<string>();
            var destType = typeof(Tdest);
            var destProperties = destType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(p => p.CanWrite);
            var sourceType = typeof(Tsource);
            var sourceProperties = sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            var propPairs = destProperties
                .Select(p => new { DestPropInfo = p, SourcePropInfo = sourceProperties.FirstOrDefault(sp => sp.Name == p.Name) });
            foreach (var pair in propPairs) {
                PropertyInfo piDest = pair.DestPropInfo;
                if (pair.SourcePropInfo != null) {
                    PropertyInfo piSource = pair.SourcePropInfo;
                    if (piSource.PropertyType.IsEnum && piDest.PropertyType.IsAssignableFrom(typeof(int))) {
                        var value = (int)piSource.GetValue(source);
                        piDest.SetValue(dest, value);
                        continue;
                    }
                    else if (piSource.PropertyType.IsConstructedGenericType && piSource.PropertyType.IsEnum && piDest.PropertyType.IsAssignableFrom(typeof(int))) {
                        var value = piSource.GetValue(source);
                        if (!(value is null))
                            piDest.SetValue(dest, (int)value);
                        continue;
                    }
                    else if (IsSimpleType(piDest.PropertyType) && piDest.PropertyType.IsAssignableFrom(piSource.PropertyType)) {
                        var value = piSource.GetValue(source);
                        if (!(value is null))
                            piDest.SetValue(dest, value);
                        continue;
                    }
                    else if (TryGetConverter(pair.DestPropInfo.Name, piDest.PropertyType, piSource.PropertyType, out var convert)) {
                        var value = piSource.GetValue(source);
                        if (!(value is null)) {
                            var convertedValue = convert(value);
                            piDest.SetValue(dest, convertedValue);
                        }
                        continue;
                    }
                    else { // Fill compatible Enums
                        if ( // alwais true: piDest.PropertyType.Name == piSource.PropertyType.Name &&
                            piDest.PropertyType.IsEnum) {
                            if (piSource.PropertyType.IsEnum) {
                                object enumValue = piSource.GetValue(source);
                                string strValue = enumValue.ToString();
                                object targetEnumValue = Enum.Parse(piDest.PropertyType, strValue, ignoreCase: true);
                                piDest.SetValue(dest, enumValue);
                                continue;
                            }
                        }
                        else if (IsNullableEnum(piDest)) {
                            if (piSource.PropertyType.IsEnum ||
                                IsNullableEnum(piSource)) {
                                object? enumValue = piSource.GetValue(source);
                                if (enumValue is not null) {
                                    object targetEnumValue = Enum.Parse(piDest.PropertyType.GenericTypeArguments[0], enumValue.ToString(), ignoreCase: true);
                                    piDest.SetValue(dest, targetEnumValue);
                                }
                                continue;
                            }
                        }
                    }
                }
                if (piDest.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() is null) {
                    propNames.Add(pair.DestPropInfo.Name);
                }
            }
            return propNames;
        }


        /// <summary>
        /// Производит поиск доступных для записи свойств простого типа и заполняет экземпляр значениями из переданной строки
        /// </summary>
        /// <param name="product">Экземпляр типа <typeparamref name="Tdest"/></param>
        /// <param name="row">Строка из таблицы <typeparamref name="SheetCommon"/></param>
        /// <returns>Список свойств типа <typeparamref name="Tdest"/>, не найденных в колонках таблицы <typeparamref name="SheetCommon"/></returns>
        public static IEnumerable<string> LoadRow<Tdest>(this Tdest product, SheetCommon.Row row) where Tdest : class {

            var allProperties = typeof(Tdest).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            var notFound = new HashSet<string>();
            foreach (var property in allProperties) {
                if (property.CanWrite) {
                    notFound.Add(property.Name);
                }
            }
            var setters = GetColumnSetters<Tdest>();
            var titles = row.Titles;
            foreach (var item in setters) {
                string propName = item.PropertyName;
                int propIndex = Array.IndexOf(titles, propName);
                if (propIndex != -1) {
                    string? value = row[propName];
                    if (!string.IsNullOrEmpty(value)) {
                        try {
                            item.Setter(product, value!);
                        }
                        catch (Exception ex) {
                            throw new InvalidOperationException($"Error attemp set value \"{value}\" to property {propName}.", ex);
                        }
                    }
                    notFound.Remove(propName);
                }
                else {

                }
            }
            return notFound.ToArray();
        }

        public static IEnumerable<string> LoadRow(this DbScsGutterUtilitySet product, SheetCommon.Row row) {
            var notMapped = LoadRow<DbScsGutterUtilitySet>(product, row);
            Queue<string> notFound = new Queue<string>();
            string value;
            foreach (var name in notMapped) {
                switch (name) {
                    case nameof(DbScsGutterUtilitySet.LevelType):
                        if (row.Titles.Contains(nameof(DbScsGutterUtilitySet.LevelType))) {
                            value = row[nameof(DbScsGutterUtilitySet.LevelType)];
                            product.LevelType = Convert<DbGcKnotLevelType>(value);
                            break;
                        }
                        break;
                    case nameof(DbScsGutterUtilitySet.StandType):
                        if (row.Titles.Contains(nameof(DbScsGutterUtilitySet.StandType))) {
                            value = row[nameof(DbScsGutterUtilitySet.StandType)];
                            product.StandType = Convert<DbGcKnotStandType>(value);
                            break;
                        }
                        break;
                    case nameof(DbScsGutterUtilitySet.StructureType):
                        if (row.Titles.Contains(nameof(DbScsGutterUtilitySet.StructureType))) {
                            value = row[nameof(DbScsGutterUtilitySet.StructureType)];
                            product.StructureType = Convert<DbGcKnotStandStructureType>(value);
                            break;
                        }
                        break;
                    case nameof(DbScsGutterUtilitySet.KnotType):
                        if (row.Titles.Contains(nameof(DbScsGutterUtilitySet.KnotType))) {
                            value = row[nameof(DbScsGutterUtilitySet.KnotType)];
                            product.KnotType = Convert<ScsGcStandType>(value);
                        }
                        break;
                    case nameof(DbScsGutterUtilitySet.InstallType):
                        if (row.Titles.Contains(nameof(DbScsGutterUtilitySet.InstallType))) {
                            value = row[nameof(DbScsGutterUtilitySet.InstallType)];
                            product.InstallType = Convert<DbGcKnotInstallType>(value);
                        }
                        break;
                    default:
                        notFound.Enqueue(name);
                        break;
                }
            }
            return notFound;
        }
        public static TEnum Convert<TEnum>(string description) where TEnum : struct, Enum {
            return EnumConverter<TEnum>.Convert(description);
        }
        public static bool TryConvert<TEnum>(string description, out TEnum value) where TEnum : struct, Enum {

            return EnumConverter<TEnum>.TryConvert(description, out value);
        }
        private static bool IsNullableEnum(PropertyInfo piSource) {
            return (piSource.PropertyType.IsGenericType &&
                                                piSource.PropertyType.GenericTypeArguments.Length == 1
                                                && piSource.PropertyType.GenericTypeArguments[0].IsEnum
                                            );
        }
        private static bool IsSimpleType(Type type) {
            return type.IsPrimitive || type == typeof(string) || type.IsEnum || IsNullableFromSimple(type);
        }
        private static bool IsNullableFromSimple(Type type) {
            return type.IsConstructedGenericType
                && (type.GenericTypeArguments[0].IsPrimitive || type.GenericTypeArguments[0] == typeof(string) || type.GenericTypeArguments[0].IsEnum);
        }
        private static string GetCacheKeyConverter(string name, Type targetType, Type sourceType) {
            return $"{name}::Converter|{targetType.FullName}|{sourceType.FullName}";
        }

        private static bool TryComputeConverter(string name, Type targetType, Type sourceType, out Func<object?, object?> convert) {
            if (sourceType == typeof(ScsGcStandMountType) ||
                sourceType == typeof(ScsGcStandMountType?)) {
                return GetConverter<ScsGcStandMountType>(targetType, $"Для свойства {name} преобразование {sourceType.FullName} => {targetType.FullName} не найдено.", out convert);
            }
            else if (sourceType == typeof(ScsGcStandType) ||
                sourceType == typeof(ScsGcStandType?)) {
                return GetConverter<ScsGcStandType>(targetType, $"Для свойства {name} преобразование {sourceType.FullName} => {targetType.FullName} не найдено.", out convert);
            }
            else if (sourceType == typeof(ScsGcConsoleMountType) ||
                sourceType == typeof(ScsGcConsoleMountType?)) {
                return GetConverter<ScsGcConsoleMountType>(targetType, $"Для свойства {name} преобразование {sourceType.FullName} => {targetType.FullName} не найдено.", out convert);
            }
            else if (sourceType == typeof(CaeMeasureUnitEnum) ||
                sourceType == typeof(CaeMeasureUnitEnum?)) {
                return GetConverter<CaeMeasureUnitEnum>(targetType, $"Для свойства {name} преобразование {sourceType.FullName} => {targetType.FullName} не найдено.", out convert);
            }
            // Not any found
            convert = (o) => throw new InvalidCastException($"Для свойства {name} преобразование {sourceType.FullName} => {targetType.FullName} не найдено.");
            return false;
        }

        private static bool GetConverter<Tenum>(Type targetType, string errMessage, out Func<object?, object?> convert)
            where Tenum : struct, Enum {
            if (targetType == typeof(string)) {
                convert = (o) => {
                    if (o is null)
                        return null;
                    string value = ReflectionHelper.GetDescription<Tenum>((Tenum)o);
                    return value;
                };
                return true;
            }
            else if (targetType == (typeof(int?))) {
                convert = (o) => {
                    if (o is null)
                        return null;
                    IConvertible convertible = (IConvertible)o;
                    int value = convertible.ToInt32(CultureInfo.InvariantCulture);
                    return value;
                };
                return true;
            }
            else if (targetType.IsAssignableFrom(typeof(int))) {
                convert = (o) => {
                    if (o is null)
                        return 0;
                    IConvertible convertible = (IConvertible)o;
                    int value = convertible.ToInt32(CultureInfo.InvariantCulture);
                    return value;
                };
                return true;
            }
            else {
                convert = (o) => throw new InvalidCastException(errMessage);
                return false;
            }
        }
        private static bool TryGetConverter(string name, Type targetType, Type sourceType, out Func<object?, object?> convert) {
            var cacheKey = GetCacheKeyConverter(name, targetType, sourceType);
            CacheConverterValue cacheValue;
            if(cacheConverters.TryGetValue(cacheKey, out cacheValue)) {
            //if (cacheConverters.Contains(cacheKey)) {
            //    cacheValue = (CacheConverterValue)cacheConverters.Get(cacheKey);
                convert = cacheValue.Convert;
                return cacheValue.HasConverter;
            }
            cacheValue = new CacheConverterValue { HasConverter = TryComputeConverter(name, targetType, sourceType, out convert), Convert = convert };
            cacheConverters.Set(cacheKey, cacheValue);
            convert = cacheValue.Convert;
            return cacheValue.HasConverter;


        }
        
        private static char[]? invalidCharts = null;
        /// <summary>
        /// Преобразует имя файла в допустимое имя, заменяя не разрешенные символы знаком подчеркивания.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static string GetValidFileName(string fileName) {
            if (invalidCharts is null) {
                invalidCharts = Path.GetInvalidFileNameChars();
                Array.Sort(invalidCharts);
            }
            char[] valid = fileName.ToCharArray();
            for (int i = 0; i < valid.Length; i++) {
                if (Array.BinarySearch(invalidCharts, valid[i]) >= 0) {
                    valid[i] = '_';
                }
            }
            string sValid = new string(valid);
            return sValid;
        }
        
    }
}
