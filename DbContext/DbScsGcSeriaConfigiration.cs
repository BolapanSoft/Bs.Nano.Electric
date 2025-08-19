namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbScsGcSeriaConfigiration")]
    public partial class DbScsGcSeriaConfigiration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? DbName { get; set; }

        public string? DbDescription { get; set; }

        public string? Description { get; set; }

        public string? DbCatalog { get; set; }
        [MaxLength(-1)]
        public string? KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? IsShowVisibleFittings { get; set; }

        public bool? InImport { get; set; }
    }
}
