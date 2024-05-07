using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElCounterType {
        [Description("Счетчик активной энергии")]
        ACTIVE_ENERGY_COUNTER = 0,
        [Description("Счетчик реактивной энергии")]
        REACTIVE_ENERGY_COUNTER = 1,
        [Description("Счетчик комплексный")]
        COMPLEX_COUNTER = 2
    }
}
