using System.ComponentModel;

namespace Nano.Electric.Enums {

    public enum InstBoxEnum {
        [Description("Навесной")]
        INST_MOUNTED = 0,
        [Description("Напольный")]
        INST_FLOOR = 1,
        [Description("Утопленный")]
        INST_FLUSH = 2
    }
}
