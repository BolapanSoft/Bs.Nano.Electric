// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElKnifeSwitch {
        
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElPoleCountEnum? Poles { get; set; }
      public ElMountRailType? RailMountTypeFlagged { get; set; }


#endif
    }
}
