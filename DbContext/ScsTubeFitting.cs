namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScsTubeFitting")]
    public partial class ScsTubeFitting
    {
        public string? Code { get; set; }

        public string? DbDocument { get; set; }

        public string? DbOtherName { get; set; }

        public string? Description { get; set; }

        public double? Diameter { get; set; }

        public double? Diameter1Branch { get; set; }

        public double? Diameter2Branch { get; set; }

        public double? DiameterMainBranch { get; set; }

        public double? DiameterOutBranch { get; set; }

#if !InitDbContextEnums
        public int? FittingType { get; set; } 
#endif

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? Manufacturer { get; set; }

        public string? Mass { get; set; }

        public string? Name { get; set; }

        public int? OrderNumber { get; set; }

        public string? Series { get; set; }

        public string? SpecDescription { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string? EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string? Url { get; set; }

#if DBNE23
        public string? DbClassificatorCode { get; set; }
#endif
    }
}
