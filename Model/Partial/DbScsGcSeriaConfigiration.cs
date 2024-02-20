using System.Text;
using System.Xml;
using System.Xml.Serialization;



namespace Nano.Electric {
    public partial class DbScsGcSeriaConfigiration {
        public void SetKitStructure(SeriaConfigirationKit kit) {
            var serializer = new XmlSerializer(typeof(SeriaConfigirationKit));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = "\t" };
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings)) {
                serializer.Serialize(xmlWriter, kit);
            }
            string xml = stringBuilder.ToString();
            KitStructure = xml;
        }
    }
}
