using Bs.XML.SpreadSheet;
using Microsoft.Extensions.Logging;
using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Bs.Nano.Electric.Model;
using Bs.Nano.Electric;
using System.Data.Entity;

namespace Bs.Nano.Electric.Report {
    /// <summary>
    /// Реализует правила контроля полноты заполнения БДИ.
    /// </summary>
    public partial class Checker {
        IdCounter counter = new IdCounter();
        //ILogger logger = DI.Provider.GetService<ILogger<Report>>()!;
        //INanocadDBConnector connector = DI.Provider.GetService<INanocadDBConnector>()!;
        private readonly ILogger logger;
        private readonly INanocadDBConnector connector;


        public Checker(ILogger logger, INanocadDBConnector connector) {
            Throw.IfNull(logger);
            Throw.IfNull(connector);
            this.logger = logger;
            this.connector = connector;
        }
        public void TestDatabase() {
            using (var context = connector.Connect()) {
                TotalTablesCount();
            }
            try {
                var tests = RuleTestHelper.GetTests<Checker>("Check Rules");
                RuleTestHelper.InvokeAll<Checker>(tests, () => this);
                //TestHelper.RunTests<Report>(RuleCategory: "Check Rules");
            }
            catch (AggregateException ex) {
                //StringBuilder sb = new StringBuilder();
                //foreach (var item in ex.InnerExceptions) {
                //    sb.AppendLine(item.InnerException?.Message ?? item.Message);
                //};
                //string message = sb.ToString();
                logger.LogError($"Не пройдено {ex.InnerExceptions.Count} правил проверки.");
                foreach (var item in ex.InnerExceptions) {
                    ReportRuleAttribute rule = item.Data.Contains("Rule") ? (ReportRuleAttribute)item.Data["Rule"] : ReportRuleAttribute.Empty;
                    using (logger.BeginScope("При прохождении теста {n} \"{name}\" найдены ошибки:", rule.ListIndex, rule.DisplayName)) {
                        if (item is AggregateException ex2) {
                            foreach (var item2 in ex2.InnerExceptions) {
                                logger.LogError(item2.Message);
                            }
                        }
                        else {
                            logger.LogError(item.Message);
                        }
                    };
                    ;
                }
            }
        }
        //[ReportRule(@"Заполнение таблиц БДИ.", 0, 1), RuleCategory("Краткий отчет по базе изделий.")]
        public void TotalTablesCount() {
            using (Context context = connector.Connect()) {
                int allProductsCount = 0;
                {
                    logger.LogInformation($"Отчет по БДИ \"{context.Database.Connection.Database}\"");

                    var productCount = context.ScsGutterCanals.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["ScsGutterCanal"]}\": {productCount} элементов.");
                    productCount = context.DbScsGutterCovers.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGutterCover"]}\": {productCount} элементов.");
                    productCount = context.DbScsGutterPartitions.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGutterPartition"]}\": {productCount} элементов.");
                    productCount = context.ScsGcFittings.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["ScsGcFitting"]}\": {productCount} элементов.");
                    productCount = context.DbScsGcCoverUnits.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGcCoverUnit"]}\": {productCount} элементов.");
                    productCount = context.DbScsGcAccessoryUnits.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGcAccessoryUnit"]}\": {productCount} элементов.");
                    productCount = context.ScsGutterBoltings.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["ScsGutterBolting"]}\": {productCount} элементов.");
                    productCount = context.DbScsGcBoltingAccessoryUnits.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGcBoltingAccessoryUnit"]}\": {productCount} элементов.");
                    productCount = context.CaeMaterialUtilities.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["CaeMaterialUtility"]}\": {productCount} элементов.");
                    productCount = context.DbScsGcSeriaConfigirations.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.DbScsGcSeriaConfiguration_Description}\": {productCount} элементов.");
                    productCount = context.DbScsGutterUtilitySets.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.DbScsGutterUtilitySet_Description}\": {productCount} элементов.");
                    productCount = context.DbGcMountSystems.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.DbGcMountSystemSet_Description}\": {productCount} элементов.");
                    logger.LogInformation($"Всего элементов в БДИ: {allProductsCount}.");

                }
            }
        }
        [ReportRule(@"Известные таблицы БДИ.", 0, 0), RuleCategory("Краткий отчет по базе изделий.")]
        public void TotalKnownTablesCount() {
            using (Context context = connector.Connect()) {
                var tables = context.GetKnownTables();
                foreach (var tableProperty in tables) {
                    (object property, string tableName) = tableProperty;
                    IQueryable dbSet = (IQueryable)property;
                    int count = 0;
                    try {
                        count = RuleTestHelper.GetCount(dbSet);
                    }
                    catch (Exception ex) {
                        string getMessage(Exception ex) {
                            if (ex.InnerException is null) {
                                return ex.Message;
                            }
                            else {
                                return getMessage(ex.InnerException);
                            }
                        }
                        logger.LogError($"Ошибка обращения к таблице \"{tableName}\": {getMessage(ex)}");
                        count = 0;
                    }
                    if (count > 0) {
                        logger.LogInformation($"Таблица \"{tableName}\": {count} элементов.");
                    }
                }
            }
        }

        [ReportRule(@"Отчет по таблицам БДИ.", 0, 1), RuleCategory("Отчет по базе изделий.")]
        public void ProductCategories() {
            string TopSerieLevel(string series) {
                if (string.IsNullOrEmpty(series))
                    return series;
                string[] sElements = series.Split('\\');
                return sElements[0];
            }

            using (Context context = connector.Connect()) {
                int allProductsCount = 0;
                var dbName = context.Database.Connection.Database;
                {
                    var productCount = context.ScsGutterCanals.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["ScsGutterCanal"]}\": {productCount} элементов.");
                    {
                        var group = Resources.ImageCategory["ScsGutterCanal"];
                        var groupsArr = context.ScsGutterCanals
                            .Select(p => new { p.GutterType, p.Series })
                            .ToArray();
                        var groups = groupsArr
                            .Select(p => new {
                                GutterType = p.GutterType.GetDescription(),
                                Serie = TopSerieLevel(p.Series)
                            })
                            .GroupBy(item => (item.GutterType, item.Serie))
                            .Select(gr => (Group: gr.Key, Count: gr.Count()));
                        foreach (var item in groups) {
                            logger.LogInformation($"{group}\t{item.Group.GutterType}\t{item.Group.Serie}\t{item.Count}");
                        }
                    }
                    productCount = context.DbScsGutterCovers.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGutterCover"]}\": {productCount} элементов.");
                    {
                        var group = Resources.ImageCategory["ScsGutterCanal"];
                        var groupsArr = context.DbScsGutterCovers
                            .Select(p => new { p.Series })
                            .ToArray();
                        var groups = groupsArr
                            .Select(p => new {
                                Serie = TopSerieLevel(p.Series)
                            })
                            .GroupBy(item => item.Serie)
                            .Select(gr => (Group: gr.Key, Count: gr.Count()));
                        foreach (var item in groups) {
                            logger.LogInformation($"{group}\t\t{item.Group}\t{item.Count}");
                        }
                    }
                    productCount = context.DbScsGutterPartitions.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGutterPartition"]}\": {productCount} элементов.");
                    productCount = context.ScsGcFittings.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["ScsGcFitting"]}\": {productCount} элементов.");
                    {

                        var group = Resources.ImageCategory["ScsGutterCanal"];
                        var groupsArr = context.ScsGcFittings
                            .Select(p => new { p.FittingType, p.Series })
                            .ToArray();
                        var groups = groupsArr
                            .Select(p => new {
                                GutterType = p.FittingType.GetDescription(),
                                Serie = TopSerieLevel(p.Series)
                            })
                            .GroupBy(item => (item.GutterType, item.Serie))
                            .Select(gr => (Group: gr.Key, Count: gr.Count()));
                        foreach (var item in groups) {
                            logger.LogInformation($"{group}\t{item.Group.GutterType}\t{item.Group.Serie}\t{item.Count}");
                        }
                    }
                    productCount = context.DbScsGcCoverUnits.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGcCoverUnit"]}\": {productCount} элементов.");
                    {
                        var group = Resources.ImageCategory["ScsGutterCanal"];
                        var groupsArr = context.DbScsGcCoverUnits
                            .Select(p => new { p.CoverType, p.Series })
                            .ToArray();
                        var groups = groupsArr
                            .Select(p => new {
                                GutterType = p.CoverType.GetDescription(),
                                Serie = TopSerieLevel(p.Series)
                            })
                            .GroupBy(item => (item.GutterType, item.Serie))
                            .Select(gr => (Group: gr.Key, Count: gr.Count()));
                        foreach (var item in groups) {
                            logger.LogInformation($"{group}\t{item.Group.GutterType}\t{item.Group.Serie}\t{item.Count}");
                        }
                    }
                    productCount = context.DbScsGcAccessoryUnits.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGcAccessoryUnit"]}\": {productCount} элементов.");
                    {
                        string GutterType(int value) {
                            var enumValue = (DbGcMsAccesoryType)value;
                            return enumValue.GetDescription();
                        }
                        var group = Resources.ImageCategory["ScsGutterCanal"];
                        var groupsArr = context.DbScsGcAccessoryUnits
                            .Select(p => new { p.AccessoryType, p.Series })
                            .ToArray();
                        var groups = groupsArr
                            .Select(p => new {
                                GutterType = GutterType(p.AccessoryType.HasValue ? (int)p.AccessoryType.Value : 0),
                                Serie = TopSerieLevel(p.Series)
                            })
                            .GroupBy(item => (item.GutterType, item.Serie))
                            .Select(gr => (Group: gr.Key, Count: gr.Count()));
                        foreach (var item in groups) {
                            logger.LogInformation($"{group}\t{item.Group.GutterType}\t{item.Group.Serie}\t{item.Count}");
                        }
                    }
                    productCount = context.ScsGutterBoltings.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["ScsGutterBolting"]}\": {productCount} элементов.");
                    {
                        string GutterType(ScsGutterBoltingTypeEnum value) {
                            var enumValue = value;
                            return enumValue.GetDescription();
                        }
                        var group = Resources.ImageCategory["ScsGutterCanal"];
                        var groupsArr = context.ScsGutterBoltings
                            .Select(p => new { p.CanalBoltingType, p.Series })
                            .ToArray();
                        var groups = groupsArr
                            .Select(p => new {
                                GutterType = GutterType(p.CanalBoltingType),
                                Serie = TopSerieLevel(p.Series)
                            })
                            .GroupBy(item => (item.GutterType, item.Serie))
                            .Select(gr => (Group: gr.Key, Count: gr.Count()));
                        foreach (var item in groups) {
                            logger.LogInformation($"{group}\t{item.Group.GutterType}\t{item.Group.Serie}\t{item.Count}");
                        }
                    }
                    productCount = context.DbScsGcBoltingAccessoryUnits.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["DbScsGcBoltingAccessoryUnit"]}\": {productCount} элементов.");

                    productCount = context.CaeMaterialUtilities.Count();
                    allProductsCount += productCount;
                    logger.LogInformation($"Таблица \"{Resources.ImageCategory["CaeMaterialUtility"]}\": {productCount} элементов.");
                    logger.LogInformation($"Всего элементов в БДИ: {allProductsCount}.");

                }
            }
        }
        [ReportRule(@"В базе значения артикула должны быть уникальными.", 4, 0), RuleCategory("Общие рекомендации.")]
        public void Rule_01_000() {
            List<(string Code, string TableName, string TypeDescription)> allCodes = new ();
            string getDescription(Type type) {
                var attr = type.GetCustomAttribute<DefaultLocalizeValueAttribute>();
                return attr?.DefaultLocalizeValue ?? string.Empty;
            }
            using (var context = connector.Connect()) {
                {
                    var description = getDescription(typeof(ScsGutterCanal));
                    var products = context.ScsGutterCanals
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.ScsGutterCanals), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGutterCover));
                    var products = context.DbScsGutterCovers
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.DbScsGutterCovers), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGutterPartition));
                    var products = context.DbScsGutterPartitions
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.DbScsGutterPartitions), description));
                    }
                }
                {
                    var description = getDescription(typeof(ScsGcFitting));
                    var products = context.ScsGcFittings
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.ScsGcFittings), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGcCoverUnit));
                    var products = context.DbScsGcCoverUnits
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.DbScsGcCoverUnits), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGcAccessoryUnit));
                    var products = context.DbScsGcAccessoryUnits
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.DbScsGcAccessoryUnits), description));
                    }
                }
                {
                    var description = getDescription(typeof(ScsGutterBolting));
                    var products = context.ScsGutterBoltings
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.ScsGutterBoltings), description));
                    }
                }
                {
                    var description = getDescription(typeof(DbScsGcBoltingAccessoryUnit));
                    var products = context.DbScsGcBoltingAccessoryUnits
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.DbScsGcBoltingAccessoryUnits), description));
                    }
                }
                {
                    var description = getDescription(typeof(CaeMaterialUtility));
                    var products = context.CaeMaterialUtilities
                        .Select(p => p.Code)
                        ;
                    foreach (var p in products) {
                        allCodes.Add((p, nameof(context.CaeMaterialUtilities), description));
                    }
                }
                CheckCodesIsUniqueness(allCodes);
            }
        }
        /// <summary>
        /// Осуществляет проверку уникальности поля <paramref name="Code"/> во входных данных.
        /// </summary>
        /// <param name="allCodes">Массив входных данных.</param>
        /// <exception cref="RuleTestException">Правило уникальности кода нарушено.</exception>
        public static void CheckCodesIsUniqueness(IEnumerable<(string Code, string TableName, string TableDescription)> allCodes) {
            var errors = allCodes.GroupBy(item => item.Code)
                                .Where(group => group.Count() > 1);
            if (errors.Any()) {
                var message = $"Имеются дублирующиеся артикулы.";
                FailRuleTest(message, errors.SelectMany(group => group));
            }
        }

        /**/
        public const string sRule001 = @"В таблице DbImages должен быть внесен файл ""File not found"" под индексом id==0";
        [ReportRule(@"В таблице DbImages должен быть внесен файл ""File not found.png"" под индексом id==0",
            4, 1), RuleCategory("Общие рекомендации.")]
        public void Rule_01_001() {
            // logger.LogInformation($"{GetIndex(nameof(Rule_01_001))}\t{sRule001}");

            using (var context = connector.Connect()) {
                var emptyImage = context.DbImages.FirstOrDefault(img => img.Id == 0);
                if (emptyImage is null || emptyImage.Text != "File not found.png") {
                    throw new RuleTestException($"Не внесен эскиз \"File not found.png\".");
                }
            }
        }
        [ReportRule(@"Для всех элементов должен быть внесен эскиз.",
            4, 2), RuleCategory("Общие рекомендации.")]
        public void Rule_01_002() {
            using (var context = connector.Connect()) {
                List<(string Code, string Description)> sketchNotFound = new List<(string Code, string Description)>();

                var products = context.ScsGutterCanals
                    .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }
                products = context.DbScsGutterCovers
                    .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }
                products = context.DbScsGutterPartitions
                    .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }
                products = context.ScsGcFittings
                    .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }
                products = context.DbScsGcCoverUnits
                    .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }
                products = context.DbScsGcAccessoryUnits
                    .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }
                products = context.ScsGutterBoltings
                    .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }
                products = context.DbScsGcBoltingAccessoryUnits
                    .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }
                products = context.CaeMaterialUtilities
                     .Where(p => p.DbImageRef < 1)
                    .Select(p => new { p.Code, p.SpecDescription });
                foreach (var p in products) {
                    sketchNotFound.Add((p.Code, p.SpecDescription));
                }

                if (sketchNotFound.Count > 0) {
                    //StringBuilder sb = new StringBuilder();
                    //sb.AppendLine($"Не внесены эскизы для {sketchNotFound.Count} элементов:");
                    //sb.AppendLine("Code\tSpecDescription");
                    //foreach (var item in sketchNotFound.Take(50)) {
                    //    sb.AppendLine($"{item.Code}\t{item.Description}");
                    //}
                    //if (sketchNotFound.Count > 100) {
                    //    sb.AppendLine($"... обрезано {sketchNotFound.Count - 50} элементов.");
                    //}
                    var message = $"Не внесены эскизы для {sketchNotFound.Count} элементов";
                    FailRuleTest(message, sketchNotFound);
                }
            }
        }
        [ReportRule(@"Для всех элементов должна быть внесена масса.",
            4, 3), RuleCategory("Общие рекомендации.")]
        public void Rule_01_003() {
            List<(string Serie, string Code, string Mass)> codes = new List<(string Serie, string Code, string Mass)>(128);
            void CheckRule(string serie, string code, string sWeight) {
                bool isMatch = (double.TryParse(sWeight, NumberStyles.Float, CultureInfo.InvariantCulture, out double weight) || double.TryParse(sWeight, NumberStyles.Float, CultureInfo.CurrentCulture, out weight)) && weight > 0.0;
                if (!isMatch) {
                    codes.Add((serie, code, sWeight));
                }
            }
            using (var context = connector.Connect()) {
                foreach (var el in context.ScsGutterCanals) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }
                foreach (var el in context.DbScsGutterCovers) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }
                foreach (var el in context.DbScsGutterPartitions) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }
                foreach (var el in context.ScsGcFittings) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }
                foreach (var el in context.DbScsGcCoverUnits) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }
                foreach (var el in context.DbScsGcAccessoryUnits) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }
                foreach (var el in context.ScsGutterBoltings) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }
                foreach (var el in context.DbScsGcBoltingAccessoryUnits) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }
                foreach (var el in context.CaeMaterialUtilities) {
                    CheckRule(el.Series, el.Code, el.Mass);
                }

            }
            //count = errors.Count;
            if (codes.Count > 0) {
                FailRuleTest($"Не внесена масса для {codes.Count} артикулов.", codes);
            }
        }
        [ReportRule(@"Для всех элементов должна быть внесена ссылка на сайт производителя.",
            4, 4), RuleCategory("Общие рекомендации.")]
        public void Rule_01_004() {
            int count = 0;
            List<(string code, string url, string err)> errors = new List<(string code, string url, string err)>(1024);
            //async Task CheckRuleAsync((string code, string url) tack) {
            //    (string code, string url) = tack;
            //    var uri = new Uri(url);
            //    var uriScheme = uri.Scheme;
            //    if (uriScheme != "http" || uriScheme != "https") {
            //        FailRuleTest($"Строка \"{url}\" не соответствует схеме http://", tack) }
            //    await Task.Delay(new Random().Next(300) + 1).ConfigureAwait(false);
            //    using (HttpClient client = new HttpClient()) {
            //        HttpResponseMessage response = await client.GetAsync(uri);
            //        if (response.IsSuccessStatusCode) {
            //            return;
            //        }
            //        else {
            //            Assert.Fail($"Query to the specified URL return StatusCode=\"{response.StatusCode}\".");
            //        }
            //    }
            //}
            void CheckRule((string code, string url) tack) {
                (string code, string url) = tack;
                if (string.IsNullOrWhiteSpace(url)) {
                    errors.Add((code, url, string.Empty));
                    return;
                }
                try {
                    var uri = new Uri(url);
                    var uriScheme = uri.Scheme;
                    if (!(uriScheme == "http" || uriScheme == "https")) {
                        var errMessage = $"Строка \"{url}\" не соответствует схеме http(s)://";
                        errors.Add((code, url, errMessage));
                    }
                }
                catch (Exception ex) {
                    errors.Add((code, url, ex.Message));
                }

            }

            var tasks = new List<(string Code, string Ur)>(1024 * 8);
            using (var context = connector.Connect()) {
                tasks.AddRange(
                 context.ScsGutterCanals
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                tasks.AddRange(
                context.DbScsGutterCovers
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                tasks.AddRange(
                context.DbScsGutterPartitions
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                tasks.AddRange(
                context.ScsGcFittings
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                tasks.AddRange(
                context.DbScsGcCoverUnits
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                tasks.AddRange(
                context.DbScsGcAccessoryUnits
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                tasks.AddRange(
                context.ScsGutterBoltings
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                tasks.AddRange(
                context.DbScsGcBoltingAccessoryUnits
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                tasks.AddRange(
                context.CaeMaterialUtilities
                    .ToArray()
                    .Select(el => (el.Code, el.Url))
                );
                //tasks.AddRange(
                //context.ScsGutterCanals
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
                //tasks.AddRange(
                //context.ScsGutterCanals
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //    );

            }
            // Запросы на сайт выполним последовательно, чтобы исключить отказ в обслуживании.

            foreach (var task in tasks) {
                CheckRule(task);
            }
            count = errors.Count;
            if (count > 0) {
                FailRuleTest($"Для {count} артикулов ссылка на сайт производителя некорректна.",
                        errors);

            }
        }

        public const string sRule201 = @"Для прямых секций лотков должны быть внесены длина, ширина, высота лотка";
        [ReportRule(@"Для прямых секций лотков должны быть внесены длина, ширина, высота лотка",
            2, 1), RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_02_001() {
            using (var context = connector.Connect()) {
                var scsGutterCanals = context.ScsGutterCanals
                    .Select(p => new { p.Code,p.Series, p.GutterDepth, p.GutterHeight, p.SegLength })
                    .ToList();
                //var errors = new LinkedList<(string Code, double? GutterDepth, double? GutterHeight, double? SegLength)>();
                //foreach (var p in scsGutterCanals) {
                //    if (p.SegLength > 1 & p.GutterDepth > 1 & p.GutterHeight > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.GutterDepth, p.GutterHeight, p.SegLength));
                //}
                var errors = scsGutterCanals
                    .Where(p => !(p.SegLength > 1 & p.GutterDepth > 1 & p.GutterHeight > 1))
                    .Select(p => $"({p.Series}\\{p.Code} {nameof( p.GutterDepth)}:{p.GutterDepth},{nameof(p.GutterHeight)}:{p.GutterDepth},{nameof(p.SegLength)}:{p.SegLength}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} элементов прямых секций лотков не внесена длина, ширина или высота лотка.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для перегородок должны быть внесены длина, высота",
            2, 2)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGutterPartition))]
        public void Rule_02_002() {
            using (var context = connector.Connect()) {
                var products = context.DbScsGutterPartitions.ToList();
                //    .Select(p => new { p.Code, p.PartitionHeight, p.PartitionLength });
                //var errors = new LinkedList<(string Code, double? PartitionHeight, double? PartitionLength)>();
                //foreach (var p in products) {
                //    if (p.PartitionHeight > 1 & p.PartitionLength > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.PartitionHeight, p.PartitionLength));
                //}
                var errors = products.Where(p => !(p.PartitionHeight > 1 & p.PartitionLength > 1))
                    .Select(p => $"({p.Series}\\{p.Code} {nameof(p.PartitionHeight)}:{p.PartitionHeight}, {nameof(p.PartitionLength)}:{p.PartitionLength}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} перегородок не внесена длина или высота лотка.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для крышек прямых секций должны быть внесены ширина, высота",
            2, 3)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGutterCover))]
        public void Rule_02_003() {
            using (var context = connector.Connect()) {
                var products = context.DbScsGutterCovers
                    .ToList();
                //    .Select(p => new { p.Code, p.CoverWidth, p.CoverLength });
                //var errors = new LinkedList<(string Code, double? CoverWidth, double? CoverLength)>();
                //foreach (var p in products) {
                //    if (p.CoverWidth > 1 & p.CoverLength > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.CoverWidth, p.CoverLength));
                //}
                var errors = products.Where(p => !(p.CoverWidth > 1 & p.CoverLength > 1))
                    .Select(p => $"({p.Series}\\{p.Code} {nameof(p.CoverWidth)}:{p.CoverWidth}, {nameof(p.CoverLength)}:{p.CoverLength}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} элементов крышек прямых секций лотков не внесена длина или ширина.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция соединительная переходная вертикальная"" должны быть внесены высота борта основного и отходящего элемента, ширина основного и отходящего элемента.",
            2, 4)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_004() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch });
                //var errors = new LinkedList<(string Code, double?, double?, double?, double?)>();
                //foreach (var p in products) {
                //    if (p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch));
                //}
                var errors = products.Where(p => !(p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.WidthMainBranch)}:{p.WidthMainBranch}, {nameof(p.HeightMainBranch)}:{p.HeightMainBranch}, {nameof(p.WidthOutBranch)}:{p.WidthOutBranch}, {nameof(p.HeightOutBranch)}:{p.HeightOutBranch}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Для {errors.Count} элементов Секция соединительная переходная вертикальная не полностью внесены геометрические размеры.",
                        errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция соединительная переходная вертикальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента, ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule(@"Внутри серии элементов \""Секция соединительная переходная вертикальная\"" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента (HeightMainBranch, HeightOutBranch), ширина основного и отходящего элемента (WidthMainBranch, WidthOutBranch)].",
            2, 5)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_028() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> sizes) {
            //    HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGutterFittingTypeEnum.VERTICAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name, size:(p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast(( p.Key, error));
                    }
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов \""Секция соединительная переходная горизонтальная\"" должны быть внесены высота борта основного и отходящего элемента, ширина основного и отходящего элемента.",
            2, 6)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_005() {
            var ft = ScsGutterFittingTypeEnum.HORIZONTAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch, p.GutterPassageType });
                //var errors = new LinkedList<(string Code, double?, double?, double?, double?)>();
                //foreach (var p in products) {
                //    if (p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1) {
                //        continue;
                //    }
                //    else
                //        errors.AddLast((p.Code, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch));
                //}
                var errors = products.Where(p => !(p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.WidthMainBranch)}:{p.WidthMainBranch}, {nameof(p.HeightMainBranch)}:{p.HeightMainBranch}, {nameof(p.WidthOutBranch)}:{p.WidthOutBranch}, {nameof(p.HeightOutBranch)}:{p.HeightOutBranch}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция соединительная переходная горизонтальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента, ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule(@"Внутри серии элементов \""Секция соединительная переходная горизонтальная\"" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента (HeightMainBranch, HeightOutBranch), ширина основного и отходящего элемента (WidthMainBranch, WidthOutBranch)].",
            2, 7)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_029() {
           //IEnumerable<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> CheckRule(IEnumerable<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> sizes) {
           //     HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)>();
           //     return sizes.Where(size => !values.Add(size));
           // };

            var ft = ScsGutterFittingTypeEnum.HORIZONTAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name, size: (p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }

            }
        }
        [ReportRule(@"Для элементов ""Секция соединительная переходная горизонтальная"" должен быть установлен тип перехода GutterPassageType",
            2, 8)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_007() {

            var ft = ScsGutterFittingTypeEnum.HORIZONTAL_PASSAGE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .Select(p => new { p.Code,p.Series, p.GutterPassageType });
                LinkedList<string> errors = new();
                foreach (var p in products) {
                    if (p.GutterPassageType.HasValue) {
                        var knownEnumValues = typeof(ScsGutterPassageType).GetEnumValues() as ScsGutterPassageType[];
                        if (knownEnumValues.Contains(p.GutterPassageType.Value))
                            continue;
                    }
                    else
                        errors.AddLast($"({p.Series}\\{p.Code} {nameof(p.GutterPassageType)}:{p.GutterPassageType})");
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция Т-образная горизонтальная"" должны быть внесены [высота борта основного и отходящего элемента (HeightMainBranch, HeightOutBranch), ширина основного и отходящего элемента (WidthMainBranch, WidthOutBranch)].",
            2, 9)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_008() {
            var ft = ScsGutterFittingTypeEnum.TRIPLE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                //.Select(p => new { p.Code, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch });
                //var errors = new LinkedList<(string Code, double?, double?, double?, double?)>();
                //foreach (var p in products) {
                //    if (p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch));
                //}
                var errors = products.Where(p => !(p.WidthMainBranch > 1 & p.HeightMainBranch > 1 & p.WidthOutBranch > 1 & p.HeightOutBranch > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.WidthMainBranch)}:{p.WidthMainBranch}, {nameof(p.HeightMainBranch)}:{p.HeightMainBranch}, {nameof(p.WidthOutBranch)}:{p.WidthOutBranch}, {nameof(p.HeightOutBranch)}:{p.HeightOutBranch}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция Т-образная горизонтальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента, ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция Т-образная горизонтальная"" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента (HeightMainBranch, HeightOutBranch), ширина основного и отходящего элемента (WidthMainBranch, WidthOutBranch)].",
            2, 10)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_030() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> sizes) {
            //    HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};

            var ft = ScsGutterFittingTypeEnum.TRIPLE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name, size: (p.WidthMainBranch, p.HeightMainBranch, p.WidthOutBranch, p.HeightOutBranch)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? WidthMainBranch, double? HeightMainBranch, double? WidthOutBranch, double? HeightOutBranch)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция угловая вертикальная внешняя"" должны быть внесены высота борта, ширина элемента и угол поворота.",
            2, 11)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_009() {

            var ft = ScsGutterFittingTypeEnum.VERTICAL_BEND_OUTER;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                //.Select(p => new { p.Code, p.Height, p.Depth, p.VerticalBendType });
                //var errors = new LinkedList<(string Code, double?, double?, ScsVerticalBendTypeEnum?)>();
                //foreach (var p in products) {
                //    if (p.Height > 1 & p.Depth > 1) {
                //        if (p.VerticalBendType.HasValue) {
                //            var knownEnumValues = typeof(ScsVerticalBendTypeEnum).GetEnumValues() as ScsVerticalBendTypeEnum[];
                //            if (knownEnumValues.Contains(p.VerticalBendType.Value))
                //                continue;
                //        }
                //    }
                //    else
                //        errors.AddLast((p.Code, p.Height, p.Depth, p.VerticalBendType));
                //}
                var knownEnumValues = typeof(ScsVerticalBendTypeEnum).GetEnumValues() as ScsVerticalBendTypeEnum[];
                var errors = products.Where(p => !( (p.Height > 1 & p.Depth > 1) &&
                        p.VerticalBendType.HasValue &&
                        knownEnumValues.Contains(p.VerticalBendType.Value) ))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}, {nameof(p.VerticalBendType)}:{p.VerticalBendType}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция угловая вертикальная внешняя\" должно быть не более одного сочетания параметров [высота борта, ширина элемента и угол поворота]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция угловая вертикальная внешняя"" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth и угол поворота VerticalBendType].",
         2, 12)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_031() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)> sizes) {
            //    HashSet<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)> values = new HashSet<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};
            var ft = ScsGutterFittingTypeEnum.VERTICAL_BEND_OUTER;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth, p.VerticalBendType })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name,size: (p.Height, p.Depth, p.VerticalBendType)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция угловая вертикальная внутренняя"" должны быть внесены высота борта Height, ширина элемента Depth и угол поворота VerticalBendType.",
            2, 13)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_010() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                //.Select(p => new { p.Code, p.Height, p.Depth, p.VerticalBendType });
                //var errors = new LinkedList<(string Code, double?, double?, ScsVerticalBendTypeEnum?)>();
                //foreach (var p in products) {
                //    if (p.Height > 1 & p.Depth > 1) {
                //        if (p.VerticalBendType.HasValue) {
                //            var knownEnumValues = typeof(ScsVerticalBendTypeEnum).GetEnumValues() as ScsVerticalBendTypeEnum[];
                //            if (knownEnumValues.Contains(p.VerticalBendType.Value))
                //                continue;
                //        }
                //    }
                //    else
                //        errors.AddLast((p.Code, p.Height, p.Depth, p.VerticalBendType));
                //}
                var knownEnumValues = typeof(ScsVerticalBendTypeEnum).GetEnumValues() as ScsVerticalBendTypeEnum[];
                var errors = products.Where(p => !((p.Height > 1 & p.Depth > 1) &&
                        p.VerticalBendType.HasValue &&
                        knownEnumValues.Contains(p.VerticalBendType.Value)))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}, {nameof(p.VerticalBendType)}:{p.VerticalBendType}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция угловая вертикальная внутренняя\" должно быть не более одного сочетания параметров [высота борта, ширина элемента и угол поворота]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция угловая вертикальная внутренняя"" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth и угол поворота VerticalBendType].",
         2, 14)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_032() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)> sizes) {
            //    HashSet<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)> values = new HashSet<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGutterFittingTypeEnum.VERTICAL_BEND_INNER;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth, p.VerticalBendType })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name,size: (p.Height, p.Depth, p.VerticalBendType)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Height, double? Depth, ScsVerticalBendTypeEnum? VerticalBendType)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Секция угловая вертикальная универсальная"" должны быть внесены высота борта Height, ширина элемента Depth и угол поворота VerticalUniversalBendType.",
         2, 15)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_011() {
            var ft = ScsGutterFittingTypeEnum.VERTICAL_BENT_UNIVERSE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                //.Select(p => new { p.Code, p.Height, p.Depth, p.VerticalUniversalBendType });
                //var errors = new LinkedList<(string Code, double?, double?, ScsVerticalUniversalBendTypeEnum?)>();
                //foreach (var p in products) {
                //    if (p.Height > 1 & p.Depth > 1) {
                //        if (p.VerticalUniversalBendType.HasValue) {
                //            var knownEnumValues = typeof(ScsVerticalUniversalBendTypeEnum).GetEnumValues() as ScsVerticalUniversalBendTypeEnum[];
                //            if (knownEnumValues.Contains(p.VerticalUniversalBendType.Value))
                //                continue;
                //        }
                //    }
                //    else
                //        errors.AddLast((p.Code, p.Height, p.Depth, p.VerticalUniversalBendType));
                //}
                var knownEnumValues = typeof(ScsVerticalUniversalBendTypeEnum).GetEnumValues() as ScsVerticalUniversalBendTypeEnum[];
                var errors = products.Where(p => !((p.Height > 1 & p.Depth > 1) &&
                        p.VerticalUniversalBendType.HasValue &&
                        knownEnumValues.Contains(p.VerticalUniversalBendType.Value)))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}, {nameof(p.VerticalUniversalBendType)}:{p.VerticalUniversalBendType}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция угловая вертикальная универсальная\" должно быть не более одного сочетания параметров [высота борта, ширина элемента и угол поворота]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция угловая вертикальная универсальная"" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth и угол поворота VerticalUniversalBendType].",
         2, 16)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_033() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? Height, double? Depth, ScsVerticalUniversalBendTypeEnum? VerticalUniversalBendType)> sizes) {
            //    HashSet<(double? Height, double? Depth, ScsVerticalUniversalBendTypeEnum? VerticalUniversalBendType)> values = new HashSet<(double? Height, double? Depth, ScsVerticalUniversalBendTypeEnum? VerticalUniversalBendType)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGutterFittingTypeEnum.VERTICAL_BENT_UNIVERSE;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth, p.VerticalUniversalBendType })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name, size: (p.Height, p.Depth, p.VerticalUniversalBendType)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Height, double? Depth, ScsVerticalUniversalBendTypeEnum? VerticalUniversalBendType)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }

            }
        }
        [ReportRule(@"Для элементов ""Секция угловая горизонтальная"" должны быть внесены высота борта Height, ширина элемента Depth и угол поворота BendType.",
            2, 17)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_012() {
            var ft = ScsGutterFittingTypeEnum.BEND;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                //.Select(p => new { p.Code, p.Height, p.Depth, p.BendType });
                //var errors = new LinkedList<(string Code, double?, double?, ScsBendTypeEnum?)>();
                //foreach (var p in products) {
                //    if (p.Height > 1 & p.Depth > 1) {
                //        if (p.BendType.HasValue) {
                //            var knownEnumValues = typeof(ScsBendTypeEnum).GetEnumValues() as ScsBendTypeEnum[];
                //            if (knownEnumValues.Contains(p.BendType.Value))
                //                continue;
                //        }
                //    }
                //    else
                //        errors.AddLast((p.Code, p.Height, p.Depth, p.BendType));
                //}
                var knownEnumValues = typeof(ScsBendTypeEnum).GetEnumValues() as ScsBendTypeEnum[];
                var errors = products.Where(p => !((p.Height > 1 & p.Depth > 1) &&
                        p.BendType.HasValue &&
                        knownEnumValues.Contains(p.BendType.Value)))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}, {nameof(p.BendType)}:{p.BendType}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция угловая горизонтальная\" должно быть не более одного сочетания параметров [высота борта, ширина элемента и угол поворота]
        /// </summary>
        [ReportRule(@"Внутри серии элементов ""Секция угловая горизонтальная"" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth и угол поворота BendType].",
         2, 18)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_034() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? Height, double? Depth, ScsBendTypeEnum? BendType)> sizes) {
            //    HashSet<(double? Height, double? Depth, ScsBendTypeEnum? BendType)> values = new HashSet<(double? Height, double? Depth, ScsBendTypeEnum? BendType)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGutterFittingTypeEnum.BEND;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth, p.BendType })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name,size: (p.Height, p.Depth, p.BendType)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Height, double? Depth, ScsBendTypeEnum? BendType)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        public const string sRule013 = @"Для элементов ""Секция Х-образная горизонтальная"" должны быть внесены высота борта основного и отходящего элемента, ширина основного и отходящего элемента.";
        [ReportRule(@"Для элементов ""Секция Х-образная горизонтальная"" должны быть внесены высота борта основного и отходящего элемента, ширина основного и отходящего элемента.",
         2, 19)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_013() {
            Console.Write($"{GetIndex(nameof(Rule_02_013))}\t{sRule013}");

            var ft = ScsGutterFittingTypeEnum.CROSS;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                    .Where(p => p.FittingType == ft)
                    .ToList();
                //.Select(p => new { p.Code, p.Width1Branch, p.Height1Branch, p.Width2Branch, p.Height2Branch });
                //var errors = new LinkedList<(string Code, double?, double?, double?, double?)>();
                //foreach (var p in products) {
                //    if (p.Width1Branch > 1 & p.Height1Branch > 1 & p.Width2Branch > 1 & p.Height2Branch > 1) {
                //        continue;
                //    }
                //    else
                //        errors.AddLast((p.Code, p.Width1Branch, p.Height1Branch, p.Width2Branch, p.Height2Branch));
                //}
                var errors = products.Where(p => !(p.Width1Branch > 1 & p.Height1Branch > 1 & p.Width2Branch > 1 & p.Height2Branch > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Width1Branch)}:{p.Width1Branch}, {nameof(p.Height1Branch)}:{p.Height1Branch}, {nameof(p.Width2Branch)}:{p.Width2Branch}, {nameof(p.Height2Branch)}:{p.Height2Branch}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Секция Х-образная горизонтальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента, ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Секция Х-образная горизонтальная\" должно быть не более одного сочетания параметров [высота борта основного и отходящего элемента (Height1Branch, Height2Branch), ширина основного и отходящего элемента (Width1Branch, Width2Branch)].",
         2, 20)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_035() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? Width1Branch, double? Height1Branch, double? Width2Branch, double? Height2Branch)> sizes) {
            //    HashSet<(double? Width1Branch, double? Height1Branch, double? Width2Branch, double? Height2Branch)> values = new HashSet<(double? Width1Branch, double? Height1Branch, double? Width2Branch, double? Height2Branch)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGutterFittingTypeEnum.CROSS;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Width1Branch, p.Height1Branch, p.Width2Branch, p.Height2Branch })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name, size: (p.Width1Branch, p.Height1Branch, p.Width2Branch, p.Height2Branch)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? Width1Branch, double? Height1Branch, double? Width2Branch, double? Height2Branch)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }

        [ReportRule("Внутри серии элементов \"Крышки\\Секция Х-образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента (CoverWidth1, CoverWidth2)].",
         2, 21)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_014() {

            var ft = ScsGcCoverType.TRIPLE;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .Select(p => new {p.Series, p.Name, p.Code, p.CoverWidth1, p.CoverWidth2 })
                    .ToArray()
                    .GroupBy(p=> p.Series, p=>(p.Name, size: (p.CoverWidth1, p.CoverWidth2)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? CoverWidth1, double? CoverWidth2)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка Т-образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышки\\Секция Т-образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента (CoverWidth1, CoverWidth2)].",
        2, 22)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_036() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? CoverWidth1, double? CoverWidth2)> sizes) {
            //    HashSet<(double? CoverWidth1, double? CoverWidth2)> values = new HashSet<(double? CoverWidth1, double? CoverWidth2)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGcCoverType.TRIPLE;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth1, p.CoverWidth2 })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name,size: (p.CoverWidth1, p.CoverWidth2)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? CoverWidth1, double? CoverWidth2)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        //public const string sRule015 = @"Для элементов ""Крышка угловая вертикальная внешняя"" должна быть внесена ширина элемента.";
        [ReportRule(@"Для элементов ""Крышка угловая вертикальная внешняя"" должна быть внесена ширина элемента.",
         2, 23)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_015() {
            var ft = ScsGcCoverType.VERTICAL_BEND_OUTER;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.CoverWidth });
                //var errors = new LinkedList<(string Code, double?)>();
                //foreach (var p in products) {
                //    if (p.CoverWidth > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.CoverWidth));
                //}
                var errors = products.Where(p => !(p.CoverWidth > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.CoverWidth)}:{p.CoverWidth}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка угловая вертикальная внешняя\" должно быть не более одного сочетания параметров [ширина элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышка угловая вертикальная внешняя\" должно быть не более одного сочетания параметров [ширина элемента CoverWidth].",
         2, 24)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_037() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<double?> sizes) {
            //    HashSet<double?> values = new HashSet<double?>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGcCoverType.VERTICAL_BEND_OUTER;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name,p.CoverWidth));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<double?> values = new();
                    var errorValues = p.Where(item => !values.Add(item.CoverWidth));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        public const string sRule016 = @"Для элементов ""Крышка угловая горизонтальная"" должна быть внесена ширина элемента";
        [ReportRule(@"Для элементов ""Крышка угловая горизонтальная"" должна быть внесена ширина элемента.",
         2, 25)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_016() {
            var ft = ScsGcCoverType.BEND;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .Select(p => new { p.Code, p.CoverWidth });
                var errors = new LinkedList<(string Code, double?)>();
                foreach (var p in products) {
                    if (p.CoverWidth > 1)
                        continue;
                    else
                        errors.AddLast((p.Code, p.CoverWidth));
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка угловая горизонтальная\" должно быть не более одного сочетания параметров [ширина элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышка угловая горизонтальная\" должно быть не более одного сочетания параметров [ширина элемента CoverWidth].",
         2, 26)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_038() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<double?> sizes) {
            //    HashSet<double?> values = new HashSet<double?>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGcCoverType.BEND;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name, p.CoverWidth));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<double?> values = new();
                    var errorValues = p.Where(item => !values.Add(item.CoverWidth));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        public const string sRule017 = @"Для элементов ""Крышка Х-образная горизонтальная"" должны быть внесены ширина основного и отходящего элемента (CoverWidth1, CoverWidth2).";
        [ReportRule(@"Для элементов ""Крышка Х-образная горизонтальная"" должны быть внесены ширина основного и отходящего элемента.",
         2, 27)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_017() {
            var ft = ScsGcCoverType.CROSS;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.CoverWidth1, p.CoverWidth2 });
                //var errors = new LinkedList<(string Code, double?, double?)>();
                //foreach (var p in products) {
                //    if (p.CoverWidth1 > 1 & p.CoverWidth2 > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.CoverWidth1, p.CoverWidth2));
                //}
                var errors = products.Where(p => !(p.CoverWidth1 > 1 & p.CoverWidth2 > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.CoverWidth1)}:{p.CoverWidth1}, {nameof(p.CoverWidth2)}:{p.CoverWidth2}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка Х-образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышка Х - образная горизонтальная\" должно быть не более одного сочетания параметров [ширина основного и отходящего элемента (CoverWidth1, CoverWidth2)].",
         2, 28)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_039() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? CoverWidth1, double? CoverWidth2)> sizes) {
            //    HashSet<(double? CoverWidth1, double? CoverWidth2)> values = new HashSet<(double? CoverWidth1, double? CoverWidth2)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGcCoverType.CROSS;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth1, p.CoverWidth2 })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name, size: (p.CoverWidth1, p.CoverWidth2)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? CoverWidth1, double? CoverWidth2)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        public const string sRule018 = @"Для элементов ""Крышка угловая вертикальная внутренняя"" должна быть внесена ширина элемента.";
        [ReportRule(@"Для элементов ""Крышка угловая вертикальная внутренняя"" должна быть внесена ширина элемента.",
         2, 29)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_018() {
            var ft = ScsGcCoverType.VERTICAL_BEND_INNER;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                    .Where(p => p.CoverType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.CoverWidth });
                //var errors = new LinkedList<(string Code, double?)>();
                //foreach (var p in products) {
                //    if (p.CoverWidth > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.CoverWidth));
                //}
                var errors = products.Where(p => !(p.CoverWidth > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.CoverWidth)}:{p.CoverWidth}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов \"Крышка угловая вертикальная внутренняя\" должно быть не более одного сочетания параметров [ширина элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов \"Крышка угловая вертикальная внутренняя\" должно быть не более одного сочетания параметров [ширина элемента CoverWidth].",
         2, 30)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGcCoverUnit))]
        public void Rule_02_040() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<double?> sizes) {
            //    HashSet<double?> values = new HashSet<double?>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};


            var ft = ScsGcCoverType.VERTICAL_BEND_INNER;
            using (var context = connector.Connect()) {
                var products = context.DbScsGcCoverUnits
                        .Where(p => p.CoverType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.CoverWidth })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name, p.CoverWidth));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<double?> values = new();
                    var errorValues = p.Where(item => !values.Add(item.CoverWidth));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        //public const string sRule020 = @"Для прямых секций лотков должны быть внесены полезная ширина, высота лотка";
        [ReportRule(@"Для прямых секций лотков должны быть внесены полезная ширина GutterUsefullHeight, высота GutterUsefullDepth лотка.",
         2, 31)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_02_019() {
            using (var context = connector.Connect()) {
                var products = context.ScsGutterCanals
                    .ToList();
                //    .Select(p => new { p.Code, p.GutterUsefullHeight, p.GutterUsefullDepth });
                //var errors = new LinkedList<(string Code, double? GutterUsefullHeight, double? GutterUsefullDepth)>();
                //foreach (var p in scsGutterCanals) {
                //    if (p.GutterUsefullHeight > 1 & p.GutterUsefullDepth > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.GutterUsefullHeight, p.GutterUsefullDepth));
                //}
                var errors = products.Where(p => !(p.GutterUsefullHeight > 1 & p.GutterUsefullDepth > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.GutterUsefullHeight)}:{p.GutterUsefullHeight}, {nameof(p.GutterUsefullDepth)}:{p.GutterUsefullDepth}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        public const string sRule021 = @"Для прямых секций лотков должен быть внесен график допустимой нагрузки.";
        [ReportRule(@"Для прямых секций лотков должен быть внесен график допустимой нагрузки.",
         2, 32)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_02_021() {
            using (var context = connector.Connect()) {
                var scsGutterCanals = context.ScsGutterCanals
                    .Select(p => new { p.Series, p.Code, p.GraphLoadingPrp });
                var errors = new LinkedList<(string, string)>();
                foreach (var p in scsGutterCanals) {
                    if (string.IsNullOrEmpty(p.GraphLoadingPrp))
                        errors.AddLast((p.Series, p.Code));
                    else
                        continue;
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Для всех лотков должен быть указан тип лотка и серия
        /// </summary>
        /// <remarks>Учитываются свойства GutterType, Series</remarks>
        [ReportRule(@"Для всех лотков должен быть указан тип лотка GutterType и серия Series.",
        2, 33)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_02_022() {
            Dictionary<(int, string), (double, double, double)> knownSeries = new Dictionary<(int, string), (double, double, double)>();

            string? CheckRule(string code, string name, int? GutterType, string? Series) {

                bool isMatch = !(GutterType.HasValue & !string.IsNullOrWhiteSpace(Series));
                return isMatch ? code : null;
            };

            using (var context = connector.Connect()) {
                var errors = new LinkedList<string>();
                foreach (var p in context.ScsGutterCanals.Select(p => new { p.Code, p.Name, p.GutterType, p.Series })) {
                    var key = CheckRule(p.Code, p.Name, (int)p.GutterType, p.Series);
                    if (key != null) {
                        errors.AddLast(key);
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии лотка должно быть не более одного сочетания параметров [Высота борта]x[ширина]x[длина секции]
        /// </summary>
        /// <remarks>Учитываются свойства GutterType, Series, GutterDepth, GutterHeight, SegLength</remarks>
        [ReportRule(@"Внутри серии лотка должно быть не более одного сочетания параметров [Высота борта GutterHeight]x[ширина GutterDepth]x[длина секции SegLength].",
          2, 34)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterCanal))]
        public void Rule_02_023() {

            //(int GutterType, string Series)? CheckRule((int GutterType, string Series) key, IEnumerable<(double? GutterDepth, double? GutterHeight, double? SegLength)> sizes) {
            //    HashSet<(double? GutterDepth, double? GutterHeight, double? SegLength)> values = new();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};
            using (var context = connector.Connect()) {
               var products = context.ScsGutterCanals
                    .Select(p => new { p.GutterType, p.Series, p.Name, p.GutterDepth, p.GutterHeight, p.SegLength })
                    .ToArray()
                    .GroupBy(p => (p.GutterType.GetDescription(), p.Series), p =>(p.Name, size: (p.GutterDepth, p.GutterHeight, p.SegLength)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? GutterDepth, double? GutterHeight, double? SegLength)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }

            }
        }

        /// <summary>
        /// Внутри серии разделительных перегородок должно быть не более одного сочетания параметров [Высота]x[длина секции]
        /// </summary>
        /// <remarks>Учитываются свойства Series, PartitionHeight, PartitionLength</remarks>
        [ReportRule(@"Внутри серии разделительных перегородок должно быть не более одного сочетания параметров [Высота PartitionHeight]x[длина секции PartitionLength].",
         2, 35)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGutterPartition))]
        public void Rule_02_026() {
            //string? CheckRule(string key, IEnumerable<(double? PartitionHeight, double? PartitionLength)> sizes) {
            //    HashSet<(double? PartitionHeight, double? PartitionLength)> values = new();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};

            using (var context = connector.Connect()) {
                var products = context.DbScsGutterPartitions
                    .Select(p => new { p.Series,p.Name, p.PartitionHeight, p.PartitionLength })
                    .ToArray()
                    .GroupBy(p => p.Series, p =>(p.Name, size: (p.PartitionHeight, p.PartitionLength)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double? PartitionHeight, double? PartitionLength)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }

            }
        }
        /// <summary>
        /// Внутри серии крышек прямых секций лотков должно быть не более одного сочетания параметров [Ширина]x[длина секции]
        /// </summary>
        /// <remarks>Учитываются свойства Series, CoverWidth, CoverLength</remarks>
        [ReportRule(@"Внутри серии крышек прямых секций лотков должно быть не более одного сочетания параметров [Ширина CoverWidth]x[длина секции CoverLength].",
          2, 36)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(DbScsGutterCover))]
        public void Rule_02_027() {

            //string? CheckRule(string key, IEnumerable<(double? CoverWidth, double? CoverLength)> sizes) {
            //    HashSet<(double?, double?)> values = new();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};

            using (var context = connector.Connect()) {
                var products = context.DbScsGutterCovers
                    .Select(p => new { p.Series,p.Name, p.CoverWidth, p.CoverLength })
                    .ToArray()
                    .GroupBy(p => p.Series, p =>(p.Name, size: (p.CoverWidth, p.CoverLength)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double?, double?)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }
        /// <summary>
        /// Для элементов  \"Торцевая заглушка\" должны быть внесены параметры [высота борта, ширина элемента]
        /// </summary>
        [ReportRule("Для элементов  \"Торцевая заглушка\" должны быть внесены параметры [высота борта Height, ширина элемента Depth].",
         2, 37)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_041() {
            bool CheckRule(double? Height, double? Depth) {

                bool isAllRight = Height.HasValue && Height.Value >= 1.0
                                    && Depth.HasValue && Depth.Value >= 1.0;
                return isAllRight;

            };
            var ft = ScsGutterFittingTypeEnum.CORK;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft).ToArray()
                    .ToList();
                //        .Select(p => new { p.Code, p.Series, p.FittingType, p.Height, p.Depth })
                //        .ToArray()
                //        ;
                //var errors = new LinkedList<(string Code, string Series, double? Height, double? Depth)>();
                //foreach (var p in grours) {
                //    if (!CheckRule(p.Code, p.Series, p.Height, p.Depth)) {
                //        errors.AddLast((p.Code, p.Series, p.Height, p.Depth));
                //    }
                //}
                var errors = products.Where(p => !CheckRule(p.Height, p.Depth))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Height)}:{p.Height}, {nameof(p.Depth)}:{p.Depth}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        /// <summary>
        /// Внутри серии элементов  \"Торцевая заглушка\" должно быть не более одного сочетания параметров [высота борта, ширина элемента]
        /// </summary>
        [ReportRule("Внутри серии элементов  \"Торцевая заглушка\" должно быть не более одного сочетания параметров [высота борта Height, ширина элемента Depth].",
         2, 38)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGcFitting))]
        public void Rule_02_042() {
            //(int type, string serie)? CheckRule((int type, string serie) key, IEnumerable<(double? Height, double? Depth)> sizes) {
            //    HashSet<(double? Height, double? Depth)> values = new HashSet<(double? Height, double? Depth)>();
            //    bool isHaveDuplicateSisez = false;
            //    foreach (var size in sizes) {
            //        bool isMatch = values.Contains(size);
            //        if (isMatch) {
            //            isHaveDuplicateSisez = true;
            //            break;
            //        }
            //        else {
            //            values.Add(size);
            //        }
            //    }
            //    return (isHaveDuplicateSisez) ? key : null;
            //};
            var ft = ScsGutterFittingTypeEnum.CORK;
            using (var context = connector.Connect()) {
                var products = context.ScsGcFittings
                        .Where(p => p.FittingType == ft)
                        .Select(p => new { p.Code, p.Series, p.Name, p.Height, p.Depth })
                        .ToArray()
                        .GroupBy(p => p.Series, p =>(p.Name,size: (p.Height, p.Depth)));
                LinkedList<object> errors = new();
                foreach (var p in products) {
                    HashSet<(double?, double?)> values = new();
                    var errorValues = p.Where(item => !values.Add(item.size));
                    foreach (var error in errorValues) {
                        errors.AddLast((p.Key, error));
                    }
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} серий.",
                        errors);
                }
            }
        }

        [ReportRule(@"Для элементов ""Консоль"" должна быть внесена полезная ширина элемента Length.",
         2, 39)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_02_050() {

            var ft = ScsGutterBoltingTypeEnum.CONSOLE;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.Length });
                //var errors = new LinkedList<(string Code, double?)>();
                //foreach (var p in products) {
                //    if (p.Length > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.Length));
                //}
                var errors = products.Where(p => !(p.Length > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Length)}:{p.Length}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Профиль"" должна быть внесена длина элемента ProfileLength.",
         2, 40)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_02_051() {
            var ft = ScsGutterBoltingTypeEnum.PROFILE;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.ProfileLength });
                //var errors = new LinkedList<(string Code, double?)>();
                //foreach (var p in products) {
                //    if (p.ProfileLength > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.ProfileLength));
                //}
                var errors = products.Where(p => !(p.ProfileLength > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.ProfileLength)}:{p.ProfileLength}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Скоба"" должна быть внесена полезная ширина элемента Length.",
         2, 41)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_02_052() {
            var ft = ScsGutterBoltingTypeEnum.CRAMP;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.Length });
                //var errors = new LinkedList<(string Code, double?)>();
                //foreach (var p in products) {
                //    if (p.Length > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.Length));
                //}
                var errors = products.Where(p => !(p.Length > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Length)}:{p.Length}")
                    .ToList();
                if (errors.Count > 0) {
                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Стойка"" должны быть внесены крепление MountType, вид стойки StandType, высота элемента Heigth.",
           2, 42)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_02_053() {
            var ft = ScsGutterBoltingTypeEnum.POST;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .Select(p => new { p.Code, p.Series,p.Name, p.MountType, p.StandType, p.Heigth });
                var errors = new LinkedList<string>();
                foreach (var p in products) {
                    if (p.Heigth > 1) {
                        if (p.MountType.HasValue) {
                            var knownEnumValues = typeof(ScsGcStandMountType).GetEnumValues() as ScsGcStandMountType[];
                            if (knownEnumValues.Contains(p.MountType.Value)) {
                                if (p.StandType.HasValue) {
                                    var knownStandTypeEnumValues = typeof(ScsGcStandType).GetEnumValues() as ScsGcStandType[];
                                    if (knownStandTypeEnumValues.Contains(p.StandType.Value)) {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    errors.AddLast($"({p.Series}\\{p.Name}  {nameof(p.MountType)}:{p.MountType?.GetDescription()}, {nameof(p.StandType)}:{p.StandType?.GetDescription()}, {nameof(p.Heigth)}:{p.Heigth}");
                }
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Шпилька"" должна быть внесена высота элемента Heigth.",
         2, 43)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_02_054() {
            var ft = ScsGutterBoltingTypeEnum.STUD;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.Heigth });
                //var errors = new LinkedList<(string Code, double?)>();
                //foreach (var p in products) {
                //    if (p.Heigth > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.Heigth));
                //}
                var errors = products.Where(p => !(p.Heigth > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Heigth)}:{p.Heigth}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""L- подвес или C- подвес"" должны быть внесена высота элемента.",
         2, 44)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_02_055() {
            var ft = ScsGutterBoltingTypeEnum.CONSOLE;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft & (p.ConsoleMountType == ScsGcConsoleMountType.CELL | p.ConsoleMountType == ScsGcConsoleMountType.L_WALL))
                    .ToList();
                //    .Select(p => new { p.Code, p.Heigth });
                //var errors = new LinkedList<(string Code, double?)>();
                //foreach (var p in products) {
                //    if (p.Heigth > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.Heigth));
                //}
                var errors = products.Where(p => !(p.Heigth > 1))
                    .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Heigth)}:{p.Heigth}")
                    .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Перекладина"" должна быть внесена полезная ширина элемента.",
         2, 45)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsGutterBolting))]
        public void Rule_02_056() {
            var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsGutterBoltings
                    .Where(p => p.CanalBoltingType == ft)
                    .ToList();
                //    .Select(p => new { p.Code, p.Length });
                //var errors = new LinkedList<(string Code, double?)>();
                //foreach (var p in products) {
                //    if (p.Length > 1)
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.Length));
                //}
                var errors = products.Where(p => !(p.Length > 1))
                   .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.Length)}:{p.Length}")
                   .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }
        [ReportRule(@"Для элементов ""Трубы. Соединительные элементы"" с типом элемента ""Другой"" должно быть внесено значение DbOtherName (Тип элемента).",
         2, 46)]
        [RuleCategory("Полнота заполнения технических данных.", nameof(ScsTubeFitting))]
        public void Rule_02_057() {
            //var ft = ScsGutterBoltingTypeEnum.CROSSBAR;
            using (var context = connector.Connect()) {
                var products = context.ScsTubeFittings
                    .Where(p => p.FittingType == ScsTubeFittingTypeEnum.OTHER)
                    .ToList();
                //    .Select(p => new { p.Code, p.DbOtherName });
                //var errors = new LinkedList<(string Code, string?)>();
                //foreach (var p in products) {
                //    if (!string.IsNullOrEmpty(p.DbOtherName))
                //        continue;
                //    else
                //        errors.AddLast((p.Code, p.DbOtherName));
                //}
                var errors = products.Where(p => !string.IsNullOrEmpty(p.DbOtherName))
                   .Select(p => $"({p.Series}\\{p.Code}  {nameof(p.DbOtherName)}:\"{p.DbOtherName}\"")
                   .ToList();
                if (errors.Count > 0) {

                    FailRuleTest($"Тест не пройден для {errors.Count} элементов.",
                       errors);
                }
            }
        }

        // TODO: Реализовать правила проверки значений Enum полей на допустимость.
        // TODO: Для элементов "Приборы контроля и учета. Приборы управления" в свойстве "Монтаж" допускаются только значения "Внутрь шкафа", "На фасад"
        // 

        /*  // Выполнить проверку артикулов и наименований на соответствие правилам.
         public const string sRule041 = @"В артикулах только английские буквы";
         /// <summary>
         /// В артикулах только английские буквы
         /// </summary>
         [ReportRule, RuleCategory("Check Rules")]
         [RuleCategory("Check IEK rules")]
         public void Rule_04_001() {
             int count = 0;
             WriteLineStartRuleCheck($"\t{sRule041}");
             void CheckRule(string code) {
                 bool isMatch = Regex.IsMatch(code, @"^[A-Za-z0-9\-]+$");
                 if (isMatch) { return; }
                 else {
                     if (count == 0) {
                         logger.LogInformation($":\nНе проходят проверку следующие артикулы:");
                     }
                     logger.LogInformation($"{code}");
                     count++;
                 }
             };

             using (var context = connector.Connect()) {
                 foreach (var code in context.ScsGutterCanals.Select(p => p.Code)) {
                     CheckRule(code);
                 }
                 foreach (var code in context.DbScsGutterCovers.Select(p => p.Code)) {
                     CheckRule(code);
                 }
                 foreach (var code in context.DbScsGutterPartitions.Select(p => p.Code)) {
                     CheckRule(code);
                 }
                 foreach (var code in context.ScsGcFittings.Select(p => p.Code)) {
                     CheckRule(code);
                 }
                 foreach (var code in context.DbScsGcCoverUnits.Select(p => p.Code)) {
                     CheckRule(code);
                 }
                 foreach (var code in context.DbScsGcAccessoryUnits.Select(p => p.Code)) {
                     CheckRule(code);
                 }
                 foreach (var code in context.ScsGutterBoltings.Select(p => p.Code)) {
                     CheckRule(code);
                 }
                 foreach (var code in context.DbScsGcBoltingAccessoryUnits.Select(p => p.Code)) {
                     CheckRule(code);
                 }
                 foreach (var code in context.CaeMaterialUtilities.Select(p => p.Code)) {
                     CheckRule(code);
                 }
             }
             AssertCountIsZero(count);
         }
         /// <summary>
         /// "х" в габаритах в наименованиях должна быть всегда на русском
         /// </summary>
         [ReportRule, RuleCategory("Check Rules")]
         [RuleCategory("Check IEK rules")]
         public void Rule_04_002() { // "х" в габаритах в наименованиях должна быть всегда на русском
             WriteLineStartRuleCheck("\"х\" в габаритах в наименованиях должна быть всегда на русском");
             int count = 0;
             void CheckRule(string code, string name) {
                 bool isMatch = Regex.IsMatch(name, @"\dx\d");
                 if (isMatch) {
                     if (count == 0) {
                         logger.LogInformation($":\nНе проходят проверку следующие наименования:");
                     }
                     logger.LogInformation($"{code}: \"{name}\"");
                     count++;
                 }
             };

             using (var context = connector.Connect()) {
                 foreach (var item in context.ScsGutterCanals.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGutterCovers.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGutterPartitions.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.ScsGcFittings.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGcCoverUnits.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGcAccessoryUnits.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.ScsGutterBoltings.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGcBoltingAccessoryUnits.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.CaeMaterialUtilities.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
             }
             AssertCountIsZero(count);
         }
         /// <summary>
         /// "М" в наименованиях метизов должна быть всегда на русском
         /// </summary>
         [ReportRule, RuleCategory("Check Rules")]
         [RuleCategory("Check IEK rules")]
         public void Rule_04_003() { //  "М" в метизах тоже - т.е. М8
             WriteLineStartRuleCheck("\"М\" в наименованиях метизов должна быть всегда на русском");
             int count = 0;
             void CheckRule(string code, string name) {
                 bool isMatch = Regex.IsMatch(name, @"M\d+[xх ]");
                 if (isMatch) {
                     if (count == 0) {
                         logger.LogInformation($":\nНе проходят проверку следующие наименования:");
                     }
                     logger.LogInformation($"{code}: \"{name}\"");
                     count++;
                 }
             };

             using (var context = connector.Connect()) {
                 foreach (var item in context.CaeMaterialUtilities.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
             }
             AssertCountIsZero(count);
         }
         /// <summary>
         /// В наименованиях отдельные слова состоят целиком из букв кириллицы, либо целиком из букв латиницы.
         /// </summary>
         [ReportRule, RuleCategory("Check Rules")]
         [RuleCategory("Check IEK rules")]
         public void Rule_04_004() { //  Тип изделий чаще всего на английском - например NKU для консолей
             Console.Write($"{GetIndex(nameof(Rule_04_004))}\tВ наименованиях отдельные слова состоят целиком из букв кириллицы, либо целиком из букв латиницы.");
             int count = 0;
             void CheckRule(string code, string name) {
                 string[] words = SplitToWords(name);
                 bool isMatch = words.All(word => IsEnglishWord(word) | IsRussianWord(word) | IsRepresentNumber(word));
                 if (!isMatch) {
                     if (count == 0) {
                         logger.LogInformation($":\nНе проходят проверку следующие наименования:");
                     }
                     Console.Write($"{code}: \"{name}\". ");
                     foreach (var word in words) {
                         if (IsEnglishWord(word) | IsRussianWord(word) | IsRepresentNumber(word))
                             continue;
                         Console.Write($"[{word}]");
                     }
                     logger.LogInformation();
                     count++;
                 }
             };

             using (var context = connector.Connect()) {
                 foreach (var item in context.ScsGutterCanals.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGutterCovers.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGutterPartitions.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.ScsGcFittings.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGcCoverUnits.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGcAccessoryUnits.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.ScsGutterBoltings.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.DbScsGcBoltingAccessoryUnits.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
                 foreach (var item in context.CaeMaterialUtilities.Select(p => new { p.Code, p.Name })) {
                     CheckRule(item.Code, item.Name);
                 }
             }
             Assert.IsTrue(count == 0, $"Не соответствуют правилу {GetIndex(nameof(Rule_04_004))}: {count} артикулов.");
             logger.LogInformation(": Соответствует.");
         }

         [ReportRule]
         public void GetGutterSeriesPattern() {


             using (var context = connector.Connect()) {
                 var series = context.ScsGutterCanals
                     .Select(sg => new { sg.Code, sg.Series, sg.GutterType })
                     .ToArray();
                 var seriesGr = series
                     .GroupBy(sg => (sg.GutterType, sg.Series))
                     .Select(gr => new {
                         Serie = $"{((ScsGutterType)gr.Key.GutterType!).GetDescription()}\\{gr.Key.Series}",
                         Count = gr.Count(),
                         Pattern = gr.Aggregate(Array.Empty<string>(), (acc, item) => {
                             if (string.IsNullOrEmpty(item.Code))
                                 return acc;
                             var itemPattern = item.Code.Split('-');
                             if (itemPattern.Length > acc.Length) {
                                 (itemPattern, acc) = (acc, itemPattern);
                             }
                             { // (itemPattern.Length <= acc.Length) 
                                 int i = 0;
                                 for (; i < itemPattern.Length; i++) {
                                     if (acc[i] != itemPattern[i]) {
                                         acc[i] = new string('X', Math.Max(acc[i].Length, itemPattern[i].Length));
                                     }
                                 }
                             }
                             return acc;
                         })
                     });
                 Dictionary<string, string> patterns = new Dictionary<string, string>();
                 logger.LogInformation($"Serie\tSerieCodePattern\tCount");
                 foreach (var item in seriesGr) {
                     StringBuilder pattern = new StringBuilder();
                     foreach (var p in item.Pattern) {
                         if (pattern.Length == 0)
                             pattern.Append(p);
                         else {
                             pattern.Append("-");
                             pattern.Append(p);
                         }
                     }
                     string serieCodePattern = pattern.ToString();
                     patterns.Add(item.Serie, serieCodePattern);
                     logger.LogInformation($"{item.Serie}\t{serieCodePattern}\t{item.Count}");
                 }
                 logger.LogInformation();
                 logger.LogInformation($"Code\tSerie\tSerieCodePattern");
                 foreach (var item in series) {
                     logger.LogInformation($"{item.Code}\t{item.Series}\t{patterns[$"{((ScsGutterType)item.GutterType!).GetDescription()}\\{item.Series}"]}");
                 }
             }
         }

         private static void AssertCountIsZero(int count, [CallerMemberName] string? caller = null) {
             Assert.IsTrue(count == 0, $": Не соответствуют правилу {GetIndex(caller)}: {count} артикулов.");
             logger.LogInformation(": Соответствует.");
         }
         */
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
