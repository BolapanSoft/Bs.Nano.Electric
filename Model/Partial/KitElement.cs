using System;
using System.Collections.Generic;
using System.Reflection;
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
            throw new NotImplementedException();
        }

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
