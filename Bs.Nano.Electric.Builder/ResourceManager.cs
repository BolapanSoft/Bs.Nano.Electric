// Ignore Spelling: Scs Gc Nano

using Bs.XML.SpreadSheet;
using Microsoft.Extensions.Configuration;
using Nano.Electric;
using System.Diagnostics.CodeAnalysis;
using Path = System.IO.Path;
using Microsoft.Extensions.Caching.Memory;

namespace Bs.Nano.Electric.Builder {
    public abstract class ResourceManager(IMemoryCache cache, IElectricBuilderConfiguration configuration) {
        public class SheetCommon<Tsc> : SheetCommon /*where Tsc : IProduct*/ {
            private DateTime _lastWriteTimeUtc = default; // время последней успешной загрузки
#if NETFRAMEWORK
            public SheetRef Source { get; set; }
            public string TableName { get; set; }
#else
            public SheetRef Source { get; init; }
            public string TableName { get; init; }
#endif
            public bool IsLoaded => _lastWriteTimeUtc > DateTime.MinValue;
            public void Load() {
                var shRef = Source;
                FileInfo fi = new FileInfo(shRef.FileName);
                if (!fi.Exists) {
                    throw new FileNotFoundException(shRef.FileName);
                }
                var currentWriteTime = fi.LastWriteTimeUtc;
                // если уже загружали и файл не изменился → выходим
                if (_lastWriteTimeUtc == currentWriteTime) {
                    return;
                }
                var tempFileName = Path.GetTempFileName();
                try {
                    File.Copy(fi.FullName, tempFileName, overwrite: true);
                    base.LoadResource(tempFileName, shRef.SheetName);
                }
                finally {
                    File.Delete(tempFileName);
                }
                _lastWriteTimeUtc = currentWriteTime;
                if (currentWriteTime == default) {
                    currentWriteTime = DateTime.UtcNow;
                }
            }
        }
        public record struct SheetRef(string FileName, string SheetName);


        //private static Dictionary<Configuration, Dictionary<SheetRef, SheetCommon>> globalSheetCache = new();
        protected readonly IMemoryCache _cache = cache;
        protected readonly IElectricBuilderConfiguration configuration = configuration;

        private string GetDbInf_Description() {
            throw new NotImplementedException();
            //// Read Resources\DbInf.Description.txt to string
            //var dbinfSection = configuration.GetSection("DbInf")
            //string? dbInfValue = configuration.GetSection("DbInf.Description").Value;
            //try {
            //    if (string.Compare(Path.GetExtension(dbInfValue), ".txt", ignoreCase: true) == 0) {
            //        string description = System.IO.File.ReadAllText(Path.Combine(configuration.ResourcesPath, dbInfValue));
            //        return description;
            //    }
            //    else if (dbInfValue is null && System.IO.File.Exists("DbInf.Description.txt")) {
            //        string description = System.IO.File.ReadAllText(Path.Combine(configuration.ResourcesPath, "DbInf.Description.txt"));
            //        return description;
            //    }
            //    else {
            //        return dbInfValue ?? string.Empty;
            //    }
            //}
            //catch (Exception ex) {
            //    throw new InvalidOperationException("При чтении значения DbInf.Description возникла ошибка.", ex);
            //}
        }

        /// <summary>
        /// Загружает в SheetCommon источник из секции Configuration.Tables с ключом, совпадающим с именем класса Tnc.
        /// </summary>
        /// <typeparam name="Tnc">Тип, имя которого является ключом вхождения элемента в секции Tables.</typeparam>
        /// <param name="resourcePath">Директория для поиска файла источника. Если параметр не передан, используется директория, указанная в секции Resources:ExcelSource.</param>
        /// <returns>Загруженная таблица или null</returns>
        public bool TryGetSheet<Tnc>([NotNullWhen(true)] out SheetCommon<Tnc> sheetCommon, string? resourcePath = null)
            where Tnc : class {
            string[] classNameParts = (typeof(Tnc).Name).Split('.');
            string className = classNameParts[classNameParts.Length - 1];
            IConfiguration fileSection = configuration.GetSection($"Tables:{className}");
            if ((fileSection is null)) {
                sheetCommon = null;
                return false;
            }
            resourcePath ??= configuration.GetSection("Resources:ExcelSource").Value ??
                "Resources\\XLSX";


            var sourcePath = Path.Combine(configuration.CurrentDirectory, resourcePath);
            if (!Directory.Exists(sourcePath)) {
                throw new DirectoryNotFoundException($"Директория \"{Path.GetFullPath(sourcePath)}\" не найдена или недостаточно прав доступа к ней.");
            }
            string fileName = fileSection["FileName"] ??
                throw new InvalidOperationException($"В конфигурации не определено значение \"Tables:{className}:FileName\".");
            var fullFileName = Path.GetFullPath(Path.Combine(sourcePath, fileName));
            string sheetName = fileSection["SheetName"] ?? "Лист1";
            SheetRef sheetRef = new(fullFileName, sheetName);
            sheetCommon = LoadShc<Tnc>(sheetRef);

            return true;

        }
        //internal async Task<SheetCommon<Tnc>?> GetSheetAsync<Tnc>(string? resourcePath = null) {
        //    string[] classNameParts = (typeof(Tnc).Name).Split('.');
        //    string className = classNameParts[classNameParts.Length - 1];
        //    if (configuration.Tables.ContainsKey(className)) {
        //        (string fileName, string sheetName) = configuration.Tables[className];
        //        SheetRef sheetRef = new(Path.Combine(resourcePath ?? configuration.XLSXPath, fileName), sheetName);
        //        SheetCommon<Tnc>? shc;
        //        if (sheetCache.ContainsKey(sheetRef)) {
        //            shc = sheetCache[sheetRef] as SheetCommon<Tnc>;
        //            if (shc is not null)
        //                return shc;
        //        }
        //        shc = new SheetCommon<Tnc>();
        //        try {
        //            await shc.LoadAsync(sheetRef.FileName, sheetRef.SheetName);
        //        }
        //        catch (Exception ex) {
        //            throw new InvalidOperationException($"Загрузка таблицы {sheetRef} завершилась ошибкой", ex);
        //        }
        //        shc.Source = sheetRef;
        //        sheetCache[sheetRef] = shc;
        //        return shc;
        //    }
        //    else
        //        return null;
        //}
        internal static string ArticleCategory(string nanoElectricTable) {
            switch (nanoElectricTable) {
                case nameof(ScsGutterCanal):
                case nameof(DbScsGutterCover):
                case nameof(DbScsGutterPartition):
                case nameof(ScsGcFitting):
                case nameof(DbScsGcCoverUnit):
                case nameof(ScsGutterBolting):
                    return "Конструкции прототипов";
                case nameof(DbScsGcAccessoryUnit):
                case nameof(DbScsGcBoltingAccessoryUnit):
                    return "Оборудование";
                case nameof(CaeMaterialUtility):
                    return "Крепежные изделия";
                default:
                    throw new ArgumentOutOfRangeException($"The function \"{nameof(ArticleCategory)}\" not defined for table name \"{nanoElectricTable}\"");
            }
        }
        //internal static Dictionary<string, (string Name, string NanoElectricTable)> KnownProducts => lzKnownProducts.Value;
        #region ResourceLoaders
        public SheetCommon<Tnc> LoadShc<Tnc>(SheetRef shRef) {
            SheetCommon<Tnc> shc;
            var cacheKey = $"{shRef.FileName}:{shRef.SheetName}";
            try {
                shc = _cache.GetOrCreate(cacheKey, entry => {
                    shc = new SheetCommon<Tnc>() { Source = shRef, TableName = typeof(Tnc) == typeof(Object) ? string.Empty : typeof(Tnc).Name };
                    return shc;
                })!;
                shc.Load();

            }
            catch (Exception ex) {
                throw new InvalidOperationException($"Загрузка таблицы {shRef} завершилась ошибкой", ex);
            }
            return shc;
        }

        //internal async Task<SheetCommon> LoadShcAsync(SheetRef shRef) {
        //    if (sheetCache.ContainsKey(shRef)) {
        //        return sheetCache[shRef];
        //    }
        //    var shc = new SheetCommon();
        //    try {
        //        await shc.LoadAsync(shRef.FileName, shRef.SheetName);
        //    }
        //    catch (Exception ex) {
        //        throw new InvalidOperationException($"Загрузка таблицы {shRef} завершилась ошибкой", ex);
        //    }
        //    sheetCache[shRef] = shc;
        //    return shc;
        //}

        #endregion
    }
}
