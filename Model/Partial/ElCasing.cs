// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElCasing {

#if InitDbContextEnums
        public InstBoxEnum DbInstType { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum CableLeadIn { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }
 
#endif
    }
}
