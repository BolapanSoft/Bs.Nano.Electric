// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ElWireMark  {

#if InitDbContextEnums
        public DbWireCategory? WireCategory { get; set;}
        public ElWireIsolationMaterial? IsolationMaterial { get; set; } // isolationMaterialId
        public ElWireConductMaterial? Material { get; set; } // materialId
        public ElWireShellFireEnum WireShellFire { get; set; } = ElWireShellFireEnum.FIRE_NOT_SET;
        public ElWireArmorEnum WireArmor { get; set; }= ElWireArmorEnum.NOT_SET;
#endif
    }
}
