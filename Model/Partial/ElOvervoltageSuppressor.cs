// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElOvervoltageSuppressor {
        
#if InitDbContextEnums
       public ElPoleCountEnum? PoleCount { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }

#endif
    }
}
