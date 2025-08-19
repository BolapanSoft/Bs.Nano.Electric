namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElControlCabel")]
    public partial class ElControlCabel
    {
        public double? CableDiameter { get; set; }

        public string? Code { get; set; }

        public int? CordCount { get; set; }

        public double? CordSection { get; set; }

        public string? Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? Manufacturer { get; set; }

        public string? Mass { get; set; }

        public string? Name { get; set; }

        public int? OrderNumber { get; set; }

        public string? AutoDbSpecDescription { get; set; }

        public string? ThreadLabel { get; set; }

        public int? ThreadType { get; set; }

        public double? minBendRadius { get; set; }

        public double? CountPerBox { get; set; }

        public int? CwPCode { get; set; }

        public int? wireMark { get; set; }

        public bool? InImport { get; set; }

        public string? Url { get; set; }
    }
}
