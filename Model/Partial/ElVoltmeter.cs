﻿// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElVoltmeter {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType MountType { get; set; }
        public ElMountRailType RailMountTypeFlagged { get; set; }
        public ElInstallationType InstallationType { get; set; }
        public ElDimensionType DimensionType { get; set; }
        public ElDimensionType FacadeDimensionType { get; set; }
        public ElDimensionType InnerDimensionType { get; set; }
        public PhaseCountEnum? PoleCount { get; set; } = PhaseCountEnum.THREE;

#endif
    }
}
