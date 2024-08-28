// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot(nameof(ElShieldingUnit))]
    public partial class ElShieldingUnit:KitElement {
        [NotMapped]
        string IProduct.Name { get=>DbName; set=>DbName=value; }
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum? CableLeadIn { get; set; }
        public PhaseCountEnum? PhaseCount { get; set; }

#endif
        [MaxLength(-1)]
        public string KitStructure { get => GetKitStructureAsXML(this); set {; } }
        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
    }
}
