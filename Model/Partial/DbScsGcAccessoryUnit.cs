using Nano.Electric.Enums;
using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class DbScsGcAccessoryUnit : DbUtilityUnit, IXmlSerializable {
        public override string UtilityTypeId => nameof(DbScsGcAccessoryUnit);
        public override string TableName => nameof(DbScsGcAccessoryUnit);
         public DbGcMsAccesoryType? AccessoryType { get; set; }
        public DbGcMsAccessorySelectType? AccessorySelectType { get; set; }

    }
}
