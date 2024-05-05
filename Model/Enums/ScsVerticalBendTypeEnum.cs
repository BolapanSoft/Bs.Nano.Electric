//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    ScsVerticalBendTypeEnum.ANGLE_45,
    //    "45",
    //    ScsVerticalBendTypeEnum.ANGLE_90,
    //    "90",
    //    ScsVerticalBendTypeEnum.ANGLE_0_90,
    //    "0-90"
    //})]
    //[EnumConvert(ScsVerticalBendTypeEnum.OTHER, ScsVerticalBendTypeEnum.ANGLE_90)]
    public enum ScsVerticalBendTypeEnum {
        [Description("90")]
        ANGLE_90 = 0,
        [Description("45")]
        ANGLE_45 = 1,
        [Description("0-90")]
        ANGLE_0_90 = 2,
        [Description("")]
        OTHER = 3
    }
}
