//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    // {
    //    DbGcKnotStandType.POST,
    //    WireCommonLocalizationDictionary.Post,
    //    DbGcKnotStandType.STUD,
    //    WireCommonLocalizationDictionary.Stud,
    //    DbGcKnotStandType.PROFILE,
    //    WireCommonLocalizationDictionary.Profile,
    //    DbGcKnotStandType.PERFOTAPE,
    //    WireCommonLocalizationDictionary.Perfotape,
    //    DbGcKnotStandType.NO,
    //    WireCommonLocalizationDictionary.NoType
    // })]
    public enum DbGcKnotStandType {
        [Description("Стойка")]
        POST,
        [Description("Шпилька")]
        STUD,
        SUPPORT,
        [Description("Профиль")]
        PROFILE,
        [Description("Отсутствует")]
        NO,
        [Description("Перфолента")]
        PERFOTAPE
    }
}
