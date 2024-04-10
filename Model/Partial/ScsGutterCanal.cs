// Ignore Spelling: Scs Cabel

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ScsGutterCanal {
#if InitDbContextEnums
        /// <summary>
        /// Тип лотка
        /// </summary>
        public ScsGutterType GutterType { get; set; } = ScsGutterType.UNKNOWN;
        public ScsBoxMeasureUnit? BoxMeasureUnit { get; set; } = ScsBoxMeasureUnit.IN_SEGMENTS; 
#endif
    }
}
