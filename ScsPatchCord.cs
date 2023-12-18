namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScsPatchCord")]
    public partial class ScsPatchCord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? EnvType { get; set; }

        public double? DbLength { get; set; }

        public int? CordCordCount { get; set; }

        public int? CordPairCount { get; set; }

        public double? Diameter { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public string Series { get; set; }

        public string Description { get; set; }

        public string SpecDescription { get; set; }

        public string DbDocument { get; set; }

        public string Code { get; set; }

        public string EtmCode { get; set; }

        public string Manufacturer { get; set; }

        public string Url { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }

        public int? DbImageRef { get; set; }

        public int? FirstPort { get; set; }

        public int? SecondPort { get; set; }

        public int? CableSystemType { get; set; }
    }
}
