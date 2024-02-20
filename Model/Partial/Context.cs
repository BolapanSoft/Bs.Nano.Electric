//using DocumentFormat.OpenXml.Bibliography;
using System.Data.Common;
using System.Data.Entity;

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
    }
}
