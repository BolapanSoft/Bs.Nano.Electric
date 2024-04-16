using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElPoleCountEnum {
        [Description("1P")]
        PC_1P = 1,
        [Description("1P+N")]
        PC_1PN = 2,
        [Description("2P")]
        PC_2P = 3,
        [Description("3P")]
        PC_3P = 4,
        [Description("3P+N")]
        PC_3PN = 5,
        [Description("4P")]
        PC_4P = 6
    }
}
