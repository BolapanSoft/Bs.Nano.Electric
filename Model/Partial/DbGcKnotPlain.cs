using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot("DbGcKnotPlain")]
    public class DbGcKnotPlain : KitElement, IXmlSerializable {
        public DbGcKnotPlain() {
            Number = 1;
            IsEnabled = true;
            IsUse = true;
        }
        public bool IsEnabled { get; set; }
        public bool IsUse { get; set; }
        public int Number { get; set; }
        public ScsGutterBolting Profile { get; set; }
        public int ProfileCount { get; set; }
        //[Property(Text = WireCommonLocalizationDictionary.СерияНесущегоЭлемента, Category = PlatformLocalizationDictionary.Parameters, TypeConverter = typeof(DbSeriesValueListConverter<DbScsGutterBolting, KnotPlainSeriaFilter>))]
        /// <summary>
        /// Серия Несущего Элемента
        /// </summary>
        public string Seria { get; set; }
        public IEnumerable<DbUtilityUnit> UtilityUnits {
            get {
                return base.GetChilds<DbUtilityUnit>();
            }
        }
        public void AddChild(DbUtilityUnit unit) {
            Children.Add(unit);
        }

        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString("IsEnabled", IsEnabled.ToString());
            writer.WriteElementString("IsUse", IsUse.ToString());
            writer.WriteElementString("Number", Number.ToString());
            if (!(Profile is null)) {
                writer.WriteStartElement("Profile");
                writer.WriteAttributeString("TableName", "ScsGutterBolting");
                writer.WriteAttributeString("Id", Profile.Id.ToString());
                writer.WriteEndElement();
            }
            writer.WriteElementString("ProfileCount", ProfileCount.ToString());
            writer.WriteElementString("Seria", Seria);
        }

    }
}
