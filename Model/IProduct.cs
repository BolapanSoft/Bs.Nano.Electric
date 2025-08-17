#pragma warning disable VSSpell001 // Spell Check

using Nano.Electric.Enums;

namespace Nano.Electric {
    public interface IHaveId {
        int Id { get; set; }
    }
    public interface IHaveImageRef {
        string Code { get; }
        //int Id { get; set; }
        int? DbImageRef { get; set; }

    }
    public interface IHaveCableSystemTypeRef {
        string Code { get; }
        //int Id { get; set; }
        ScsCableSystemType? CableSystemType { get; set; }

    }

    public interface IProduct : IHaveId {
        string Code { get; }
        //int Id { get; set; }
        int? DbImageRef { get; set; }
        string Name { get; set; }
        string Manufacturer { get; set; }
    }
    public interface ICommonProduct : IProduct { 
        string? Series { get; set; }
        string? Description { get; set; }
        string? SpecDescription { get; set; }
        string? Url { get; set; }

    }
    public partial class SafeDegree : IHaveId { }
    public partial class ClimateTable : IHaveId { }
    public partial class DbLtKiTable : IHaveId { }
    public partial class ExplodeSafeLevel : IHaveId { }
    public partial class ScsPortType : IHaveId { }
    public partial class ElWire : IProduct, IHaveCableSystemTypeRef, IHaveImageRef { }
    public partial class ElWireConductMaterial : IHaveId { }
    public partial class ElWireIsolationMaterial : IHaveId { }
    public partial class ScsCableSystemType : IHaveId { }
    public partial class ScsGutterCanal : ICommonProduct, IHaveImageRef { }
    public partial class ScsCabelCanal : ICommonProduct, IHaveImageRef { }
    public partial class ScsCableFitting : ICommonProduct, IHaveImageRef { }
    public partial class ScsPatchCord : ICommonProduct, IHaveCableSystemTypeRef, IHaveImageRef { }
    public partial class ScsPipe : ICommonProduct, IHaveImageRef { }
    public partial class ScsTubeFitting : ICommonProduct, IHaveImageRef { }
    public partial class DbScsGutterCover : ICommonProduct, IHaveImageRef { }
    public partial class DbScsGutterPartition : ICommonProduct, IHaveImageRef { }
    public partial class ScsGcFitting : ICommonProduct, IHaveImageRef { }
    public partial class DbScsGcCoverUnit : ICommonProduct, IHaveImageRef { }
    public partial class DbScsGcAccessoryUnit : ICommonProduct, IHaveImageRef { }
    public partial class ScsGutterBolting : ICommonProduct, IHaveImageRef { }
    public partial class DbScsGcBoltingAccessoryUnit : ICommonProduct, IHaveImageRef { }
    public partial class DcCableCanalCover : ICommonProduct, IHaveImageRef   { }
    public partial class DbCableCanalPartition : ICommonProduct, IHaveImageRef   { }
    public partial class CaeMaterialUtility : ICommonProduct, IHaveImageRef { }
    public partial class ElBoard : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElBox : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElShieldingUnit : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElAutomat : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElSafeDevice : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElKnifeSwitch : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElUzo : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class DbElUzdp : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElStarter : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElOvervoltageSuppressor : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElFrequenceTransformer : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElCasing : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElFiderUtility : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElBoardUtility : ICommonProduct, IHaveImageRef { }
    public partial class ElBlock : ICommonProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ElPushButtonStation : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElControlDevice : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElCurrentTransformer : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElCounter : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElVoltmeter : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElAmperemeter : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElLighting : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElLamp : ICommonProduct, IHaveImageRef { }
    public partial class ElLightUtility : ICommonProduct, IHaveImageRef { }
    public partial class ElDbEngine : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElDbHeater : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElDbComplex : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class DbElSwitch : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class DbElCSwitch : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class DbElSocket : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElDbCase : ICommonProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElSocketUtility : ICommonProduct, IHaveImageRef, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElWireMark : IHaveId { }
    public partial class DbGraphic : IHaveId, IDbGraphic { }

    public partial class ScsPanelUtilityUnit : ICommonProduct, IHaveImageRef { }
    public partial class ScsShellUtility : ICommonProduct, IHaveImageRef { }
    public partial class ScsSwitchSocketPanel : ICommonProduct, IHaveCableSystemTypeRef, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsOrganaizerPanel : ICommonProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsSwitchUtpPanel : ICommonProduct, IHaveCableSystemTypeRef, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsShellDistr : ICommonProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsUtpSocket : ICommonProduct, IHaveCableSystemTypeRef, IHaveImageRef { }
    public partial class ScsCommutatorPanel : ICommonProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsHatchUtilityUnit : ICommonProduct, IHaveImageRef{ }
    public partial class ScsServiceColumnUtilityUnit : ICommonProduct, IHaveImageRef{ }
    public partial class ScsHatch : ICommonProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsServiceColumn : ICommonProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsCord : IProduct, IHaveCableSystemTypeRef, IHaveImageRef { }
    public partial class DbScsTubeSeriesConfiguration : IHaveId { }
    /*
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
*/

}
