namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElCase")]
    public partial class ElCase
    {
        public string Code { get; set; }

        public int? DbClimate { get; set; }

        public double? DbDepth { get; set; }

        public string DbDocument { get; set; }

        public double? DbHeight { get; set; }

        public int? DbInputCount { get; set; }

        public int? DbInstType { get; set; }

        public double? DbMaxThreadDiameter { get; set; }

        public int? DbSafeDegree { get; set; }

        public double? DbWidth { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }
    }
}
