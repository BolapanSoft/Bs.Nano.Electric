// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot(nameof(ScsCommutatorPanel))]
    public partial class ScsCommutatorPanel:KitElement {
#if InitDbContextEnums
        // Сохранить в поле KitStructure
        ActiveDeviceType DeviceType { get; set; }
        public string BlockName { get; set; }
        public string BackBlockName { get; set; }
        public string BlockFileName { get; set; } = "19-PANELS.dwg";
        public string BackBlockFileName { get; set; } = "19-PANELS.dwg";
#endif
        [MaxLength(-1)]
        public string KitStructure { get => GetKitStructureAsXML(this); set {; } }
        protected override void WriteProperties(XmlWriter writer) {
            ;
        }
    }
}
