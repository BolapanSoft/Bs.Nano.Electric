using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot("DbUtilityUnit")]
    public abstract class DbUtilityUnit : KitElement, IXmlSerializable {
        public struct UtilityObject {
            public string TableName;
            public int Id;
        }
        protected UtilityObject utilityObject;

        /* Unmerged change from project 'Iek.MakeModelStudioCS'
        Before:
                protected DbUtilityUnit()
                { 
        After:
                protected DbUtilityUnit() {
        */
        protected DbUtilityUnit() {
            SpecCount = 1;
        }
        [NotMapped]
        public virtual UtilityObject UtilityObj => utilityObject;
        [NotMapped]
        public abstract string UtilityTypeId { get; }
        [NotMapped]
        public abstract string TableName { get; }
        public abstract int Id { get; set; }
        [NotMapped]
        public int SpecCount { get; set; }
        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString("SpecCount", SpecCount.ToString());
            {
                writer.WriteStartElement("UtilityObject");
                writer.WriteAttributeString("TableName", TableName);
                writer.WriteAttributeString("Id", Id.ToString());
                writer.WriteEndElement();
            }
            writer.WriteElementString("UtilityTypeId", UtilityTypeId);
        }
    }

}
