namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElKnifeSwitch")]
    public partial class ElKnifeSwitch
    {
#if !InitDbContextEnums
        public int? IsExplSafe { get; set; }
        public int? MountType { get; set; }
        public int? Poles { get; set; }
      public int? RailMountTypeFlagged { get; set; }


#endif
        public double? ActiveResistance { get; set; }

        public string Code { get; set; }

        public int? DbClimate { get; set; }

        public double? DbDepth { get; set; }

        public string DbDocument { get; set; }

        public double? DbHeight { get; set; }

        public int? DbSafeDegree { get; set; }

        public double? DbWidth { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public double? MaxCordS { get; set; }

        public string Name { get; set; }

        public string NameConsistingOfNcu { get; set; }

        public double? NominalCurrent { get; set; }

        public int? OrderNumber { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public double? Voltage { get; set; }


        public bool? DbIsModule { get; set; }

        public double? DbModuleCount { get; set; }

        public double? MaxDynCurrent { get; set; }


        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? ExplodeLevel { get; set; }


        public string EtmCode { get; set; }

          public bool? InImport { get; set; }

        public string Purpose { get; set; }

        public string Url { get; set; }

        public int? DbGraphicRef { get; set; }

        public string DbClassificatorCode { get; set; }

        public string SafeDegreeIk { get; set; }
    }
}
