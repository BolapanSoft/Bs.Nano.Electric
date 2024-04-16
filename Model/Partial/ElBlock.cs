// Ignore Spelling: Scs Cabel

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElBlock {
#if InitDbContextEnums
        public AutomatContactType? ContactType { get; set; } = AutomatContactType.STATIC; 
#endif
    }

}
