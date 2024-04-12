namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScsShellDistr")]
    public partial class ScsShellDistr
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public int? DbCellCount { get; set; }

        public string Name { get; set; }

        public string Series { get; set; }

        public string Description { get; set; }

        public string SpecDescription { get; set; }

        public string DbDocument { get; set; }

        public string Code { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public double? DbHeight { get; set; }

        public double? DbWidth { get; set; }

        public double? DbDepth { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

        public string EtmCode { get; set; }

        public int? DbGraphicRef { get; set; }

        public string DbClassificatorCode { get; set; }
    }
}
