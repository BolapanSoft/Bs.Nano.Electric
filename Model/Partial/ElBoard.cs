// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot(nameof(ElBoard))]
    public partial class ElBoard:KitElement {
#if InitDbContextEnums
        public BoardStructureType? StructureType { get; set; }
        public BoardType? BoardType { get; set; }
        public InstBoxEnum? DbInstType { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum? CableLeadIn { get; set; }

#endif
        [MaxLength(-1)]
        public string? KitStructure { get=>GetKitStructureAsXML(this); set{ ;} }
        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
    }
}
