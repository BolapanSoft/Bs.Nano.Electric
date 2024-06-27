// Ignore Spelling: Scs Cabel

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ScsGutterCanal {
#if InitDbContextEnums
        /// <summary>
        /// Тип лотка
        /// </summary>
        public ScsGutterType GutterType { get; set; } = ScsGutterType.UNKNOWN;
        public ScsGutterCanalMeasureUnit? BoxMeasureUnit { get; set; } = ScsGutterCanalMeasureUnit.IN_SEGMENTS; 
#endif
    }
}
