//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
namespace Nano.Electric.Enums {

    //    [Enum(new object[]
    //{
    //        DbGcKnotLevelType.CONSOLE,
    //        WireCommonLocalizationDictionary.Console,
    //        DbGcKnotLevelType.C_BRACKET,
    //        WireCommonLocalizationDictionary.CBracket,
    //        DbGcKnotLevelType.L_BRACKET,
    //        WireCommonLocalizationDictionary.LBracket,
    //        DbGcKnotLevelType.CROSSBAR,
    //        WireCommonLocalizationDictionary.Crossbar,
    //        DbGcKnotLevelType.CRAMP,
    //        WireCommonLocalizationDictionary.Cramp,
    //        DbGcKnotLevelType.PROFILE,
    //        WireCommonLocalizationDictionary.Profile,
    //        DbGcKnotLevelType.NO,
    //        WireCommonLocalizationDictionary.NoType
    //})]
    public enum DbGcKnotLevelType {
        [Description("Консоль")]
        CONSOLE,
        [Description("Перекладина")]
        CROSSBAR,
        [Description("Скоба")]
        CRAMP,
        [Description("Профиль")]
        PROFILE,
        [Description("Отсутствует")]
        NO,
        [Description("C-подвес")]
        C_BRACKET,
        [Description("L-подвес")]
        L_BRACKET
    }
}
