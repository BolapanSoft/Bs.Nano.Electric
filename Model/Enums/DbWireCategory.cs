using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum DbWireCategory {
		[Description("Силовой")]
        POWER = 0,
        [Description("Контрольный")]
        CONTROL = 1,
        [Description("Пара/Витая пара")]
        PAIR = 2,
        [Description("Оптический")]
        OPTIC = 3
    }
}
