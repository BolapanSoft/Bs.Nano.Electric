using Nano.Electric.Enums;
//using Cadwise.ObjectLib.WireCommon.Db.DbWireCanals.TrayCanal.DbBoltingUnits;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class ScsGutterBolting : DbUtilityUnit, IXmlSerializable {
        public override string UtilityTypeId => "ScsGutterBolting";
        public override string TableName => "ScsGutterBolting";
        
        [Column("СanalBoltingType")] // Achtung! Cirillic!!
        public ScsGutterBoltingTypeEnum CanalBoltingType { get; set; }
        public ScsGcStandMountType? MountType { get; set; }
        public ScsGcStandType? StandType { get; set; }
        public ScsGcConsoleMountType? ConsoleMountType { get; set; }
        public bool IsDbShelf() {
            return CanalBoltingType == ScsGutterBoltingTypeEnum.CONSOLE
                 | CanalBoltingType == ScsGutterBoltingTypeEnum.CROSSBAR
                 | CanalBoltingType == ScsGutterBoltingTypeEnum.CRAMP
                 | CanalBoltingType == ScsGutterBoltingTypeEnum.PROFILE;
        }
    }
}
