// Ignore Spelling: Expl

namespace Nano.Electric {
    public partial class ElWire : IProduct {
        string IProduct.Code { get => Code; }
        int? IProduct.DbImageRef { get => DbImageRef; set { DbImageRef = value; } }
        string IProduct.Name { get => wireName; set { wireName = value; } }
        string IProduct.Manufacturer { get => mfrId; set { mfrId = value; } }
        int IHaveId.Id { get => Id; set { Id = value; } }
    }

}
