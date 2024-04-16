// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
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
}
