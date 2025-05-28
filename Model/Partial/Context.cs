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

