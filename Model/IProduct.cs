#pragma warning disable VSSpell001 // Spell Check

namespace Nano.Electric {
    public interface IHaveId {
        int Id { get; set; }
    }
    public partial class SafeDegree : IHaveId { }
    public partial class ClimateTable : IHaveId { }
    public interface IProduct: IHaveId {
        string Code { get; }
        //int Id { get; set; }
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
    public partial class ElBoard : IProduct { }
}
