namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScsGutterSupplement")]
    public partial class ScsGutterSupplement
    {
        public int? CanalSupplement { get; set; }

        public string Code { get; set; }

        public string DbDocument { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public double? CountPerBox { get; set; }

        public int? CoverType { get; set; }

        public double? PartitionHeight { get; set; }

        public double? CoverWidth { get; set; }

        public double? CoverLength { get; set; }

        public double? PartitionLength { get; set; }

        public double? CoverWidth1 { get; set; }

        public double? CoverWidth2 { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

        public string DbClassificatorCode { get; set; }
    }
}
