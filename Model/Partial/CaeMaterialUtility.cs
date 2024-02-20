//using Cadwise.Data;
//using Cadwise.ObjectLib.WireCommon.Db.DbWireCanals.TrayCanal.DbKnot;
//using Cadwise.Studio.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Nano.Electric {

    //[XmlRoot("CaeMaterialUtility")]
    public partial class CaeMaterialUtility : DbUtilityUnit, IXmlSerializable {
        [NotMapped]
        public override string UtilityTypeId => nameof(CaeMaterialUtility);
        [NotMapped]
        public override string TableName => nameof(CaeMaterialUtility);
        [NotMapped]
        public override int Id => id;
    }
}
