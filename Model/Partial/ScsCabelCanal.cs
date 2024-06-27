// Ignore Spelling: Scs Cabel

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ScsCabelCanal {
#if InitDbContextEnums
        public ScsGutterCanalMeasureUnit? BoxMeasureUnit { get; set; } = ScsGutterCanalMeasureUnit.IN_SEGMENTS; 
#endif
    }
    public partial class ScsPipe {
#if InitDbContextEnums
        public TbTypeEnum? TubeType { get; set; }
#endif
    }
    public partial class ScsTubeFitting {
#if InitDbContextEnums
        public ScsTubeFittingTypeEnum? FittingType { get; set; }
#endif
    }

}
