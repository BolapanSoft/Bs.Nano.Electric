using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot("DbGcKnotLevel")]
    public class DbGcKnotLevel : KitElement, IXmlSerializable {
        public DbGcKnotLevel() {
            Height = 0;
            Number = 1;
            IsEnabled = true;
            IsUse = true;
        }
        public int Height { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsUse { get; set; }
        public int Number { get; set; }
        public IEnumerable<DbGcKnotPlain> KnotPlains=>GetChildren<DbGcKnotPlain>();
        public void AddChild(DbGcKnotPlain knotPlain) {
            Children.Add(knotPlain);
        }
        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString("Height", Height.ToString());
            writer.WriteElementString("IsEnabled", IsEnabled.ToString());
            writer.WriteElementString("IsUse", IsUse.ToString());
            writer.WriteElementString("Number", Number.ToString());
        }

    }
}
