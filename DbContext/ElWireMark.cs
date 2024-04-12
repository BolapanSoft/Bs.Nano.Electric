namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElWireMark")]
    public partial class ElWireMark
    {
        public string DbDocument { get; set; }

        public double? higherPermTemp { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public int? isolationMaterialId { get; set; }

        public double? lowerPermTemp { get; set; }

        public string markName { get; set; }

        public string MarkNameMain { get; set; }

        public int? materialId { get; set; }

        public double? minBendRadius { get; set; }

        public int? OrderNumber { get; set; }

        public string SpecСomment { get; set; }

        public double? voltage { get; set; }

        public int? WireArmor { get; set; }

        public int? WireShellFire { get; set; }

        public string сomment { get; set; }

        public int? CwPCode { get; set; }

        public int? WireCategory { get; set; }

        public bool? InImport { get; set; }
    }
}
