//using DocumentFormat.OpenXml.Bibliography;

// Ignore Spelling: Tdest

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;

namespace Nano.Electric {
    public partial class Context {
        private static Dictionary<string, string[]> propertiesCache = new Dictionary<string, string[]>();
        public Context(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) {

        }

        partial void InitializeModel(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
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
        public IList<(string, Exception)> FillProperties<Tdest>(Tdest product, IEnumerable<string> propNames, IReadOnlyDictionary<string, string> item) where Tdest : class, IProduct {
            var results = new List<(string, Exception)>();
            foreach (var propName in propNames) {
                var propertyInfo = typeof(Tdest).GetProperty(propName);
                if (propertyInfo == null || !propertyInfo.CanWrite ) {
                    continue;
                }
                if (propertyInfo.GetCustomAttribute<KeyAttribute>() != null) {
                    results.Add((propName, new InvalidOperationException($"Операция не разрешена. Свойство {propName} является частью ключа.")));

                    continue;
                }

                try {
                    object value = null;
                    bool success = false;
                    string sourceValue = item[propName];
                    if (propertyInfo.PropertyType == typeof(int)) {
                        success = int.TryParse(sourceValue, out int result);
                        value = result;
                    }
                    else if (propertyInfo.PropertyType == typeof(int?)) {
                        if (int.TryParse(sourceValue, out int result)) {
                            value = result;
                            success = true;
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof(double)) {
                        success = double.TryParse(sourceValue, out double result);
                        value = result;
                    }
                    else if (propertyInfo.PropertyType == typeof(double?)) {
                        if (double.TryParse(sourceValue, out double result)) {
                            value = result;
                            success = true;
                        }
                    }
                    //else if (propertyInfo.PropertyType.IsEnum) {
                    //    value = Enum.Parse(propertyInfo.PropertyType, item[propName]);
                    //}
                    else {
                        value = Convert.ChangeType(sourceValue, propertyInfo.PropertyType);
                        success = true;
                    }
                    if (success) {
                        propertyInfo.SetValue(product, value); 
                    }
                    else {
                        results.Add((propName, new InvalidOperationException($"Операция не выполнена. Значение \"{sourceValue}\" столбца {propName} не удается привести к типу {propertyInfo.PropertyType}.")));
                    }
                }
                catch (Exception ex) {
                    results.Add((propName, new InvalidOperationException($"Не удалось заполнить свойство {propName}. Тип свойства назначения \"{propertyInfo.PropertyType}\"", ex)));
                }
            }
            return results;
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
                throw new InvalidOperationException($"Тип {typeName} не имеет аттрибута TableAttribute.");
            }
            // out public propertis, exept properties width attribute [NotMapped]
            properties = type.GetProperties()
                .Where(p => p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() == null && p.CanWrite)
                .Select(p => p.Name)
                .ToArray();
            propertiesCache[typeName] = properties;
            return properties;
        }
    }
}
