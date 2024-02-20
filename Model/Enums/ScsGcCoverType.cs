

//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    ScsGcCoverType.BEND,
    //    WireCommonLocalizationDictionary.СекцияУгловаяГоризонтальная,
    //    ScsGcCoverType.VERTICAL_BEND_INNER,
    //    WireCommonLocalizationDictionary.СекцияУгловаяВертикальнаяВнутренняя,
    //    ScsGcCoverType.VERTICAL_BEND_OUTER,
    //    WireCommonLocalizationDictionary.СекцияУгловаяВертикальнаяВнешняя,
    //    ScsGcCoverType.TRIPLE,
    //    WireCommonLocalizationDictionary.СекцияТ_образнаяГоризонтальная,
    //    ScsGcCoverType.CROSS,
    //    WireCommonLocalizationDictionary.СекцияХ_образнаяГоризонтальная,
    //    ScsGcCoverType.HORIZONTAL_PASSAGE,
    //    WireCommonLocalizationDictionary.СекцияСоединительнаяПереходнаяГоризонтальная,
    //    ScsGcCoverType.VERTICAL_PASSAGE,
    //    WireCommonLocalizationDictionary.СекцияСоединительнаяПереходнаяВертикальная
    //})]
    public enum ScsGcCoverType {
        /// <summary>
        /// Прямая
        /// </summary>
        [Description("Прямая")]
        STRIGHT = 0,
        /// <summary>
        /// Секция угловая горизонтальная
        /// </summary>
        [Description("Секция угловая горизонтальная")]
        BEND = 1,
        /// <summary>
        /// Секция угловая вертикальная внутренняя
        /// </summary>
        [Description("Секция угловая вертикальная внутренняя")]
        VERTICAL_BEND_INNER = 2,
        /// <summary>
        /// Секция угловая вертикальная внешняя
        /// </summary>
        [Description("Секция угловая вертикальная внешняя")]
        VERTICAL_BEND_OUTER = 3,
        /// <summary>
        /// Секция Т-образная горизонтальная
        /// </summary>
        [Description("Секция Т-образная горизонтальная")]
        TRIPLE = 4,
        /// <summary>
        /// Секция Х-образная горизонтальная
        /// </summary>
        [Description("Секция Х-образная горизонтальная")]
        CROSS = 5,
        /// <summary>
        /// Секция соединительная переходная горизонтальная
        /// </summary>
        [Description("Секция соединительная переходная горизонтальная")]
        HORIZONTAL_PASSAGE = 6,
        /// <summary>
        /// Секция соединительная переходная вертикальная
        /// </summary>
        [Description("Секция соединительная переходная вертикальная")]
        VERTICAL_PASSAGE = 7
    }
}
