namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElControlWireMark")]
    public partial class ElControlWireMark
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string MarkNameMain { get; set; }

        public string markName { get; set; }

        public string сomment { get; set; }

        public string SpecСomment { get; set; }

        public string DbDocument { get; set; }

        public double? voltage { get; set; }

        public double? higherPermTemp { get; set; }

        public double? lowerPermTemp { get; set; }

        public int? WireShellFire { get; set; }

        public int? WireArmor { get; set; }

        public string Code { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? isolationMaterialId { get; set; }

        public int? materialId { get; set; }

        public bool? InImport { get; set; }
    }
}
