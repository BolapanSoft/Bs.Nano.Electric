﻿// Ignore Spelling: Expl

using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    [XmlRoot(nameof(ScsOrganaizerPanel))]
    public partial class ScsOrganaizerPanel:KitElement {
#if InitDbContextEnums
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
