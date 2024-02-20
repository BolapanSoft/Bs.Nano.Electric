//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    ScsGcStandType.ONE_SIDE,
    //    WireCommonLocalizationDictionary.OneSide,
    //    ScsGcStandType.TWO_SIDE,
    //    WireCommonLocalizationDictionary.TwoSide
    //})]
    public enum ScsGcStandType {
        /// <summary>
        /// Односторонняя
        /// </summary>
        [Description("Односторонняя")]
        ONE_SIDE = 0,
        /// <summary>
        /// Двусторонняя
        /// </summary>
        [Description("Двусторонняя")]
        TWO_SIDE = 1
    }
}
