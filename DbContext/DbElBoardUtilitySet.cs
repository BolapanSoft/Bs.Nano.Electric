using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Nano.Electric {

    [Table("DbElBoardUtilitySet")]
    public partial class DbElBoardUtilitySet {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string DbName { get; set; }

        public string DbNaming { get; set; }

        public string DbDescription { get; set; }

        public string UserCategory { get; set; }
        [MaxLength(-1)]
        public string KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public bool? InImport { get; set; }
    }
}
