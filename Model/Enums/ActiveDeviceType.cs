using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ActiveDeviceType {
        [Description("Коммутатор")]
        Commutator = 0,
        [Description("Сервер")]
        Server = 1,
        [Description("Пользовательский тип")]
        Other = 2
    }
}
