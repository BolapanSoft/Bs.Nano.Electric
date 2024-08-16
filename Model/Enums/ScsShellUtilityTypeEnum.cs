using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ScsShellUtilityTypeEnum {
        [Description("Дверь шкафа")]
        SHELL_DOOR = 0,
        [Description("Панель")]
        PANEL = 1,
        [Description("Замок для запирания двери")]
        LOCK = 2,
        [Description("Крышка")]
        HEAD = 3,
        [Description("Полка")]
        SHELF = 4,
        [Description("Блок вентиляторов")]
        FAN_BLOCK = 5,
        [Description("Основание")]
        FOUNDATION = 6,
        [Description("Другой")]
        OTHER = 7
    }
}
