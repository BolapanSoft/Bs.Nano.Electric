using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ScsCableFittingTypeEnum {
        [Description("Угол внутренний")]
        INSIDE_ANGLE = 1,
        [Description("Угол внешний")]
        OUTSIDE_ANGLE = 2,
        [Description("Угол плоский")]
        PLAIN_ANGLE = 3,
        [Description("Т-переход")]
        TRIPLE = 4,
        [Description("Другой")]
        OTHER = 5,
        [Description("Заглушка")]
        CORK = 6,
        [Description("Соединение на стык")]
        JOINT = 7
    }
}
