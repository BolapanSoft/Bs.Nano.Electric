using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    public enum ScsGutterFittingTypeEnum {
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
        /// Секция Т-образная горизонтальная
        /// </summary>
        [Description("Секция Т-образная горизонтальная")]
        TRIPLE = 3,
        /// <summary>
        /// Секция Х-образная горизонтальная
        /// </summary>
        [Description("Секция Х-образная горизонтальная")]
        CROSS = 4,
        [Description("")]
        OTHER = 5,
        /// <summary>
        /// Соединитель на стык
        /// </summary>
        [Description("Соединитель на стык")]
        JOINT = 6,
        /// <summary>
        /// Секция угловая вертикальная внешняя
        /// </summary>
        [Description("Секция угловая вертикальная внешняя")]
        VERTICAL_BEND_OUTER = 7,
        /// <summary>
        /// Торцевая заглушка
        /// </summary>
        [Description("Торцевая заглушка")]
        CORK = 8,
        /// <summary>
        /// Хомут
        /// </summary>
        [Description("Хомут")]
        COLLAR = 9,
        /// <summary>
        /// Секция соединительная переходная горизонтальная
        /// </summary>
        [Description("Секция соединительная переходная горизонтальная")]
        HORIZONTAL_PASSAGE = 10,
        /// <summary>
        /// Секция соединительная переходная вертикальная
        /// </summary>
        [Description("Секция соединительная переходная вертикальная")]
        VERTICAL_PASSAGE = 11,
        /// <summary>
        /// Секция угловая вертикальная универсальная
        /// </summary>
        [Description("Секция угловая вертикальная универсальная")]
        VERTICAL_BENT_UNIVERSE = 12
    }
}
