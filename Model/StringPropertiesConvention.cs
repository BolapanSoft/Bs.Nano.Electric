using System.Data.Entity.ModelConfiguration.Conventions;

namespace Nano.Electric {
    public class StringPropertiesConvention : Convention {
        public StringPropertiesConvention() {
            Properties<string>().Configure(c => {
                c.HasColumnType("ntext");
            });
        }
    }
}
