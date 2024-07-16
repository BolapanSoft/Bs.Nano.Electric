// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElControlDevice {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        private ElInstallationType? installationType { get; set; }

        public ElInstallationType? InstallationType {
            get => installationType;
            set {
                if (value != ElInstallationType.OnApparatus)
                    installationType = value;
            }
        }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }
        public ElDimensionType? DimensionType { get; set; }
        public ElDimensionType? FacadeDimensionType { get; set; }
        public ElDimensionType? InnerDimensionType { get; set; }

#endif
    }
}
