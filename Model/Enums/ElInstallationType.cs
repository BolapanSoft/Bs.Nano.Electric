using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElInstallationType {
        [Description("Внутрь шкафа")]
        InsideShell = 1,
        [Description("На фасад")]
        ToFacade = 2,
        [Description("На аппарат")]
        OnApparatus = 3
    }
}
