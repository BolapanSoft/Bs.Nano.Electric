// Ignore Spelling: Expl

using Nano.Electric.Enums;

namespace Nano.Electric {
    public partial class ExplodeSafeLevel {
#if InitDbContextEnums
        public EnvExplosionLevelEnum ExplLevel { get; set; }
        public EnvExplosionMixtureCategoryEnum ExplMixCategory { get; set; }
        public EnvExplosionMixtureGroupEnum ExplGroup { get; set; }
        public EnvExplosionTypeEnum ExplType { get; set; }
#endif
    }
}
