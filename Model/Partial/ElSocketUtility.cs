// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class ElSocketUtility {
       
#if InitDbContextEnums
        public InstWallEnum DbInstType { get; set; }
        [Column("DbSafeDegree")]
        public int? SafeDegree { get; set; }
        [Column("DbClimate")]
        public int? Climate { get; set; }
        public ElSocketUtilityTypeEnum SocketUtilityType { get; set; }
#endif
    }
}
