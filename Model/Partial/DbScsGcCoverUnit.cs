using Nano.Electric.Enums;
//using Cadwise.ObjectLib.WireCommon.Db.DbWireCanals.TrayCanal.DbBoltingUnits;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class DbScsGcCoverUnit : DbUtilityUnit, IXmlSerializable {
        public override string UtilityTypeId => "DbScsGcCoverUnit";
        public override string TableName => "DbScsGcCoverUnit";
        [Column("CoverType")]
        public ScsGcCoverType CoverType { get; set; } = ScsGcCoverType.STRIGHT;



    }
}
