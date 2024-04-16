using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElDeviceTypeEnum {
        [Description("Пускатель")]
        STARTER = 0,
        [Description("Контактор")]
        CONTACTOR = 1,
        [Description("Реле")]
        RELAY = 2
    }
}
