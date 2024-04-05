using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class DbCaeMaterialUtilitySet : DbUtilityUnit, IXmlSerializable {
        public override string UtilityTypeId => nameof(DbCaeMaterialUtilitySet);
        public override string TableName => nameof(DbCaeMaterialUtilitySet);
        
    }
}
