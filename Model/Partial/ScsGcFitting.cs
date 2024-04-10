using Nano.Electric.Enums;
//using Cadwise.ObjectLib.WireCommon.Db.DbWireCanals.TrayCanal.DbBoltingUnits;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class ScsGcFitting : DbUtilityUnit, IXmlSerializable {
        public override string UtilityTypeId => "ScsGcFitting";
        public override string TableName => "ScsGcFitting";

#if InitDbContextEnums
        [Column("FittingType")]
        public ScsGutterFittingTypeEnum FittingType { get; set; } = ScsGutterFittingTypeEnum.OTHER;
        //ScsVerticalUniversalBendTypeEnum
        public ScsVerticalUniversalBendTypeEnum? VerticalUniversalBendType { get; set; }
        public ScsVerticalBendTypeEnum? VerticalBendType { get; set; }
        public ScsBendTypeEnum? BendType { get; set; }
        public ScsGutterPassageType? GutterPassageType { get; set; } 
#endif
    }
}
