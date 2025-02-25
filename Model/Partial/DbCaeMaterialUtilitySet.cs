using System.Xml.Serialization;

namespace Nano.Electric {
    public partial class DbCaeMaterialUtilitySet : DbUtilityUnit, IXmlSerializable, IHaveImageRef {
        public override string UtilityTypeId => nameof(DbCaeMaterialUtilitySet);
        public override string TableName => nameof(DbCaeMaterialUtilitySet);

        string IHaveImageRef.Code => DbName;
        public void AddChildren(DbUtilityUnit child) { 
            Children.Add(child);
        }
    }
}
