using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElBoxAppointment {
        [Description("Ящик управления электродвигателями")]
        ENGINE_BOX = 0,
        [Description("Ящик управления нагревателями и освещением")]
        HEAT_LIGHT_BOX = 1,
        [Description("Ящик с аппаратурой")]
        APR_BOX = 2
    }
}
