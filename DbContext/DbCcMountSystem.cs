namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbCcMountSystem")]
    public partial class DbCcMountSystem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] 
        public int Id { get; set; }
        /// <summary>
        /// Количество перегородок.
        /// </summary>
        public double? PartitionCount { get; set; }

        public string DbName { get; set; }

        public string DbNaming { get; set; }

        public string DbDescription { get; set; }

        public string DbCatalog { get; set; }

        public string LayerName { get; set; }

        public string LayerColor { get; set; }

        public bool? LayerIsPrintable { get; set; }

        public int? LayerLineWeigh { get; set; }
        [MaxLength(-1)]
        public string KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? CableCanal { get; set; }

        public bool? InImport { get; set; }
    }
}
