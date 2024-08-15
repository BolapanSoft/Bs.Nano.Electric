using System;
using System.ComponentModel;

namespace Nano.Electric.Enums {
    [Flags]
    public enum ElMountRailType {
        [Description("35х7.5")]
        SIZE_35X75 = 1,
        [Description("35х15")]
        SIZE_35X15 = 2,
        [Description("G 32x15")]
        SIZE_G_32X15 = 4,
        [Description("C 27x15")]
        SIZE_C_27X15 = 8,
        [Description("27x7.5")]
        SIZE_27X75 = 0x10,
        [Description("27x15")]
        SIZE_27X15 = 0x20
    }
    public enum ElWireThreadType {
        [Description("Однопроволочная")]
        SINGLE_THREAD = 0,
        [Description("Многопроволочная")]
        MULTY_THREAD = 1
    }
    public enum ElWireShapeEnum {
        [Description("Круглая")]
        ROUND = 0,
        [Description("Не задана")]
        NOT_ROUND = 1,
        [Description("Секторная")]
        SECTOR = 2,
        [Description("")]
        PLAIN = 3,
        [Description("Другая")]
        OTHER = 4
    }
}
