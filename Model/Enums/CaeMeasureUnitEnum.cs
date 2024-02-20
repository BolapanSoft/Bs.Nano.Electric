using System.ComponentModel;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    CaeMeasureUnitEnum.UNIT_PCS,
    //    DataBaseLocalizationDictionary.Шт,
    //    CaeMeasureUnitEnum.UNIT_MS,
    //    WireCommonLocalizationDictionary.М,
    //    CaeMeasureUnitEnum.UNIT_M2,
    //    WireCommonLocalizationDictionary.М2,
    //    CaeMeasureUnitEnum.UNIT_M3,
    //    WireCommonLocalizationDictionary.М3,
    //    CaeMeasureUnitEnum.UNIT_KMS,
    //    WireCommonLocalizationDictionary.Км,
    //    CaeMeasureUnitEnum.UNIT_KGS,
    //    WireCommonLocalizationDictionary.Кг,
    //    CaeMeasureUnitEnum.UNIT_TS,
    //    WireCommonLocalizationDictionary.Т
    //})]
    public enum CaeMeasureUnitEnum {
        /// <summary>
        /// "шт"
        /// </summary>
        [Description("шт")]
        UNIT_PCS = 0,
        /// <summary>
        /// "м"
        /// </summary>
        [Description("м")]
        UNIT_MS = 1,
        /// <summary>
        /// "м²"
        /// </summary>
        [Description("м²")]
        UNIT_M2 = 2,
        /// <summary>
        /// "м³"
        /// </summary>
        [Description("м³")]
        UNIT_M3 = 3,
        /// <summary>
        /// "км"
        /// </summary>
        [Description("км")]
        UNIT_KMS = 4,
        /// <summary>
        /// "кг"
        /// </summary>
        [Description("кг")]
        UNIT_KGS = 5,
        /// <summary>
        /// "т"
        /// </summary>
        [Description("т")]
        UNIT_TS = 6
    }
}
