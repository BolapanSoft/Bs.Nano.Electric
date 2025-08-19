// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.PerformanceData;
using System.Xml;
using System.Xml.Serialization;
using static System.Security.Cryptography.ECCurve;

namespace Nano.Electric {
    [XmlRoot(nameof(ElBox))]
    public partial class ElBox:KitElement {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum CableLeadIn { get; set; }
        public ElBoxAppointment? BoxAppointment { get; set; }

#endif
        [MaxLength(-1)]
        public string? KitStructure { get => GetKitStructureAsXML(this); set {; } }
        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
    }
}
