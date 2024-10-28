using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    /// <summary>
    /// Конфигурация трасс коробов.
    /// </summary>
    public partial class DbCcMountSystem : KitElement, IXmlSerializable/*, IProduct*/, IHaveId {
        public DbCcMountSystem()
        {
            plain = new DbCcMsMount();
            Children.Add(plain);
        }
        [NotMapped]
        public string Code => DbName;
        [NotMapped]
        public string Name { get => DbNaming; set => DbNaming = value; }
        [NotMapped]
        public string Manufacturer { get => string.Empty; set {; } }
        private readonly DbCcMsMount plain;
        [NotMapped]
        public DbCcMsMount DbCcMsMountPlain => plain;
        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
        //protected override void WriteChildren(XmlWriter writer) {
        //    foreach (var child in this.GetChilds<DbGcSystemPlain>()) {
        //        child.WriteXml(writer);
        //    }
        //}
    }}
