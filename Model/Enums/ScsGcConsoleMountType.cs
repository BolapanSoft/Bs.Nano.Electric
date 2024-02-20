//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
//using System;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //    {
    //    ScsGcConsoleMountType.POST,
    //    WireCommonLocalizationDictionary.PostMountType,
    //    ScsGcConsoleMountType.WALL,
    //    WireCommonLocalizationDictionary.WallMountType,
    //    ScsGcConsoleMountType.UNIVERSAL,
    //    WireCommonLocalizationDictionary.UniversalMountType,
    //    ScsGcConsoleMountType.CELL,
    //    WireCommonLocalizationDictionary.CBracket,
    //    ScsGcConsoleMountType.L_WALL,
    //    WireCommonLocalizationDictionary.LBracket
    //    })]
    public enum ScsGcConsoleMountType {
        /// <summary>
        /// Стоечная
        /// </summary>
        [Description("Стоечная")]
        POST = 0,
        /// <summary>
        /// Настенная
        /// </summary>
        [Description("Настенная")]
        WALL = 1,
        /// <summary>
        /// Универсальная
        /// </summary>
        [Description("Универсальная")]
        UNIVERSAL = 2,
        /// <summary>
        /// C-подвес
        /// </summary>
        [Description("C-подвес")]
        CELL = 3,
        /// <summary>
        /// L-подвес
        /// </summary>
        [Description("L-подвес")]
        L_WALL = 4
    }
}
