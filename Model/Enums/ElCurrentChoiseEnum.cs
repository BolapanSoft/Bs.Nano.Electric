using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElCurrentChoiseEnum {
        [Description("По кратности (Km)")]
        BY_MULTIPLICITY = 0,
        [Description("По току (Im)")]
        BY_CURRENT = 1,
        [Description("По типу (A/B/C/D...)")]
        BY_CURVE = 2
    }
}
