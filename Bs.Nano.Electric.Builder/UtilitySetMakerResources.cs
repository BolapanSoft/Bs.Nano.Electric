// Ignore Spelling: Gc

using Bs.XML.SpreadSheet;
using Microsoft.Extensions.Configuration;
using Nano.Electric;
using Nano.Electric.Enums;
using System.Globalization;
using static Bs.Nano.Electric.Builder.ResourceManager;


#if NET48
using System.Data.Entity; // EF6
#else
#endif

namespace Bs.Nano.Electric.Builder {
    public abstract class UtilitySetMakerResources {
        //private class LazyEnumerable<T>(Func<IEnumerable<T>> ctor) : IEnumerable<T> {
        //    public IEnumerator<T> GetEnumerator() {
        //        return ctor().GetEnumerator();
        //    }

        //    IEnumerator IEnumerable.GetEnumerator() {
        //        return GetEnumerator();
        //    }
        //}
#if NETFRAMEWORK
        public class MountSystemSetJobPart {
            public string SetCode { get; set; }
            public string SetCodeSuffix { get; set; }
            public string Material { get; set; }
            public string ItemCode { get; set; }
            public int ItemQuantity { get; set; }
            public KitStructureType KitStructureItem { get; set; }
            public int Uid { get; set; }
            public int? ParentUid { get; set; }
            public int? TotalLayersHeight { get; set; }
            public int? LayerHeight { get; set; }
            public DbGcStrightSegmentComplectType? ComplectType { get; set; }

            public MountSystemSetJobPart(string SetCode, string SetCodeSuffix, string Material, string ItemCode, int ItemQuantity,
                KitStructureType KitStructureItem, int Uid, int? ParentUid, int? TotalLayersHeight,
                int? LayerHeight, DbGcStrightSegmentComplectType? ComplectType) {
                this.SetCode = SetCode;
                this.SetCodeSuffix = SetCodeSuffix;
                this.Material = Material;
                this.ItemCode = ItemCode;
                this.ItemQuantity = ItemQuantity;
                this.KitStructureItem = KitStructureItem;
                this.Uid = Uid;
                this.ParentUid = ParentUid;
                this.TotalLayersHeight = TotalLayersHeight;
                this.LayerHeight = LayerHeight;
                this.ComplectType = ComplectType;
            }
        }

        public class MountSystemSetJob {
            public string Code { get; set; }
            public string CodeSuffix { get; set; }
            public string Material { get; set; }
            public GcMountSystemJob Attribute { get; set; }
            public LinkedList<MountSystemSetJobPart> JobParts { get; set; }

            public MountSystemSetJob(
                string Code, string CodeSuffix, string Material,
                GcMountSystemJob Attribute,
                LinkedList<MountSystemSetJobPart> JobParts) {
                this.Code = Code;
                this.CodeSuffix = CodeSuffix;
                this.Material = Material;
                this.Attribute = Attribute;
                this.JobParts = JobParts;
            }
        }

        public class UtilityUnit {
            public string Code { get; set; }
            public string UtilityUnitCode { get; set; }
            public int SpecCount { get; set; }

            public UtilityUnit(string Сode, string UtilityUnitCode, int SpecCount = 1) {
                this.Code = Сode;
                this.UtilityUnitCode = UtilityUnitCode;
                this.SpecCount = SpecCount;
            }
        }

        public class CcMountSystemSetJob {
            public string DbName { get; set; }
            public string DbNaming { get; set; }
            public string DbDescription { get; set; }
            public string DbImageRef { get; set; }
            public (string CableCanalCode, int PartitionCount) CableCanal { get; set; }
            public (bool IsUse, double PostDistance) CcMsMount { get; set; }
            public IEnumerable<UtilityUnit> UtilityUnits { get; set; }

            public CcMountSystemSetJob(
                    string DbName,
                    string DbNaming,
                    string DbDescription,
                    string DbImageRef,
                    (string, int) CableCanal,
                    (bool, double) CcMsMount,
                    IEnumerable<UtilityUnit> UtilityUnits) {
                this.DbName = DbName;
                this.DbNaming = DbNaming;
                this.DbDescription = DbDescription;
                this.DbImageRef = DbImageRef;
                this.CableCanal = CableCanal;
                this.CcMsMount = CcMsMount;
                this.UtilityUnits = UtilityUnits;
            }
        }

        public class TbMountSystemJob {
            public string DbName { get; set; }
            public string DbNaming { get; set; }
            public string DbDescription { get; set; }
            public string DbImageRef { get; set; }
            public string TubeCanalCode { get; set; }
            public (bool IsUse, double PostDistance) CcMsMount { get; set; }
            public IEnumerable<UtilityUnit> UtilityUnits { get; set; }

            public TbMountSystemJob(
                    string DbName,
                    string DbNaming,
                    string DbDescription,
                    string DbImageRef,
                    string TubeCanalCode,
                    (bool, double) CcMsMount,
                    IEnumerable<UtilityUnit> UtilityUnits) {
                this.DbName = DbName;
                this.DbNaming = DbNaming;
                this.DbDescription = DbDescription;
                this.DbImageRef = DbImageRef;
                this.TubeCanalCode = TubeCanalCode;
                this.CcMsMount = CcMsMount;
                this.UtilityUnits = UtilityUnits;
            }
        }

        public class DbScsTubeSeriesConfigurationJob {
            public string DbName { get; set; }
            public string DbDescription { get; set; }
            public string TubeCode { get; set; }
            public string AngleCode { get; set; }
            public string TripleCode { get; set; }
            public string CrossCode { get; set; }

            public DbScsTubeSeriesConfigurationJob(
                   string DbName,
                   string DbDescription,
                   string TubeCode,
                   string AngleCode,
                   string TripleCode,
                   string CrossCode) {
                this.DbName = DbName;
                this.DbDescription = DbDescription;
                this.TubeCode = TubeCode;
                this.AngleCode = AngleCode;
                this.TripleCode = TripleCode;
                this.CrossCode = CrossCode;
            }

        }

        public class SeriesConfigurationPartItem {
            public string Code { get; set; }
            public int Amount { get; set; }

            public SeriesConfigurationPartItem(string Code, int Amount) {
                this.Code = Code;
                this.Amount = Amount;
            }
        }

        public class SeriesConfigurationStraightPartItem : SeriesConfigurationPartItem {
            public DbGcStrightSegmentComplectType ComplectType { get; set; }

            public SeriesConfigurationStraightPartItem(string Code, int Amount, DbGcStrightSegmentComplectType ComplectType)
                : base(Code, Amount) {
                this.ComplectType = ComplectType;
            }
        }

        public class SeriesConfigurationJobPart {
            public string SectionName { get; set; }
            public IEnumerable<SeriesConfigurationPartItem> Parts { get; set; }

            public SeriesConfigurationJobPart(string SectionName, IEnumerable<SeriesConfigurationPartItem> Parts) {
                this.SectionName = SectionName;
                this.Parts = Parts;
            }
        }

        public class SeriesConfigurationJob {
            public string Code { get; set; }
            public (string DbDescription, string Description, string DbCatalog) Attribute { get; set; }
            public IEnumerable<SeriesConfigurationJobPart> Sections { get; set; }

            public SeriesConfigurationJob(string Code, (string, string, string) Attribute,
                IEnumerable<SeriesConfigurationJobPart> Sections) {
                this.Code = Code;
                this.Attribute = Attribute;
                this.Sections = Sections;
            }
        }
        /// <summary>
        /// Представляет элемент таблицы Узлы крепления
        /// </summary>
        public class ScsGutterUtilitySetJob {
            public string Code { get; init; }
            public string DbName { get; init; }
            public string DbDescription { get; init; }
            public string Description { get; init; }
            public string Material { get; init; }
            public string DbCatalog { get; init; }
            public string DbImageRef { get; init; }
            public DbGcKnotInstallType InstallType { get; init; }
            public DbGcKnotStandStructureType StructureType { get; init; }
            public ScsGcStandType KnotType { get; init; }
   
            public ScsGutterUtilitySetJob(
                string Code,
                string DbName,
                string DbDescription,
                 string Description,
               string Material,
                string DbCatalog,
                string DbImageRef,
                DbGcKnotInstallType InstallType,
                DbGcKnotStandStructureType StructureType,
                ScsGcStandType KnotType) {
                this.Code = Code;
                this.DbName = DbName;
                this.DbDescription = DbDescription;
                this.Description = Description;
                this.Material = Material;
                this.DbCatalog = DbCatalog;
                this.DbImageRef = DbImageRef;
                this.InstallType = InstallType;
                this.StructureType = StructureType;
                this.KnotType = KnotType;
                
            }
        }

        /// <summary>
        /// Представляет элемент таблицы Трассы лотков
        /// </summary>
        public class GcMountSystemJob {
            public string Code { get; init; }
            public string CodeSuffix { get; init; }
            public string StandDbName { get; init; }
            public string DbName { get; init; }
            public string DbNaming { get; init; }
            public string DbDescription { get; init; }
            public string Material { get; init; }
            public string DbCatalog { get; init; }
            public string DbImageRef { get; init; }
            public double PostDistance { get; init; }

            public GcMountSystemJob(
                string Code,
                string CodeSuffix,
                string StandDbName,
                string DbName,
                string DbNaming,
                string DbDescription,
                string Material,
                string DbCatalog,
                string DbImageRef,
                double PostDistance) {
                this.Code = Code;
                this.CodeSuffix = CodeSuffix;
                this.StandDbName = StandDbName;
                this.DbName = DbName;
                this.DbNaming = DbNaming;
                this.DbDescription = DbDescription;
                this.Material = Material;
                this.DbCatalog = DbCatalog;
                this.DbImageRef = DbImageRef;
                this.PostDistance = PostDistance;
            }
        }
        public record DbScsGutterUtilitySetJob(
            ScsGutterUtilitySetJob Attribute,
            IEnumerable<MountSystemSetJobPart> JobParts);

#else
        public record MountSystemSetJobPart(
            string SetCode, 
            string SetCodeSuffix, 
            string Material, 
            string ItemCode, 
            int ItemQuantity, 
            KitStructureType KitStructureItem, 
            int Uid, 
            int? ParentUid, 
            int? TotalLayersHeight, 
            int? LayerHeight, 
            DbGcStrightSegmentComplectType? ComplectType);
        public record MountSystemSetJob(string Code, string CodeSuffix,
            string Material,
            GcMountSystemJob Attribute,
            LinkedList<MountSystemSetJobPart> JobParts);
        public record DbScsGutterUtilitySetJobPart(
            DbScsGutterUtilitySetJob Parent,
            string ItemCode,
            int ItemQuantity,
            KitStructureType KitStructureItem,
            int Uid,
            int? ParentUid,
            int? TotalLayersHeight,
            int? LayerHeight);
        public record DbScsGutterUtilitySetJob(
            ScsGutterUtilitySetJob Attribute,
            IEnumerable<MountSystemSetJobPart> JobParts);
        public record UtilityUnit(string Code, string UtilityUnitCode, int SpecCount = 1);
        /// <summary>
        /// Представляет элемент таблицы Узлы крепления
        /// </summary>
        public record ScsGutterUtilitySetJob(
            string Code,
            string DbName,
            string DbDescription,
            string Description,
            string Material,
            string DbCatalog,
            string DbImageRef,
            DbGcKnotInstallType InstallType,
            DbGcKnotStandStructureType StructureType,
            ScsGcStandType KnotType);
        /// <summary>
        /// Представляет элемент таблицы Трассы лотков
        /// </summary>
        public record GcMountSystemJob(
            string Code,
            string CodeSuffix,
            string StandDbName,
            string DbName,
            string DbNaming,
            string DbDescription,
            string Material,
            string DbCatalog,
            string DbImageRef,
            double PostDistance
           );
        /// <summary>
        /// Представляет задание на построение конфигурации трасс настенных коробов.
        /// </summary>
        public record CcMountSystemSetJob(
            string DbName, string DbNaming, string DbDescription, string DbImageRef,
            (string CableCanalCode, int PartitionCount) CableCanal,
            (bool IsUse, double PostDistance) CcMsMount,
            IEnumerable<UtilityUnit> UtilityUnits);
        /// <summary>
        /// Представляет задание на построение конфигурации трасс труб.
        /// </summary>
        public record TbMountSystemJob(
            string DbName, string DbNaming, string DbDescription, string DbImageRef,
            string TubeCanalCode,
            (bool IsUse, double PostDistance) CcMsMount,
            IEnumerable<UtilityUnit> UtilityUnits);
        /// <summary>
        /// Представляет задание на построение конфигурации соединительных элементов труб.
        /// </summary>
        public record DbScsTubeSeriesConfigurationJob(string DbName, string DbDescription, string TubeCode, string AngleCode, string TripleCode, string CrossCode);
        public record SeriesConfigurationPartItem(string Code, int Amount);
        public record SeriesConfigurationStraightPartItem(string Code, int Amount, DbGcStrightSegmentComplectType ComplectType) : SeriesConfigurationPartItem(Code, Amount);
        /// <summary>
        /// Представляет секцию KitStructure.
        /// </summary>
        public record SeriesConfigurationJobPart(string SectionName, IEnumerable<SeriesConfigurationPartItem>? Parts);
        //public record SeriesConfigurationJobPart(string SetCode, string ItemCode, int ItemQuantity, string KitStructureItem, int Uid, int ParentUid);
        /// <summary>
        /// Представляет задание на создание конфигурации соединительных элементов лотков.
        /// </summary>
        /// <param name="Code">Обозначение</param>
        /// <param name="JobParts">Секции KitStructure</param>
        public record SeriesConfigurationJob(string Code, (string DbDescription, string Description, string DbCatalog) Attribute, IEnumerable<SeriesConfigurationJobPart> Sections);
#endif
        private readonly IElectricBuilderConfiguration configuration;
        private readonly ResourceManager resourceManager;
        private readonly Lazy<IEnumerable<SheetCommon.Row>> lzGutterUtilitySetSource;
        //private readonly Lazy<IEnumerable<SheetCommon.Row>> lzDbGcMountSystemSet;
        private readonly Lazy<IEnumerable<SheetCommon.Row>> lzScsGutterUtilitySet;
        private readonly Lazy<IEnumerable<SheetCommon.Row>> lzGcMountSystemSet;

        private readonly Lazy<IEnumerable<CcMountSystemSetJob>> lzCcMountSystemSource;
        private readonly Lazy<IEnumerable<TbMountSystemJob>> lzDbTbMountSystemSource;

        private readonly Lazy<IEnumerable<DbScsTubeSeriesConfigurationJob>> lzDbScsTubeSeriesConfigurationSource;
        private readonly Lazy<Dictionary<string, SeriesConfigurationJob>> lzSeriesConfigurationJobs;
        private readonly Lazy<Dictionary<string, string>> lzSeriaConfigurationMapping;
        public UtilitySetMakerResources(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            this.configuration = configuration;
            this.resourceManager = resourceManager;
            lzGutterUtilitySetSource = new Lazy<IEnumerable<SheetCommon.Row>>(GetGutterUtilitySetSource(resourceManager, configuration));
            //lzDbGcMountSystemSet = new Lazy<IEnumerable<SheetCommon.Row>>(GetDbGcMountSystemSet(resourceManager, configuration));
            lzScsGutterUtilitySet = new(GetScsGutterUtilitySet(resourceManager, configuration));
            lzGcMountSystemSet = new(GetGcMountSystemSet(resourceManager, configuration));

            lzSeriesConfigurationJobs = new(GetSeriesConfigurationJobs(resourceManager, configuration), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
            lzCcMountSystemSource = new(GetCcMountSystemJobs(resourceManager, configuration), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
            lzDbScsTubeSeriesConfigurationSource = new(GetDbScsTubeSeriesConfigurationJobs(resourceManager, configuration), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
            lzDbTbMountSystemSource = new(GetDbTbMountSystemJobs(resourceManager, configuration), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
            lzSeriaConfigurationMapping = new(GetSeriaConfigurationMapping(resourceManager, configuration), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }


        /// <summary>
        /// Перечень алиасов для материалов и типов покрытия.
        /// </summary>
        public abstract IEnumerable<string> KnownMaterials { get; }
        /// <summary>
        /// Крепления лотков. Комплектации узлов крепления.
        /// </summary>
        /// <exception cref="SectionNotFoundException">В конфигурации не определена секция или один из необходимых ключей секции.</exception>
        public IEnumerable<SheetCommon.Row> GutterUtilitySetSource => lzGutterUtilitySetSource.Value;
        /// <summary>
        /// Конфигурации КНС. Конфигурации трасс лотков
        /// </summary>
        /// <exception cref="SectionNotFoundException">В конфигурации не определена секция или один из необходимых ключей секции.</exception>
        //public IEnumerable<SheetCommon.Row> DbGcMountSystemSet => lzDbGcMountSystemSet.Value;
        /// <summary>
        /// Конфигурации узлов крепления. Общие параметры
        /// </summary>
        /// <exception cref="SectionNotFoundException">В конфигурации не определена секция или один из необходимых ключей секции.</exception>
        public IEnumerable<SheetCommon.Row> ScsGutterUtilitySet => lzScsGutterUtilitySet.Value;
        /// <summary>
        /// Конфигурации трасс лотков. Общие параметры
        /// </summary>
        /// <exception cref="SectionNotFoundException">В конфигурации не определена секция или один из необходимых ключей секции.</exception>
        public IEnumerable<SheetCommon.Row> GcMountSystemSet => lzGcMountSystemSet.Value;
        /// <summary>
        /// Конфигурации КНС. Конфигурации трасс настенных коробов
        /// </summary>
        /// <exception cref="SectionNotFoundException">В конфигурации не определена секция или один из необходимых ключей секции.</exception>
        public IEnumerable<CcMountSystemSetJob> CcMountSystemSource => lzCcMountSystemSource.Value;
        /// <summary>
        /// Конфигурации КНС. Конфигурации трасс труб.
        /// </summary>
        /// <exception cref="SectionNotFoundException">В конфигурации не определена секция или один из необходимых ключей секции.</exception>
        public IEnumerable<TbMountSystemJob> DbTbMountSystemSource => lzDbTbMountSystemSource.Value;
        /// <summary>
        /// Конфигурации КНС. Трубы. Конфигурации соединительных элементов.
        /// </summary>
        /// <exception cref="SectionNotFoundException">В конфигурации не определена секция или один из необходимых ключей секции.</exception>
        public IEnumerable<DbScsTubeSeriesConfigurationJob> DbScsTubeSeriesConfigurationSource => lzDbScsTubeSeriesConfigurationSource.Value;
        /// <summary>
        /// КНС#Задание на Конфигурации соединительных элементов.xlsx
        /// </summary>
        /// <exception cref="SectionNotFoundException">В конфигурации не определена секция или один из необходимых ключей секции.</exception>
        public Dictionary<string, SeriesConfigurationJob> SeriesConfigurationJobs => lzSeriesConfigurationJobs.Value;
        public Dictionary<string, string> SeriaConfigurationMapping => lzSeriaConfigurationMapping.Value;
        /// <summary>
        /// Загружает Конфигурации узлов крепления. Общие параметры
        /// </summary>
        /// <param name="resourceManager"></param>
        private Func<IEnumerable<SheetCommon.Row>> GetScsGutterUtilitySet(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {
                var section = configuration.GetSection("KitStructureSource:ScsGutterUtilitySetSource");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:ScsGutterUtilitySetSource\" (Конфигурации узлов крепления. Общие параметры).");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:ScsGutterUtilitySetSource\" (Конфигурации узлов крепления. Общие параметры) не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);

                SheetCommon setSource = resourceManager.LoadShc<object>(shref);
                return setSource;
            };
        }
        /// <summary>
        /// Загружает Конфигурации трасс лотков. Общие параметры
        /// </summary>
        /// <param name="resourceManager"></param>
        private Func<IEnumerable<SheetCommon.Row>> GetGcMountSystemSet(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {
                var section = configuration.GetSection("KitStructureSource:GcMountSystemSetSource");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:GcMountSystemSetSource\" (Конфигурации трасс лотков. Общие параметры).");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:GcMountSystemSetSource\" (Конфигурации трасс лотков. Общие параметры) не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);

                SheetCommon setSource = resourceManager.LoadShc<object>(shref);
                return setSource;
            };

        }
        private static Func<Dictionary<string, SeriesConfigurationJob>> GetSeriesConfigurationJobs(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {
                var section = configuration.GetSection("KitStructureSource:SeriaConfigurationSource");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:SeriaConfigurationSource\" (Конфигурации соединительных элементов).");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:SeriaConfigurationSource\" (Конфигурации соединительных элементов) не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);

                SheetCommon setSource = resourceManager.LoadShc<object>(shref);
                SeriesConfigurationPartItem createPartItem(string partName, SheetCommon.Row item) {
                    switch (partName) {
                        case nameof(DbGcStrightSegment):
                        case nameof(DbGcStrightPartition): {
                                return new SeriesConfigurationStraightPartItem(
                                    Code: item[partName],
                                    Amount: int.TryParse(item[$"Count {partName}"], out int amount) ? amount :
                                        throw new InvalidDataException($"Для элемента {partName}:{item[partName]} неверно указано количество."),
                                    ComplectType: EnumConverter<DbGcStrightSegmentComplectType>.TryConvert(item[$"ComplectType {partName}"], out var complectType) ? complectType :
                                        DbGcStrightSegmentComplectType.SEGMENT
                                    );
                            }
                        default: {
                                return new SeriesConfigurationPartItem(
                                    Code: item[partName],
                                    Amount: int.TryParse(item[$"Count {partName}"], out int amount) ? amount : throw new InvalidDataException($"Для элемента {partName}:{item[partName]} неверно указано количество.")
                                );

                            }
                    }
                }
                Dictionary<string, SeriesConfigurationJob> dic = setSource.Where(row => !string.IsNullOrWhiteSpace(row["DbName"]))
                    .GroupBy(row => row["DbName"])
                    .Select(gr => new SeriesConfigurationJob(Code: gr.Key,
                        Attribute: (gr.First()["DbDescription"], gr.First()["Description"], gr.First()["DbCatalog"]),
                        Sections: SeriaConfigirationKit.PartNames
                        .Select(partName => new SeriesConfigurationJobPart(SectionName: partName,
                        Parts: gr.Where(item => !string.IsNullOrEmpty(item[partName]))
                        .Select(item => createPartItem(partName, item))
                        ))
                    ))
                    .ToDictionary(scj => scj.Code);
                return dic;

            };
        }
        private static Func<IEnumerable<TbMountSystemJob>> GetDbTbMountSystemJobs(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {

                var section = configuration.GetSection("KitStructureSource:DbTbMountSystemSource");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:DbTbMountSystemSource\" (Конфигурации трасс труб).");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:DbTbMountSystemSource\" (Конфигурации трасс труб) не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string utilityUnitsSheet = section["SheetUtilityUnits"] ?? "Элементы крепления";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);
                SheetRef shrefUnitsSource = new(fullFileName, utilityUnitsSheet);

                SheetCommon utilityUnitsSource = resourceManager.LoadShc<object>(shrefUnitsSource);
                SheetCommon setSource = resourceManager.LoadShc<object>(shref);
                var jobs = setSource
                .Select(row => new TbMountSystemJob(
                    row["DbName"],
                    row["DbNaming"],
                    row["DbDescription"],
                    row["DbImageRef"],
                    row["TubeCanalCode"],
                    CcMsMount: (IsUse: row["IsUse"] == "Да",
                        PostDistance: double.TryParse(row["PostDistance"], NumberStyles.Number, CultureInfo.GetCultureInfo("Ru-ru"), out double dist) ? dist :
                            double.TryParse(row["PostDistance"], NumberStyles.Number, CultureInfo.InvariantCulture, out dist) ? dist : 1.0),
                    UtilityUnits: string.IsNullOrEmpty(row["Code"]) ? Array.Empty<UtilityUnit>() :
                        utilityUnitsSource
                            .Where(r => r["Code"] == row["Code"])
                            .Select(r => new UtilityUnit(r["Code"], UtilityUnitCode: r["UtilityUnitCode"], SpecCount: int.TryParse(r["SpecCount"], out int spCount) ? spCount : 1))
                            .ToArray()
                ))
                .Where(el => !string.IsNullOrEmpty(el.DbName));
                return jobs;

            };
        }
        private static Func<IEnumerable<SheetCommon.Row>> GetDbGcMountSystemSet(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {
                var section = configuration.GetSection("KitStructureSource:MountSystemSetSource");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:MountSystemSetSource\" (Конфигурации трасс лотков).");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:MountSystemSetSource\" (Конфигурации трасс лотков) не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);
                return resourceManager.LoadShc<object>(shref);
            };
        }
        private static Func<IEnumerable<SheetCommon.Row>> GetGutterUtilitySetSource(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {
                var section = configuration.GetSection("KitStructureSource:GutterUtilitySetSource");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:GutterUtilitySetSource\" (Конфигурации узлов крепления).");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:GutterUtilitySetSource\" (Конфигурации узлов крепления) не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);
                return resourceManager.LoadShc<object>(shref);
            };
        }
        private static Func<IEnumerable<CcMountSystemSetJob>> GetCcMountSystemJobs(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {
                var section = configuration.GetSection("KitStructureSource:CcMountSystemSource");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:CcMountSystemSource\" (Конфигурации трасс настенных коробов).");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:CcMountSystemSource\" (Конфигурации трасс настенных коробов) не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);
                string utilityUnitsSheet = section["SheetName"] ?? "Элементы крепления";

                SheetCommon setSource = resourceManager.LoadShc<object>(shref);
                SheetCommon utilityUnitsSource = resourceManager.LoadShc<object>(new(fullFileName, utilityUnitsSheet));
                IEnumerable<CcMountSystemSetJob> jobs = setSource
                    .Select(row => new CcMountSystemSetJob(
                        row["DbName"],
                        row["DbNaming"],
                        row["DbDescription"],
                        row["DbImageRef"],
                        CableCanal: new(row["CableCanalCode"], int.TryParse(row["PartitionCount"], out int i) ? i : 0),
                        CcMsMount: (IsUse: row["IsUse"] == "Да",
                            PostDistance: double.TryParse(row["PostDistance"], NumberStyles.Number, CultureInfo.GetCultureInfo("Ru-ru"), out double dist) ? dist :
                                double.TryParse(row["PostDistance"], NumberStyles.Number, CultureInfo.InvariantCulture, out dist) ? dist : 1.0),
                        UtilityUnits: string.IsNullOrEmpty(row["Code"]) ? Array.Empty<UtilityUnit>() :
                            utilityUnitsSource
                                .Where(r => r["Code"] == row["Code"])
                                .Select(r => new UtilityUnit(r["Code"], UtilityUnitCode: r["UtilityUnitCode"], SpecCount: int.TryParse(r["SpecCount"], out int spCount) ? spCount : 1))
                                .ToArray()
                        ))
                    .Where(el => !string.IsNullOrEmpty(el.DbName));
                return jobs;

            };
        }

        private Func<IEnumerable<DbScsTubeSeriesConfigurationJob>> GetDbScsTubeSeriesConfigurationJobs(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {
                var section = configuration.GetSection("KitStructureSource:DbScsTubeSeriesConfigurationSource");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:DbScsTubeSeriesConfigurationSource\" (Конфигурации соединительных элементов труб).");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:DbScsTubeSeriesConfigurationSource\" (Конфигурации соединительных элементов труб) не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);

                SheetCommon setSource = resourceManager.LoadShc<object>(shref);
                var jobs = setSource
                    .Select(row => new DbScsTubeSeriesConfigurationJob(
                        row["DbName"],
                        row["DbDescription"],
                        row["TubeCode"],
                        row["AngleCode"],
                        row["TripleCode"],
                        row["CrossCode"]
                        ));
                return jobs;

            };
        }
        private Func<Dictionary<string, string>> GetSeriaConfigurationMapping(ResourceManager resourceManager, IElectricBuilderConfiguration configuration) {
            return () => {
                var section = configuration.GetSection("KitStructureSource:SeriaConfigurationMapping");
                if (!section.Exists()) {
                    throw new SectionNotFoundException("В конфигурации не определена секция \"KitStructureSource:SeriaConfigurationMapping\".");
                }
                var kitStructuresPath = configuration.GetSection("KitStructureSource:Path").Value ?? "Resources\\Конфигурации";
                string xlsxResource = section["FileName"] ??
                      throw new SectionNotFoundException("В конфигурации в секции \"KitStructureSource:SeriaConfigurationMapping\" не определено имя файла \"FileName\".");
                string xlsxSheet = section["SheetName"] ?? "Лист1";
                string fullFileName = Path.GetFullPath(Path.Combine(configuration.CurrentDirectory, kitStructuresPath, xlsxResource));
                SheetRef shref = new(fullFileName, xlsxSheet);

                SheetCommon setSource = resourceManager.LoadShc<object>(shref);
                var jobs = setSource
                .ToDictionary(
                    keySelector: row => row["Key"],
                    elementSelector: row => row["DbName"]
                    );
                return jobs;

            };
        }
    }
}
