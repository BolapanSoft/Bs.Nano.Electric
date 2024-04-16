using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElStarterTypeEnum {
        [Description("Реверсивный")]
        REVERSE = 0,
        [Description("Нереверсивный")]
        NOT_REVERSE = 1,
        [Description("Звезда-треугольник")]
        TRIANGLE_STAR = 2
    }
}
