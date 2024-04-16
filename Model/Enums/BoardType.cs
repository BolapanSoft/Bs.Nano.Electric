using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum BoardType {
        [Description("Универсальный")]
        UNIVERSAL = 0,
        [Description("Модульный")]
        MODULAR = 1,
        [Description("Кабельный отсек")]
        CABLE_BAY = 2
    }
  
    public enum ElStarterTypeEnum {
        [Description("Реверсивный")]
        REVERSE = 0,
        [Description("Нереверсивный")]
        NOT_REVERSE = 1,
        [Description("Звезда-треугольник")]
        TRIANGLE_STAR = 2
    }
}
