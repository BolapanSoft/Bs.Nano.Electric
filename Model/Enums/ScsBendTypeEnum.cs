//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    // {
    //    ScsBendTypeEnum.ANGLE_45,
    //    "45",
    //    ScsBendTypeEnum.DIRECT,
    //    "90",
    //    ScsBendTypeEnum.OPTIONAL_0_45,
    //    "0-45",
    //    ScsBendTypeEnum.OPTIONAL,
    //    "0-90"
    // })]
    //[EnumConvert(ScsBendTypeEnum.OTHER, ScsBendTypeEnum.DIRECT)]
    public enum ScsBendTypeEnum {
        [Description("90")]
        DIRECT = 0,
        [Description("")]
        OTHER = 1,
        [Description("0-90")]
        OPTIONAL = 2,
        [Description("45")]
        ANGLE_45 = 3,
        [Description("0-45")]
        OPTIONAL_0_45 = 4
    }
}
