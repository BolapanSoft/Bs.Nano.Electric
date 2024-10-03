// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ScsUtpSocket {
#if InitDbContextEnums
        public ScsCableSystemType? CableSystemType { get; set; }
        public ScsUtpSocketTypeEnum? UtpSocketType { get; set; }
        public ScsPortType? PortType { get; set; }
#endif

    }
}
