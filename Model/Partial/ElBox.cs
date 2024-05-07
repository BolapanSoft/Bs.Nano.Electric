// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.Diagnostics.PerformanceData;

namespace Nano.Electric {
    public partial class ElBox {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum CableLeadIn { get; set; }
        public ElBoxAppointment? BoxAppointment { get; set; }

#endif
    }
    public partial class ElPushButtonStation {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }

#endif
    }
    public partial class ElControlDevice {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType MountType { get; set; }
        public ElMountRailType RailMountTypeFlagged { get; set; }
        public ElDimensionType DimensionType { get; set; }
        public ElDimensionType FacadeDimensionType { get; set; }
        public ElDimensionType InnerDimensionType { get; set; }
        private ElInstallationType _installationType = ElInstallationType.InsideShell;
        public ElInstallationType InstallationType {
            get {
                return this._installationType;
            }
            set {
                if (value != ElInstallationType.OnApparatus) {
                    this._installationType = value;
                }
            }
        }
#endif
    }
    public partial class ElCurrentTransformer {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType MountType { get; set; }
        public ElMountRailType RailMountTypeFlagged { get; set; }
        public ElInstallationType InstallationType { get; set; }
        public ElDimensionType DimensionType { get; set; }
        public ElDimensionType FacadeDimensionType { get; set; }
        public ElDimensionType InnerDimensionType { get; set; }
#endif
    }
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
#endif
    }
    public partial class ElVoltmeter {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType MountType { get; set; }
        public ElMountRailType RailMountTypeFlagged { get; set; }
        public ElInstallationType InstallationType { get; set; }
        public ElDimensionType DimensionType { get; set; }
        public ElDimensionType FacadeDimensionType { get; set; }
        public ElDimensionType InnerDimensionType { get; set; }
#endif
    }
    public partial class ElAmperemeter {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType MountType { get; set; }
        public ElMountRailType RailMountTypeFlagged { get; set; }
        public ElInstallationType InstallationType { get; set; }
        public ElDimensionType DimensionType { get; set; }
        public ElDimensionType FacadeDimensionType { get; set; }
        public ElDimensionType InnerDimensionType { get; set; }
        public ElControlMeasureDeviceInstallType InstallType { get; set; }
#endif
    }
}
