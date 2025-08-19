using Microsoft.Extensions.Configuration;

namespace Bs.Nano.Electric.Builder {
    public interface IElectricBuilderConfiguration: IConfiguration {
        string CurrentDirectory { get; }
    }
    /// <summary>
    /// Представляет ошибку поиска секции в конфигурации.
    /// </summary>
    public class SectionNotFoundException:Exception {
        public SectionNotFoundException(string message):base(message) {
            
        }
    }
}
