namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    [Table("DbScsGcCoverUnit")]
    public partial class DbScsGcCoverUnit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]
        public override int Id { get; set; }

        public double? CountPerBox { get; set; }

#if !InitDbContextEnums
        public int? CoverType { get; set; } 
#endif

        public string Mass { get; set; }

        public double? CoverWidth { get; set; }

        public double? CoverWidth1 { get; set; }

        public double? CoverWidth2 { get; set; }

        public string Name { get; set; }

        public string Series { get; set; }

        public string Description { get; set; }

        public string SpecDescription { get; set; }

        public string DbDocument { get; set; }

        public string Code { get; set; }

        public string Manufacturer { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

#if DBNE23
        public string DbClassificatorCode { get; set; }
#endif
    }
}
