namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbLtKiTable")]
    public partial class DbLtKiTable
    {
#if !InitDbContextEnums
        public int? CurveType { get; set; }
        [MaxLength(-1)]
        public string? CurveDb { get; set; }
#endif
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? DbName { get; set; }

        public string? DbDescription { get; set; }

        public string? Category { get; set; }
     
        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }

    }
}
