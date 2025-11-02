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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
#endif
using Microsoft.Extensions.Caching.Memory;

namespace Nano.Electric {
    public partial class Context {
        private class TableColumn {
            public string Name { get; set; }
        }
        private static readonly MemoryCache _cache = new(new MemoryCacheOptions());
        //private static Dictionary<string, string[]> propertiesCache = new Dictionary<string, string[]>();
        //private static readonly Dictionary<Type, string> knownLocalizeValues = new Dictionary<Type, string>();
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
        public IList<(string, Exception)> LoadProperties<Tdest>(Tdest product, IEnumerable<string> propNames, IDictionary<string, string> dest) where Tdest : class {
            var results = new List<(string, Exception)>();
            foreach (var propName in propNames) {
                var propertyInfo = typeof(Tdest).GetProperty(propName);
                if (propertyInfo == null || !propertyInfo.CanRead) {
                    continue;
                }
                try {
                    var value = propertyInfo.GetValue(product, BindingFlags.Public, FieldBinder.Instance, null, CultureInfo.GetCultureInfo("Ru-ru"));
                    if (value is null) {
                        dest[propName] = string.Empty;
                        continue;
                    }
                    var type = propertyInfo.PropertyType;
                    var underlyingType = Nullable.GetUnderlyingType(type) ?? type; // если Nullable<T>, достаем T

                    if (underlyingType.IsEnum) {
                        string description = ReflectionHelper.GetEnumDescription(underlyingType, value);
                        dest[propName] = description;
                        continue;
                    }
                    else {
                        dest[propName] = value?.ToString() ?? string.Empty;
                        continue;
                    }
                }
                catch (Exception ex) {
                    results.Add((propName, new InvalidOperationException($"Не удалось выполнить чтение свойства {propName}.", ex)));
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
            //if (propertiesCache.TryGetValue(typeName, out var properties)) {
            //    return properties;
            //}
            if (_cache.TryGetValue(typeName, out string[]? props) && props is not null) {
                return props;
            }
            // Use GetOrCreate for thread-safe lazy initialization
            return _cache.GetOrCreate(typeName, entry => {
                //entry.SlidingExpiration = TimeSpan.FromMinutes(30); // optional, adjust to your needs
                entry.Priority = CacheItemPriority.Normal;

                var contextType = typeof(Context);
                if (type.Assembly != contextType.Assembly) {
                    throw new InvalidOperationException(
                        $"Тип {typeName} не принадлежит сборке, в которой определен класс {contextType.FullName}.");
                }

                var attr = type.GetCustomAttribute<TableAttribute>();
                if (attr is null) {
                    throw new InvalidOperationException(
                        $"Тип {typeName} не имеет атрибута {nameof(TableAttribute)}.");
                }

                // Collect public properties, excluding [NotMapped] and read-only
                return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p =>
                        p.CanWrite &&
                        p.GetCustomAttribute<NotMappedAttribute>() == null)
                    .Select(p => p.Name)
                    .ToArray();
            })!;
            //var contextType = typeof(Context);
            //if (type.Assembly != contextType.Assembly) {
            //    throw new InvalidOperationException($"Тип {typeName} не принадлежит сборке, в которой определен класс Nano.Electric.Context.");
            //}
            //var attr = typeof(Tdest).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            //if (attr is null) {
            //    throw new InvalidOperationException($"Тип {typeName} не имеет атрибута TableAttribute.");
            //}
            //// out public propertis, exept properties width attribute [NotMapped]
            //properties = type.GetProperties()
            //    .Where(p => /*p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() == null &&*/ p.CanWrite)
            //    .Select(p => p.Name)
            //    .ToArray();
            //propertiesCache[typeName] = properties;
            //return properties;
        }
        public static string GetDefaultLocalizeValue<T>() where T : class {
            Type productType = typeof(T);
            return GetDefaultLocalizeValue(productType);
        }
        public static string GetDatabaseTableName(Type productType) {
            var table = productType.GetCustomAttribute<TableAttribute>()?.Name ?? productType.Name;
            return table;
        }
        //public static string GetDefaultLocalizeValue(Type productType) {
        //    if (knownLocalizeValues.ContainsKey(productType))
        //        return knownLocalizeValues[productType];
        //    var value = productType.GetCustomAttribute<DefaultLocalizeValueAttribute>()?.DefaultLocalizeValue ?? string.Empty;
        //    knownLocalizeValues.Add(productType, value);
        //    return value;
        //}
        public static string GetDefaultLocalizeValue(Type productType) {
            var cacheKey = $"DefaultLocalizeValue_{productType.FullName}";
            if (_cache.TryGetValue(cacheKey, out string? value))
                return value ?? string.Empty;

            value = productType.GetCustomAttribute<DefaultLocalizeValueAttribute>()?.DefaultLocalizeValue ?? string.Empty;

            // сохраняем в кэш с политикой
            _cache.Set(
                cacheKey,
                value,
                new MemoryCacheEntryOptions {
                    //SlidingExpiration = TimeSpan.FromHours(1), // очистка при неиспользовании
                    Priority = CacheItemPriority.Normal
                });

            return value;
        }
#if NETFRAMEWORK
        public int? FindImageId(string? imgName, string? category = null) {
            if (string.IsNullOrEmpty(imgName))
                return null;
            var tracked = this.ChangeTracker
                             .Entries<DbImage>()
                             .Where(e => e.State != EntityState.Deleted)
                             .Select(e => e.Entity)
                             .Where(img => img.Text == imgName);
            var query = this.Set<DbImage>().AsNoTracking().Where(img => DbFunctions.Like(img.Text, imgName));
            if (!string.IsNullOrEmpty(category)) {
                tracked=tracked.Where(img=>img.Category == category);
                query = query.Where(img => DbFunctions.Like(img.Category, category));
            }
                var id = tracked.Select(img => (int?)img.Id).FirstOrDefault()??
                    query.Select(img => (int?)img.Id).FirstOrDefault();
                return id;
        } 
#else
        public int? FindImageId(string? imgName, string? category = null) {
            if (string.IsNullOrEmpty(imgName))
                return null;
            var tracked = this.ChangeTracker
                             .Entries<DbImage>()
                             .Where(e => e.State != EntityState.Deleted)
                             .Select(e => e.Entity)
                             .Where(img => img.Text == imgName);
            var query = this.Set<DbImage>().AsNoTracking().Where(img => EF.Functions.Like(img.Text, imgName));
            if (category is not null) {
                tracked = tracked.Where(img => img.Category == category);
                query = query.Where(img => EF.Functions.Like(img.Category, category));
            }
            var id = tracked.Select(img => (int?)img.Id).FirstOrDefault() ??
                query.Select(img => (int?)img.Id).FirstOrDefault();
            return id;
        }

#endif
        public DbImage CreateImage(string? imgName, string category, byte[] image) {
            //DbImage dbImg = this.DbImages.Create();
            var dbImg = new DbImage {
                Text = imgName ?? string.Empty,
                Category = category,
                Image = image
            };
            dbImg.Id = GetNextId<DbImage>();
            this.DbImages.Add(dbImg);
            return dbImg;
        }
        public int GetNextId<Tentity>() where Tentity : class, IHaveId {
            int id;
            var addedMaxId = this.ChangeTracker.Entries<Tentity>()
                       .Where(e => e.State == EntityState.Added)
                       .Select(e => (int?)e.Entity.Id)
                       .Max()
                       ?? 0;
            int dbMaxId = this.Set<Tentity>()
                   .Select(s => (int?)s.Id)   // делаем nullable, чтобы Max над пустой выборкой вернул null
                   .Max() ?? 0;
            id = Math.Max(addedMaxId, dbMaxId) + 1;
            return id;
        }
#if NETFRAMEWORK
        public Context(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) {
        }
        partial void InitializeModel(DbModelBuilder modelBuilder) {
#else
        partial void InitializeModel(ModelBuilder modelBuilder) {
#endif
            // var connectionString = this.Database.Connection.ConnectionString;
            //    var providerName = this.Database.Connection.GetType().Name;

#if NETFRAMEWORK
            if (this.Database.Connection is SqlCeConnection) {
                modelBuilder.Conventions.Add(new NanoCadPropertiesConvention());
            }
            modelBuilder.Entity<DbGcMountSystem>()
                .HasOptional(p => p.StandGutterUtilitySet)
                .WithMany()
                .HasForeignKey(ms => ms.Stand);
#if InitDbContextEnums
            modelBuilder.Entity<DbLtKiTable>()
               .HasKey(e => e.Id);
            modelBuilder.Entity<ElLighting>()
                .HasOptional(p => p.DbLtKiTable)
                .WithMany()
                .Map(m => m.MapKey("KiTable"));
            modelBuilder.Entity<ElWireMark>()
                .HasOptional(p => p.IsolationMaterial)
                .WithMany()
                .Map(m => m.MapKey("isolationMaterialId"));
            modelBuilder.Entity<ElWireMark>()
                .HasOptional(p => p.Material)
                .WithMany()
                .Map(m => m.MapKey("materialId"));
            modelBuilder.Entity<ElWire>()
                .HasOptional(p => p.CableSystemType)
                .WithMany()
                .Map(m => m.MapKey("CableSystemType"));
            modelBuilder.Entity<ElWire>()
                .HasOptional(p => p.wireMark)
                .WithMany()
                .Map(m => m.MapKey("wireMark"));
            modelBuilder.Entity<ScsSwitchSocketPanel>()
                .HasOptional(p => p.CableSystemType)
                .WithMany()
                .Map(m => m.MapKey("CableSystemType"));
            modelBuilder.Entity<ScsSwitchSocketPanel>()
                .HasOptional(p => p.PortType)
                .WithMany()
                .Map(m => m.MapKey("PortType"));
            modelBuilder.Entity<ScsUtpSocket>()
                .HasOptional(p => p.CableSystemType)
                .WithMany()
                .Map(m => m.MapKey("CableSystemType"));
            modelBuilder.Entity<ScsUtpSocket>()
                .HasOptional(p => p.PortType)
                .WithMany()
                .Map(m => m.MapKey("PortType"));
            modelBuilder.Entity<ScsPatchCord>()
                .HasOptional(p => p.CableSystemType)
                .WithMany()
                .Map(m => m.MapKey("CableSystemType"));
            modelBuilder.Entity<ElLighting>()
                .HasOptional(l => l.Lamp)
                .WithMany()
                .Map(m => m.MapKey("Lamp"));
#endif
#else
            modelBuilder.Entity<DbGcMountSystem>()
                .HasOne(p => p.StandGutterUtilitySet)
                .WithMany()
                .HasForeignKey(ms => ms.Stand)
                .IsRequired(false);
#if InitDbContextEnums
            modelBuilder.Entity<DbLtKiTable>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<ElLighting>()
                .HasOne(p => p.DbLtKiTable)
                .WithMany()
                .HasForeignKey("KiTable")
                .IsRequired(false);
            modelBuilder.Entity<ElWireMark>()
                .HasOne(p => p.IsolationMaterial)
                .WithMany()
                .HasForeignKey("isolationMaterialId")
                .IsRequired(false);

            modelBuilder.Entity<ElWireMark>()
                .HasOne(p => p.Material)
                .WithMany()
                .HasForeignKey("materialId")
                .IsRequired(false);

            modelBuilder.Entity<ElWire>()
                .Property<int?>("CableSystemTypeId")
                .HasColumnName("CableSystemType");
            modelBuilder.Entity<ElWire>()
                .Property<int?>("CableSystemTypeId")
                .HasColumnName("CableSystemType");

            modelBuilder.Entity<ElWire>()
                .HasOne(p => p.wireMark)
                .WithMany()
                .HasForeignKey("wireMarkId")
                .IsRequired(false);
            modelBuilder.Entity<ElWire>()
                .Property<int?>("wireMarkId")
                .HasColumnName("wireMark");

            //modelBuilder.Entity<ScsSwitchSocketPanel>()
            //    .Property<int?>("CableSystemTypeId")
            //    .HasColumnName("CableSystemType");
            //modelBuilder.Entity<ScsSwitchSocketPanel>()
            //    .Property<int?>("PortTypeId")
            //    .HasColumnName("PortType");
            //modelBuilder.Entity<ScsSwitchUtpPanel>()
            //    .Property<int?>("CableSystemTypeId")
            //    .HasColumnName("CableSystemType");
            //modelBuilder.Entity<ScsSwitchUtpPanel>()
            //    .Property<int?>("PortTypeId")
            //    .HasColumnName("PortType");
            modelBuilder.Entity<ScsUtpSocket>()
                .Property<int?>("CableSystemTypeId")
                .HasColumnName("CableSystemType");
            modelBuilder.Entity<ScsUtpSocket>()
                .Property<int?>("PortTypeId")
                .HasColumnName("PortType");
            modelBuilder.Entity<ElWire>()
                .Property<int?>("CableSystemTypeId")
                .HasColumnName("CableSystemType");
            modelBuilder.Entity<ScsPatchCord>()
                .Property<int?>("CableSystemTypeId")
                .HasColumnName("CableSystemType");

            modelBuilder.Entity<ElLighting>()
                .HasOne(l => l.Lamp)
                .WithMany()
                .HasForeignKey("LampId")
                .IsRequired(false);
            modelBuilder.Entity<ElLighting>()
                .Property<int?>("LampId")
                .HasColumnName("Lamp");
            modelBuilder.Entity<ScsCommutatorPanel>()
                .Property<int?>("PortTypeInId")
                .HasColumnName("PortTypeIn");
            modelBuilder.Entity<ScsCommutatorPanel>()
                .Property<int?>("PortTypeOutId")
                .HasColumnName("PortTypeOut");

#endif
#endif
            modelBuilder.Entity<CaeMaterialUtility>()
                .Property(t => t.MeashureUnits)
                .HasColumnName("MeashureUnits");
            modelBuilder.Ignore<DbGcKnotStand>();
            modelBuilder.Ignore<DbGcKnotPlain>();
            modelBuilder.Ignore<DbGcKnotLevel>();
            modelBuilder.Ignore<DbGcSystemPlain>();

        }

        public bool IsHaveColumns(string tableName, params string[] columns) {
#if NET6_0_OR_GREATER
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

