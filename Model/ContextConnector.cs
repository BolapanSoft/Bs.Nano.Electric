using System;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization;
#if NETFRAMEWORK
using System.Data.SQLite;
using System.Data.SqlServerCe;
#elif NET8_0_OR_GREATER
using Microsoft.Data.Sqlite;
#endif
using NanoCadContext = Nano.Electric.Context;

namespace Bs.Nano.Electric.Model {
    [Serializable]
    public class ContextConnector : INanocadDBConnector, ISerializable {
        private enum DbProvider {
            SqlServerCompact,
            SQLite
        }

        public readonly string dbFileName;
        private readonly string connectionString;
        private readonly DbProvider provider;

        public ContextConnector(string fullFileName) {
            if (string.IsNullOrWhiteSpace(fullFileName)) {
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(fullFileName));
            }

            string lowerFileName = fullFileName.ToLower();
            if (!lowerFileName.EndsWith(".sdf") && !lowerFileName.EndsWith(".db")) {
                throw new ArgumentException($"Файл \"{fullFileName}\" должен иметь расширение .sdf или .db", nameof(fullFileName));
            }

            if (!File.Exists(fullFileName)) {
                throw new FileNotFoundException($"Файл не найден: \"{fullFileName}\".");
            }

            dbFileName = fullFileName;
            (connectionString, provider) = CreateConnectionString(fullFileName);
        }

        protected ContextConnector(SerializationInfo info, StreamingContext context) {
            dbFileName = info.GetString(nameof(dbFileName)) ?? throw new SerializationException("Не удалось десериализовать путь к файлу.");
            (connectionString, provider) = CreateConnectionString(dbFileName);
        }

        public NanoCadContext Connect() => new NanoCadContext(GetConnection(), contextOwnsConnection: true);
        public string ConnectionString => connectionString;
        public DbConnection GetConnection() => provider switch {
#if NETFRAMEWORK
            DbProvider.SqlServerCompact => new SqlCeConnection(connectionString),
#endif
            DbProvider.SQLite => CreateSQLiteConnection(),
            _ => throw new InvalidOperationException($"Для файла {Path.GetFileName(dbFileName)} провайдер не определен.")
        };

        private static (string ConnectionString, DbProvider Provider) CreateConnectionString(string fileName) {
#if NETFRAMEWORK
            return fileName.ToLower() switch {
                var name when name.EndsWith(".sdf", StringComparison.OrdinalIgnoreCase) => ($"Data Source=\"{fileName}\";Max Database Size=2560", DbProvider.SqlServerCompact),
                var name when name.EndsWith(".db", StringComparison.OrdinalIgnoreCase) => ($"Data Source=\"{fileName}\";Version=3;", DbProvider.SQLite),
                _ => throw new ArgumentOutOfRangeException(nameof(fileName), $"Файл \"{fileName}\" должен иметь расширение .sdf или .db")
            }; 
#else
            return fileName.ToLower() switch {
                //var name when name.EndsWith(".sdf") => ($"Data Source=\"{fileName}\";Max Database Size=2560", DbProvider.SqlServerCompact),
                var name when name.EndsWith(".db", StringComparison.OrdinalIgnoreCase) => ($"Data Source=\"{fileName}\"", DbProvider.SQLite),
                _ => throw new ArgumentOutOfRangeException(nameof(fileName), $"Файл \"{fileName}\" должен иметь расширение .db")
            }; 
#endif
        }
        public static string? GetDbName(DbConnection connection) {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            var builder = new DbConnectionStringBuilder {
                ConnectionString = connection.ConnectionString
            };

            // Для SqlClient → Initial Catalog, для SQLite → Data Source
            if (builder.TryGetValue("Initial Catalog", out var sqlName))
                return sqlName?.ToString();
            if (builder.TryGetValue("Data Source", out var fileName))
                try {
                    return Path.GetFileName(fileName.ToString());
                }
                catch {
                    return fileName.ToString();
                }

            if (builder.TryGetValue("Database", out var dbName))
                return dbName?.ToString();


            return connection.Database; // fallback
        }
        private DbConnection CreateSQLiteConnection() =>
#if NETFRAMEWORK
            new SQLiteConnection(connectionString);
#elif NET8_0_OR_GREATER
            new SqliteConnection(connectionString);
#endif

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(dbFileName), dbFileName);
        }
    }
}