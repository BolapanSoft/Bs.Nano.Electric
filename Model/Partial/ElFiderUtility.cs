// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class ElFiderUtility {

#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }
        public ElInstallationType? InstallationType { get; set; }
        public ElDimensionType? FacadeDimensionType { get; set; }
        public ElDimensionType? InnerDimensionType { get; set; }
        public ElDimensionType? DimensionType { get; set; }

        public ElFiderUtilityTypeEnum? FiderUtilityType { get; set; }
        [NotMapped]
        public int? ExplodeLevel { get; set; }


#endif
    }
}
