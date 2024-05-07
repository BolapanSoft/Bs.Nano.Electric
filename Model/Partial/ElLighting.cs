// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nano.Electric {
    public partial class ElLighting : IProduct, IHaveExplodeLevel {
#if InitDbContextEnums
        [NotMapped]
        public string Name { get=>DbName; set { DbName = value;} }
        public EnvExplSafeBoolEnum? IsExplSafe { get; set; }
        public ElLightingPurpouse LightingPurpouse { get; set; }
        public LampExistance LampExistance { get; set; }
        public ElLampSocleEnum Socle { get; set; }
        //public ElLamp Lamp { get; set; }
        public KccTypeEnum KccPmType { get; set; }
        public InstLightingEnum DbInstType { get; set; }
        //public EnvExplSafeBoolEnum IsExplosionSafe { get; set; }
        //public ExplodeSafeLevel ExplodeLevel { get; set; }
        //public ClimateTable Climate { get; set; }
        public ElLightingDimensionType DimensionType { get; set; }
#endif
    }
}
