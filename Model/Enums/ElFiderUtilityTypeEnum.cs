using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElFiderUtilityTypeEnum {
        [Description("Вспомогательные контакты")]
        UTIL_HELP_CONTACT = 0,
        [Description("Сигнальные контакты")]
        UTIL_SIGNAL_CONTACT = 1,
        [Description("Независимые расцепители")]
        UTIL_FREE_TRIP = 2,
        [Description("Расцепители минимального напряжения")]
        UTIL_MIN_VOLTAGE_TRIP = 3,
        [Description("Моторные приводы")]
        UTIL_ENGINE_DRIVE = 4,
        [Description("Автоматические повторные взводы")]
        UTIL_AUTO_REPEAT_NOTCH = 5,
        [Description("Блокираторы")]
        UTIL_BLOCKER = 6,
        [Description("Держатели плавких вставок")]
        UTIL_FUSE_HOLDER = 7,
        [Description("Другое")]
        UTIL_OTHER = 100
    }
}
