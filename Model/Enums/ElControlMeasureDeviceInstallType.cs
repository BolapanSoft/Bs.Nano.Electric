using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElControlMeasureDeviceInstallType {
        [Description("Непосредственно в силовую цепь")]
        DIRECT_INTO_LINE = 0,
        [Description("Через трансформатор тока")]
        THROUGH_TRANSFORMATOR = 1
    }
}
