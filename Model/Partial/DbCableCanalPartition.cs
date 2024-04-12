using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class DbCableCanalPartition {
#if InitDbContextEnums
        public ScsBoxMeasureUnit? BoxMeasureUnit { get; set; } = ScsBoxMeasureUnit.IN_SEGMENTS;

#endif
    }
    public partial class ElBoard {
#if InitDbContextEnums
        public BoardStructureType StructureType { get; set; }
        public BoardType BoardType { get; set; }
        public InstWallEnum DbInstType { get; set; }
        public EnvExplSafeBoolEnum IsExplSafe { get; set; }

#endif
    }
}
