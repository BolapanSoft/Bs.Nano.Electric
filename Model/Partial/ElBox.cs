// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.Diagnostics.PerformanceData;
using static System.Security.Cryptography.ECCurve;

namespace Nano.Electric {
    public partial class ElBox {
#if InitDbContextEnums
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum CableLeadIn { get; set; }
        public ElBoxAppointment? BoxAppointment { get; set; }

#endif
    }
}
