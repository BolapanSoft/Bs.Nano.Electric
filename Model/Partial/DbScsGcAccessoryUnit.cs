using Nano.Electric.Enums;
using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class DbScsGcAccessoryUnit : DbUtilityUnit, IXmlSerializable {
        public override string UtilityTypeId => nameof(DbScsGcAccessoryUnit);
        public override string TableName => nameof(DbScsGcAccessoryUnit);
         public DbGcMsAccessoryType AccessoryType { get; set; } // default=>0;
        public DbGcMsAccessorySelectType? AccessorySelectType { get; set; }

    }
}
