// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElBox {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum CableLeadIn { get; set; }
        public ElBoxAppointment? BoxAppointment { get; set; } 

#endif
    }
}
