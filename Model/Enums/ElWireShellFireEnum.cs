using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElWireShellFireEnum {
        [Description("Нераспр. горение (пучок)")]
        FIRE_SAFE_BUNCH = 0,
        [Description("Нераспр. горение (один. прокладка)")]
        FIRE_SAFE_SINGLE = 1,
        [Description("Горючая")]
        FIRE_UNSAFE = 2,
        [Description("Не задан")]
        FIRE_NOT_SET = 3
    }
}
