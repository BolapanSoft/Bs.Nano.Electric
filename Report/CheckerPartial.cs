using Bs.Nano.Electric.Model;
using Microsoft.Extensions.Logging;
using Nano.Electric;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Bs.Nano.Electric.Report {
    public partial class Checker {
        private struct Product : IProduct {
            public string Code { get; set; }

            public int Id { get; set; }
            public int? DbImageRef { get; set; }
            public string Name { get; set; }
            public string Manufacturer { get; set; }
        }
        internal class Material {
            public string Code { get; set; }
            public string Mass { get; set; }
        }
        private static Lazy<(MethodInfo mi, RuleCategoryAttribute[] ruleCategories)[]> lzAllRules = new(() => {
            var rules = typeof(Checker).GetMethods(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public)
            .Select(mi => (mi, mi.GetCustomAttributes<RuleCategoryAttribute>().ToArray()))
            .OrderBy(m => GetPriority(m.mi))
            .ToArray();
            return rules;
        });
        public static IEnumerable<MethodInfo> GetTests(string testCategory, string[] tables) {
            return lzAllRules.Value
            .Where(m => m.ruleCategories
                .Any(a => a.TestCategories.Contains(testCategory) && a.TestCategories.Any(tc => tables.Contains(tc))))
            .Select(m => m.mi)
            ;
        }
        public static string[] SplitToWords(string value) {
            // Split the string into words based on letters and digits from any language
            var matches = Regex.Matches(value, @"(?>[\p{L}\p{N}]+([,.]\p{N}+)*)");
            List<string> words = new List<string>(64);
            foreach (Match m in matches) {
                words.Add(m.Value);
            }
            return words.ToArray();
        }
        public static bool IsEnglishWord(string word) {
            // Match the word against the pattern that allows only English alphabets and digits
            bool isMatch = Regex.IsMatch(word, @"^[A-Za-z0-9]+$");
            return isMatch;
        }
        public static bool IsRussianWord(string word) {
            // Match the word against the pattern that allows only Russian alphabets and digits
            return Regex.IsMatch(word, @"^[ А-Яа-я0-9]+$");
        }
        public static bool IsRepresentNumber(string word) {
            // Match the word that represent a number 
            return Regex.IsMatch(word, @"\p{N}+([,.]\p{N}+)*");
        }
        public static bool IsDoubleValuesList(string input) {
            return input.Split('/').All(v => TryParseAsDouble(v, out double value).IsSuccess && value > 0);
        }
        protected static void ReportErrors(ILogger logger, AggregateException ex, bool isTruncateReport) {
            //ReportErrors(logger, (Exception)ex);
            foreach (Exception item in ex.InnerExceptions) {
                ReportErrors(logger, item, isTruncateReport);
            }
        }
        protected static void ReportErrors(ILogger logger, Exception ex, bool isTruncateReport) {
            if (ex is null) { return; }
            if (ex is AggregateException aggrEx) {
                ReportErrors(logger, aggrEx, isTruncateReport);
                return;
            }

            logger.LogWarning(ex.Message);
            if (ex is RuleTestException rtEx) {
                if (ex.Data.Count > 0) {
                    logger.LogInformation("Содержание ex.Data:");
                    var data = ex.Data;
                    int i = 0;
                    foreach (var key in data.Keys) {
                        logger.LogInformation($"{key} = {data[key]}");
                        if (i++ > 50 & isTruncateReport) { break; }
                    }
                    if (i > 50 & isTruncateReport) {
                        logger.LogInformation("... усечено...");
                    }
                }
            }
            else {
#if DEBUG
                logger.LogInformation("Call stack:");
                logger.LogInformation(ex.StackTrace);
#endif
            }
            ReportErrors(logger, ex.InnerException, isTruncateReport);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetPriority(MethodInfo mi) {
            return mi.GetCustomAttribute<PriorityAttribute>()?.Priority ?? 0;
        }
        /// <summary>
        /// Проверяет что переданное значение входит в множество возможных значений перечисления.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool IsDefined<TEnum>(TEnum value) where TEnum : Enum {
            return EnumConverter<TEnum>.IsDefineValue(value);
        }
        /// <summary>
        /// Для элемента перечислений возвращает значение атрибута Description, если оно определено.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetDescription<TEnum>(TEnum value) where TEnum : System.Enum {
            if (!EnumConverter<TEnum>.IsDefineValue(value)) {
                return $"Некорректное значение:{System.Convert.ToUInt64(value)}";
            }
            return EnumConverter<TEnum>.GetDescription(value);
        }
        /// <summary>
        /// Возвращает запись индекса для имени переменной.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        /// <remarks>Имя переменной должно соответствовать шаблону "\w+(_\n+)+".</remarks>
        private static string GetIndex(string? ruleName) {
            if (string.IsNullOrEmpty(ruleName))
                return string.Empty;
            return ruleName!.Replace('_', '.');
        }
        private List<(string Code, string ArticleName, string TableName, string TypeDescription)> LoadCodes(Context context, IQueryable dbSet, string tableName, Type entityType) {
            string typeDescription = Context.GetDefaultLocalizeValue(entityType);
            if (typeof(IProduct).IsAssignableFrom(entityType)) {
                try {
                    List<(string Code, string ArticleName, string TableName, string TypeDescription)> products = new(1024);
                    /*using (var context = connector.Connect())*/
                    {
                        //var query = dbSet.Cast<object>().ToList();// Convert dbSet to query
                        // Load all entity from dbSet query
                        foreach (var entity in dbSet.AsNoTracking()) {
                            IProduct product = (IProduct)entity;
                            products.Add((product.Code, product.Name, tableName, typeDescription));
                        }
                    }
                    return products;
                }
                catch (Exception ex) {
                    logger.LogWarning(ex.ToString());
                    return new();
                }
            }
            else {
                return new();
            }
        }
        private List<(string Code, string ArticleName, string TableName, string TypeDescription)> LoadAllCodes() {
            List<(string Code, string ArticleName, string TableName, string TypeDescription)> allCodes = new();
            using (var context = connector.Connect()) {
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(ScsGutterCanal));
                    var products = context.ScsGutterCanals
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.ScsGutterCanals), description));
                    }
                }
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(DbScsGutterCover));
                    var products = context.DbScsGutterCovers
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGutterCovers), description));
                    }
                }
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(DbScsGutterPartition));
                    var products = context.DbScsGutterPartitions
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGutterPartitions), description));
                    }
                }
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(ScsGcFitting));
                    var products = context.ScsGcFittings
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.ScsGcFittings), description));
                    }
                }
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(DbScsGcCoverUnit));
                    var products = context.DbScsGcCoverUnits
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGcCoverUnits), description));
                    }
                }
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(DbScsGcAccessoryUnit));
                    var products = context.DbScsGcAccessoryUnits
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGcAccessoryUnits), description));
                    }
                }
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(ScsGutterBolting));
                    var products = context.ScsGutterBoltings
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.ScsGutterBoltings), description));
                    }
                }
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(DbScsGcBoltingAccessoryUnit));
                    var products = context.DbScsGcBoltingAccessoryUnits
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGcBoltingAccessoryUnits), description));
                    }
                }
                {
                    var description = Context.GetDefaultLocalizeValue(typeof(CaeMaterialUtility));
                    var products = context.CaeMaterialUtilities
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.CaeMaterialUtilities), description));
                    }
                }

            }

            return allCodes;
        }
        private static void FailRuleTest<T>(string message, IEnumerable<T> data, [CallerMemberName] string? methodName = null) {
            string listIndex = typeof(Checker)
                .GetMethod(methodName)
                .GetCustomAttribute<ReportRuleAttribute>()?
                .ListIndex ??
                string.Empty;
            int i = 0;
            var ex = new RuleTestException(message);
            if (data is not null) {
                foreach (var item in data) {
                    ex.Data.Add($"{listIndex}.{i++}", item);
                }
            }
            throw ex;
        }
        private static void FailRuleTest(string message, KeyValuePair<object, object> item = default) {
            var ex = new RuleTestException(message);
            if (item.Key is not null) {
                ex.Data.Add(item.Key, item.Value);
            }
            throw ex;
        }
        private static List<string> GetTablesWithCodeFieldAndRecords(DbContext context) {
            var tableNames = new List<string>();
            var connection = (SqlCeConnection)context.Database.Connection;

            // Открываем соединение
            connection.Open();

            var schema = connection.GetSchema("Tables");
            foreach (System.Data.DataRow row in schema.Rows) {
                string tableName = row["TABLE_NAME"].ToString();
                string query = $"SELECT COUNT(*) FROM [{tableName}] WHERE [Code] IS NOT NULL";

                using (var command = new SqlCeCommand(query, connection)) {
                    try {
                        int count = (int)command.ExecuteScalar();
                        if (count > 0) {
                            tableNames.Add(tableName);
                        }
                    }
                    catch (SqlCeException) {
                        // Пропускаем таблицы, у которых нет поля "Code"
                    }
                }
            }

            connection.Close();
            return tableNames;
        }
        private static (bool isHaveField, IEnumerable<T> values) GetValues<T>(INanocadDBConnector connector, string tableName, string columnName) where T : struct {
            List<T> values = new();
            bool allRight = false;
            if (!(IsCorrectTableName(tableName) && IsCorrectColumnName(columnName))) {
                return (allRight, values);
            }
            try {
                using (var context = connector.Connect()) {
                    string strQuery = $"SELECT [{columnName}] FROM [{tableName}]";
                    var query = context.Database.SqlQuery<T>(strQuery);
                    values = query.ToList();
                    allRight = true;
                }
            }
            catch (Exception) {
            }
            return (allRight, values);
        }
        private static (bool isHaveMaterial, IEnumerable<Material> values) GetMaterialValues(INanocadDBConnector connector, string tableName) {
            List<Material> values = new();
            bool allRight = false;
            if (!(IsCorrectTableName(tableName))) {
                return (allRight, Array.Empty<Material>());
            }
            try {
                using (var context = connector.Connect()) {
                    string strQuery = $"SELECT [Code], [Mass] FROM [{tableName}]";
                    var query = context.Database.SqlQuery<Material>(strQuery);
                    values = query.ToList();
                    allRight = true;
                }
            }
            catch (Exception ex) {
                ;
            }
            return (allRight, values);
        }

        protected static IEnumerable<IProduct> GetProducts(Context context, string tableName) {
            if (!(IsCorrectTableName(tableName))) {
                throw new ArgumentException($"Строка \"{tableName}\" нея является допустимым именем таблицы.", nameof(tableName));
            }
            string strQuery = $"SELECT [Code], [DbImageRef], [Name], [Manufacturer], [Id] FROM [{tableName}]";
            var query = context.Database.SqlQuery<NtProduct>(strQuery);
            var l = query.ToList();
            return l;
        }
        private static IEnumerable<(string code, string uri)> GetUriValues(Context context, string tableName) {
            List<(string code, string uri)> values = new();
            if (!(IsCorrectTableName(tableName))) {
                return Array.Empty<(string code, string uri)>();
            }
            try {
                string strQuery = $"SELECT [Code], [Uri] FROM [{tableName}]";
                var query = context.Database.SqlQuery<(string code, string uri)>(strQuery);
                values = query.ToList();
                return values;

            }
            catch (Exception ex) {
                return Array.Empty<(string code, string uri)>();
            }
        }
        private static bool IsCorrectTableName(string tableName) {
            // Check for null or empty
            if (string.IsNullOrEmpty(tableName))
                return false;

            // Check for valid length (usually <= 128 characters in many SQL implementations)
            if (tableName.Length > 128)
                return false;

            // Check for allowed characters (letters, digits, underscores)
            // Adjust the allowed pattern if necessary (e.g., for case sensitivity or specific naming conventions)
            var validTableNamePattern = @"^[a-zA-Z0-9_]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(tableName, validTableNamePattern))
                return false;

            // Additional checks can be added here (e.g., reserved words, specific prefix requirements)

            return true;
        }

        private static bool IsCorrectColumnName(string columnName) {
            // Check for null or empty
            if (string.IsNullOrEmpty(columnName))
                return false;

            // Check for valid length (usually <= 128 characters in many SQL implementations)
            if (columnName.Length > 128)
                return false;

            // Check for allowed characters (letters, digits, underscores)
            // Adjust the allowed pattern if necessary (e.g., for case sensitivity or specific naming conventions)
            var validColumnNamePattern = @"^[a-zA-Z0-9_]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(columnName, validColumnNamePattern))
                return false;

            // Additional checks can be added here (e.g., reserved words, specific prefix requirements)

            return true;
        }
        private static bool IsCorrectCurrentScaleUzo(double value) {
            if (((double)((int)value)) == value &&
                 value >= 1.0 && value <= 300.0
                 ) {
                return true;
            }
            return false;
        }
        private static (bool IsSuccess, string Value) TryParseAsInt(string str, out int intValue) {
            if (string.IsNullOrEmpty(str)) {
                intValue = 0;
                return (false, string.Empty);
            }
            bool isSuccess = int.TryParse(str, NumberStyles.None, CultureInfo.GetCultureInfo("Ru-ru"), out intValue);
            string value = isSuccess ? str : $"Значение \"{str}\" не соответствует целому числу.";
            return (isSuccess, value);
        }

        private static (bool IsSuccess, string Value) TryParseAsDouble(string str, out double dValue) {
            if (string.IsNullOrEmpty(str)) {
                dValue = double.NaN;
                return (false, string.Empty);
            }
            bool isSuccess = double.TryParse(str, NumberStyles.Number, CultureInfo.GetCultureInfo("Ru-ru"), out dValue);
            string value = isSuccess ? str : $"Значение \"{str}\" не соответствует шаблону \"0,##\"";
            return (isSuccess, value);
        }
        private static string Convert(bool? v) {
            return v.HasValue ? (v == true ? "Да" : "Нет") : string.Empty;
        }
    }
}
