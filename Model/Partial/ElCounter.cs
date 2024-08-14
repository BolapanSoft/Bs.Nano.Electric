// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.Collections.Concurrent;

namespace Nano.Electric {
    public partial class ElCounter {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType MountType { get; set; }
        public ElMountRailType RailMountTypeFlagged { get; set; }
        public ElInstallationType InstallationType { get; set; }
        public ElDimensionType DimensionType { get; set; }
        public ElDimensionType FacadeDimensionType { get; set; }
        public ElDimensionType InnerDimensionType { get; set; }
        public ElCounterType CounterType { get; set; }
        public ElControlMeasureDeviceInstallType InstallType { get; set; }
        public PhaseCountEnum? PoleCount { get; set; }

#endif
    }
    public partial class ScsPatchCord {
#if InitDbContextEnums
       public CordTypeEnum? EnvType { get; set; }

#endif
    }
}
