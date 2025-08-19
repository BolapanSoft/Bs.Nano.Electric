namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ImportConflictField")]
    public partial class ImportConflictField
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? TableName { get; set; }

        public int? ConflictObjectId { get; set; }

        public string? DbFieldName { get; set; }

        public string? ExcelValue { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }
    }
}
