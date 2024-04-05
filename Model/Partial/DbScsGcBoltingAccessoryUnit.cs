using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class DbScsGcBoltingAccessoryUnit : DbUtilityUnit, IXmlSerializable {
        public override string UtilityTypeId => "DbScsGcBoltingAccessoryUnits";
        public override string TableName => "DbScsGcBoltingAccessoryUnits";
       
    }

}
