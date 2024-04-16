using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElInstantKzCurrentChoiseEnum {
        [Description("По кратности (Ki)")] 
        BY_MULTIPLICITY = 0,
        [Description("По току (Ii)")] 
        BY_CURRENT = 1
    }
}
