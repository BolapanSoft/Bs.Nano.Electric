namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScsPipe")]
    public partial class ScsPipe
    {
        public string Code { get; set; }

        public string DbDocument { get; set; }

        public string Description { get; set; }

        public double? ExternalDiameter { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public double? InternalDiameter { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public double? SegLength { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public int? TubeType { get; set; }

        public string Url { get; set; }

        public string DbClassificatorCode { get; set; }
    }
}
