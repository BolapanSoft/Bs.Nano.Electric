using Bs.XML.SpreadSheet;
using Microsoft.Extensions.Logging;
using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bs.Nano.Electric.Model;
using System.Data.Entity;
using System.Data.SqlServerCe;
using System.ComponentModel;

namespace Bs.Nano.Electric.Report {
    /// <summary>
    /// Реализует правила контроля полноты заполнения БДИ.
    /// </summary>
    public partial class Checker {
        IdCounter counter = new IdCounter();
        protected readonly ILogger logger;
        protected readonly INanocadDBConnector connector;


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
        [ReportRule(@"Известные таблицы БДИ.", 0, 0), RuleCategory("Краткий отчет по базе изделий.", "AllTables")]
        public void TotalKnownTablesCount() {
            using (Context context = connector.Connect()) {
                var tables = context.GetKnownTables();
                foreach (var tableProperty in tables) {
                    (object property, string tableDescription, _) = tableProperty;
                    IQueryable dbSet = (IQueryable)property;
                    int count = 0;
                    try {
                        count = RuleTestHelper.GetCount(dbSet);
                        if (count > 0) {
                            logger.LogInformation($"Таблица \"{tableDescription}\": {count} элементов.");
                        }
                    }
                    catch (Exception ex) {
                        count = 0;
                    }

                }
            }
        }

        [ReportRule(@"Отчет по таблицам БДИ.", 0, 0), RuleCategory("Отчет по базе изделий.")]
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
                            var enumValue = (DbGcMsAccessoryType)value;
                            return enumValue.GetDescription();
                        }
                        var group = Resources.ImageCategory["ScsGutterCanal"];
                        var groupsArr = context.DbScsGcAccessoryUnits
                            .Select(p => new { p.AccessoryType, p.Series })
                            .ToArray();
                        var groups = groupsArr
                            .Select(p => new {
                                GutterType = GutterType((int)p.AccessoryType),
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
        [ReportRule(@"В базе значения артикула должны быть уникальными.", 0, 1), RuleCategory("Общие рекомендации.", "AllTables")]
        public void Rule_00_001() {
            //var allCodes = LoadAllCodes();
            List<(string Code, string ArticleName, string TableName, string TypeDescription)> allCodes = new();
            using (Context context = connector.Connect()) {
                var tables = context.GetKnownTables();
                foreach (var tableDef in tables) {
                    var tableCodes = LoadCodes(context, (IQueryable)tableDef.property, Context.GetDatabaseTableName(tableDef.EntityType), tableDef.EntityType);
                    allCodes.AddRange(tableCodes);
                }
            }
            CheckCodesIsUniqueness(allCodes);
        }


        /// <summary>
        /// Осуществляет проверку уникальности поля <paramref name="Code"/> во входных данных.
        /// </summary>
        /// <param name="allCodes">Массив входных данных.</param>
        /// <exception cref="RuleTestException">Правило уникальности кода нарушено.</exception>
        public static void CheckCodesIsUniqueness(IEnumerable<(string Code, string ArticleName, string TableName, string TableDescription)> allCodes) {
            var errors = allCodes.GroupBy(item => item.Code)
                                .Where(group => group.Count() > 1);
            if (errors.Any()) {
                var message = $"Имеются дублирующиеся артикулы.";
                FailRuleTest(message, errors.SelectMany(group => group));
            }
        }

        [ReportRule(@"В таблице DbImages должен быть внесен файл ""File not found.png"" под индексом id==0",
            0, 2), RuleCategory("Общие рекомендации.", "AllTables")]
        public void Rule_00_002() {
            // logger.LogInformation($"{GetIndex(nameof(Rule_01_001))}\t{sRule001}");

            using (var context = connector.Connect()) {
                var emptyImage = context.DbImages.FirstOrDefault(img => img.Id == 0);
                if (emptyImage is null || emptyImage.Text != "File not found.png") {
                    throw new RuleTestException($"Не внесен эскиз \"File not found.png\" или Id эскиза не равно 0.");
                }
            }
        }
        [ReportRule(@"Для всех элементов должен быть внесен эскиз.",
            0, 3), RuleCategory("Общие рекомендации.", "AllTables")]
        public void Rule_00_003() {
            Queue<(string TableName, string Code, string ArticleName )> errors = new();
            using (Context context = connector.Connect()) {
                IEnumerable<(object property, string tableDescription, Type EntityType)> tables = context.GetKnownTables();
                foreach (var tableDef in tables) {
                    (object property, string tableDescription, Type entityType) = tableDef;
                    var dbSet = (IQueryable)property;
                    if (typeof(IHaveImageRef).IsAssignableFrom(entityType) & typeof(IProduct).IsAssignableFrom(entityType)) {
                        foreach (var entity in dbSet.AsNoTracking()) {
                            IHaveImageRef product = (IHaveImageRef)entity;
                            if (!(
                                ((IHaveImageRef)entity).DbImageRef.HasValue &&
                                ((IHaveImageRef)entity).DbImageRef > 0
                                )) {
                                errors.Enqueue((tableDescription,  ((IProduct)entity).Code, ((IProduct)entity).Name));
                            }
                        }
                    }
                }
            }
            if (errors.Count > 0) {
                var message = $"Эскиз не внесен для {errors.Count} элементов.";
                FailRuleTest(message, errors);
            }
            //using (var context = connector.Connect()) {
            //    List<(string Code, string Description)> sketchNotFound = new List<(string Code, string Description)>();

            //    var products = context.ScsGutterCanals
            //        .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }
            //    products = context.DbScsGutterCovers
            //        .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }
            //    products = context.DbScsGutterPartitions
            //        .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }
            //    products = context.ScsGcFittings
            //        .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }
            //    products = context.DbScsGcCoverUnits
            //        .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }
            //    products = context.DbScsGcAccessoryUnits
            //        .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }
            //    products = context.ScsGutterBoltings
            //        .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }
            //    products = context.DbScsGcBoltingAccessoryUnits
            //        .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }
            //    products = context.CaeMaterialUtilities
            //         .Where(p => p.DbImageRef < 1)
            //        .Select(p => new { p.Code, p.SpecDescription });
            //    foreach (var p in products) {
            //        sketchNotFound.Add((p.Code, p.SpecDescription));
            //    }

            //    if (sketchNotFound.Count > 0) {
            //        //StringBuilder sb = new StringBuilder();
            //        //sb.AppendLine($"Не внесены эскизы для {sketchNotFound.Count} элементов:");
            //        //sb.AppendLine("Code\tSpecDescription");
            //        //foreach (var item in sketchNotFound.Take(50)) {
            //        //    sb.AppendLine($"{item.Code}\t{item.Description}");
            //        //}
            //        //if (sketchNotFound.Count > 100) {
            //        //    sb.AppendLine($"... обрезано {sketchNotFound.Count - 50} элементов.");
            //        //}
            //        var message = $"Не внесены эскизы для {sketchNotFound.Count} элементов";
            //        FailRuleTest(message, sketchNotFound);
            //    }
        }

        [ReportRule(@"Для всех элементов должна быть внесена масса.",
            0, 4), RuleCategory("Общие рекомендации.", "AllTables")]
        public void Rule_00_004() {
            Queue<(string tableDescription, string Code, string message)> errors = new();
            using (Context context = connector.Connect()) {
                IEnumerable<(object property, string tabltableDescriptioneName, Type EntityType)> tables = context.GetKnownTables();
                foreach (var tableDef in tables) {
                    (object property, string tableDescription, Type entityType) = tableDef;
                    var typeDescription = Context.GetDefaultLocalizeValue(entityType);
                    (bool isHaveField, IEnumerable<Material> values) = GetMaterialValues(connector, Context.GetDatabaseTableName(entityType));
                    if (isHaveField) {
                        foreach (var row in values) {
                            string mass = row.Mass;
                            if (string.IsNullOrEmpty(mass) ) {
                                errors.Enqueue((tableDescription, row.Code, mass));
                            }
                            if(! double.TryParse(mass, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("Ru-ru"), out _)) {
                                errors.Enqueue((tableDescription,  row.Code, $"Значение \"{mass}\" не соответствует шаблону 0,##"));

                            }
                        }
                    }
                }
            }
            if (errors.Count > 0) {
                var message = $"Масса не внесена или внесена некорректно для {errors.Count} элементов.";
                FailRuleTest(message, errors);
            }

            //List<(string Serie, string Code, string Mass)> codes = new List<(string Serie, string Code, string Mass)>(128);
            //void CheckRule(string serie, string code, string sWeight) {
            //    bool isMatch = (double.TryParse(sWeight, NumberStyles.Float, CultureInfo.InvariantCulture, out double weight) || double.TryParse(sWeight, NumberStyles.Float, CultureInfo.CurrentCulture, out weight)) && weight > 0.0;
            //    if (!isMatch) {
            //        codes.Add((serie, code, sWeight));
            //    }
            //}
            //using (var context = connector.Connect()) {
            //    foreach (var el in context.ScsGutterCanals) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }
            //    foreach (var el in context.DbScsGutterCovers) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }
            //    foreach (var el in context.DbScsGutterPartitions) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }
            //    foreach (var el in context.ScsGcFittings) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }
            //    foreach (var el in context.DbScsGcCoverUnits) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }
            //    foreach (var el in context.DbScsGcAccessoryUnits) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }
            //    foreach (var el in context.ScsGutterBoltings) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }
            //    foreach (var el in context.DbScsGcBoltingAccessoryUnits) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }
            //    foreach (var el in context.CaeMaterialUtilities) {
            //        CheckRule(el.Series, el.Code, el.Mass);
            //    }

            //}
            ////count = errors.Count;
            //if (codes.Count > 0) {
            //    FailRuleTest($"Не внесена масса для {codes.Count} артикулов.", codes);
            //}
        }
        [ReportRule(@"Для всех элементов должна быть внесена ссылка на сайт производителя.",
            0, 5), RuleCategory("Общие рекомендации.")]
        public void Rule_00_005() {
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
        [ReportRule(@"Для всех элементов должен быть внесен артикул.",
            0, 6), RuleCategory("Общие рекомендации.")]
        public void Rule_00_006() {
            var allCodes = LoadAllCodes();
            var errors = allCodes.Where(item => string.IsNullOrEmpty(item.Code));
            if (errors.Any()) {
                var message = $"Имеются не заполненные артикулы.";
                FailRuleTest(message, errors);
            }
        }

    }
}
