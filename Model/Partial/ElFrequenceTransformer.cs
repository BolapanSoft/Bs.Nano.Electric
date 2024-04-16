// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElFrequenceTransformer {

#if InitDbContextEnums
        public PhaseCountEnum? PhaseCount { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }
 
#endif
    }
}
