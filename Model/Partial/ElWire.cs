// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElWire : IProduct {
        string IProduct.Code { get => Code; }
        int? IProduct.DbImageRef { get => DbImageRef; set { DbImageRef = value; } }
        string IProduct.Name { get => wireName; set { wireName = value; } }
        string IProduct.Manufacturer { get => mfrId; set { mfrId = value; } }
        int IHaveId.Id { get => Id; set { Id = value; } }
#if InitDbContextEnums
        public ElWireThreadType? ThreadType { get; set; }
        public int? ThreadCondShape { get; set; }
        public ScsCableSystemType? CableSystemType { get; set; }
        public ElWireMark? wireMark { get; set; }

#endif
    }
    public partial class ScsUtpSocket {
#if InitDbContextEnums
        public ScsCableSystemType? CableSystemType { get; set; }
        public ScsUtpSocketTypeEnum? UtpSocketType { get; set; }
        public int? PortType { get; set; }
#endif

    }
    public partial class ScsCommutatorPanel {
#if InitDbContextEnums
        // Сохранить в поле KitStructure
        ActiveDeviceType DeviceType { get; set; }
        public string BlockName { get; set; }
        public string BackBlockName { get; set; }
        public string BlockFileName { get; set; } = "19-PANELS.dwg";
        public string BackBlockFileName { get; set; } = "19-PANELS.dwg";
#endif

    }
    public partial class ScsOrganaizerPanel {
#if InitDbContextEnums
        public string BlockName { get; set; }
        public string BackBlockName { get; set; }
        public string BlockFileName { get; set; } = "19-PANELS.dwg";
        public string BackBlockFileName { get; set; } = "19-PANELS.dwg";
#endif

    }
    public partial class ScsSwitchUtpPanel {
#if InitDbContextEnums
        public string BlockName { get; set; }
        public string BackBlockName { get; set; }
        public string BlockFileName { get; set; } = "19-PANELS.dwg";
        public string BackBlockFileName { get; set; } = "19-PANELS.dwg";
#endif

    }
    public partial class ScsSwitchSocketPanel {
#if InitDbContextEnums
        public string BlockName { get; set; }
        public string BackBlockName { get; set; }
        public string BlockFileName { get; set; } = "19-PANELS.dwg";
        public string BackBlockFileName { get; set; } = "19-PANELS.dwg";
        public ScsCableSystemType? CableSystemType { get; set; }
#endif

    }
}
