using System;
using System.ComponentModel;

namespace Nano.Electric.Enums {
    [Flags]
    public enum RatedNetVoltageType {
        [Description ("380/220")]
        Voltage380220 = 1,
        [Description ("660/380")]
        Voltage660380 = 2
    }
}
