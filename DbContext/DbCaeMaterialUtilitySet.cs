namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbCaeMaterialUtilitySet")]
    public partial class DbCaeMaterialUtilitySet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public override int Id { get; set; }

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
