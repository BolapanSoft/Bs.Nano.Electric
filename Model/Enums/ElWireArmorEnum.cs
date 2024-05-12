using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElWireArmorEnum {
        [Description("Небронированный")]
        NO_ARMOR = 0,
        [Description("Бронированный")]
        ARMOR = 1,
        [Description("Экранированный")]
        SCREENED = 2,
        [Description("Не задана")]
        NOT_SET = 3
    }
}
