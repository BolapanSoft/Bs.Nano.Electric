using Nano.Electric.Enums;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Nano.Electric {
    /// <summary>
    /// Описание трассы (одной линии) лотка
    /// </summary>
    /// <remarks>Представляет элемент "Лоток" в дереве DbGcMountSystem-DbGcSystemPlain-DbGcSystemGutter</remarks>
    [XmlRoot(nameof(DbGcSystemGutter))]
    public class DbGcSystemGutter : KitElement, IXmlSerializable {
        public ScsGutterCanal Gutter { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsUse { get; set; } = true;
        public DbGcStrightSegmentComplectType ComplectType { get; set; }
        public DbScsGcSeriaConfigiration Configuration { get; set; }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChildren<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Throw.IfNull(unit);
            Children.Add(unit);
        }
        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString(nameof(ComplectType), ComplectType.ToString());
            if (!(Configuration is null)) {
                XElement el = new XElement(nameof(Configuration));
                el.Add(new XAttribute("TableName", nameof(DbScsGcSeriaConfigiration)));
                el.Add(new XAttribute("Id", Configuration.Id));
                el.WriteTo(writer);
            }
            if (!(Gutter is null)) {
                XElement el = new XElement(nameof(Gutter));
                el.Add(new XAttribute("TableName", nameof(ScsGutterCanal)));
                el.Add(new XAttribute("Id", Gutter.Id));
                el.WriteTo(writer);
            }
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
        }
    }
}
