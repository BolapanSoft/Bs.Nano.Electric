using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class DbCableCanalPartition {
#if InitDbContextEnums
        public ScsBoxMeasureUnit? BoxMeasureUnit { get; set; } = ScsBoxMeasureUnit.IN_SEGMENTS;

#endif
  }
}
