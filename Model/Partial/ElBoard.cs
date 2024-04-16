// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class ElBoard {
#if InitDbContextEnums
        public BoardStructureType StructureType { get; set; }
        public BoardType BoardType { get; set; }
        public InstBoxEnum DbInstType { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum CableLeadIn { get; set; }

#endif
    }
    public partial class ElSafeDevice {
        [NotMapped]
        int? IHaveSafeDegree.DbSafeDegree { get => SafeDegree; set => SafeDegree = value; }
        [NotMapped]
        int? IHaveDbClimate.DbClimate { get => Climate; set => Climate=value; }
#if InitDbContextEnums
       public ElPoleCountEnum? DbPoleCountEnum { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }

#endif
    }
    public partial class ElFiderUtility {

#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }
        public ElInstallationType? InstallationType { get; set; }
        public ElDimensionType? FacadeDimensionType { get; set; }
        public ElDimensionType? InnerDimensionType { get; set; }
        public ElDimensionType? DimensionType { get; set; }


 
#endif
    }
}
