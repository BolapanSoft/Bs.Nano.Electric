// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElStarter {
#if InitDbContextEnums
       public ElPoleCountEnum? DbPoleCountEnum { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElDeviceTypeEnum? DbDeviceType { get; set; }

        public ElStarterTypeEnum? DbStarterType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }

 
#endif
    }
}
