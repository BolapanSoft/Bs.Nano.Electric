// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class ElAutomat {
        [NotMapped]
        int? IHaveSafeDegree.DbSafeDegree { get => SafeDegree; set => SafeDegree = value; }
        [NotMapped]
        int? IHaveDbClimate.DbClimate { get => Climate; set => Climate=value; }


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
