//using Cadwise.Reflection;
//using Cadwise.ObjectLib.WireCommon.Db.Localization;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    DbGcMsAcceesorySelectType.NO,
    //    WireCommonLocalizationDictionary.НеПодбиратьПоРазмерамЛотка,
    //    DbGcMsAcceesorySelectType.SEL_H,
    //    WireCommonLocalizationDictionary.ПодбиратьПоВысотеЛотка,
    //    DbGcMsAcceesorySelectType.SEL_W,
    //    WireCommonLocalizationDictionary.ПодбиратьПоШиринеЛотка,
    //    DbGcMsAcceesorySelectType.SEL_W_H,
    //    WireCommonLocalizationDictionary.ПодбиратьПоВысотеИШиринеЛотка,
    //    DbGcMsAcceesorySelectType.SEL_DELW_H,
    //    WireCommonLocalizationDictionary.ПодбиратьПоПерепадуШириныИВысотеЛотка,
    //    DbGcMsAcceesorySelectType.SEL_DELH_W,
    //    WireCommonLocalizationDictionary.ПодбиратьПоПерепадуВысотыИШиринеЛотка
    //})]
    public enum DbGcMsAccessorySelectType {
        /// <summary>
        ///    Не подбирать по размерам лотка
        /// </summary>
        [Description("Не подбирать по размерам лотка")]
        NO = 0,
        /// <summary>
        ///    Подбирать по высоте лотка
        /// </summary>
        [Description("Подбирать по высоте лотка")]
        SEL_H = 1,
        /// <summary>
        ///    Подбирать по ширине лотка
        /// </summary>
        [Description("Подбирать по ширине лотка")]
        SEL_W = 2,
        /// <summary>
        ///   Подбирать по высоте и ширине лотка
        /// </summary>
        [Description("Подбирать по высоте и ширине лотка")]
        SEL_W_H = 3,
        /// <summary>
        ///    Подбирать по перепаду ширины и высоте лотка
        /// </summary>
        [Description("Подбирать по перепаду ширины и высоте лотка")]
        SEL_DELW_H = 4,
        /// <summary>
        ///    Подбирать по перепаду высоты и ширине лотка
        /// </summary>
        [Description("Подбирать по перепаду высоты и ширине лотка")]
        SEL_DELH_W = 5
    }
}
