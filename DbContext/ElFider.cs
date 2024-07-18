namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElFider")]
    public partial class ElFider
    {
#if !InitDbContextEnums
#else
#endif
        public int? BlockParent { get; set; }

        public int? BlockParent_index { get; set; }

        public int? BoxParent { get; set; }

        public int? BoxParent_index { get; set; }

        public int? FiderFillingEnum { get; set; }

        public int? FiderType { get; set; }

        public int? FrequenceTransformer { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public int? InputBusBoardParent { get; set; }

        public int? InputBusBoardParent_index { get; set; }

        public bool? IsFrequenceTransformer { get; set; }

        public bool? IsKnifeSwitch { get; set; }

        public bool? IsSafeAutomat { get; set; }

        public bool? IsSafeFuse { get; set; }

        public bool? IsStarter { get; set; }

        public bool? IsUzo { get; set; }

        public int? KnifeSwitch { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public int? PanelParent { get; set; }

        public int? PanelParent_index { get; set; }

        public int? PoleCount { get; set; }

        public int? SafeAutomat { get; set; }

        public int? SafeFuse { get; set; }

        public int? ShieldUnitParent { get; set; }

        public int? ShieldUnitParent_index { get; set; }

        public int? StandartBoardParent { get; set; }

        public int? StandartBoardParent_index { get; set; }

        public int? Starter { get; set; }

        public int? Uzo { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }
    }
}
