// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class DbElUzdp {
#if InitDbContextEnums
        [Column("Poles")]
       public ElPoleCountEnum? DbPoleCountEnum { get; set; }

        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElMountRailType? RailMountTypeFlagged { get; set; }

#endif
    }
}
