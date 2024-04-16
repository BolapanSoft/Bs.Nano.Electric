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
}
