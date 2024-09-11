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
    public interface IHaveScsCableSystemTypeRef {
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
    public partial class SafeDegree : IHaveId { }
    public partial class ClimateTable : IHaveId { }
    public partial class DbLtKiTable : IHaveId { }
    public partial class ExplodeSafeLevel : IHaveId { }
    public partial class ScsPortType : IHaveId { }
    public partial class ElWire : IProduct, IHaveScsCableSystemTypeRef, IHaveImageRef { }
    public partial class ElWireConductMaterial : IHaveId { }
    public partial class ElWireIsolationMaterial : IHaveId { }
    public partial class ScsCableSystemType : IHaveId { }
    public partial class ScsGutterCanal : IProduct, IHaveImageRef { }
    public partial class ScsCabelCanal : IProduct, IHaveImageRef { }
    public partial class ScsCableFitting : IProduct, IHaveImageRef { }
    public partial class ScsPatchCord : IProduct, IHaveImageRef { }
    public partial class ScsPipe : IProduct, IHaveImageRef { }
    public partial class ScsTubeFitting : IProduct, IHaveImageRef { }
    public partial class DbScsGutterCover : IProduct, IHaveImageRef { }
    public partial class DbScsGutterPartition : IProduct, IHaveImageRef { }
    public partial class ScsGcFitting : IProduct, IHaveImageRef { }
    public partial class DbScsGcCoverUnit : IProduct, IHaveImageRef { }
    public partial class DbScsGcAccessoryUnit : IProduct, IHaveImageRef { }
    public partial class ScsGutterBolting : IProduct, IHaveImageRef { }
    public partial class DbScsGcBoltingAccessoryUnit : IProduct, IHaveImageRef { }
    public partial class DcCableCanalCover : IProduct, IHaveImageRef   { }
    public partial class DbCableCanalPartition : IProduct, IHaveImageRef   { }
    public partial class CaeMaterialUtility : IProduct, IHaveImageRef { }
    public partial class ElBoard : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElBox : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElShieldingUnit : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElAutomat : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElSafeDevice : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElKnifeSwitch : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElUzo : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElStarter : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElOvervoltageSuppressor : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElFrequenceTransformer : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElCasing : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElFiderUtility : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElBoardUtility : IProduct, IHaveImageRef { }
    public partial class ElBlock : IProduct, IHaveImageRef { }
    public partial class ElPushButtonStation : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElControlDevice : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElCurrentTransformer : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElCounter : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElVoltmeter : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElAmperemeter : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElLighting : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElLamp : IProduct, IHaveImageRef { }
    public partial class ElLightUtility : IProduct, IHaveImageRef { }
    public partial class ElDbEngine : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElDbHeater : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElDbComplex : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate { }
    public partial class DbElSwitch : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class DbElCSwitch : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class DbElSocket : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElDbCase : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class ElSocketUtility : IProduct, IHaveImageRef, IHaveSafeDegree, IHaveDbClimate { }
    public partial class ElWireMark : IHaveId { }
    public partial class DbGraphic : IHaveId, IDbGraphic { }

    public partial class ScsPanelUtilityUnit : IProduct, IHaveImageRef { }
    public partial class ScsShellUtility : IProduct, IHaveImageRef { }
    public partial class ScsSwitchSocketPanel : IProduct, IHaveScsCableSystemTypeRef, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsOrganaizerPanel : IProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsSwitchUtpPanel : IProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsShellDistr : IProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsUtpSocket : IProduct, IHaveScsCableSystemTypeRef, IHaveImageRef { }
    public partial class ScsCommutatorPanel : IProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsHatchUtilityUnit : IProduct, IHaveImageRef{ }
    public partial class ScsServiceColumnUtilityUnit : IProduct, IHaveImageRef{ }
    public partial class ScsHatch : IProduct, IHaveImageRef, IHaveDbGraphicRef { }
    public partial class ScsServiceColumn : IProduct, IHaveImageRef, IHaveDbGraphicRef { }
    /*
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
    public partial class __ : IProduct, IHaveImageRef, IHaveExplodeLevel, IHaveSafeDegree, IHaveDbClimate, IHaveDbGraphicRef { }
*/

}
