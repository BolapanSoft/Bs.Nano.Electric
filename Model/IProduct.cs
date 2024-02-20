namespace Nano.Electric {
    public interface IProduct {
        string Code { get; }
        int id { get; set; }
        int? DbImageRef { get; set; }
        string Name { get; set; }
        string Manufacturer { get; set; }
    }

    public partial class ScsGutterCanal : IProduct { }
    public partial class DbScsGutterCover : IProduct { }
    public partial class DbScsGutterPartition : IProduct { }
    public partial class ScsGcFitting : IProduct { }
    public partial class DbScsGcCoverUnit : IProduct { }
    public partial class DbScsGcAccessoryUnit : IProduct { }
    public partial class ScsGutterBolting : IProduct { }
    public partial class DbScsGcBoltingAccessoryUnit : IProduct { }
    public partial class CaeMaterialUtility : IProduct { }
}
