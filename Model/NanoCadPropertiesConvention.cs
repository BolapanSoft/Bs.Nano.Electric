using System.Data.Entity.ModelConfiguration.Conventions;

namespace Nano.Electric {
    public class NanoCadPropertiesConvention : Convention {
        public NanoCadPropertiesConvention() {
            Properties<string>().Configure(c => {
                c.HasColumnType("ntext");
            });
            //Properties<byte[]>().Configure(c => {
            //    c.HasMaxLength(-1);
            //});
        }
    }
}
