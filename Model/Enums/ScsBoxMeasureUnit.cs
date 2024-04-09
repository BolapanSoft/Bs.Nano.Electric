using System.ComponentModel;
namespace Nano.Electric.Enums {
    /// <summary>
    /// Единица Измерения В Упаковке
    /// </summary>
    public enum ScsBoxMeasureUnit {
        [Description("м.")]
        /// <summary>
        /// В метрах
        /// </summary>
        IN_METERS = 0,
        [Description("шт.")]
        /// <summary>
        /// шт.
        /// </summary>
        IN_SEGMENTS = 1
    }
}
