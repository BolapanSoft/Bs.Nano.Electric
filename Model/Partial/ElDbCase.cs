// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class ElDbCase {
       
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public InstWallEnum DbInstType { get; set; }
        [Column("DbSafeDegree")]
        public int? SafeDegree { get; set; }
        [Column("DbClimate")]
        public int? Climate { get; set; }

#endif
    }
    public partial class ElResistReactor {
       
#if InitDbContextEnums
        [Column("DbSafeDegree")]
        public int? SafeDegree { get; set; }
        [Column("DbClimate")]
        public int? Climate { get; set; }

#endif
    }
}
