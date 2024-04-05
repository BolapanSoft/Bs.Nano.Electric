//using DocumentFormat.OpenXml.Bibliography;

// Ignore Spelling: Tdest

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace Nano.Electric {
    public partial class Context {
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
        public IEnumerable<(string, Exception)> FillProperties<Tdest>(Tdest product, IEnumerable<string> propNames, IReadOnlyDictionary<string, string> item) where Tdest : class, IProduct {
            throw new NotImplementedException();
        }
        public string[] GetProductProperties<Tdest>() {
            throw new NotImplementedException();
        }
    }
}
