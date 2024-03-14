namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElInstallBox")]
    public partial class ElInstallBox
    {
        public string Code { get; set; }

        public int? DbClimate { get; set; }

        public double? DbDepth { get; set; }

        public string DbDim { get; set; }

        public string DbDocument { get; set; }

        public double? DbHeight { get; set; }

        public int? DbInstType { get; set; }

        public double? DbWidth { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }
    }
}