// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class ElShieldingUnit {
        [NotMapped]
        string IProduct.Name { get=>DbName; set=>DbName=value; }
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum CableLeadIn { get; set; }
        public PhaseCountEnum? PhaseCount { get; set; }

#endif
    }
}
