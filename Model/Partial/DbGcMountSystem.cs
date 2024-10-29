using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    /// <summary>
    /// Конфигурация трасс лотков.
    /// </summary>
    public partial class DbGcMountSystem : KitElement, IXmlSerializable/*, IProduct*/, IHaveId {
        [NotMapped]
        public string Code => DbName;
        [NotMapped]
        public string Name { get => DbNaming; set => DbNaming = value; }
        [NotMapped]
        public string Manufacturer { get => string.Empty; set {; } }
        /// <summary>
        /// Конфигурация узлов крепления
        /// </summary>
        public DbScsGutterUtilitySet StandGutterUtilitySet { get; set; }
        [NotMapped]
        public IEnumerable<DbGcSystemPlain> SystemPlains {
            get {
                return GetChildren<DbGcSystemPlain>();
            }

        }

        public void Add(DbGcSystemPlain plain) {
            Throw.IfNull(plain);
            Children.Add(plain);
        }
        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
        //protected override void WriteChildren(XmlWriter writer) {
        //    foreach (var child in this.GetChilds<DbGcSystemPlain>()) {
        //        child.WriteXml(writer);
        //    }
        //}
    }
}
