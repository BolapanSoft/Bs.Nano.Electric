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
    public enum ElCounterType {
        [Description("Счетчик активной энергии")]
        ACTIVE_ENERGY_COUNTER = 0,
        [Description("Счетчик реактивной энергии")]
        REACTIVE_ENERGY_COUNTER = 1,
        [Description("Счетчик комплексный")]
        COMPLEX_COUNTER = 2
    }
    public enum ElControlMeasureDeviceInstallType {
        [Description("Непосредственно в силовую цепь")]
        DIRECT_INTO_LINE = 0,
        [Description("Через трансформатор тока")]
        THROUGH_TRANSFORMATOR = 1
    }
}
