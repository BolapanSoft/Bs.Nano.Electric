using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;


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
        public static SeriaConfigirationKit ParseKitStructure(string strKitStructure)
        {
            if (string.IsNullOrWhiteSpace(strKitStructure))
                throw new ArgumentException("Input XML string is null or empty.", nameof(strKitStructure));

            var serializer = new XmlSerializer(typeof(SeriaConfigirationKit));
            using (var stringReader = new StringReader(strKitStructure))
            {
                return (SeriaConfigirationKit)serializer.Deserialize(stringReader)!;
            }
        }
    }
}
