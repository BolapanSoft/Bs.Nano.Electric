using Nano.Electric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        private List<(string Code, string ArticleName, string TableName, string TypeDescription)> LoadAllCodes() {
            List<(string Code, string ArticleName, string TableName, string TypeDescription)> allCodes = new();
            string getDescription(Type type) {
                var attr = type.GetCustomAttribute<DefaultLocalizeValueAttribute>();
                return attr?.DefaultLocalizeValue ?? string.Empty;
            }
            using (var context = connector.Connect()) {
                {
                    var description = getDescription(typeof(ScsGutterCanal));
                    var products = context.ScsGutterCanals
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.ScsGutterCanals), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGutterCover));
                    var products = context.DbScsGutterCovers
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGutterCovers), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGutterPartition));
                    var products = context.DbScsGutterPartitions
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGutterPartitions), description));
                    }
                }
                {
                    var description = getDescription(typeof(ScsGcFitting));
                    var products = context.ScsGcFittings
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.ScsGcFittings), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGcCoverUnit));
                    var products = context.DbScsGcCoverUnits
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGcCoverUnits), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGcAccessoryUnit));
                    var products = context.DbScsGcAccessoryUnits
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGcAccessoryUnits), description));
                    }
                }
                {
                    var description = getDescription(typeof(ScsGutterBolting));
                    var products = context.ScsGutterBoltings
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.ScsGutterBoltings), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGcBoltingAccessoryUnit));
                    var products = context.DbScsGcBoltingAccessoryUnits
                        .Select(p => new { p.Code, p.SpecDescription })
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p.Code, p.SpecDescription, nameof(context.DbScsGcBoltingAccessoryUnits), description));
                    }
                }
                {
                    var description = getDescription(typeof(CaeMaterialUtility));
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

    }
}
