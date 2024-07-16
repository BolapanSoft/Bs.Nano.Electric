//using DocumentFormat.OpenXml.Bibliography;

// Ignore Spelling: Tdest

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;

namespace Nano.Electric {
    public partial class Context {
        private static Dictionary<string, string[]> propertiesCache = new Dictionary<string, string[]>();
        private static readonly Dictionary<Type, string> knownLocalizeValues = new Dictionary<Type, string>();
        public Context(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) {

        }

        partial void InitializeModel(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Add(new NanoCadPropertiesConvention());
            
            modelBuilder.Entity<CaeMaterialUtility>()
                .Property(t => t.MeashureUnits)
                .HasColumnName("MeashureUnits");
#if ExportToEplan
#else
            modelBuilder.Entity<DbGcMountSystem>()
                .HasOptional(p => p.StandGutterUtilitySet)
                .WithMany()
                .HasForeignKey(ms => ms.Stand);
            modelBuilder.Ignore<DbGcKnotStand>();
            modelBuilder.Ignore<DbGcKnotPlain>();
            modelBuilder.Ignore<DbGcKnotLevel>();
            modelBuilder.Ignore<DbGcSystemPlain>();
#endif
#if InitDbContextEnums
            modelBuilder.Entity<DbLtKiTable>()
               .HasKey(e => e.Id);
            modelBuilder.Entity<ElLighting>()
                .HasOptional(p => p.DbLtKiTable)
                .WithMany()
                .Map(m=>m.MapKey("KiTable"));
            modelBuilder.Entity<ElWireMark>()
                .HasOptional(p => p.IsolationMaterial)
                .WithMany()
                .Map(m=>m.MapKey("isolationMaterialId"));
            modelBuilder.Entity<ElWireMark>()
                .HasOptional(p => p.Material)
                .WithMany()
                .Map(m=>m.MapKey("materialId"));

#endif
            //modelBuilder.Entity<DbLtKiTable>()
            //    .Property(p => p.CurveDb)
            //    .HasColumnType("ntext");
            //modelBuilder.Entity<DbScsGutterUtilitySet>()
            //    .Property(p=>p.LevelType)
            //    .HasColumnName("LevelType")
            //    .HasColumnType("int")
            //    .IsOptional();
            //modelBuilder.Entity<DbScsGutterUtilitySet>()
            //    .Property(p => p.StandType)
            //    .HasColumnName("StandType")
            //    .HasColumnType("int")
            //    .IsOptional();
            //modelBuilder.Entity<DbScsGutterUtilitySet>()
            //    .Property(p => p.StandType)
            //    .HasColumnName("StructureType")
            //    .HasColumnType("int")
            //    .IsOptional();

        }
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
                catch(Exception ex) {
                    results.Add((propName, new InvalidOperationException($"Операция не выполнена. Значение \"{sourceValue}\" столбца {propName} не удается привести к типу {propertyInfo.PropertyType}.",ex)));
                }
            }
            return results;
        }
        public bool TrySetProperty<Tdest,Tprop>(Tdest product, string propName, Tprop sourceValue) where Tdest : class {
            var propertyInfo = product.GetType().GetProperty(propName);
            if (propertyInfo == null || !propertyInfo.CanWrite) {
                return false;
            }
            if (propertyInfo.GetCustomAttribute<KeyAttribute>() != null) {
                throw new InvalidOperationException($"Операция не разрешена. Свойство {propName} является частью ключа.");
            }
            try {  
                propertyInfo.SetValue(product, sourceValue, BindingFlags.Public, FieldBinder.Instance, null, CultureInfo.GetCultureInfo("Ru-ru"));
                return true;
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
        public string[] GetProductProperties<Tdest>() where Tdest : class {
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
        public static string GetDefaultLocalizeValue<T>() where T:class {
            Type productType = typeof(T);
            if (knownLocalizeValues.ContainsKey(productType))
                return knownLocalizeValues[productType];
            var value = productType.GetCustomAttribute<DefaultLocalizeValueAttribute>()?.DefaultLocalizeValue ?? string.Empty;
            knownLocalizeValues.Add(productType, value);
            return value;
        }
    }
}
