namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElWireConductMaterial")]
    public partial class ElWireConductMaterial
    {
        public double? Bconst { get; set; }

        public string condMatName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }
    }
}
