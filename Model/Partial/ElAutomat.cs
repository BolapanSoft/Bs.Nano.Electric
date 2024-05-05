// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class ElAutomat {

#if InitDbContextEnums
        public ElPoleCountEnum? DbPoleCountEnum { get; set; }
        public ElCurrentChoiseEnum? CurrentChoice { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public AutomatContactType? ContactType { get; set; }
        public ElControlRegisterDeviceMountType? MountType { get; set; }
        public ElInstantKzCurrentChoiseEnum? KzInstantCurrentChoice { get; set; }
        // TODO: Реализовать импорт [Flags] Enum
        public RatedNetVoltageType? VoltageType { get; set; }

#endif
    }
}
