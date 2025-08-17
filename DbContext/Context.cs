// Ignore Spelling: Amperemeters
#pragma warning disable VSSpell001 // Spell Check

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#if NETFRAMEWORK
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace Nano.Electric {
    public partial class Context : DbContext {
#if NETFRAMEWORK
        public Context() : base("name=Context") { }
        public Context(string context) : base($"name={context}") { }
#else
        private readonly string _connectionString;

        public Context() {
            _connectionString = "Data Source=database.db";
        }

        public Context(string connectionString) {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(_connectionString);
        }
#endif

        public virtual DbSet<AutomatValuesScale> AutomatValuesScales { get; set; }
        public virtual DbSet<CaeMaterialUtility> CaeMaterialUtilities { get; set; }
        public virtual DbSet<ClimateTable> ClimateTables { get; set; }
        public virtual DbSet<CurrentReleaseScale> CurrentReleaseScales { get; set; }
        public virtual DbSet<CurrentScale> CurrentScales { get; set; }
        public virtual DbSet<CurrentScaleUzo> CurrentScaleUzoes { get; set; }
        public virtual DbSet<DbCableCanalPartition> DbCableCanalPartitions { get; set; }
        public virtual DbSet<DbCaeMaterialUtilitySet> DbCaeMaterialUtilitySets { get; set; }
        public virtual DbSet<DbCcMountSystem> DbCcMountSystems { get; set; }
        public virtual DbSet<DbDwgFile> DbDwgFiles { get; set; }
        public virtual DbSet<DbElBoardUtilitySet> DbElBoardUtilitySets { get; set; }
        public virtual DbSet<DbElCSwitch> DbElCSwitches { get; set; }
        public virtual DbSet<DbElFiderUtilitySet> DbElFiderUtilitySets { get; set; }
        public virtual DbSet<DbElLightUtilitySet> DbElLightUtilitySets { get; set; }
        public virtual DbSet<DbElSocket> DbElSockets { get; set; }
        public virtual DbSet<DbElSocketUtilitySet> DbElSocketUtilitySets { get; set; }
        public virtual DbSet<DbElSwitch> DbElSwitches { get; set; }
        public virtual DbSet<DbGcMountSystem> DbGcMountSystems { get; set; }
        public virtual DbSet<DbGraphic> DbGraphics { get; set; }
        public virtual DbSet<DbImage> DbImages { get; set; }
        public virtual DbSet<DbLtKiTable> DbLtKiTables { get; set; }
        public virtual DbSet<DbScsGcAccessoryUnit> DbScsGcAccessoryUnits { get; set; }
        public virtual DbSet<DbScsGcBoltingAccessoryUnit> DbScsGcBoltingAccessoryUnits { get; set; }
        public virtual DbSet<DbScsGcCoverUnit> DbScsGcCoverUnits { get; set; }
        public virtual DbSet<DbScsGcSeriaConfigiration> DbScsGcSeriaConfigirations { get; set; }
        public virtual DbSet<DbScsGutterCover> DbScsGutterCovers { get; set; }
        public virtual DbSet<DbScsGutterPartition> DbScsGutterPartitions { get; set; }
        public virtual DbSet<DbScsGutterUtilitySet> DbScsGutterUtilitySets { get; set; }
        //public virtual DbSet<DbScsHatchUtilitySet> DbScsHatchUtilitySets { get; set; }
        public virtual DbSet<DbScsPanelUtilitySet> DbScsPanelUtilitySets { get; set; }
        //public virtual DbSet<DbScsPhoneCrossUtilitySet> DbScsPhoneCrossUtilitySets { get; set; }
        //public virtual DbSet<DbScsServiceColumnUtilitySet> DbScsServiceColumnUtilitySets { get; set; }
        public virtual DbSet<DbScsShellUtilitySet> DbScsShellUtilitySets { get; set; }
        public virtual DbSet<DbScsTubeSeriesConfiguration> DbScsTubeSeriesConfigurations { get; set; }
        public virtual DbSet<DbTbMountSystem> DbTbMountSystems { get; set; }
        public virtual DbSet<DcCableCanalCover> DcCableCanalCovers { get; set; }
        public virtual DbSet<DSInformation> DSInformations { get; set; }
        public virtual DbSet<ElAmperemeter> ElAmperemeters { get; set; }
        public virtual DbSet<ElAutomat> ElAutomats { get; set; }
        public virtual DbSet<ElBlock> ElBlocks { get; set; }
        public virtual DbSet<ElBlockBoard> ElBlockBoards { get; set; }
        public virtual DbSet<ElBoard> ElBoards { get; set; }
        public virtual DbSet<ElBoardUtility> ElBoardUtilities { get; set; }
        public virtual DbSet<ElBox> ElBoxes { get; set; }
        public virtual DbSet<ElCase> ElCases { get; set; }
        public virtual DbSet<ElCasing> ElCasings { get; set; }
        public virtual DbSet<ElCompensatorReactivePower> ElCompensatorReactivePowers { get; set; }
        public virtual DbSet<ElControlCabel> ElControlCabels { get; set; }
        public virtual DbSet<ElControlDevice> ElControlDevices { get; set; }
        public virtual DbSet<ElControlWireMark> ElControlWireMarks { get; set; }
        public virtual DbSet<ElCounter> ElCounters { get; set; }
        public virtual DbSet<ElCurrentTransformer> ElCurrentTransformers { get; set; }
        public virtual DbSet<ElDbCase> ElDbCases { get; set; }
        public virtual DbSet<ElDbComplex> ElDbComplexes { get; set; }
        public virtual DbSet<ElDbEngine> ElDbEngines { get; set; }
        public virtual DbSet<ElDbHeater> ElDbHeaters { get; set; }
        public virtual DbSet<ElDistPanel> ElDistPanels { get; set; }
        public virtual DbSet<ElFider> ElFiders { get; set; }
        public virtual DbSet<ElFiderUtility> ElFiderUtilities { get; set; }
        public virtual DbSet<ElFrequenceTransformer> ElFrequenceTransformers { get; set; }
        public virtual DbSet<ElInputAprBoard> ElInputAprBoards { get; set; }
        public virtual DbSet<ElInputBusBoard> ElInputBusBoards { get; set; }
        public virtual DbSet<ElInstallBox> ElInstallBoxes { get; set; }
        public virtual DbSet<ElKnifeSwitch> ElKnifeSwitches { get; set; }
        public virtual DbSet<ElLamp> ElLamps { get; set; }
        public virtual DbSet<ElLighting> ElLightings { get; set; }
        public virtual DbSet<ElLightUtility> ElLightUtilities { get; set; }
        public virtual DbSet<ElModuleBoard> ElModuleBoards { get; set; }
        public virtual DbSet<ElOvervoltageSuppressor> ElOvervoltageSuppressors { get; set; }
        public virtual DbSet<ElPushButtonStation> ElPushButtonStations { get; set; }
        public virtual DbSet<ElRailBoard> ElRailBoards { get; set; }
        public virtual DbSet<ElResistReactor> ElResistReactors { get; set; }
        public virtual DbSet<ElSafeDevice> ElSafeDevices { get; set; }
        public virtual DbSet<ElShieldingUnit> ElShieldingUnits { get; set; }
        public virtual DbSet<ElSocketUtility> ElSocketUtilities { get; set; }
        public virtual DbSet<ElStandartDistrBoard> ElStandartDistrBoards { get; set; }
        public virtual DbSet<ElStarter> ElStarters { get; set; }
        public virtual DbSet<ElTransformer> ElTransformers { get; set; }
        public virtual DbSet<ElUzo> ElUzoes { get; set; }
        public virtual DbSet<DbElUzdp> DbElUzdpes { get; set; }
        public virtual DbSet<ElVoltmeter> ElVoltmeters { get; set; }
        public virtual DbSet<ElWire> ElWires { get; set; }
        public virtual DbSet<ElWireConductMaterial> ElWireConductMaterials { get; set; }
        public virtual DbSet<ElWireIsolationMaterial> ElWireIsolationMaterials { get; set; }
        public virtual DbSet<ElWireMark> ElWireMarks { get; set; }
        public virtual DbSet<ExplodeSafeLevel> ExplodeSafeLevels { get; set; }
        public virtual DbSet<FieldOldValue> FieldOldValues { get; set; }
        public virtual DbSet<ImportConflictField> ImportConflictFields { get; set; }
        public virtual DbSet<ImportObjectStatu> ImportObjectStatus { get; set; }
        public virtual DbSet<MultiplicityScale> MultiplicityScales { get; set; }
        public virtual DbSet<SafeDegree> SafeDegrees { get; set; }
        //public virtual DbSet<ScsAtc> ScsAtcs { get; set; }
        public virtual DbSet<ScsCabelCanal> ScsCabelCanals { get; set; }
        public virtual DbSet<ScsCableCanalSupplement> ScsCableCanalSupplements { get; set; }
        public virtual DbSet<ScsCableFitting> ScsCableFittings { get; set; }
        public virtual DbSet<ScsCableSystemType> ScsCableSystemTypes { get; set; }
        public virtual DbSet<ScsCommutatorPanel> ScsCommutatorPanels { get; set; }
        public virtual DbSet<ScsCord> ScsCords { get; set; }
        public virtual DbSet<ScsGcFitting> ScsGcFittings { get; set; }
        public virtual DbSet<ScsGutterBolting> ScsGutterBoltings { get; set; }
        public virtual DbSet<ScsGutterCanal> ScsGutterCanals { get; set; }
        public virtual DbSet<ScsGutterSupplement> ScsGutterSupplements { get; set; }
        public virtual DbSet<ScsHatch> ScsHatches { get; set; }
        public virtual DbSet<ScsHatchUtilityUnit> ScsHatchUtilityUnits { get; set; }
        public virtual DbSet<ScsOrganaizerPanel> ScsOrganaizerPanels { get; set; }
        public virtual DbSet<ScsPanelUtilityUnit> ScsPanelUtilityUnits { get; set; }
        public virtual DbSet<ScsPatchCord> ScsPatchCords { get; set; }
        public virtual DbSet<ScsPhoneCross> ScsPhoneCrosses { get; set; }
        public virtual DbSet<ScsPhoneCrossUtilityUnit> ScsPhoneCrossUtilityUnits { get; set; }
        public virtual DbSet<ScsPhoneProfile> ScsPhoneProfiles { get; set; }
        public virtual DbSet<ScsPipe> ScsPipes { get; set; }
        public virtual DbSet<ScsPlintModule> ScsPlintModules { get; set; }
        public virtual DbSet<ScsPortType> ScsPortTypes { get; set; }
        public virtual DbSet<ScsServiceColumn> ScsServiceColumns { get; set; }
        public virtual DbSet<ScsServiceColumnUtilityUnit> ScsServiceColumnUtilityUnits { get; set; }
        public virtual DbSet<ScsShellDistr> ScsShellDistrs { get; set; }
        public virtual DbSet<ScsShellUtility> ScsShellUtilities { get; set; }
        public virtual DbSet<ScsSwitchSocketPanel> ScsSwitchSocketPanels { get; set; }
        public virtual DbSet<ScsSwitchUtpPanel> ScsSwitchUtpPanels { get; set; }
        public virtual DbSet<ScsTubeFitting> ScsTubeFittings { get; set; }
        public virtual DbSet<ScsUtpSocket> ScsUtpSockets { get; set; }
        public virtual DbSet<ScsWorkPlaceDbKit> ScsWorkPlaceDbKits { get; set; }

#if NETFRAMEWORK
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            InitializeModel(modelBuilder);
        }
        partial void InitializeModel(DbModelBuilder modelBuilder); 
#elif NETCOREAPP
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            InitializeModel(modelBuilder);
        }
        partial void InitializeModel(ModelBuilder modelBuilder); 
#endif
    }
}
