using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Nano.Electric {
    public abstract class KitElement : IXmlSerializable {
        protected readonly List<IXmlSerializable> Children = new List<IXmlSerializable>();
        public XmlSchema? GetSchema() {
            return null;
        }

        public virtual void ReadXml(XmlReader reader) {
            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();

            // Read properties
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Properties") {
                ReadProperties(reader);
                reader.ReadEndElement();
            }

            // Read children
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Children") {
                reader.ReadStartElement();
                while (reader.NodeType == XmlNodeType.Element && reader.Name == "Child") {
                    string typeName = reader.GetAttribute("TypeName");
                    reader.ReadStartElement("Child");

                    // Dynamically create instance by type name
                    var type = Type.GetType(typeName, throwOnError: false);
                    if (type != null && typeof(IXmlSerializable).IsAssignableFrom(type)) {
                        var serializer = new XmlSerializer(type);
                        var child = (IXmlSerializable)serializer.Deserialize(reader);
                        Children.Add(child);
                    }
                    else {
                        // Skip unknown child
                        reader.Skip();
                    }
                    reader.ReadEndElement(); // Child
                }
                reader.ReadEndElement(); // Children
            }

            // Move past end element of root
            if (reader.NodeType == XmlNodeType.EndElement)
                reader.ReadEndElement();
        }

        // You must implement this in derived classes
        protected abstract void ReadProperties(XmlReader reader);

        public virtual void WriteXml(XmlWriter writer) {
            writer.WriteStartElement("Properties");
            WriteProperties(writer);
            writer.WriteEndElement();
            WriteChildren(writer);
        }
        public KitElement Clone() {
            return (KitElement)this.MemberwiseClone();

        }
        protected virtual void WriteChildren(XmlWriter writer) {
            if (Children.Count == 0)
                return;
            writer.WriteStartElement("Children");
            foreach (var child in Children) {
                string typeName = GetXmlRootAttribute(child)?.ElementName;
                writer.WriteStartElement("Child");
                writer.WriteAttributeString("TypeName", typeName ?? string.Empty);
                child.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        protected abstract void WriteProperties(XmlWriter writer);
        protected static string GetKitStructureAsXML<Tkit>(Tkit kit) where Tkit : KitElement {
            var serializer = new XmlSerializer(typeof(Tkit));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings)) {
                serializer.Serialize(xmlWriter, kit);
            }
            string xml = stringBuilder.ToString();
            return xml;
        }
        protected static XmlRootAttribute GetXmlRootAttribute(IXmlSerializable instance) {
            if (instance is null)
                return null;
            Type type = instance.GetType();
            while (type != typeof(object)) {
                XmlRootAttribute attribute = (XmlRootAttribute)type.GetCustomAttribute(typeof(XmlRootAttribute));
                if (!(attribute is null)) {
                    return attribute;
                }
                type = type.BaseType;
            }
            return null;
        }
        internal T GetChild<T>() where T : class {
            foreach (var child in Children) {
                T val = child as T;
                if (val != null) {
                    return val;
                }
            }
            return null;
        }
        internal IEnumerable<T> GetChildren<T>() where T : class {
            foreach (var item in Children) {
                if (item is T child) {
                    yield return child;
                }
            }
        }
    }

}
