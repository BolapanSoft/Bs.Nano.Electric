//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    // {
    //    DbGcKnotStandStructureType.ONE,
    //    WireCommonLocalizationDictionary._1Несущее,
    //    DbGcKnotStandStructureType.TWO,
    //    WireCommonLocalizationDictionary._2Несущих_поКраям_,
    //    DbGcKnotStandStructureType.BACK_TO_BACK,
    //    WireCommonLocalizationDictionary._2Несущих_спинаКСпине_
    // })]
    public enum DbGcKnotStandStructureType {
        [Description("1 несущее")]
        ONE,
        [Description("2 несущих «по краям»")]
        TWO,
        [Description("2 несущих «спина к спине»")]
        BACK_TO_BACK
    }
}
