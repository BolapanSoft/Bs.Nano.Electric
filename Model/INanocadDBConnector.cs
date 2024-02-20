using NanoCadContext = Nano.Electric.Context;

namespace Bs.Nano.Electric.Model {
    public interface INanocadDBConnector {
        NanoCadContext Connect();
    }
}
