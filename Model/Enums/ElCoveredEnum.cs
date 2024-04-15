using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElCoveredEnum {
        [Description("Не защищенный (IP20-23)")]
        NOT_COVERED = 0,
        [Description("Защищенный (IP44-55)")]
        COVERED = 1
    }
}
