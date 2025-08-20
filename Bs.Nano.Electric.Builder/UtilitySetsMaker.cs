// Ignore Spelling: Gc

using Bs.Nano.Electric.Model;
using Bs.XML.SpreadSheet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nano.Electric;
using Nano.Electric.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using static Bs.Nano.Electric.Builder.ResourceManager;
using static Bs.Nano.Electric.Builder.UtilitySetMakerResources;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


#if NET48
using System.Data.Entity; // EF6
#else
using Microsoft.EntityFrameworkCore; // EF Core
#endif
using Bs.Nano.Electric.Builder.Internals;

namespace Bs.Nano.Electric.Builder {
    /// <summary>
    /// Создание конфигураций трасс.
    /// </summary>
    public class UtilitySetsMaker {
        private const System.Globalization.NumberStyles PositiveNumberStyle =
           NumberStyles.AllowLeadingWhite |
           NumberStyles.AllowTrailingWhite |
           NumberStyles.AllowDecimalPoint;
        internal class PlainCounter : IEnumerable<(int LayerNumber, int Number)> {

            private readonly int capacity;
            private readonly bool isTwoSide;
            private readonly IEnumerable<int> _Numbers = new int[] { 1, 2 };
            public PlainCounter(int total, ScsGcStandType KnotType = ScsGcStandType.ONE_SIDE) {
                isTwoSide = KnotType == ScsGcStandType.TWO_SIDE;
                capacity = total > 0 ? total : 0;
                capacity = isTwoSide ? capacity + capacity % 2 : capacity;
            }
            public IEnumerator<(int LayerNumber, int Number)> GetEnumerator() {
                int count = capacity;
                if (isTwoSide) {
                    int layer = 1;
                    int number = 1;
                    while (count > 0) {
                        yield return (layer, number);
                        if (--count == 0)
                            yield break;
                        number++;
                        yield return (layer, number);
                        layer++;
                        number = 1;
                        if (--count == 0)
                            yield break;
                    }
                }
                else {
                    int layer = 1;
                    while (count-- > 0) {
                        yield return (layer, 1);
                        layer++;
                    }
                }
            }
            public IEnumerable<(int LayerNumber, IEnumerable<int> Numbers)> GetLayers() {
                int count = capacity;
                if (isTwoSide) {
                    int layer = 1;
                    while (count > 0) {
                        yield return (layer++, new int[] { 1, 2 });
                        count -= 2;
                        if (count <= 0)
                            yield break;
                    }
                }
                else {
                    int layer = 1;
                    while (count > 0) {
                        yield return (layer++, new int[] { 1 });
                        count--;
                    }
                }
            }
            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
        private readonly ILogger logger;
        private readonly IElectricBuilderConfiguration configuration;
        //private readonly ResourceManager resourceManager;
        private readonly UtilitySetMakerResources resources;

        public UtilitySetsMaker(ILogger logger, UtilitySetMakerResources resources, IElectricBuilderConfiguration configuration) {
            this.logger = logger;
            this.configuration = configuration;
            //this.resourceManager = resourceManager;
            this.resources = resources;
        }
        /// <summary>
        /// Создание конфигураций соединительных элементов.
        /// </summary>
        /// <param name="dbFileName"></param>
        public void MakeScsGutterCanalConfigurations(INanocadDBConnector connector) {
            int i = 0;
            Dictionary<string, SeriesConfigurationJob> job;
            try {
                job = resources.SeriesConfigurationJobs;
                logger.LogInformation("Загружено задание на создание конфигураций соединительных элементов.");
            }
            catch (SectionNotFoundException ex) {
                logger.LogWarning($"Создание конфигураций соединительных элементов пропущено. {ex.Message}");
                return;
            }
            foreach (var item in job) {
                using (var context = connector.Connect())
                using (var trans = context.Database.BeginTransaction()) {
                    try {
                        DbScsGcSeriaConfigiration config = MakeSeriesConfiguration(context, item.Value);
                        trans.Commit();
                        logger.LogInformation("Конфигурация соединительных элементов \"{}\" сохранена в базу.", item.Key);
                        i++;
                    }

                    catch (Exception ex) {
                        trans.Rollback();
                        logger.LogError(ex, "При построении конфигурации соединительных элементов \"{}\" произошла ошибка.", item.Key);

                    }
                }
            }
            logger.LogInformation($"Внесено {i} конфигураций соединительных элементов.");

        }

        /// <summary>
        /// Создание конфигураций трасс лотков
        /// </summary>
        public void MakeDbGcMountSystemSet(INanocadDBConnector connector) {
            IEnumerable<SheetCommon.Row> mountSetSource;
            int i = 0;
            try {
                mountSetSource = resources.DbGcMountSystemSet;
                logger.LogInformation("Загружены Конфигурации трасс лотков и узлов крепления, общие параметры.");
            }
            catch (SectionNotFoundException ex) {
                logger.LogWarning($"Создание конфигураций трасс лотков и узлов крепления пропущено. {ex.Message}");
                return;
            }
            IEnumerable<SheetCommon.Row> gusSource;
            try {
                gusSource = resources.GutterUtilitySetSource;
                logger.LogInformation("Загружены Конфигурации трасс лотков и узлов крепления, комплектации узлов крепления.");
            }            catch (SectionNotFoundException ex) {
                logger.LogWarning($"Создание конфигураций трасс лотков и узлов крепления пропущено. {ex.Message}");
                return;
            }

            //Func<SheetCommon.Row, string> getCatalog = (row) => string.IsNullOrEmpty(row["MountType"]) ?
            //    row["InstallType"] : $"{row["InstallType"]}\\{row["MountType"]}";
            IEnumerable<SheetCommon.Row> setSource_Senz = mountSetSource
                .Where(row => !string.IsNullOrEmpty(row["Code"]));
            List<MountSystemSetJobPart> mssJobParts = new();
            var titles = mountSetSource.First().Titles;
            var knownCoatingTypes = resources.KnownMaterials
                .Where(s => titles.Any(t => string.Compare(s, t, StringComparison.Ordinal) == 0))
                .ToArray();
            foreach (string coatingTypeName in knownCoatingTypes) {
                mssJobParts.AddRange(setSource_Senz.Select(row => ParceMountSystemSetJobPart(row, coatingTypeName)));
            }
            int count = 0;
            IEnumerable<MountSystemSetJob> jobSource = gusSource
                .Where(row => !string.IsNullOrEmpty(row["Code"]) )
                .Select(row => ParceMountSystemSetJob(row, mssJobParts)
                );

            logger.LogInformation("Запуск построения конфигураций трасс лотков");

            foreach (var job in jobSource) {
                logger.LogInformation("Построение конфигурации трасс лотков \"{}\".", job.Attribute.DbName);
                using (Context context = connector.Connect())
                using (var trans = context.Database.BeginTransaction()) {
                    try {
                        MakeMountSystemSet(context, job);
                        context.SaveChanges();
                        trans.Commit();
                        count++;
                        logger.LogInformation("Конфигурация трасс лотков \"{}\" сохранена в базу.", job.Attribute.DbName);
                    }
                    catch (Exception ex) {
                        trans.Rollback();
                        logger.LogError(ex, "При построении конфигурации трасс лотков \"{}\" произошли ошибки.", job.Attribute.DbName);
                    }
                }

            }
            logger.LogInformation("Внесено {count} элементов конфигураций трасс лотков (DbGcMountSystem).", count);

        }


        /// <summary>
        /// Построение конфигураций трасс настенных коробов.
        /// </summary>
        /// <param name="connector"></param>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public void MakeDbCcMountSystemSet(INanocadDBConnector connector) {
            IEnumerable<CcMountSystemSetJob> jobs;
            try {
                jobs = resources.CcMountSystemSource;
                logger.LogInformation("Загружено задание на создание конфигураций трасс настенных коробов.");
            }
            catch (SectionNotFoundException ex) {
                logger.LogWarning($"Создание конфигураций трасс настенных коробов пропущено. {ex.Message}");
                return;
            }
            int i = 0;
            foreach (var confJob in jobs) {
                using (var context = connector.Connect())
                using (var trans = context.Database.BeginTransaction()) {
                    try {
                        DbCcMountSystem config = MakeDbCcMountSystem(context, confJob);
                        context.SaveChanges();
                        trans.Commit();
                        logger.LogInformation("Конфигурация трасс настенных коробов \"{}\" сохранена в базу.", confJob.DbName);
                        i++;
                    }

                    catch (Exception ex) {
                        trans.Rollback();
                        logger.LogError(ex, "При построении конфигурации трасс настенных коробов \"{}\" произошла ошибка.", confJob.DbName);
                    }
                }
                i++;
            }
            logger.LogInformation($"Сохранено {i} конфигураций трасс настенных коробов.");


        }
        /// <summary>
        /// Построение конфигураций трасс труб.
        /// </summary>
        /// <param name="connector"></param>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public void MakeDbTbMountSystemSet(INanocadDBConnector connector) {
            IEnumerable<TbMountSystemJob> jobs;
            try {
                jobs = resources.DbTbMountSystemSource;
                logger.LogInformation("Загружено задание на создание конфигураций трасс труб.");
            }
            catch (SectionNotFoundException ex) {
                logger.LogWarning($"Создание конфигураций трасс трасс труб. {ex.Message}");
                return;
            }
            int i = 0;
            foreach (var confJob in jobs) {
                using (var context = connector.Connect())
                using (var trans = context.Database.BeginTransaction()) {
                    try {
                        DbTbMountSystem config = MakeDbTbMountSystem(context, confJob);
                        context.SaveChanges();
                        trans.Commit();
                        logger.LogInformation("Конфигурация трасс трасс труб \"{}\" сохранена в базу.", confJob.DbName);
                        i++;
                    }

                    catch (Exception ex) {
                        trans.Rollback();
                        logger.LogError(ex, "При построении конфигурации трасс трасс труб \"{}\" произошла ошибка.", confJob.DbName);
                    }
                }
                i++;
            }
            logger.LogInformation($"Сохранено {i} конфигураций трасс трасс труб.");

        }
        /// <summary>
        /// Построение конфигураций соединительных элементов для труб.
        /// </summary>
        /// <param name="connector"></param>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public void MakeGetDbScsTubeSeriesConfigurationSet(INanocadDBConnector connector) {
            IEnumerable<DbScsTubeSeriesConfigurationJob> jobs;
            try {
                jobs = resources.DbScsTubeSeriesConfigurationSource.Where(el => !string.IsNullOrEmpty(el.DbName));
                logger.LogInformation("Загружено задание на создание конфигураций соединительных элементов для труб.");
            }
            catch (SectionNotFoundException ex) {
                logger.LogWarning($"Создание конфигураций соединительных элементов для труб пропущено. {ex.Message}");
                return;
            }
            int i = 0;
            foreach (var confJob in jobs) {
                using (var context = connector.Connect())
                using (var trans = context.Database.BeginTransaction()) {
                    try {
                        DbScsTubeSeriesConfiguration config = MakeTubeSeriesConfiguration(context, confJob);
                        context.SaveChanges();
                        trans.Commit();
                        logger.LogInformation("Конфигурация трасс соединительных элементов для труб \"{}\" сохранена в базу.", confJob.DbName);
                        i++;
                    }
                    catch (Exception ex) {
                        trans.Rollback();
                        logger.LogError(ex, "При построении конфигурации трасс соединительных элементов для труб \"{}\" произошла ошибка.", confJob.DbName);
                    }
                }
                i++;
            }
            logger.LogInformation($"Сохранено {i} соединительных элементов для труб.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string GetDbScsGcSeriesConfiguration_Key(ScsGutterType gutterType, string gutterSerie) {
            var key = $"{EnumConverter<ScsGutterType>.GetDescription(gutterType)}\\{gutterSerie}";
            if (resources.SeriaConfigurationMapping.TryGetValue(key, out var keySeriaConfiguration) && !string.IsNullOrWhiteSpace(keySeriaConfiguration)) {
                return keySeriaConfiguration;
            }
            return key;
        }
        private DbCcMountSystem MakeDbCcMountSystem(Context context, CcMountSystemSetJob confJob) {
            var kit = context.DbCcMountSystems.FirstOrDefault(gsc => gsc.DbName == confJob.DbName);
            if (kit is null) {
                kit = new DbCcMountSystem();
                int id = Maker.GetMaxId(context.DbCcMountSystems) + 1;
                kit.Id = id;
                kit.DbName = confJob.DbName;
                context.DbCcMountSystems.Add(kit);
            }
            kit.DbDescription = confJob.DbDescription;
            kit.LayerIsPrintable = true;
            kit.LayerLineWeigh = -1;
            kit.OrderNumber = 1;
            kit.CwPCode = 0;
            kit.DbImageRef = 0;
            kit.InImport = false;
            var cableCanal = context.ScsCabelCanals
                .Select(p => new { p.Id, p.Code, p.Series })
                .FirstOrDefault(p => p.Code == confJob.CableCanal.CableCanalCode);
            if (cableCanal is null) {
                throw new InvalidDataException($"Артикул {confJob.CableCanal.CableCanalCode} не найден в таблице \"Короба\".");
            }
            kit.DbCatalog = cableCanal.Series;
            kit.CableCanal = cableCanal.Id;
            kit.PartitionCount = confJob.CableCanal.PartitionCount;
            kit.MountPlain.IsUse = confJob.CcMsMount.IsUse;
            kit.MountPlain.PostDistance = confJob.CcMsMount.PostDistance;
            foreach (var unit in confJob.UtilityUnits) {
                if (TryFindDbUtilityUnit(context, unit.UtilityUnitCode, out var element)) {
                    kit.MountPlain.Add(element);
                }
                else
                    logger.LogWarning($"Ошибка в конфигурации для элемента {unit.Code}: артикул {unit.UtilityUnitCode} не является элементом таблицы \"Материалы\" или \"Комплектации материалов\".");
            }
            kit.KitStructure = GetKitStructureAsXML(kit);
            return kit;
        }
        private DbTbMountSystem MakeDbTbMountSystem(Context context, TbMountSystemJob confJob) {

            var kit = context.DbTbMountSystems.FirstOrDefault(gsc => gsc.DbName == confJob.DbName);
            if (kit is null) {
                kit = new DbTbMountSystem();
                int id = Maker.GetMaxId(context.DbTbMountSystems) + 1;
                kit.Id = id;
                kit.DbName = confJob.DbName;
                context.DbTbMountSystems.Add(kit);
            }
            kit.DbDescription = confJob.DbDescription;
            kit.LayerIsPrintable = true;
            kit.LayerLineWeigh = -1;
            kit.OrderNumber = 1;
            kit.CwPCode = 0;
            kit.DbImageRef = 0;
            kit.InImport = false;
            var tubeCanal = context.ScsPipes
                .Select(p => new { p.Id, p.Code, p.Series })
                .FirstOrDefault(p => p.Code == confJob.TubeCanalCode);
            if (tubeCanal is null) {
                throw new InvalidDataException($"Артикул {confJob.TubeCanalCode} не найден в таблице \"Трубы\".");
            }
            kit.DbCatalog = tubeCanal.Series;
            kit.TubeCanal = tubeCanal.Id;
            kit.MountPlain.IsUse = confJob.CcMsMount.IsUse;
            kit.MountPlain.PostDistance = confJob.CcMsMount.PostDistance;
            foreach (var unit in confJob.UtilityUnits) {
                if (TryFindDbUtilityUnit(context, unit.UtilityUnitCode, out var element)) {
                    kit.MountPlain.Add(element);
                }
                else
                    logger.LogWarning($"Ошибка в конфигурации для элемента {unit.Code}: артикул {unit.UtilityUnitCode} не является элементом таблицы \"Материалы\" или \"Комплектации материалов\".");
            }
            kit.KitStructure = GetKitStructureAsXML(kit);
            return kit;
        }

        private DbScsTubeSeriesConfiguration MakeTubeSeriesConfiguration(Context context, DbScsTubeSeriesConfigurationJob confJob) {
            if (string.IsNullOrEmpty(confJob.TubeCode)) {
                throw new InvalidDataException($"В конфигурации \"{confJob.DbName}\" не внесен артикул трубы (TubeCode).");
            }
            var kit = context.DbScsTubeSeriesConfigurations.FirstOrDefault(gsc => gsc.DbName == confJob.DbName);
            if (kit is null) {
                kit = new DbScsTubeSeriesConfiguration();
                int id = Maker.GetMaxId(context.DbScsTubeSeriesConfigurations) + 1;
                kit.Id = id;
                kit.DbName = confJob.DbName;
                context.DbScsTubeSeriesConfigurations.Add(kit);
            }
            kit.DbDescription = confJob.DbDescription;
            kit.OrderNumber = 1;
            kit.CwPCode = 0;
            kit.InImport = false;
            var tube = context.ScsPipes
                .Select(p => new { p.Id, p.Code, p.Series })
                .FirstOrDefault(p => p.Code == confJob.TubeCode);
            if (tube is null) {
                throw new InvalidDataException($"Артикул {confJob.TubeCode} не найден в таблице \"Трубы\".");
            }
            else {
                kit.TubeSeria = tube.Series;
            }
            if (!string.IsNullOrEmpty(confJob.AngleCode)) {
                var pipeFitting = context.ScsTubeFittings
                .Select(p => new { p.Id, p.Code, p.Series, p.FittingType })
                .FirstOrDefault(p => p.Code == confJob.AngleCode);
                if (pipeFitting is null) {
                    throw new InvalidDataException($"Артикул {confJob.AngleCode} не найден в таблице \"Трубы\\Соединительные элементы\".");
                }
                else if (pipeFitting.FittingType == ScsTubeFittingTypeEnum.ANGLE) {
                    kit.AngleSeria = pipeFitting.Series;
                }
                else {
                    throw new InvalidDataException($"В поле AngleCode необходимо указать артикул элемента типа \"Угол\".");
                }
            }
            if (!string.IsNullOrEmpty(confJob.TripleCode)) {
                var pipeFitting = context.ScsTubeFittings
                .Select(p => new { p.Id, p.Code, p.Series, p.FittingType })
                .FirstOrDefault(p => p.Code == confJob.TripleCode);
                if (pipeFitting is null) {
                    throw new InvalidDataException($"Артикул {confJob.TripleCode} не найден в таблице \"Трубы\\Соединительные элементы\".");
                }
                else if (pipeFitting.FittingType == ScsTubeFittingTypeEnum.TRIPLE) {
                    kit.TripleSeria = pipeFitting.Series;
                }
                else {
                    throw new InvalidDataException($"В поле AngleCode необходимо указать артикул элемента типа \"Т-переход\".");
                }
            }
            if (!string.IsNullOrEmpty(confJob.CrossCode)) {
                var pipeFitting = context.ScsTubeFittings
                .Select(p => new { p.Id, p.Code, p.Series, p.FittingType })
                .FirstOrDefault(p => p.Code == confJob.CrossCode);
                if (pipeFitting is null) {
                    throw new InvalidDataException($"Артикул {confJob.CrossCode} не найден в таблице \"Трубы\\Соединительные элементы\".");
                }
                else if (pipeFitting.FittingType == ScsTubeFittingTypeEnum.CROSS) {
                    kit.CrossSeria = pipeFitting.Series;
                }
                else {
                    throw new InvalidDataException($"В поле AngleCode необходимо указать артикул элемента типа \"Х-переход\".");
                }
            }
            return kit;
        }

        private DbScsGcSeriaConfigiration MakeSeriesConfiguration(Context context, SeriesConfigurationJob kitJob) {
            var kit = context.DbScsGcSeriaConfigirations.Local
                                    .FirstOrDefault(gsc => (gsc.DbName == kitJob.Code));
            if (kit is not null)
                return kit;
            kit = context.DbScsGcSeriaConfigirations
                                    .FirstOrDefault(gsc => gsc.DbName == kitJob.Code);
            {
                if (kit is null) {
                    kit = new DbScsGcSeriaConfigiration();
                    int id = 1;
                    if (context.DbScsGcSeriaConfigirations.Any()) {
                        id = context.DbScsGcSeriaConfigirations.Max(sc => sc.Id) + 1;
                    }
                    kit.Id = id;
                    kit.DbName = kitJob.Code;
                    context.DbScsGcSeriaConfigirations.Add(kit);
                }
                kit.DbCatalog = kitJob.Attribute.DbCatalog;
                kit.DbDescription = kitJob.Attribute.DbDescription;
                kit.Description = kitJob.Attribute.Description;
                SeriaConfigirationKit kitStructure = new SeriaConfigirationKit();
                Queue<Exception> innerExceptions = new();

                foreach (var item in kitJob.Sections) {
                    var sectionName = item.SectionName;
                    var parts = item.Parts;
                    if (parts is not null) {
                        try {
                            switch (sectionName) {
                                case nameof(SeriaConfigirationKit.DbGcStrightSegment):
                                    #region Make DbGcStrightSegment
                                    {
                                        var segment = kitStructure.DbGcStrightSegment;
                                        Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
                                        while (sectionParts.Count > 0) {
                                            var partItem = sectionParts.Dequeue();
                                            if (!(partItem is SeriesConfigurationStraightPartItem configPartItem)) {
                                                configPartItem = new SeriesConfigurationStraightPartItem(partItem.Code, partItem.Amount, DbGcStrightSegmentComplectType.SEGMENT);
                                            }
                                            var itemCode = configPartItem.Code;
                                            {
                                                var gcPart = context.ScsGutterCanals
                                                    .AsNoTracking()
                                                    .FirstOrDefault(sgc => sgc.Code == itemCode);
                                                if (gcPart is not null) { // Лоток
                                                                          //kit.DbCatalog = gcPart.Series;
                                                                          //kit.Description = gcPart.Description;
                                                    segment.SetGutterSeria(gcPart);
                                                    //var confCode = GetDbScsGcSeriesConfiguration_Key(gcPart.GutterType, gcPart.Series);
                                                    //kit.DbName = confCode;
                                                    segment.ComplectType = configPartItem.ComplectType;
                                                    continue;
                                                }
                                            }
                                            {
                                                var gcPart = context.DbScsGutterCovers
                                                    .AsNoTracking()
                                                    .FirstOrDefault(sgc => sgc.Code == itemCode);
                                                if (gcPart is not null) { // Лоток
                                                    segment.SetGutterCoverSeria(gcPart);
                                                    continue;
                                                }
                                            }
                                            {
                                                int itemCount = configPartItem.Amount;
                                                while (sectionParts.Count > 0) {
                                                    partItem = sectionParts.Peek();
                                                    if (partItem.Code != itemCode)
                                                        break;
                                                    itemCount += partItem.Amount;
                                                    sectionParts.Dequeue();
                                                }
                                                if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {

                                                    utilityUnit.SpecCount = itemCount;
                                                    segment.AddChild(utilityUnit);
                                                }
                                                else {
                                                    throw new InvalidDataException($"Элемент \"{itemCode}\" структуры структуры {sectionName} не распознан.");
                                                }
                                            }
                                        }
                                        ;
                                    }
                                    #endregion
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcStrightPartition):
                                    #region Make DbGcStrightPartition
                                    {
                                        var segment = kitStructure.DbGcStrightPartition;
                                        Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
                                        while (sectionParts.Count > 0) {

                                            var partItem = sectionParts.Dequeue();
                                            if (!(partItem is SeriesConfigurationStraightPartItem configPartItem)) {
                                                configPartItem = new SeriesConfigurationStraightPartItem(partItem.Code, partItem.Amount, DbGcStrightSegmentComplectType.SEGMENT);
                                            }

                                            var itemCode = configPartItem.Code;
                                            {
                                                var gcPart = context.DbScsGutterPartitions
                                                    .AsNoTracking()
                                                    .FirstOrDefault(sgc => sgc.Code == itemCode);
                                                if (gcPart is not null) { // Разделитель лотка
                                                                          //kit.DbCatalog = gcPart.Series;
                                                                          //kit.Description = gcPart.Description;
                                                    segment.SetGutterPartitionSeria(gcPart);
                                                    segment.ComplectType = configPartItem.ComplectType;
                                                    continue;
                                                }
                                            }
                                            {
                                                int itemCount = configPartItem.Amount;
                                                while (sectionParts.Count > 0) {
                                                    partItem = sectionParts.Peek();
                                                    if (partItem.Code != itemCode)
                                                        break;
                                                    itemCount += partItem.Amount;
                                                    sectionParts.Dequeue();
                                                }
                                                ;
                                                if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                                                    utilityUnit.SpecCount = itemCount;
                                                    segment.AddChild(utilityUnit);
                                                }
                                                else {
                                                    throw new InvalidDataException($"Элемент \"{itemCode}\" структуры {sectionName} не распознан.");
                                                }
                                            }
                                        }
                                        ;
                                    }
                                    #endregion
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcHBend_Direct):
                                    MakeDbGcHBend(context, kitStructure.DbGcHBend_Direct, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcHBend_ANGLE_45):
                                    MakeDbGcHBend(context, kitStructure.DbGcHBend_ANGLE_45, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcHBend_OPTIONAL):
                                    MakeDbGcHBend(context, kitStructure.DbGcHBend_OPTIONAL, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcHBend_OPTIONAL_0_45):
                                    MakeDbGcHBend(context, kitStructure.DbGcHBend_OPTIONAL_0_45, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcVBendInner_ANGLE_90):
                                    MakeDbGcHBend(context, kitStructure.DbGcVBendInner_ANGLE_90, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcVBendInner_ANGLE_45):
                                    MakeDbGcHBend(context, kitStructure.DbGcVBendInner_ANGLE_45, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcVBendInner_ANGLE_0_90):
                                    MakeDbGcHBend(context, kitStructure.DbGcVBendInner_ANGLE_0_90, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcVBendOuter_ANGLE_90):
                                    MakeDbGcHBend(context, kitStructure.DbGcVBendOuter_ANGLE_90, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcVBendOuter_ANGLE_45):
                                    MakeDbGcHBend(context, kitStructure.DbGcVBendOuter_ANGLE_45, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcVBendOuter_ANGLE_0_90):
                                    MakeDbGcHBend(context, kitStructure.DbGcVBendOuter_ANGLE_0_90, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcVBendUniversal):
                                    MakeDbGcHBend(context, kitStructure.DbGcVBendUniversal, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcTriple_0):
                                    MakeDbGcHBend(context, kitStructure.DbGcTriple_0, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcTriple_1):
                                    MakeDbGcHBend(context, kitStructure.DbGcTriple_1, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcCross_0):
                                    MakeDbGcHBend(context, kitStructure.DbGcCross_0, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcCross_1):
                                    MakeDbGcHBend(context, kitStructure.DbGcCross_1, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcMsPassage_0):
                                    MakeDbGcHBend(context, kitStructure.DbGcMsPassage_0, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcMsPassage_1):
                                    MakeDbGcHBend(context, kitStructure.DbGcMsPassage_1, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcMsJoint):
                                    MakeDbGcHBend(context, kitStructure.DbGcMsJoint, parts);
                                    break;
                                case nameof(SeriaConfigirationKit.DbGcMsCork):
                                    MakeDbGcHBend(context, kitStructure.DbGcMsCork, parts);
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex) {
                            innerExceptions.Enqueue(new InvalidDataException($"Ошибка обработки секции {sectionName} в конфигурации \"{kitJob.Code}\".", ex));
                        }
                    }
                }
                if (innerExceptions.Count > 1) {
                    throw new AggregateException($"Ошибка обработки конфигурации \"{kitJob.Code}\".", innerExceptions);
                }
                else if (innerExceptions.Count == 1) {
                    throw new InvalidDataException($"Ошибка обработки конфигурации \"{kitJob.Code}\".", innerExceptions.Dequeue());
                }
                else {
                    // All right
                    kit.SetKitStructure(kitStructure);
                    context.SaveChanges();
                    if (kit.DbName == kitJob.Code) {
                        logger.LogInformation("Конфигурация \"{}\" успешно сохранена.", kitJob.Code);
                    }
                    else {
                        logger.LogInformation("Конфигурация \"{}\" сохранена под именем \"{}\".", kitJob.Code, kit.DbName);
                    }
                }
            }
            return kit;
        }

        private void MakeDbGcHBend(Context context, DbGcHBend segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetBendSeria(gcPart);
                        continue;
                    }
                }
                {
                    DbScsGcCoverUnit? gcPart = TryFindDbScsGcCoverUnit(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetBendCoverSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcHBend не распознан.");
                    }
                }
            }
            ;
        }
        private void MakeDbGcHBend(Context context, DbGcVBendInner segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetBendInnerSeria(gcPart);
                        continue;
                    }
                }
                {
                    DbScsGcCoverUnit? gcPart = TryFindDbScsGcCoverUnit(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetBendInnerCoverSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcVBendInner не распознан.");
                    }
                }
            }
            ;
        }
        private void MakeDbGcHBend(Context context, DbGcVBendOuter segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetBendOuterSeria(gcPart);
                        continue;
                    }
                }
                {
                    DbScsGcCoverUnit? gcPart = TryFindDbScsGcCoverUnit(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetBendOuterCoverSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcVBendOuter не распознан.");
                    }
                }
            }
            ;
        }
        private void MakeDbGcHBend(Context context, DbGcVBendUniversal segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) {
                        segment.SetBendOuterSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcVBendUniversal не распознан.");
                    }
                }
            }
            ;
        }
        private void MakeDbGcHBend(Context context, DbGcTriple segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetTripleSeria(gcPart);
                        continue;
                    }
                }
                {
                    DbScsGcCoverUnit? gcPart = TryFindDbScsGcCoverUnit(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetTripleCoverSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcTriple не распознан.");
                    }
                }
            }
            ;
        }
        private void MakeDbGcHBend(Context context, DbGcCross segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) {
                        segment.SetCrossSeria(gcPart);
                        continue;
                    }
                }
                {
                    DbScsGcCoverUnit? gcPart = TryFindDbScsGcCoverUnit(context, itemCode);
                    if (gcPart is not null) {
                        segment.SetCrossCoverSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcCross не распознан.");
                    }
                }
            }
            ;
        }
        private void MakeDbGcHBend(Context context, DbGcMsPassage segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) {
                        segment.SetPassageSeria(gcPart);
                        continue;
                    }
                }
                {
                    DbScsGcCoverUnit? gcPart = TryFindDbScsGcCoverUnit(context, itemCode);
                    if (gcPart is not null) {
                        segment.SetPassageCoverSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcMsPassage не распознан.");
                    }
                }
            }
            ;
        }
        private void MakeDbGcHBend(Context context, DbGcMsJoint segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetJointSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcMsJoint не распознан.");
                    }
                }
            }
            ;
        }
        private void MakeDbGcHBend(Context context, DbGcMsCork segment, IEnumerable<SeriesConfigurationPartItem>? parts) {
            Queue<SeriesConfigurationPartItem> sectionParts = new Queue<SeriesConfigurationPartItem>(parts);
            while (sectionParts.Count > 0) {
                var configPartItem = sectionParts.Dequeue();
                var itemCode = configPartItem.Code;
                {
                    ScsGcFitting? gcPart = TryFindScsGcFitting(context, itemCode);
                    if (gcPart is not null) { // Лоток
                        segment.SetCorkSeria(gcPart);
                        continue;
                    }
                }
                {
                    int itemCount = configPartItem.Amount;
                    while (sectionParts.Count > 0) {
                        configPartItem = sectionParts.Peek();
                        if (configPartItem.Code != itemCode)
                            break;
                        itemCount += configPartItem.Amount;
                        sectionParts.Dequeue();
                    }
                    DbScsGcAccessoryUnit? accessoryUnit = TryFindDbScsGcAccessoryUnit(context, itemCode);
                    if (accessoryUnit is not null) {
                        DbGcMsAccessory accessory = DbGcMsAccessory.From(accessoryUnit);
                        accessory.Count = itemCount;
                        segment.AddChild(accessory);
                        continue;
                    }
                    if (TryFindDbUtilityUnit(context, itemCode, out var utilityUnit)) {
                        utilityUnit.SpecCount = itemCount;
                        segment.AddChild(utilityUnit);
                        continue;
                    }
                    {
                        throw new InvalidDataException($"Элемент \"{itemCode}\" структуры DbGcMsCork не распознан.");
                    }
                }
            }
            ;
        }

        private void MakeMountSystemSet(Context context, MountSystemSetJob job) {
            var gutterUtilitySet = MakeDbScsGutterUtilitySet(context, job);
            var query = context.DbGcMountSystems.Where(s => s.DbName == job.Attribute.DbName);
            //DbGcMountSystem? mountSystemSet = context.DbGcMountSystems
            //            .FirstOrDefault(s => s.DbName == job.Attribute.DbName);
            DbGcMountSystem? mountSystemSet = query.FirstOrDefault();
            if (mountSystemSet is null) {
                mountSystemSet = context.DbGcMountSystems.CreateEntity();
                int id;
                if (context.DbGcMountSystems.Any())
                    id = context.DbGcMountSystems.Select(s => s.Id).Max() + 1;
                else
                    id = 1;
                mountSystemSet.Id = id;
                context.DbGcMountSystems.Add(mountSystemSet);
                //context.SaveChanges();
            }
            mountSystemSet.DbName = job.Attribute.DbName;
            mountSystemSet.Name = job.Attribute.DbDescription;
            mountSystemSet.DbCatalog = job.Attribute.DbCatalog;
            mountSystemSet.LayerName = string.Empty;
            mountSystemSet.LayerColor = "'Blue'";
            mountSystemSet.LayerIsPrintable = true;
            mountSystemSet.LayerLineWeigh = 30;
            mountSystemSet.PostDistance = job.Attribute.PostDistance;
            mountSystemSet.DbDescription = job.Attribute.Description;
            //SheetCommon.Row sourceRow = source.FirstOrDefault(row => row["DbName"] == setRow.Code);
            //if (sourceRow.Titles.Length == 0) {// not found; create from mountSetResource
            //mountSystemSet.LayerName = "КНС";
            string imgFileName = job.Attribute.DbImageRef;
            //string imageName = $"{Path.GetFileNameWithoutExtension(setRow.ImageFileName)}.MountSystemSet.png";
            string imagesPart = configuration.GetSection("KitStructureSource:ImagesPath").Value ??
                throw new InvalidOperationException($"В конфигурации не установлен путь для загрузки эскизов типовых решений (секция KitStructureSource:ImagesPath).");
            string fullImageFileName = Path.Combine(imagesPart, imgFileName);
            var image = GetOrCreate(context, fullImageFileName, target: (imgFileName, job.Attribute.Description, "Конфигурации трасс лотков"));
            mountSystemSet.DbImageRef = image.Id;
            //image.Category = "Конфигурации КНС\\Конфигурации трасс лотков";
            // Make KitStructure
            {
                MakeKitStructure(context, mountSystemSet, gutterUtilitySet, job);
                DbScsGutterUtilitySet utilitySet;
                utilitySet = mountSystemSet.StandGutterUtilitySet = gutterUtilitySet;
                utilitySet.DbCatalog = job.Attribute.DbCatalog;

                //try {
                //    context.SaveChanges();
                //}
                //catch (System.Data.Entity.Validation.DbEntityValidationException ex) {
                //    foreach (var vd in ex.EntityValidationErrors) {
                //        foreach (var item in vd.ValidationErrors) {
                //            Console.WriteLine($"{item.PropertyName}:\"{item.ErrorMessage}\".");
                //        }
                //    }
                //    throw;
                //}
            }


            // Save KitStructure
            mountSystemSet.KitStructure = GetKitStructureAsXML(mountSystemSet);
        }
        // Конфигурации трасс лотков
        private void MakeKitStructure(Context context, DbGcMountSystem mountSystemSet, DbScsGutterUtilitySet sguSet, MountSystemSetJob job) {
            string dbName = job.Attribute.DbName;
            var dbShelfs = job.JobParts.Where(item => item.KitStructureItem == KitStructureType.DbShelf).ToArray();
            var plainCounter = new PlainCounter(dbShelfs.Length, sguSet.KnotType ?? ScsGcStandType.ONE_SIDE).GetEnumerator();
            Dictionary<(int, int), (int Uid, int? ParentUid)> knownDbShelfs = new();
            Dictionary<string, SeriesConfigurationJob>? seriesConfigurationJob = null;
            foreach (var item in dbShelfs) {
                plainCounter.MoveNext();
                knownDbShelfs[plainCounter.Current] = (item.Uid, item.ParentUid);
            }
            foreach (var level in sguSet.Stand.Levels) {
                foreach (var knotPlain in level.KnotPlains) {
                    DbGcSystemPlain plain = new DbGcSystemPlain();
                    plain.LayerNumber = level.Number;
                    plain.Number = knotPlain.Number;
                    plain.DbShelf = knotPlain.Profile;
                    plain.IsAutoSelection = true;
                    double profileLength = GetGutterBoltingLength(plain.DbShelf) ?? 0;
                    plain.DbProfileLength = (int)(profileLength);
                    int parentUid = knownDbShelfs[(level.Number, knotPlain.Number)].Uid;
                    var gutterRow = job.JobParts.FirstOrDefault(item => item.KitStructureItem == KitStructureType.Gutter && item.ParentUid == parentUid); // Лоток, подчиненный соответствующей полке

                    if (gutterRow is not null && !string.IsNullOrEmpty(gutterRow.ItemCode)) {
                        ScsGutterCanal gutterCanal = context.ScsGutterCanals
                            .AsNoTracking()
                            .FirstOrDefault(sgc => sgc.Code == gutterRow.ItemCode);
                        if (gutterCanal is null) {
                            throw new InvalidDataException($"При продукт {gutterRow.ItemCode} не найден в таблице \"{Context.GetDefaultLocalizeValue<ScsGutterCanal>()}\".");
                        }

                        DbScsGcSeriaConfigiration? config = null;
                        try {
                            var confCode = GetDbScsGcSeriesConfiguration_Key(gutterCanal.GutterType, gutterCanal.Series);
                            config = context.DbScsGcSeriaConfigirations
                                .AsNoTracking()
                                .FirstOrDefault(item => item.DbName == confCode);
                            if (config is null) {
                                logger.LogInformation($"Конфигурация соединительных элементов \"{confCode}\" не найдена. Построим из задания.");
                                seriesConfigurationJob ??= resources.SeriesConfigurationJobs;
                                if (seriesConfigurationJob.TryGetValue(confCode, out var confJob)) {
                                    config = MakeSeriesConfiguration(context, confJob);
                                }
                                else {
                                    throw new InvalidDataException($"В задании на конфигурации соединительных элементов не содержится описания конфигурации \"{confCode}\".");
                                }
                            }
                        }
                        catch (Exception ex) {
                            throw new InvalidDataException($"При создании или извлечении конфигурации соединительных элементов для артикула [{gutterRow.ItemCode}] произошла ошибка.", ex);
                        }

                        var gutter = plain.Gutter = new DbGcSystemGutter {
                            Gutter = gutterCanal,
                            ComplectType = gutterRow.ComplectType ?? DbGcStrightSegmentComplectType.SEGMENT,
                            IsEnabled = true,
                            IsUse = true,
                            Configuration = config
                        };
                        #region DbGcSystemGutter.DbUtilityUnit
                        var gutterAccessoryList = job.JobParts.Where(item => item.KitStructureItem == KitStructureType.DbUtilityUnit && item.ParentUid == gutterRow.Uid); // Лоток\Комплектующие
                        foreach (var gutterAccessoryRow in gutterAccessoryList) {
                            //DbUtilityUnit? dbUtilityUnit;
                            if (TryFindDbUtilityUnit(context, gutterAccessoryRow.ItemCode, out var dbUtilityUnit)) {
                                dbUtilityUnit.SpecCount = gutterAccessoryRow.ItemQuantity;
                                plain.AddChild(dbUtilityUnit);
                            }
                            else {
                                throw new InvalidDataException($"При извлечении продукта {gutterAccessoryRow.ItemCode} как элемента \"Комплектующие\" произошла ошибка: Элемент среди допустимых комплектующих не найден.");
                            }
                        }
                        #endregion
                    }
                    #region DbGcSystemPlain.DbUtilityUnit
                    //foreach (var dbUtilityUnit in knotPlain.UtilityUnits) {
                    //    plain.AddChild(dbUtilityUnit);
                    //}
                    //var accessoryList = row.Items.Where(item => item.KitStructureItem == "DbGcSystemPlain.DbUtilityUnit");
                    //foreach (var accessory in accessoryList) {
                    //    var unitCode = accessory.Code;
                    //    if (TryFindDbUtilityUnit(context, unitCode, out var dbUtilityUnit)) {
                    //        throw new InvalidDataException($"При извлечении продукта {unitCode} как элемента DbUtilityUnit произошла ошибка: Элемент не найден.");
                    //    }
                    //    int accessoryQuantity = accessory.Quantity;
                    //    dbUtilityUnit.SpecCount = accessoryQuantity;
                    //    plain.Add(dbUtilityUnit);
                    //}
                    #endregion
                    mountSystemSet.Add(plain);
                }
            }

            //string systemSetCode = job.DbName;
            //var scsGutterBolting = row.Items.FirstOrDefault(item => item.KitStructureItem == "DbGcSystemPlain.DbShelf");
            //string gutterBoltingCode = scsGutterBolting.Code;
            //{
            #region DbGcSystemPlain.DbShelf
            //    try {
            //        int quantity = scsGutterBolting.Quantity < 1 ? 1 : scsGutterBolting.Quantity;
            //        var sgusSource = resources.GutterUtilitySetSource;
            //        var sguSourceRow = sgusSource.First(row => row["DbName"] == systemSetCode);
            //        ScsGcStandType knotType = sguSourceRow["KnotType"] == "Двусторонняя" ? ScsGcStandType.TWO_SIDE : ScsGcStandType.ONE_SIDE;
            //        ScsGutterBolting? dbShelf = null;
            //        if (!(string.IsNullOrEmpty(gutterBoltingCode)))
            //            try {
            //                dbShelf = context.ScsGutterBoltings.First(p => DbFunctions.Like(p.Code, gutterBoltingCode));
            //            }
            //            catch (Exception ex) {
            //                throw new InvalidDataException($"При извлечении продукта {gutterBoltingCode} из таблицы ScsGutterBoltings произошла ошибка.", ex);
            //            }

            //        PlainCounter counter = new PlainCounter(quantity, knotType);
            //        foreach (var item in counter) {
            //            DbGcSystemPlain plain = new DbGcSystemPlain();
            //            plain.LayerNumber = item.LayerNumber;
            //            plain.Number = item.Number;
            //            plain.DbShelf = dbShelf;

            //            plain.IsAutoSelection = true;
            //            double profileLength = GetGutterBoltingLength(dbShelf) ?? 250.0;
            //            plain.DbProfileLength = (int)(profileLength);
            //            var gutterRow = row.Items.FirstOrDefault(item => item.KitStructureItem == "DbGcSystemGutter.Gutter"); // Несущий элемент|Лоток
            //            if (!string.IsNullOrEmpty(gutterRow.Code)) {
            //                ScsGutterCanal gutterCanal;
            //                try {
            //                    gutterCanal = context.ScsGutterCanals.AsNoTracking().First(sgc => DbFunctions.Like(sgc.Code, gutterRow.Code));
            //                }
            //                catch (Exception ex) {
            //                    throw new InvalidDataException($"При извлечении продукта {gutterRow.Code} из таблицы ScsGutterCanals произошла ошибка.", ex);
            //                }
            //                DbScsGcSeriaConfigiration? config = null;
            //                try {
            //                    var confCode = GetDbScsGcSeriesConfiguration_Key((int)gutterCanal.GutterType, gutterCanal.Series);
            //                    var confJob = resources.SeriesConfigurationJobs[confCode];
            //                    config = MakeSeriesConfiguration(context, confJob);
            //                    config.DbName = confCode;
            //                }
            //                catch (Exception ex) {
            //                    throw new InvalidDataException($"При создании или извлечении конфигурации соединительных элементов для артикула [{gutterRow.Code}] произошла ошибка.", ex);
            //                }

            //                var gutter = plain.Gutter = new DbGcSystemGutter {
            //                    Gutter = gutterCanal,
            //                    ComplectType = DbGcStrightSegmentComplectType.SEGMENT,
            //                    IsEnabled = true,
            //                    IsUse = true,
            //                    IElectricBuilderConfiguration = config
            //                };
            //                #region DbGcSystemGutter.DbUtilityUnit
            //                var gutterAccessoryList = row.Items.Where(item => item.KitStructureItem == "DbGcSystemGutter.DbUtilityUnit"); // Лоток\Комплектующие
            //                foreach (var gutterAccessoryRow in gutterAccessoryList) {
            //                    //DbUtilityUnit? dbUtilityUnit;
            //                    if (TryFindDbUtilityUnit(context, gutterAccessoryRow.Code, out var dbUtilityUnit)) {
            //                        dbUtilityUnit.SpecCount = gutterAccessoryRow.Quantity;
            //                        gutter.AddChild(dbUtilityUnit);
            //                    }
            //                    else {
            //                        throw new InvalidDataException($"При извлечении продукта {gutterAccessoryRow.Code} как элемента DbUtilityUnit произошла ошибка: Элемент не найден.");
            //                    }
            //                }
            //                #endregion
            //            }
            //            #region DbGcSystemPlain.DbUtilityUnit
            //            var accessoryList = row.Items.Where(item => item.KitStructureItem == "DbGcSystemPlain.DbUtilityUnit");
            //            foreach (var accessory in accessoryList) {
            //                var unitCode = accessory.Code;
            //                if (TryFindDbUtilityUnit(context, unitCode, out var dbUtilityUnit)) {
            //                    throw new InvalidDataException($"При извлечении продукта {unitCode} как элемента DbUtilityUnit произошла ошибка: Элемент не найден.");
            //                }
            //                int accessoryQuantity = accessory.Quantity;
            //                dbUtilityUnit.SpecCount = accessoryQuantity;
            //                plain.Add(dbUtilityUnit);
            //            }
            //            #endregion
            //            mountSystemSet.Add(plain);
            //        }
            //    }
            //    catch (Exception ex) {
            //        throw new InvalidDataException($"При создании KitStructure для элемента {systemSetCode} произошла ошибка.", ex);
            //    }
            //}
            #endregion
        }

        // Конфигурация узлов крепления
        private DbScsGutterUtilitySet MakeDbScsGutterUtilitySet(Context context, MountSystemSetJob job) {
            logger.LogInformation("Построение конфигурации узлов крепления \"{}\".", job.Attribute.DbName);
            try {
                string dbName = job.Attribute.DbName;

                //var sgusSource = resources.GutterUtilitySetSource;
                //var sourceRow = sgusSource.First(row => row["DbName"] == code);
                int id;
                var gus = context.DbScsGutterUtilitySets
                      .FirstOrDefault(gus => gus.DbName == dbName);
                int? baseId = gus?.Id;
                DbScsGutterUtilitySet sguSet;
                if (baseId.HasValue) {
                    sguSet = context.DbScsGutterUtilitySets.Find(baseId.Value);
                }
                else {
                    if (context.DbScsGutterUtilitySets.Any()) {
                        id = context.DbScsGutterUtilitySets.Select(s => s.Id).Max() + 1;
                    }
                    else
                        id = 1;
                    sguSet = new DbScsGutterUtilitySet { Id = id };
                    sguSet.DbName = dbName;
                    context.DbScsGutterUtilitySets.Add(sguSet);
                }
                //sguSet.LoadRow(sourceRow);
                sguSet.DbDescription = job.Attribute.DbDescription;
                sguSet.Description = job.Attribute.Description;
                sguSet.DbCatalog = job.Attribute.DbCatalog;
                sguSet.KnotType = job.Attribute.KnotType;
                sguSet.InstallType = job.Attribute.MountType;
                sguSet.StructureType = job.Attribute.StructureType;
                //sguSet.Seria= // not used
                string imgFileName = job.Attribute.DbImageRef;
                string imagesPart = configuration.GetSection("KitStructureSource:ImagesPath").Value ??
                    throw new InvalidOperationException($"В конфигурации не установлен путь для загрузки эскизов типовых решений (секция KitStructureSource:ImagesPath).");
                string fullImageFileName = Path.Combine(imagesPart, imgFileName);
                var image = GetOrCreate(context, fullImageFileName, target: (imgFileName, job.Attribute.Description, "Конфигурации трасс лотков"));
                sguSet.DbImageRef = image.Id;
                //string? imgFileName = $"Resources\\\\Решения АТР\\{sourceRow["SourceImageFileName"]}";
                //string? targetName = sourceRow["TargetImageFileName"];
                //string? imgDescription = sourceRow["Description"];
                //if (!string.IsNullOrEmpty(imgFileName)) {
                //    var image = GetOrCreate(context, imgFileName!, target: (targetName, imgDescription, "Крепления лотков\\Конфигурации узлов крепления"));
                //    //image.Category = "Крепления лотков\\Конфигурации узлов крепления";
                //    sguSet.DbImageRef = image.Id;
                //}

                DbGcKnotStand ks = sguSet.Stand = new DbGcKnotStand {
                    IsEnabled = true,
                    IsUse = true,
                    Length = 0,
                };
                var btRow = job.JobParts.FirstOrDefault(item => item.KitStructureItem == KitStructureType.Bolting);
                //            Queue<MountSystemSetJobPart> dbShelfs = new();
                if (btRow is null) {
                    sguSet.StandType = DbGcKnotStandType.NO;
                }
                else {
                    string btRowCode = btRow.ItemCode;
                    /*if (!string.IsNullOrEmpty(btRowCode))*/
                    {
                        var bt = context.ScsGutterBoltings
                            .Select(bt => new { bt.Id, bt.Code, bt.Length, bt.ProfileLength, bt.Heigth, bt.CanalBoltingType })
                            .FirstOrDefault(bt => bt.Code == btRowCode);
                        if (bt is null) {
                            throw new InvalidDataException($"Элемент с артикулом {btRowCode} не найден в таблице {Context.GetDefaultLocalizeValue<ScsGutterBolting>()}");
                        }
                        switch (bt.CanalBoltingType) {
                            case ScsGutterBoltingTypeEnum.CONSOLE:
                            case ScsGutterBoltingTypeEnum.POST:
                                sguSet.StandType = DbGcKnotStandType.POST;
                                break;
                            case ScsGutterBoltingTypeEnum.STUD:
                                sguSet.StandType = DbGcKnotStandType.STUD;
                                break;
                            case ScsGutterBoltingTypeEnum.PROFILE:
                                sguSet.StandType = DbGcKnotStandType.PROFILE;
                                break;
                            default:
                                throw new InvalidDataException($"Указан некорректный артикул крепления ярусов \"{btRowCode}\". Изделие должно являться стойкой, профилем или шпилькой.");
                        }
                        var btLength = btRow.TotalLayersHeight;
                        if (!btLength.HasValue) {
                            btLength = bt.CanalBoltingType == ScsGutterBoltingTypeEnum.PROFILE ? (bt.ProfileLength.HasValue ? (int)bt.ProfileLength : 0)
                            : bt.CanalBoltingType == ScsGutterBoltingTypeEnum.POST ? (bt.Heigth.HasValue ? (int)bt.Heigth : 0)
                            : bt.CanalBoltingType == ScsGutterBoltingTypeEnum.STUD ? (bt.Heigth.HasValue ? (int)bt.Heigth : 0)
                            : 0;
                        }
                        ks.Bolting = context.ScsGutterBoltings.Find(bt.Id); // new ScsGutterBolting { Id = bt.Id };
                        ks.Length = btLength.Value;
                    }
                    var btRowId = btRow.Uid;
                    foreach (var childItem in job.JobParts.Where(item => item.ParentUid == btRowId).ToArray()) {
                        if (childItem.KitStructureItem == KitStructureType.DbUtilityUnit) {
                            var unitCode = childItem.ItemCode;
                            if (!TryFindDbUtilityUnit(context, unitCode, out var dbUtilityUnit)) {
                                throw new InvalidDataException($"При извлечении продукта {unitCode} как элемента \"Комплектующие\" произошла ошибка: Элемент среди допустимых комплектующих не найден.");
                            }
                            dbUtilityUnit.SpecCount = childItem.ItemQuantity;
                            ks.AddChild(dbUtilityUnit);
                        }
                        else if (childItem.KitStructureItem == KitStructureType.DbShelf) {
                        }
                        else {
                            throw new InvalidDataException($"При построении элемента {childItem} произошла ошибка. Для элемента конфигурации \"Крепление ярусов\" в качестве подчиненного можно указать только элемент \"Полка\" или \"Комплектующие\".");
                        }
                    }
                }
                if (sguSet.KnotType == ScsGcStandType.TWO_SIDE) {
                    int shadowId = int.MinValue;
                    var node = job.JobParts.First;
                    while (node != null) {
                        var jobPartNode = node;
                        var jobPart = node.Value;
                        node = node.Next;
                        if (jobPart.KitStructureItem == KitStructureType.DbShelf) {
                            var dbShelf = jobPart;
                            var gutterRows = job.JobParts.Where(item => item.KitStructureItem == KitStructureType.Gutter && item.ParentUid == dbShelf.Uid).GetEnumerator();
                            if (gutterRows.MoveNext() && gutterRows.MoveNext()) {
                                // Если два подчиненных лотка, вставляется полка без комплектующих и второй лоток переподчиняется созданной полке.
                                MountSystemSetJobPart gutterRow = gutterRows.Current;
                                job.JobParts.Remove(gutterRow);
                                MountSystemSetJobPart dbShelf1 = new MountSystemSetJobPart(dbShelf.SetCode, dbShelf.Material, dbShelf.ItemCode, 0, dbShelf.KitStructureItem, shadowId++, dbShelf.ParentUid, dbShelf.TotalLayersHeight, dbShelf.LayerHeight, dbShelf.ComplectType);
                                MountSystemSetJobPart gutterRow1 = new MountSystemSetJobPart(gutterRow.SetCode, gutterRow.Material, gutterRow.ItemCode, 1, gutterRow.KitStructureItem, gutterRow.Uid, dbShelf1.Uid, gutterRow.TotalLayersHeight, gutterRow.LayerHeight, gutterRow.ComplectType);
                                job.JobParts.AddAfter(jobPartNode, gutterRow1);
                                job.JobParts.AddAfter(jobPartNode, dbShelf1);

                            }
                        }
                    }
                }
                var dbShelfs = job.JobParts.Where(item => item.KitStructureItem == KitStructureType.DbShelf).ToArray();
                if (dbShelfs.Length == 0) {
                    throw new InvalidDataException($"В структуре подчиненности для элемента \"{job.Attribute.DbName}\" не указано ни одного элемента конфигурации типа \"Полка\".");
                }
                var plainCounter = new PlainCounter(dbShelfs.Length, sguSet.KnotType.Value).GetEnumerator();
                DbGcKnotLevel level = null;
                int levelCount = 0;
                //var lastCanalBoltingType = ScsGutterBoltingTypeEnum.OTHER;
                foreach (var item in dbShelfs) {
                    plainCounter.MoveNext();
                    string gutterBoltingCode = item.ItemCode;
                    ScsGutterBolting? dbShelf;
                    if (string.IsNullOrEmpty(gutterBoltingCode) || item.ItemQuantity == 0) {
                        dbShelf = null;
                        sguSet.LevelType = DbGcKnotLevelType.NO;
                    }
                    else {
                        dbShelf = context.ScsGutterBoltings.FirstOrDefault(p => p.Code == gutterBoltingCode);

                        if (dbShelf is null) {
                            throw new InvalidDataException($"Элемент с артикулом {gutterBoltingCode} не найден в таблице {Context.GetDefaultLocalizeValue<ScsGutterBolting>()}");
                        }
                        // Проверка изделия на допустимый тип элемента яруса 

                        switch (dbShelf.CanalBoltingType) {
                            case ScsGutterBoltingTypeEnum.PROFILE:
                                sguSet.LevelType = DbGcKnotLevelType.PROFILE;
                                break;
                            case ScsGutterBoltingTypeEnum.CROSSBAR:
                                sguSet.LevelType = DbGcKnotLevelType.CROSSBAR;
                                break;
                            case ScsGutterBoltingTypeEnum.CRAMP:
                                sguSet.LevelType = DbGcKnotLevelType.CRAMP;
                                break;
                            case ScsGutterBoltingTypeEnum.CONSOLE:
                                ScsGcConsoleMountType consoleType = dbShelf.ConsoleMountType!.Value;
                                switch (consoleType) {
                                    case ScsGcConsoleMountType.CELL:
                                        sguSet.LevelType = DbGcKnotLevelType.C_BRACKET;
                                        break;
                                    case ScsGcConsoleMountType.L_WALL:
                                        sguSet.LevelType = DbGcKnotLevelType.L_BRACKET;
                                        break;
                                    default:
                                        sguSet.LevelType = DbGcKnotLevelType.CONSOLE;
                                        break;
                                }
                                break;
                            case ScsGutterBoltingTypeEnum.OTHER:
                                sguSet.LevelType = DbGcKnotLevelType.NO;
                                break;
                            default:
                                StringBuilder sb = new StringBuilder();
                                sb.Append($"Для элемента \"{KitStructureType.DbShelf.GetDescription()}\" указанное изделие \"{dbShelf.CanalBoltingType.GetDescription()}\" недопустимо. ");
                                sb.Append($"Необходимо указать {ScsGutterBoltingTypeEnum.PROFILE.GetDescription()}, ");
                                sb.Append($"{ScsGutterBoltingTypeEnum.CROSSBAR.GetDescription()}, {ScsGutterBoltingTypeEnum.CONSOLE.GetDescription()}, ");
                                sb.Append($"или {ScsGutterBoltingTypeEnum.CRAMP.GetDescription()}.");
                                throw new InvalidDataException(sb.ToString());
                        }
                    }
                    if (sguSet.KnotType == ScsGcStandType.TWO_SIDE && plainCounter.Current.Number == 2) {
                        level.NotNull();
                        //lastCanalBoltingType = dbShelf.CanalBoltingType;
                    }
                    else {
                        level = new DbGcKnotLevel {
                            Height = item.LayerHeight ?? 0,
                            IsEnabled = true,
                            IsUse = true,
                            Number = plainCounter.Current.LayerNumber,
                        };
                        levelCount++;
                        ks.AddChild(level);
                    }
                    var itemItemQuantity = item.ItemQuantity;
                    DbGcKnotPlain dbGcKnotPlain = new DbGcKnotPlain {
                        IsEnabled = true,
                        IsUse = true,
                        Number = plainCounter.Current.Number,
                        ProfileCount = 1,
                        Seria = itemItemQuantity > 0 ? dbShelf.Series : string.Empty,
                        Profile = dbShelf,
                        //Profile = (itemItemQuantity > 0) ? dbShelf : new ScsGutterBolting { Id = -1, CanalBoltingType = lastCanalBoltingType },
                    };
                    level.AddChild(dbGcKnotPlain);
                    var itemUid = item.Uid;
                    foreach (var childItem in job.JobParts.Where(item => item.ParentUid == itemUid).ToArray()) {
                        if (childItem.KitStructureItem == KitStructureType.DbUtilityUnit) {
                            var unitCode = childItem.ItemCode;
                            if (!TryFindDbUtilityUnit(context, unitCode, out var dbUtilityUnit)) {
                                throw new InvalidDataException($"При извлечении продукта \"{unitCode}\" как элемента \"Комплектующие\" произошла ошибка: Элемент среди допустимых комплектующих не найден.");
                            }
                            dbUtilityUnit.SpecCount = childItem.ItemQuantity;
                            dbGcKnotPlain.AddChild(dbUtilityUnit);
                        }
                        else if (childItem.KitStructureItem == KitStructureType.Gutter) {

                        }
                        else {
                            throw new InvalidDataException($"При построении элемента {childItem} произошла ошибка. Для элемента конфигурации \"Полка\" в качестве подчиненного можно указать только элемент\"Лоток\" или \"Комплектующие\". ");
                        }
                    }
                }
                sguSet.LevelCount = levelCount;
                sguSet.KitStructure = GetKitStructureAsXML(sguSet);
                logger.LogInformation("Конфигурация узлов крепления \"{}\" построена успешно.", job.Attribute.DbName);
                return sguSet;
            }
            catch (Exception ex) {
                throw new InvalidOperationException($"При построении конфигурации узла крепления {job.Attribute.DbName} произошла ошибка.", ex);
            }

        }

        internal static DbImage GetOrCreate(Context context, string fullSourceFileName, (string? ImageName, string? Description, string Category) target) {
            string imageName = Path.ChangeExtension(target.ImageName, ".png"); // in nanoCad Electro image always in PNG format
            DbImage image = context.DbImages.Local.FirstOrDefault(p => p.Text == imageName) ??
                context.DbImages.FirstOrDefault(p => p.Text == imageName);
            if (image is null) {
                if (string.IsNullOrEmpty(target.ImageName)) {
                    throw new InvalidDataException("Не задано имя файла изображения.");
                }
                //string imgFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fullSourceFileName);
                FileInfo fi = new FileInfo(fullSourceFileName);
                image = Maker.LoadImage(context, target.ImageName!, target.Category, fi);
                //var image = context.DbImages.Find(idImg);
                image.Description = image.Description;
                //image.Category = target.Category;
            }
            return image;
        }
        private static double? GetGutterBoltingLength(ScsGutterBolting? dbShelf) {
            if (dbShelf is null) { return null; }
            double? profileLength;
            switch (dbShelf.CanalBoltingType) {
                case ScsGutterBoltingTypeEnum.POST:
                case ScsGutterBoltingTypeEnum.STUD:
                    profileLength = dbShelf.Heigth;
                    break;
                case ScsGutterBoltingTypeEnum.CROSSBAR:
                case ScsGutterBoltingTypeEnum.CRAMP:
                case ScsGutterBoltingTypeEnum.CONSOLE:
                    profileLength = dbShelf.Length;
                    break;
                case ScsGutterBoltingTypeEnum.PROFILE:
                    profileLength = dbShelf.ProfileLength;
                    break;
                case ScsGutterBoltingTypeEnum.HUNGER:
                case ScsGutterBoltingTypeEnum.OTHER:
                case ScsGutterBoltingTypeEnum.CRONSH:
                case ScsGutterBoltingTypeEnum.SUPPORT:
                case ScsGutterBoltingTypeEnum.CONNECTOR:
                default:
                    profileLength = null;
                    break;
            }
            return profileLength;
        }
        private static DbScsGcCoverUnit? TryFindDbScsGcCoverUnit(Context context, string itemCode) {
            var gcPart = context.DbScsGcCoverUnits
                .Select(sgc => new { Code = sgc.Code, Series = sgc.Series, CoverType = sgc.CoverType })
                .AsNoTracking()
                .FirstOrDefault(sgc => sgc.Code == itemCode);
            if (gcPart is null)
                return null;
            DbScsGcCoverUnit coverUnit = new DbScsGcCoverUnit {
                Code = gcPart.Code,
                Series = gcPart.Series,
                CoverType = gcPart.CoverType
            };
            return coverUnit;
        }
        private static ScsGcFitting? TryFindScsGcFitting(Context context, string itemCode) {
            var gcPart = context.ScsGcFittings
                .AsNoTracking()
                .FirstOrDefault(sgc => sgc.Code == itemCode);
            return gcPart;
        }
        private static DbScsGcAccessoryUnit? TryFindDbScsGcAccessoryUnit(Context context, string itemCode) {
            var gcPart = context.DbScsGcAccessoryUnits
                .AsNoTracking()
                .FirstOrDefault(sgc => sgc.Code == itemCode);
            if (gcPart is null ||
                !gcPart.AccessorySelectType.HasValue ||
                gcPart.AccessorySelectType == DbGcMsAccessorySelectType.NO)
                return null;
            return gcPart;
        }
        /// <summary>
        /// Производит поиск элемента для использования как элемента конфигурации.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="code">Артикул элемента.</param>
        /// <returns>Если элемента с <paramref name="code"/> найден, возвращается клонированный экземпляр.</returns>
        private static bool TryFindDbUtilityUnit(Context context, string code, [NotNullWhen(true)] out DbUtilityUnit? dbUtilityUnit) {
            // Убедимся, что локальные коллекции загружены в память
            EnsureLoaded(context);

            // Поиск по Local (уже в памяти, с нормальным сравнением строк)
            DbUtilityUnit? unit;
            do {
                unit = context.CaeMaterialUtilities.Local.FirstOrDefault(u => string.Equals(u.Code, code, StringComparison.Ordinal));
                if (unit is not null)
                    break;
                unit = context.DbCaeMaterialUtilitySets.Local.FirstOrDefault(u => string.Equals(u.DbName, code, StringComparison.Ordinal));
                if (unit is not null)
                    break;
                unit = context.DbScsGcAccessoryUnits.Local.FirstOrDefault(u => string.Equals(u.Code, code, StringComparison.Ordinal));
                if (unit is not null)
                    break;
                unit = context.DbScsGcBoltingAccessoryUnits.Local.FirstOrDefault(u => string.Equals(u.Code, code, StringComparison.Ordinal));
                if (unit is not null)
                    break;
                unit = context.ScsGutterBoltings.Local.FirstOrDefault(u => string.Equals(u.Code, code, StringComparison.Ordinal));
            } while (false);
            if (unit is not null) {
                dbUtilityUnit = (DbUtilityUnit)unit.Clone();
                return true;
            }

            dbUtilityUnit = null;
            return false;
        }
        /// <summary>
        /// Гарантирует, что таблицы загружены в память (Local).
        /// Вызов безопасен несколько раз – повторной загрузки не будет.
        /// </summary>
        private static void EnsureLoaded(Context context) {
            if (!context.CaeMaterialUtilities.Local.Any())
                context.CaeMaterialUtilities.Load();
            if (!context.DbCaeMaterialUtilitySets.Local.Any())
                context.DbCaeMaterialUtilitySets.Load();
            if (!context.DbScsGcAccessoryUnits.Local.Any())
                context.DbScsGcAccessoryUnits.Load();
            if (!context.DbScsGcBoltingAccessoryUnits.Local.Any())
                context.DbScsGcBoltingAccessoryUnits.Load();
            if (!context.ScsGutterBoltings.Local.Any())
                context.ScsGutterBoltings.Load();
        }
        //private static bool TryFindDbUtilityUnit(Context context, string code, out DbUtilityUnit dbUtilityUnit) {
        //    DbUtilityUnit unit;
        //    unit = context.CaeMaterialUtilities.FirstOrDefault(p => p.Code == code);
        //    if (unit is not null) {
        //        dbUtilityUnit = (DbUtilityUnit)unit.Clone();
        //        return true;
        //    }
        //    unit = context.DbCaeMaterialUtilitySets.FirstOrDefault(p => p.DbName == code);
        //    if (unit is not null) {
        //        dbUtilityUnit = (DbUtilityUnit)unit.Clone();
        //        return true;
        //    }
        //    unit = context.DbScsGcAccessoryUnits.FirstOrDefault(p => p.Code == code);
        //    if (unit is not null) {
        //        dbUtilityUnit = (DbUtilityUnit)unit.Clone();
        //        return true;
        //    }
        //    unit = context.DbScsGcBoltingAccessoryUnits.FirstOrDefault(p => p.Code == code);
        //    if (unit is not null) {
        //        dbUtilityUnit = (DbUtilityUnit)unit.Clone();
        //        return true;
        //    }
        //    unit = context.ScsGutterBoltings.FirstOrDefault(p => p.Code == code);
        //    if (unit is not null) {
        //        dbUtilityUnit = (DbUtilityUnit)unit.Clone();
        //        return true;
        //    }
        //    dbUtilityUnit = null;
        //    return false;
        //}
        private static MountSystemSetJob ParceMountSystemSetJob(SheetCommon.Row row, List<MountSystemSetJobPart> mssJobParts) {
            return new MountSystemSetJob(
                Code: row["Code"], Material: row["Исполнение"],
                Attribute: (row["DbName"], row["DbDescription"], row["Description"], row["DbCatalog"], row["DbImageRef"],
                    PostDistance: double.TryParse(row["PostDistance"], PositiveNumberStyle, CultureInfo.InvariantCulture, out double height) ? height :
                        double.TryParse(row["PostDistance"], PositiveNumberStyle, CultureInfo.GetCultureInfo("Ru-ru"), out height) ? height :
                        throw new InvalidDataException($"Значение \"{row["PostDistance"]}\" столбца \"PostDistance\" для элемента {row["Code"]} заполнено некорректно. Необходимо внести число от 0,0 до 9,0."),
                    MountType: EnumConverter<DbGcKnotInstallType>.TryConvert(row["MountType"], out var mountType) ? mountType :
                        throw new InvalidDataException($"Значение \"{row["MountType"]}\" столбца \"MountType\" для элемента {row["Code"]} заполнено некорректно. Допустимые значения \"{EnumConverter<DbGcKnotInstallType>.GetDescription(DbGcKnotInstallType.CEIL)}\", {EnumConverter<DbGcKnotInstallType>.GetDescription(DbGcKnotInstallType.GROUND)}\", либо \"{EnumConverter<DbGcKnotInstallType>.GetDescription(DbGcKnotInstallType.WALL)}\"."),
                    KnotType: EnumConverter<ScsGcStandType>.TryConvert(row["KnotType"], out var standType) ? standType :
                        throw new InvalidDataException($"Значение \"{row["KnotType"]}\" столбца \"KnotType\" для элемента {row["Code"]} заполнено некорректно. Допустимые значения \"{EnumConverter<ScsGcStandType>.GetDescription(ScsGcStandType.ONE_SIDE)}\", либо \"{EnumConverter<ScsGcStandType>.GetDescription(ScsGcStandType.TWO_SIDE)}\"."),
                    StructureType: EnumConverter<DbGcKnotStandStructureType>.TryConvert(row["StructureType"], out var structureType) ? structureType :
                        throw new InvalidDataException($"Значение \"{row["StructureType"]}\" столбца \"StructureType\" для элемента {row["Code"]} заполнено некорректно. Допустимые значения \"{EnumConverter<DbGcKnotStandStructureType>.GetDescription(DbGcKnotStandStructureType.ONE)}\", \"{EnumConverter<DbGcKnotStandStructureType>.GetDescription(DbGcKnotStandStructureType.TWO)}\" либо \"{EnumConverter<DbGcKnotStandStructureType>.GetDescription(DbGcKnotStandStructureType.BACK_TO_BACK)}\".")
                    ),
                JobParts: new LinkedList<MountSystemSetJobPart>(mssJobParts.Where(item => item.SetCode == row["Code"] & item.Material == row["Исполнение"]))

                );
        }

        private static MountSystemSetJobPart ParceMountSystemSetJobPart(SheetCommon.Row row, string coatingTypeName) {
            string setCode = row["Code"]
            ?? throw new InvalidDataException("Столбец 'Code' не может быть пустым.");

            string itemCode = row[coatingTypeName]
                ?? throw new InvalidDataException($"Столбец '{coatingTypeName}' не может быть пустым (элемент {setCode}).");

            if (!int.TryParse(row["Quantity"], out int itemQuantity))
                throw new InvalidDataException($"Значение \"{row["Quantity"]}\" столбца 'Quantity' для элемента {setCode} невозможно привести к целому числу.");

            if (!EnumConverter<KitStructureType>.TryConvert(row["Элемент конфигурации"], out var kitStructureValue))
                throw new InvalidDataException($"Значение \"{row["Элемент конфигурации"]}\" столбца 'Элемент конфигурации' для элемента {setCode} не входит в перечень возможных значений.");

            if (!int.TryParse(row["Uid"], out int uid))
                throw new InvalidDataException($"Значение \"{row["Uid"]}\" столбца 'Uid' для элемента {setCode} невозможно привести к целому числу.");

            int? parentUid = int.TryParse(row["ParentUid"], out int parent) ? parent : null;
            int? totalLayersHeight = int.TryParse(row["TotalLayersHeight"], out int totalHeight) ? totalHeight : null;
            int? layerHeight = int.TryParse(row["LayerHeight"], out int lHeight) ? lHeight : null;

            DbGcStrightSegmentComplectType? complectType =
                EnumConverter<DbGcStrightSegmentComplectType>.TryConvert(row["ComplectType"], out var cType)
                    ? cType : null;
            MountSystemSetJobPart value = new MountSystemSetJobPart(
            SetCode: setCode,
            Material: coatingTypeName,
            ItemCode: itemCode,
            ItemQuantity: itemQuantity,
            KitStructureItem: kitStructureValue,
            Uid: uid,
            ParentUid: parentUid,
            TotalLayersHeight: totalLayersHeight,
            LayerHeight: layerHeight,
            ComplectType: complectType
        );
            
            return value;
        }
        private static string GetKitStructureAsXML<Tkit>(Tkit kit) where Tkit : KitElement {
            var serializer = new XmlSerializer(typeof(Tkit));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings)) {
                serializer.Serialize(xmlWriter, kit);
            }
            string xml = stringBuilder.ToString();
            return xml;
        }

    }
}
