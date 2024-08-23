// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
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
}
