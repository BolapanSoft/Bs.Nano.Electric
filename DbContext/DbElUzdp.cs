namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbElUzdp")]
    public partial class DbElUzdp {
#if !InitDbContextEnums
        public int? DbPoleCountEnum { get; set; }
        public int? IsExplSafe { get; set; }
        public int? MountType { get; set; }
        public int? RailMountTypeFlagged { get; set; }


#endif
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] 
        public int Id { get; set; }      
        public string Purpose { get; set; }
        public string NameConsistingOfNcu { get; set; }
        public double? Voltage { get; set; }
        public double? NominalCurrent { get; set; }
        public double? MaxDynCurrent { get; set; }
        public double? ActiveResistance { get; set; }
        public double? MaxCordS { get; set; }
        public bool? DbIsModule { get; set; }
        public double? DbModuleCount { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string Description { get; set; }
        public string SpecDescription { get; set; }
        public string DbDocument { get; set; }
        public string Code { get; set; }
        public string EtmCode { get; set; }
        public string Manufacturer { get; set; }
        public string Url { get; set; }
        public double? DbHeight { get; set; }
        public double? DbWidth { get; set; }
        public double? DbDepth { get; set; }
        public string Mass { get; set; }
        public int? OrderNumber { get; set; }
        public int? CwPCode { get; set; }
        public bool? InImport { get; set; }
        public int? DbGraphicRef { get; set; }
        public int? DbImageRef { get; set; }
        [Column("DbClimate")]
        public int? Climate { get; set; }
        public int? ExplodeLevel { get; set; }
        [Column("DbSafeDegree")]
        public int? SafeDegree { get; set; }
#if DBNE23
        public string DbClassificatorCode { get; set; }
        public string SafeDegreeIk { get; set; }
#endif
    }
}
