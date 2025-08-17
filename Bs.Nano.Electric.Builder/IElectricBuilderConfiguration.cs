using Microsoft.Extensions.Configuration;

namespace Bs.Nano.Electric.Builder {
    public interface IElectricBuilderConfiguration: IConfiguration {
        string CurrentDirectory { get; }
    }
}
