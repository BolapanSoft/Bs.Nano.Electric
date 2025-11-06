using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#if NETFRAMEWORK
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;
#endif
namespace Nano.Electric.Extensions {
    public static class DbContextExtensions {
        //// Кеш метаданных на экземпляр DbContext.
        //// ConditionalWeakTable хранит "ключ" (DbContext) слабой ссылкой — при сборке мусора контекста запись удаляется.
        //// Значение CachedMeta не должно хранить ссылку на сам DbContext (и не хранит).
        //private static readonly ConditionalWeakTable<object, CachedMeta> _cache = new();

        //private sealed class CachedMeta {
        //    // Список записей: имя свойства DbSet, тип сущности, локализованное описание, имя таблицы и схема (если есть)
        //    public List<CachedEntry> Entries { get; } = new List<CachedEntry>();
        //}

        //private sealed class CachedEntry {
        //    public string PropertyName { get; init; } = string.Empty; // имя свойства DbSet<T>
        //    public Type EntityType { get; init; } = null!;
        //    public string TableDescription { get; init; } = string.Empty;
        //    public string? Schema { get; init; } // может быть null
        //    public string TableName { get; init; } = string.Empty;

        //    // Кешируем одноразово вычисленный count (nullable — если null, ещё не вычислено)
        //    // Важно: это значение актуально для конкретного экземпляра source, т.к. кеш привязан к экземпляру контекста.
        //    public int? CachedCount;
        //}

        /// <summary>
        /// Возвращает информацию по DbSet-ам в DbContext: сам экземпляр DbSet (получается динамически),
        /// локализованное описание типа, тип сущности и (по возможности) количество строк.
        /// </summary>
        public static IEnumerable<(object property, string tableDescription, Type EntityType, int count)> GetKnownTables(this Context source) {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var ctxType = source.GetType();

            var dbSetProperties = ctxType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => {
                    var pt = p.PropertyType;
                    return IsDbSetType(pt);
                });

            foreach (PropertyInfo propInfo in dbSetProperties) {
                Type propType = propInfo.PropertyType.GetGenericArguments()[0];
                string tableDescription = Context.GetDefaultLocalizeValue(propType); // ваш метод
                object? dbSetInstance = propInfo.GetValue(source); // Get the instance of the DbSet<T> property
                if (dbSetInstance is null) { continue; }

                int count = 0;
                try {
                    // GetCount должен принимать IQueryable
                    count = GetCount((IQueryable)dbSetInstance);
                }
                catch {
                    continue;
                }
                yield return (dbSetInstance, tableDescription, propType, count);
            }
        }

        // --- вспомогательные методы ---
        private static bool IsDbSetType(Type t) {
            if (t == null)
                return false;
            if (!t.IsGenericType)
                return false;

            var gd = t.GetGenericTypeDefinition();
            // Быстрый и безопасный тест по имени generic дефиниции
            // Обычно имя будет "DbSet`1".
            if (string.Equals(gd.Name, "DbSet`1", StringComparison.Ordinal))
                return true;

            // Дополнительные проверки по fullName/namespace — на случай вариаций:
            var full = gd.FullName ?? string.Empty;

            // Покрываем EF6 и EF Core возможные полные имена
            if (full.Equals("System.Data.Entity.DbSet`1", StringComparison.Ordinal)
                || full.Equals("Microsoft.EntityFrameworkCore.DbSet`1", StringComparison.Ordinal))
                return true;

            // Иногда generic def может иметь другое пространство имён — ищем просто "DbSet`1" в конце
            if (full.EndsWith(".DbSet`1", StringComparison.Ordinal))
                return true;

            return false;
        }
        private static int GetCount(IQueryable q) {
            if (q == null)
                throw new ArgumentNullException(nameof(q));

            var elementType = q.ElementType;

            // Queryable.Count<T>(IQueryable<T>)
            var countMethod = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == "Count" && m.GetParameters().Length == 1)
                .MakeGenericMethod(elementType);

            var callExpr = Expression.Call(null, countMethod, q.Expression);

            try {
                // Попытка выполнить как SQL COUNT(*)
                var result = q.Provider.Execute(callExpr);
                if (result == null)
                    return 0;

                try {
                    return Convert.ToInt32(result);
                }
                catch (OverflowException) {
                    return int.MaxValue;
                }
            }
            catch {
                try {
                    // Безопасный фоллбек: посчитать через IEnumerable
                    var enumerable = (IEnumerable)q;
                    int count = 0;
                    var enumerator = enumerable.GetEnumerator();
                    while (enumerator.MoveNext())
                        count++;
                    return count;
                }
                catch {
                    return 0;
                }
            }
        }

        //        // Попытка безопасно получить название таблицы и схему из модели (EF Core или EF6).
        //        // Возвращает (schema, table) — если не удалось определить, возвращает (null, null).
        //        private static (string? schema, string? table) GetTableSchemaAndNameFromModel(object contextInstance, Type entityType) {
        //#if NETFRAMEWORK
        //        // EF6 path
        //        try
        //        {
        //            var oc = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)contextInstance).ObjectContext;
        //            var mdw = oc.MetadataWorkspace;

        //            // Ищем соответствующий EntitySet в SSpace (storage)
        //            var entitySets = mdw.GetItems<EntityContainer>(DataSpace.SSpace)
        //                                .SelectMany(c => c.BaseEntitySets);

        //            // Элемент может быть назван по-разному, используем совпадение с именем типа сущности (ElementType.Name)
        //            var set = entitySets.FirstOrDefault(s =>
        //                string.Equals(s.ElementType.Name, entityType.Name, StringComparison.OrdinalIgnoreCase) ||
        //                string.Equals(s.Name, entityType.Name, StringComparison.OrdinalIgnoreCase));

        //            if (set == null) return (null, null);

        //            string table = (set.MetadataProperties.Contains("Table") && set.MetadataProperties["Table"].Value != null)
        //                ? set.MetadataProperties["Table"].Value.ToString()!
        //                : set.Name;

        //            string? schema = (set.MetadataProperties.Contains("Schema") && set.MetadataProperties["Schema"].Value != null)
        //                ? set.MetadataProperties["Schema"].Value.ToString()
        //                : null;

        //            return (schema, table);
        //        }
        //        catch
        //        {
        //            return (null, null);
        //        }
        //#else
        //            // EF Core path
        //            try {
        //                var modelProperty = contextInstance.GetType().GetProperty("Model");
        //                if (modelProperty == null) {
        //                    // Попробуем через DbContext.Model (т.к. contextInstance может быть из другого типа)
        //                    var et = TryFindEntityTypeInEfCoreModel(contextInstance, entityType);
        //                    if (et == null)
        //                        return (null, null);
        //                    var table = GetTableNameFromEfCoreEntityType(et, out var schema);
        //                    return (schema, table);
        //                }
        //                else {
        //                    var model = modelProperty.GetValue(contextInstance);
        //                    var findMethod = model.GetType().GetMethod("FindEntityType", new[] { typeof(Type) });
        //                    if (findMethod == null)
        //                        return (null, null);
        //                    var et = findMethod.Invoke(model, new object[] { entityType });
        //                    if (et == null)
        //                        return (null, null);
        //                    var table = GetTableNameFromEfCoreEntityType(et, out var schema);
        //                    return (schema, table);
        //                }
        //            }
        //            catch {
        //                return (null, null);
        //            }
        //#endif
        //        }

        //#if !NETFRAMEWORK
        //        // Helper: obtain table name and schema using reflection on IEntityType (EF Core internals)
        //        private static string? GetTableNameFromEfCoreEntityType(object entityTypeMetadata, out string? schema) {
        //            schema = null;
        //            try {
        //                // entityTypeMetadata has methods GetTableName() and GetSchema() in EF Core
        //                var getTableName = entityTypeMetadata.GetType().GetMethod("GetTableName", Type.EmptyTypes);
        //                var getSchema = entityTypeMetadata.GetType().GetMethod("GetSchema", Type.EmptyTypes);
        //                if (getTableName == null)
        //                    return null;
        //                var tableName = getTableName.Invoke(entityTypeMetadata, null)?.ToString();
        //                schema = getSchema?.Invoke(entityTypeMetadata, null)?.ToString();
        //                return tableName;
        //            }
        //            catch {
        //                schema = null;
        //                return null;
        //            }
        //        }

        //        // Try to find IEntityType in EF Core model via reflection if we don't have direct access to Microsoft.EntityFrameworkCore types.
        //        private static object? TryFindEntityTypeInEfCoreModel(object contextInstance, Type entityType) {
        //            try {
        //                var modelProp = contextInstance.GetType().GetProperty("Model");
        //                if (modelProp == null)
        //                    return null;
        //                var model = modelProp.GetValue(contextInstance);
        //                var findMethod = model.GetType().GetMethod("FindEntityType", new[] { typeof(Type) });
        //                if (findMethod == null)
        //                    return null;
        //                return findMethod.Invoke(model, new object[] { entityType });
        //            }
        //            catch {
        //                return null;
        //            }
        //        }
        //#endif

        //        // Попробуем определить, существует ли таблица в физической базе данных (provider-specific).
        //        // Возвращает true, если таблица найдена или если проверка невозможна — тогда мы считаем "существует" (по консервативной логике).
        //        // Для некоторых провайдеров (SQLite, SQL Server) выполнена явная проверка.
        //        private static bool TryCheckTableExistsInDatabase(object contextInstance, string? schema, string table) {
        //            if (string.IsNullOrEmpty(table))
        //                return false;

        //            try {
        //                DbConnection? conn = null;
        //#if NETFRAMEWORK
        //            // EF6
        //            var dbContext = (System.Data.Entity.DbContext)contextInstance;
        //            conn = dbContext.Database.Connection;
        //#else
        //                // EF Core
        //                var getConnMethod = contextInstance.GetType().GetMethod("Database")?.DeclaringType == null ? null : null;
        //                // simpler: try to get property Database and call GetDbConnection()
        //                var dbProp = contextInstance.GetType().GetProperty("Database");
        //                if (dbProp != null) {
        //                    var dbObj = dbProp.GetValue(contextInstance);
        //                    var getDbConn = dbObj?.GetType().GetMethod("GetDbConnection");
        //                    if (getDbConn != null) {
        //                        conn = (DbConnection?)getDbConn.Invoke(dbObj, null);
        //                    }
        //                }
        //#endif
        //                if (conn == null)
        //                    return true; // не смогли получить connection — считаем "существует" (консервативно)

        //                bool needClose = false;
        //                if (conn.State != ConnectionState.Open) {
        //                    conn.Open();
        //                    needClose = true;
        //                }

        //                // Подготовим несколько подходов: SQLite (sqlite_master), SQL Server (INFORMATION_SCHEMA.TABLES), generic INFORMATION_SCHEMA
        //                string tableOnly = table;
        //                string schemaOnly = schema;

        //                // Detect provider by connection.GetType().Name or connection.GetType().FullName
        //                var connTypeName = conn.GetType().Name.ToLowerInvariant();

        //                if (connTypeName.Contains("sqlite")) {
        //                    using var cmd = conn.CreateCommand();
        //                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name = @p";
        //                    var p = cmd.CreateParameter();
        //                    p.ParameterName = "@p";
        //                    p.Value = tableOnly;
        //                    cmd.Parameters.Add(p);
        //                    using var rdr = cmd.ExecuteReader();
        //                    var exists = rdr.Read();
        //                    if (needClose)
        //                        conn.Close();
        //                    return exists;
        //                }
        //                else if (connTypeName.Contains("sqlconnection") || connTypeName.Contains("sqlclient")) {
        //                    using var cmd = conn.CreateCommand();
        //                    cmd.CommandText = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tbl" + (string.IsNullOrEmpty(schemaOnly) ? "" : " AND TABLE_SCHEMA = @sch");
        //                    var pt = cmd.CreateParameter();
        //                    pt.ParameterName = "@tbl";
        //                    pt.Value = tableOnly;
        //                    cmd.Parameters.Add(pt);
        //                    if (!string.IsNullOrEmpty(schemaOnly)) {
        //                        var ps = cmd.CreateParameter();
        //                        ps.ParameterName = "@sch";
        //                        ps.Value = schemaOnly;
        //                        cmd.Parameters.Add(ps);
        //                    }
        //                    var scalar = cmd.ExecuteScalar();
        //                    var exists = Convert.ToInt32(scalar ?? 0) > 0;
        //                    if (needClose)
        //                        conn.Close();
        //                    return exists;
        //                }
        //                else {
        //                    // Fallback: try generic INFORMATION_SCHEMA
        //                    using var cmd = conn.CreateCommand();
        //                    cmd.CommandText = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tbl";
        //                    var pt = cmd.CreateParameter();
        //                    pt.ParameterName = "@tbl";
        //                    pt.Value = tableOnly;
        //                    cmd.Parameters.Add(pt);
        //                    var scalar = cmd.ExecuteScalar();
        //                    if (needClose)
        //                        conn.Close();
        //                    if (scalar == null)
        //                        return true; // не смогли проверить — считаем "существует"
        //                    return Convert.ToInt32(scalar) > 0;
        //                }
        //            }
        //            catch {
        //                // если проверка не удалась по какой-то причине — оставим консервативное поведение: позволяем считать что таблица существует,
        //                // чтобы не скрывать данные в случаях, когда драйвер не поддерживается.
        //                return true;
        //            }
        //        }

        //// Получение количества записей из IQueryable (работает и с EF6, и с EF Core).
        //// Используем Expression.Call(Queryable.Count, elementType) и Provider.Execute<int>.
        //private static int GetCountFromQueryable(IQueryable q) {
        //    if (q == null)
        //        throw new ArgumentNullException(nameof(q));
        //    var elementType = q.ElementType;
        //    var countMethod = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
        //        .Where(m => m.Name == "Count" && m.GetParameters().Length == 1)
        //        .Single()
        //        .MakeGenericMethod(elementType);

        //    var expr = Expression.Call(null, countMethod, q.Expression);
        //    var result = q.Provider.Execute(expr);
        //    return Convert.ToInt32(result);
        //}

        //// Вспомогалка: безопасное локализованное имя типа (здесь вызывает ваш статический метод Context.GetDefaultLocalizeValue если есть)
        //private static string GetDefaultLocalizeValueSafe(Type type) {
        //    try {
        //        // Если у вас есть метод Context.GetDefaultLocalizeValue(Type), вызовите его
        //        var ctxType = typeof(object).Assembly; // dummy to avoid compiler warning; замените ниже если нужно
        //                                               // Используем рефлексию на тип Context, если он доступен в сборке
        //        var contextType = AppDomain.CurrentDomain.GetAssemblies()
        //            .SelectMany(a => a.GetTypesSafe())
        //            .FirstOrDefault(t => t.Name == "Context");
        //        if (contextType != null) {
        //            var method = contextType.GetMethod("GetDefaultLocalizeValue", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
        //            if (method != null) {
        //                var val = method.Invoke(null, new object[] { type });
        //                return val?.ToString() ?? type.Name;
        //            }
        //        }

        //        return type.Name;
        //    }
        //    catch {
        //        return type.Name;
        //    }
        //}

        //// Extensions helper to get types from assembly safely (ignore reflection load exceptions)
        //private static IEnumerable<Type> GetTypesSafe(this Assembly a) {
        //    Type[] types;
        //    try { types = a.GetTypes(); }
        //    catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray()!; }
        //    return types;
        //}
    }

}