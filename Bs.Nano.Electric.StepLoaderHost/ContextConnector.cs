using System;
using System.Data.Common;
using System.IO;
using Bs.Nano.Electric.Model;
using NanoCadContext = Nano.Electric.Context;
using System.Runtime.Serialization;
using Microsoft.Data.Sqlite;

namespace Bs.Nano.Electric.StepLoaderHost {
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
            if (string.IsNullOrWhiteSpace(fullFileName) ||  !fullFileName.ToLower().EndsWith(".db"))
                throw new ArgumentOutOfRangeException(nameof(fullFileName), $"Переданная строка \"{fullFileName}\" должна ссылаться на файл .db");
            //string fullFileName = Path.Combine(configuration.CurrentDirectory, fullFileName);
            if (File.Exists(fullFileName)) {
                this.dbFileName = fullFileName;
                connectionString = $"Data Source=\"{fullFileName}\";";
                provider = DbProvider.SQLite;
            }
            else
            {
                throw new FileNotFoundException($"Файл не найден: \"{fullFileName}\".");
            }
        }
        protected ContextConnector(SerializationInfo info, StreamingContext context) {
            dbFileName = info.GetString(nameof(dbFileName)) ?? string.Empty;
            if (dbFileName.ToLower().EndsWith(".db")) {
                connectionString = $"Data Source=\"{dbFileName}\";";
                provider = DbProvider.SQLite;
            }
            else 
                connectionString= string.Empty;
        }
        public string ConnectionString => connectionString;
        public string DbFileName=>dbFileName;
        public NanoCadContext Connect() {
            var context = new NanoCadContext(GetConnection(), contextOwnsConnection: true);
            return context;
        }
        public DbConnection GetConnection() {
            switch (provider) {
                case DbProvider.SQLite:
                    return GetSQLiteConnection();
                default:
                    throw new InvalidOperationException($"Для файла {Path.GetFileName(connectionString)} провайдер не определен.");
            }
        }
        private DbConnection GetSQLiteConnection() {
            DbConnection connection = new SqliteConnection(connectionString);
            return connection;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(dbFileName), dbFileName);
        }
    }
}
