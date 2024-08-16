using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElDimensionType {
        [Description("Параллелепипед")]
        Parallelepiped = 1,
        [Description("Цилиндр")]
        Cylinder = 2
    }
    public enum ScsUtpSocketTypeEnum {
        [Description("Суппорт")]
        SUPPORT = 0,
        [Description("Рамка")]
        FRAME = 1,
        [Description("Модуль")]
        MODUL = 2,
        [Description("Адаптер")]
        ADAPTER = 3,
        [Description("Подрозетник")]
        SUB_SOCKET = 4,
        [Description("Другое")]
        OTHER = 5
    }
    public enum ActiveDeviceType {
        [Description("Другое")]
        Commutator = 0,
        [Description("Другое")]
        Server = 1,
        [Description("Другое")]
        Other = 2
    }
}
