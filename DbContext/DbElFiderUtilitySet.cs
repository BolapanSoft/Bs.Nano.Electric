namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbElFiderUtilitySet")]
    public partial class DbElFiderUtilitySet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string DbName { get; set; }

        public string DbNaming { get; set; }

        public string DbDescription { get; set; }

        public string UserCategory { get; set; }

        public string KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public bool? InImport { get; set; }
    }
}
