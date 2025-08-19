namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElWireIsolationMaterial")]
    public partial class ElWireIsolationMaterial
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? mName { get; set; }

        public int? OrderNumber { get; set; }

        public double? Tfire { get; set; }

        public double? TkzUseabilityMax { get; set; }

        public double? TkzUseabilityMin { get; set; }

        public double? Tlong { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }
    }
}
