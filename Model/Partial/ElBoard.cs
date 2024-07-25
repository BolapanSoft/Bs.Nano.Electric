// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElBoard {
#if InitDbContextEnums
        public BoardStructureType? StructureType { get; set; }
        public BoardType? BoardType { get; set; }
        public InstBoxEnum? DbInstType { get; set; }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElCableLeadEnum? CableLeadIn { get; set; }

#endif
    }
}
