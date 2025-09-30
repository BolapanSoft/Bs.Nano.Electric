// Ignore Spelling: Expl

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot(nameof(ScsSwitchSocketPanel))]
    public partial class ScsSwitchSocketPanel:KitElement {
#if InitDbContextEnums
        [Column("BlockName")]
        public string? BlockName { get; set; }
        public string? BackBlockName { get; set; }
        public string BlockFileName { get; set; } = "19-PANELS.dwg";
        public string BackBlockFileName { get; set; } = "19-PANELS.dwg";
        public ScsCableSystemType? CableSystemType { get; set; }
        public ScsPortType? PortType { get; set; }
#endif
        [MaxLength(-1)]
        public string KitStructure { get => GetKitStructureAsXML(this); set {; } }
        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
    }
}
