namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElBlock")]
    public partial class ElBlock
    {
        public string? Code { get; set; }

        public double? DbDepth { get; set; }

        public double? DbHeight { get; set; }

        public double? DbWidth { get; set; }

        public string? Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? Manufacturer { get; set; }

        public string? Mass { get; set; }

        public string? Name { get; set; }

        public int? OrderNumber { get; set; }

        public string? Series { get; set; }

        public string? SpecDescription { get; set; }

#if !InitDbContextEnums
        public int? ContactType { get; set; } 
#endif
        //[MaxLength(-1)]
        //public string? KitStructure { get; set; }

        public int? CwPCode { get; set; }

        public string? EtmCode { get; set; }

        public bool? InImport { get; set; }

        public int? DbImageRef { get; set; }

        public string? Url { get; set; }

        public int? DbGraphicRef { get; set; }

#if DBNE23
        public string? DbClassificatorCode { get; set; }
#endif
    }
}
