using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElControlRegisterDeviceMountType {
        [Description("Монтажная рейка")]
        MOUNT_RAIL = 1,
        [Description("Монтажная плата")]
        MOUNT_BOARD = 2,
        [Description("Монтажная рейка или плата")]
        MOUNT_RAIL_OR_BOARD = 3
    }
}
