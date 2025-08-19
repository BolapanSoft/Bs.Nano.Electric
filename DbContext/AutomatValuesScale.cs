namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("AutomatValuesScale")]
    public partial class AutomatValuesScale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
            public int Id { get; set; }

        public string? Name { get; set; }

        public string? LongDescription { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }
    }
}
