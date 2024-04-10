using Nano.Electric;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bs.Nano.Electric.Report
{
    public partial class Checker {
        private struct Product : IProduct {
            public string Code { get; set; }

            public int id { get; set; }
            public int? DbImageRef { get; set; }
            public string Name { get; set; }
            public string Manufacturer { get; set; }
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

    }
}
