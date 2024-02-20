//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    ScsGutterBoltingTypeEnum.POST,
    //    WireCommonLocalizationDictionary.Post,
    //    ScsGutterBoltingTypeEnum.STUD,
    //    WireCommonLocalizationDictionary.Stud,
    //    ScsGutterBoltingTypeEnum.PROFILE,
    //    WireCommonLocalizationDictionary.Profile,
    //    ScsGutterBoltingTypeEnum.CONSOLE,
    //    WireCommonLocalizationDictionary.Console,
    //    ScsGutterBoltingTypeEnum.CROSSBAR,
    //    WireCommonLocalizationDictionary.Crossbar,
    //    ScsGutterBoltingTypeEnum.CRAMP,
    //    WireCommonLocalizationDictionary.Cramp
    //})]
    //[EnumConvert(ScsGutterBoltingTypeEnum.CRONSH, ScsGutterBoltingTypeEnum.OTHER)]
    //[EnumConvert(ScsGutterBoltingTypeEnum.HUNGER, ScsGutterBoltingTypeEnum.OTHER)]
    //[EnumConvert(ScsGutterBoltingTypeEnum.CONNECTOR, ScsGutterBoltingTypeEnum.OTHER)]
    public enum ScsGutterBoltingTypeEnum {
        /// <summary>
        ///   Стойка
        /// </summary>
        [Description("Стойка")]
        POST = 0,
        /// <summary>
        ///   Шпилька
        /// </summary>
        [Description("Шпилька")]
        STUD = 1,
        /// <summary>
        /// Перекладина
        /// </summary>
        [Description("Перекладина")]
        CROSSBAR = 2,
        /// <summary>
        /// Устарело
        /// </summary>
        [Description("")]
        CONNECTOR = 3,
        /// <summary>
        /// Суппорт
        /// </summary>
        [Description("Суппорт")]
        SUPPORT = 4,
        /// <summary>
        ///  Консоль
        /// </summary>
        [Description("Консоль")]
        CONSOLE = 5,
        [Description("")]
        OTHER = 6,
        /// <summary>
        /// Устарело
        /// </summary>
        [Description("")]
        CRONSH = 7,
        /// <summary>
        /// Устарело
        /// </summary>
        [Description("")]
        HUNGER = 8,
        /// <summary>
        ///  Скоба
        /// </summary>
        [Description("Скоба")]
        CRAMP = 9,
        /// <summary>
        ///  Профиль
        /// </summary>
        [Description("Профиль")]
        PROFILE = 10
    }
}
