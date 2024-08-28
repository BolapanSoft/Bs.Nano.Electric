namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElShieldingUnit")]
    public partial class ElShieldingUnit
    {
#if !InitDbContextEnums
        public int? IsExplSafe { get; set; }

        public int? CableLeadIn { get; set; }
        public int? PhaseCount { get; set; }
#endif     

        public string Code { get; set; }
        [Column("DbClimate")]
        public int? Climate { get; set; }

        public double? DbDepth { get; set; }

        public string DbDocument { get; set; }

        public double? DbHeight { get; set; }

        public string DbName { get; set; }
        [Column("DbSafeDegree")]
        public int? SafeDegree { get; set; }

        public double? DbWidth { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public double? NominalPower { get; set; }

        public int? OrderNumber { get; set; }


        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public double? VoltageUp { get; set; }
        //[MaxLength(-1)]
        //public string KitStructure { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? ExplodeLevel { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

        public double? VoltageDownValue { get; set; }

        public int? DbGraphicRef { get; set; }
#if DBNE23
        public string DbClassificatorCode { get; set; }
#endif

#if DBNE23
        public string SafeDegreeIk { get; set; }
#endif
    }
}
