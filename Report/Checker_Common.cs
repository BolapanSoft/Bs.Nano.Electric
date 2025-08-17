using Bs.Nano.Electric.Model;
using Bs.XML.SpreadSheet;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
#if NETFRAMEWORK
using System.Data.Entity;
using System.Data.SqlServerCe; 
#else
using Microsoft.EntityFrameworkCore;
#endif
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Bs.Nano.Electric.Report {
    public class NtProduct : IProduct {
        public string Code { get; set; }

        public int? DbImageRef { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public int Id { get; set; }
        public string SpecDescription { get; set; }
    }
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
        public void TestDatabase(string[] categories, string[] tables, bool isTruncateReport) {


            foreach (var category in categories) {
                try {
                    var tests = GetTests(category, tables).ToList();
                    if (tests.Count > 0) {
                        logger.LogInformation($"\nПроверка {tests.Count} правил категории \"{category}\".");
                        var checker = new Checker(logger, connector);
                        int count = 0;
                        foreach (MethodInfo test in tests) {
                            ReportRuleAttribute rule = test.GetReportRule();
                            try {
                                var miParameters = test.GetParameters();
                                if (miParameters.Length == 0) {
                                    test.Invoke(checker, null);
                                    logger.LogInformation($"Проверка правила {rule} завершена без ошибок.");
                                    count++;
                                }
                                else
                                    throw new NotImplementedException("Допускается вызов только методов без параметров.");
                            }
                            catch (AggregateException ex) {
                                logger.LogWarning($"Не пройдено правило проверки {rule}", ex.InnerException);
                                ReportErrors(logger, ex, isTruncateReport);
                            }
                            catch (TargetInvocationException tiEx) {
                                logger.LogWarning($"Не пройдено правило проверки {rule}");
                                ReportErrors(logger, tiEx.InnerException, isTruncateReport);

                            }
                            catch (Exception ex) {
                                logger.LogWarning($"Не пройдено правило проверки {rule}");
                                ReportErrors(logger, ex, isTruncateReport);
                            }
                        }
                        logger.LogInformation($"Завершена проверка правил категории \"{category}\".\n————————————————————————————————————————————————————————————————————————");
                    }
                    //foreach (System.Reflection.MethodInfo testItem in tests) {
                    //    ReportRuleAttribute rule = testItem.GetReportRule();
                    //    logger.LogInformation($"  {rule.ListIndex} \"{(string.IsNullOrEmpty(rule.DisplayName)?testItem.Name:rule.DisplayName)}\"");
                    //}

                }
                catch (Exception ex) {
                    logger.LogError($"При проверке правил из категории\"{category}\" возникла ошибка.");
                    ReportErrors(logger, ex, isTruncateReport);
                }
            }
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
                    }
                    ;
                    ;
                }
            }
        }
        //[ReportRule(@"Заполнение таблиц БДИ.", 0, 1), RuleCategory("Краткий отчет по базе изделий.")]
        public void TotalTablesCount() {
            using (Context context = connector.Connect()) {
                int allProductsCount = 0;
                {
#if NETFRAMEWORK
                    logger.LogInformation($"Отчет по БДИ \"{context.Database.Connection.Database}\"");
#else
                    logger.LogInformation($"Отчет по БДИ \"{context.Database.GetDbConnection().Database}\"");


#endif
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
                int totalCount = 0;
                var tables = context.GetKnownTables();
                foreach (var tableProperty in tables) {
                    (object property, string tableDescription, Type entityType, int count) = tableProperty;
                    if (string.IsNullOrEmpty(tableDescription)) {
                        tableDescription = entityType.Name.Split('.').Last();
                    }
                    if (count > 0) {
                        totalCount += count;
                        logger.LogInformation($"Таблица \"{tableDescription}\": {count} элементов.");
                    }
                }
                logger.LogInformation($"Всего элементов в БДИ: {totalCount}.");
            }
        }
        [ReportRule(@"Состав БДИ.", 0, 100), RuleCategory("Состав БДИ.", "AllTables")]
        public void AllKnownTables() {
            using (Context context = connector.Connect()) {
                var csvFile = $"Состав БДИ {DateTime.Now.ToString("yyyy.MM.dd")}.csv";
                csvFile = Path.Combine(Directory.GetCurrentDirectory(), csvFile);
                var tables = context.GetKnownTables();
                using (var csv = new StreamWriter(csvFile, false, new UTF8Encoding(false))) {
                    try {
                        csv.WriteLine($"Code;Name;SpecDescription");
                        csv.WriteLine($"Код оборудования, изделия, материала;Наименование (Тип);Описание в спецификации");
                        foreach (var tableProperty in tables) {
                            (object property, string tableDescription, Type entityType, int count) = tableProperty;
                            if (typeof(IProduct).IsAssignableFrom(entityType)) {
                                if (string.IsNullOrEmpty(tableDescription)) {
                                    tableDescription = entityType.Name.Split('.').Last();
                                }
                                if (count > 0) {
                                    var tableName = Context.GetDatabaseTableName(entityType);
                                    var products = GetProducts(context, tableName);
                                    csv.WriteLine($"Таблица \"{tableDescription}\": {count} элементов.");
                                    foreach (var product in products) {
                                        csv.WriteLine($"{product.Code};{product.Name};{product.SpecDescription}");
                                    }
                                }
                            }

                        }
                    }
                    finally {
#if NETFRAMEWORK
                        var connectString = context.Database.Connection.ConnectionString;
#else
                        var connectString = context.Database.GetConnectionString();

#endif           
                        csv.Close();
                        logger.LogInformation($"Состав БДИ \"{connectString}\" экспортирован в файл \"{csvFile}\"");
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
                //var dbName = context.Database.Connection.Database;
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
                    IQueryable dbSet = (IQueryable)tableDef.property;
                    var tableName = Context.GetDatabaseTableName(tableDef.EntityType);
                    try {
                        int count = tableDef.count;
                        if (count > 0) {
                            var tableCodes = LoadCodes(context, dbSet, tableName, tableDef.EntityType);
                            allCodes.AddRange(tableCodes);
                        }
                    }
                    catch (Exception ex) {
                        logger.LogWarning(ex, "При загрузке таблицы  \"{}\" произошла ошибка.", tableName);
                    }

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
            Queue<(string TableName, string Code, string ArticleName)> errors = new();
            using (Context context = connector.Connect()) {
                foreach (var tableDef in context.GetKnownTables()) {
                    (object property, string tableDescription, Type entityType, int count) = tableDef;
                    var dbSet = (IQueryable)property;
                    if (typeof(IHaveImageRef).IsAssignableFrom(entityType) & typeof(IProduct).IsAssignableFrom(entityType)) {
                        var tableName = Context.GetDatabaseTableName(tableDef.EntityType);
                        if (count > 0) {
                            try {
#if NETFRAMEWORK
                                foreach (object? entity in dbSet.AsNoTracking()) {
                                    IHaveImageRef product = (IHaveImageRef)entity;
                                    if (!(
                                        ((IHaveImageRef)entity).DbImageRef.HasValue &&
                                        ((IHaveImageRef)entity).DbImageRef > 0
                                        )) {
                                        errors.Enqueue((tableDescription, ((IProduct)entity).Code, ((IProduct)entity).Name));
                                    }
                                }
#else
                                var entities = (IQueryable<IHaveImageRef>)dbSet;
                                foreach (var entity in entities.AsNoTracking()) {
                                    var productWithImage = (IHaveImageRef)entity;
                                    if (!(productWithImage.DbImageRef.HasValue && productWithImage.DbImageRef > 0)) {
                                        var product = (IProduct)entity;
                                        errors.Enqueue((tableDescription, product.Code, product.Name));
                                    }
                                }
#endif
                            }
                            catch (Exception ex) {
                                logger.LogWarning(ex, "При загрузке таблицы \"{}\" произошла ошибка.", tableName);

                            }
                        }
                    }
                }
            }
            if (errors.Count > 0) {
                var message = $"Эскиз не внесен для {errors.Count} элементов.";
                FailRuleTest(message, errors);
            }

        }

        [ReportRule(@"Для всех элементов должна быть внесена масса.",
            0, 4), RuleCategory("Общие рекомендации.", "AllTables")]
        public void Rule_00_004() {
            Queue<(string tableDescription, string Code)> errorsIsNull = new();
            Queue<(string tableDescription, string Code, string message)> errorsFormat = new();
            using (Context context = connector.Connect()) {
                foreach (var tableDef in context.GetKnownTables()) {
                    (object property, string tableDescription, Type entityType, int count) = tableDef;
                    if (count > 0) {
                        //var typeDescription = Context.GetDefaultLocalizeValue(entityType);
                        (bool isHaveField, IEnumerable<Material> values) = GetMaterialValues(connector, Context.GetDatabaseTableName(entityType));
                        if (isHaveField) {
                            foreach (var row in values) {
                                string mass = row.Mass;
                                if (string.IsNullOrEmpty(mass)) {
                                    errorsIsNull.Enqueue((tableDescription, row.Code));
                                }
                                else if (!double.TryParse(mass, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("Ru-ru"), out _)) {
                                    errorsFormat.Enqueue((tableDescription, row.Code, mass));

                                }
                            }
                        }
                    }
                }
            }
            if (errorsIsNull.Count > 0) {
                var message = $"Масса не внесена для {errorsIsNull.Count} элементов.";
                FailRuleTest(message, errorsIsNull);
            }
            if (errorsFormat.Count > 0) {
                var message = $"Значение не соответствует шаблону 0,## для {errorsFormat.Count} элементов.";
                FailRuleTest(message, errorsFormat);
            }
        }
        [ReportRule(@"Для всех элементов должна быть внесена ссылка на сайт производителя.",
            0, 5), RuleCategory("Общие рекомендации.", "AllTables")]
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
            var tasks = new List<(string Code, string Uri)>(1024 * 8);

            using (var context = connector.Connect()) {
                foreach (var tableDef in context.GetKnownTables()) {
                    string tableName = Context.GetDatabaseTableName(tableDef.EntityType);
                    tasks.AddRange(GetUriValues(context, tableName));
                }
                //tasks.AddRange(
                //     context.ScsGutterCanals
                //        .ToArray()
                //        .Select(el => (el.Code, el.Url))
                //    );
                //tasks.AddRange(
                //context.DbScsGutterCovers
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
                //tasks.AddRange(
                //context.DbScsGutterPartitions
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
                //tasks.AddRange(
                //context.ScsGcFittings
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
                //tasks.AddRange(
                //context.DbScsGcCoverUnits
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
                //tasks.AddRange(
                //context.DbScsGcAccessoryUnits
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
                //tasks.AddRange(
                //context.ScsGutterBoltings
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
                //tasks.AddRange(
                //context.DbScsGcBoltingAccessoryUnits
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
                //tasks.AddRange(
                //context.CaeMaterialUtilities
                //    .ToArray()
                //    .Select(el => (el.Code, el.Url))
                //);
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
            0, 6), RuleCategory("Общие рекомендации.", "AllTables")]
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
