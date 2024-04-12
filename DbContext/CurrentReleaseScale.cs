namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("CurrentReleaseScale")]
    public partial class CurrentReleaseScale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] 
        public int Id { get; set; }

        public string LongDescription { get; set; }

        public int? OrderNumber { get; set; }

        public string Scale { get; set; }

        public int? CwPCode { get; set; }
    }
}
