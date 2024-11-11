using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlServerCe;
using System.IO;
using Bs.Nano.Electric.Model;
using NanoCadContext = Nano.Electric.Context;
using System.Runtime.Serialization;

namespace Bs.Nano.Electric.Model {
    [Serializable]
    public class ContextConnector : INanocadDBConnector, ISerializable {
        private enum DbProvider {
            SqlServerCompact,
            SQLite
        }
        [NonSerialized]
        private readonly string connectionString;
        [NonSerialized]
        private readonly DbProvider provider;
        public readonly string dbFileName;
        public ContextConnector(string fullFileName) {
            if (string.IsNullOrWhiteSpace(fullFileName) || 
                !(fullFileName.ToLower().EndsWith(".sdf") || fullFileName.ToLower().EndsWith(".db")))
                throw new ArgumentException(nameof(fullFileName), $"Переданная строка \"{fullFileName}\" должна ссылаться на файл .sdf или .db");
            if (File.Exists(fullFileName)) {
                this.dbFileName = fullFileName;
                if (dbFileName.ToLower().EndsWith(".sdf")) {
                    connectionString = $"Data Source=\"{fullFileName}\";Max Database Size=2560"; 
                    provider= DbProvider.SqlServerCompact;
                }
                else if (dbFileName.ToLower().EndsWith(".db")) {
                    connectionString = $"Data Source=\"{fullFileName}\";Version=3;";
                    provider= DbProvider.SQLite;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(dbFileName), $"Переданная строка \"{dbFileName}\" должна ссылаться на файл .sdf или .db");
                }
            }
            else
            {
                throw new FileNotFoundException($"Файл не найден: \"{fullFileName}\".");
            }
        }
        protected ContextConnector(SerializationInfo info, StreamingContext context) {
            dbFileName = info.GetString(nameof(dbFileName));
            if (dbFileName.ToLower().EndsWith(".sdf")) {
                connectionString = $"Data Source=\"{dbFileName}\";Max Database Size=2560";
                provider = DbProvider.SqlServerCompact;
            }
            else if (dbFileName.ToLower().EndsWith(".db")) {
                connectionString = $"Data Source=\"{dbFileName}\";Version=3;";
                provider = DbProvider.SQLite;
            }
            else 
                connectionString= string.Empty;
        }
        public NanoCadContext Connect() {
            var context = new NanoCadContext(GetConnection(), contextOwnsConnection: true);
            return context;
        }
        public DbConnection GetConnection() {
            switch (provider) {
                case DbProvider.SqlServerCompact:
                    return GetSqlCEConnection();
                case DbProvider.SQLite:
                    return GetSQLiteConnection();
                default:
                    throw new InvalidOperationException($"Для файла {Path.GetFileName(connectionString)} провайдер не определен.");
            }
        }
        private DbConnection GetSqlCEConnection() {
            DbConnection connection = new SqlCeConnection(connectionString);
            return connection;
        }
        private DbConnection GetSQLiteConnection() {
            DbConnection connection = new SQLiteConnection(connectionString);
            return connection;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(dbFileName), dbFileName);
        }
    }
}
