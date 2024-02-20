using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {

    public enum ScsGutterType {
        [Description("Не задан")]
        /// <summary>
        /// Не задан
        /// </summary>
        UNKNOWN = 0,
        [Description("Неперфорированный")]
        /// <summary>
        /// Неперфорированный
        /// </summary>
        NOT_PERFO = 1,
        [Description("Перфорированный")]
        /// <summary>
        /// Перфорированный
        /// </summary>
        PERFO = 2,
        [Description("Проволочный")]
        /// <summary>
        /// Проволочный
        /// </summary>
        WIRED = 3,
        [Description("Лестничный")]
        /// <summary>
        /// Лестничный
        /// </summary>
        LADDER = 4
    }

}
