using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ScsTubeFittingTypeEnum {
        [Description("Угол")]
        ANGLE = 1,
        [Description("Т-переход")]
        TRIPLE = 2,
        [Description("X-переход")]
        CROSS = 3,
        [Description("Другой")]
        OTHER = 4
    }
}
