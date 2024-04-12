using System.ComponentModel;

namespace Nano.Electric.Enums {
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
