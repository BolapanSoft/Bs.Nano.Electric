namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScsCord")]
    public partial class ScsCord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string DbComment { get; set; }

        public string Description { get; set; }

        public string DbSeries { get; set; }

        public string Standart { get; set; }

        public string Code { get; set; }

        public double? CountPerBox { get; set; }

        public string Manufacturer { get; set; }

        public int? EnvType { get; set; }

        public int? CordPairCount { get; set; }

        public int? CordCordCount { get; set; }

        public double? Diameter { get; set; }

        public string Mass { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? CableSystemType { get; set; }

        public double? Resistance { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }
    }
}
