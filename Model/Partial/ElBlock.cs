// Ignore Spelling: Scs Cabel

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot(nameof(ElBlock))]
    public partial class ElBlock:KitElement {
#if InitDbContextEnums
        public AutomatContactType? ContactType { get; set; } = AutomatContactType.STATIC;
#endif
        [MaxLength(-1)]
        public string KitStructure { get => GetKitStructureAsXML(this); set {; } }
        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
    }

}
