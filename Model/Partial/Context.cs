//using DocumentFormat.OpenXml.Bibliography;

// Ignore Spelling: Tdest

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
#if NETFRAMEWORK
using System.Data.Entity;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
#else
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

#endif

namespace Nano.Electric {
    public partial class Context {
        private class TableColumn {
            public string Name { get; set; }
        }

        private static Dictionary<string, string[]> propertiesCache = new Dictionary<string, string[]>();
        private static readonly Dictionary<Type, string> knownLocalizeValues = new Dictionary<Type, string>();
        /// <summary>
        /// Выполняет заполнение свойств сущности из сериализованного в строку источника.
        /// </summary>
        /// <typeparam name="Tdest">Тип сущности</typeparam>
        /// <param name="product">Заполняемый экземпляр.</param>
        /// <param name="propNames">Перечень наименований свойств для заполнения.</param>
        /// <param name="item">Источник сериализованных в строку значений свойств.</param>
        /// <returns>Перечень обработанных свойств и связанных исключений.</returns>
        /// <remarks>Если свойство не удалось заполнить - возвращается имя свойства и возникшее при обработке исключение. 
        /// Процедура обрабатывает только доступные для записи свойства. 
        /// Если свойство является частью ключа сущности, то оно не может быть заполнено.</remarks>
        public IList<(string, Exception)> FillProperties<Tdest>(Tdest product, IEnumerable<string> propNames, IReadOnlyDictionary<string, string> item, bool skipIfEmptyValue = true) where Tdest : class {
            var results = new List<(string, Exception)>();
            foreach (var propName in propNames) {
                var propertyInfo = typeof(Tdest).GetProperty(propName);
                if (propertyInfo == null || !propertyInfo.CanWrite) {
                    continue;
                }

                if (propertyInfo.GetCustomAttribute<KeyAttribute>() != null) {
                    results.Add((propName, new InvalidOperationException($"Операция не разрешена. Свойство {propName} является частью ключа.")));

                    continue;
                }
                string sourceValue = item[propName];
                if (skipIfEmptyValue && string.IsNullOrEmpty(sourceValue)) {
                    continue;
                }
                try {
                    propertyInfo.SetValue(product, sourceValue, BindingFlags.Public, FieldBinder.Instance, null, CultureInfo.GetCultureInfo("Ru-ru"));
                }
                catch (Exception ex) {
                    results.Add((propName, new InvalidOperationException($"Операция не выполнена. Значение \"{sourceValue}\" столбца {propName} не удается привести к типу {propertyInfo.PropertyType}.", ex)));
                }
            }
            return results;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Tdest"></typeparam>
        /// <typeparam name="Tprop"></typeparam>
        /// <param name="product"></param>
        /// <param name="propName"></param>
        /// <param name="sourceValue"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetProperty<Tdest, Tprop>(Tdest product, string propName, Tprop sourceValue) where Tdest : class {
            var productType = product.GetType();
            var propertyInfo = productType.GetProperty(propName);
            if (propertyInfo == null) { throw new ArgumentException($"Свойство {propName} не найдено в типе {productType.Name}."); }
            if (!propertyInfo.CanWrite) {
                throw new ArgumentException($"Свойство {propName} не в типе {productType.Name} не доступно для записи.");
            }
            if (propertyInfo.GetCustomAttribute<KeyAttribute>() != null) {
                throw new InvalidOperationException($"Операция не разрешена. Свойство {propName} является частью ключа.");
            }
            try {
                propertyInfo.SetValue(product, sourceValue, BindingFlags.Public, FieldBinder.Instance, null, CultureInfo.GetCultureInfo("Ru-ru"));
            }
            catch (Exception ex) {
                throw new InvalidOperationException($"Операция не выполнена. Значение \"{sourceValue}\" свойства {propName} не удается привести к типу {propertyInfo.PropertyType}.", ex);
            }
        }
        private static bool TryConvertToBool(string sourceValue, out bool? result) {
            if (string.Compare(sourceValue, "True", ignoreCase: true, CultureInfo.InvariantCulture) == 0 ||
                string.Compare(sourceValue, "Да", ignoreCase: true, CultureInfo.GetCultureInfo("Ru-ru")) == 0
                ) {
                result = true;
                return true;
            }
            else if (string.Compare(sourceValue, "False", ignoreCase: true, CultureInfo.InvariantCulture) == 0 ||
                string.Compare(sourceValue, "Нет", ignoreCase: true, CultureInfo.GetCultureInfo("Ru-ru")) == 0
                ) {
                result = false;
                return true;
            }
            else {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Tdest"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string[] GetProductProperties<Tdest>() where Tdest : class {
            var type = typeof(Tdest);
            var typeName = type.Name;
            if (propertiesCache.TryGetValue(typeName, out var properties)) {
                return properties;
            }
            var contextType = typeof(Context);
            if (type.Assembly != contextType.Assembly) {
                throw new InvalidOperationException($"Тип {typeName} не принадлежит сборке, в которой определен класс Nano.Electric.Context.");
            }
            var attr = typeof(Tdest).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            if (attr is null) {
                throw new InvalidOperationException($"Тип {typeName} не имеет атрибута TableAttribute.");
            }
            // out public propertis, exept properties width attribute [NotMapped]
            properties = type.GetProperties()
                .Where(p => /*p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() == null &&*/ p.CanWrite)
                .Select(p => p.Name)
                .ToArray();
            propertiesCache[typeName] = properties;
            return properties;
        }
        public static string GetDefaultLocalizeValue<T>() where T : class {
            Type productType = typeof(T);
            return GetDefaultLocalizeValue(productType);
        }
        public static string GetDatabaseTableName(Type productType) {
            var table = productType.GetCustomAttribute<TableAttribute>()?.Name ?? productType.Name;
            return table;
        }
        public static string GetDefaultLocalizeValue(Type productType) {
            if (knownLocalizeValues.ContainsKey(productType))
                return knownLocalizeValues[productType];
            var value = productType.GetCustomAttribute<DefaultLocalizeValueAttribute>()?.DefaultLocalizeValue ?? string.Empty;
            knownLocalizeValues.Add(productType, value);
            return value;
        }
        public DbImage CreateImage(string? imgName, string category, byte[] image) {
            //DbImage dbImg = this.DbImages.Create();
            var dbImg = new DbImage {
                Text = imgName ?? string.Empty,
                Category = category,
                Image = image
            };
            //this.DbImages.Add(dbImg);
            int id = 1;
            if (this.DbImages.Local.Any()) {
                id = Math.Max(id, this.DbImages.Local.Max(img => img.Id) + 1);
            }
            if (this.DbImages.Any()) {
                id = Math.Max(this.DbImages.Max(img => img.Id) + 1, id);
            }
            dbImg.Id = id;
            //dbImg.Text = imgName ?? string.Empty;
            //dbImg.Category = category;
            //dbImg.Image = image;
            this.DbImages.Add(dbImg);
            return dbImg;
        }
#if NETFRAMEWORK
        public Context(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) {
        }
        partial void InitializeModel(DbModelBuilder modelBuilder) { 
#else
        public Context(DbConnection existingConnection, bool contextOwnsConnection)
           : base(new DbContextOptionsBuilder<Context>().UseSqlite(existingConnection).Options) {
        }
        partial void InitializeModel(ModelBuilder modelBuilder) {
#endif
            // var connectionString = this.Database.Connection.ConnectionString;
            //    var providerName = this.Database.Connection.GetType().Name;

#if NETFRAMEWORK
            if (this.Database.Connection is SqlCeConnection) {
                modelBuilder.Conventions.Add(new NanoCadPropertiesConvention());
            }
#endif
            modelBuilder.Entity<CaeMaterialUtility>()
                .Property(t => t.MeashureUnits)
                .HasColumnName("MeashureUnits");
        }

        public bool IsHaveColumns(string tableName, params string[] columns) {
#if NET8_0_OR_GREATER
            var connection = Database.GetDbConnection(); // Используем GetDbConnection() в EF Core
            if (connection is Microsoft.Data.Sqlite.SqliteConnection) {
#else
    var connection = Database.Connection;
    if (connection is System.Data.SQLite.SQLiteConnection) {
#endif
                try {
                    var tableSchema = Database.SqlQuery<TableColumn>($"PRAGMA table_info({tableName});").ToList();
                    var columnNames = tableSchema.Select(col => col.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
                    return columns.All(column => columnNames.Contains(column));
                }
                catch (Exception) {
                    return false;
                }
            }

#if NETFRAMEWORK
    if (connection is System.Data.SqlServerCe.SqlCeConnection) {
        try {
            string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName";
            var columnNames = Database.SqlQuery<string>(query, new System.Data.SqlClient.SqlParameter("@tableName", tableName))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            return columns.All(column => columnNames.Contains(column));
        }
        catch (Exception) {
            return false;
        }
    }
#endif

            throw new NotImplementedException($"Не реализовано для типа подключения {connection.GetType().Name}");
        }
    }
}

