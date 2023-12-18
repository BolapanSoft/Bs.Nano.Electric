namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbTbMountSystem")]
    public partial class DbTbMountSystem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string DbName { get; set; }

        public string DbNaming { get; set; }

        public string DbDescription { get; set; }

        public string DbCatalog { get; set; }

        public string LayerName { get; set; }

        public string LayerColor { get; set; }

        public bool? LayerIsPrintable { get; set; }

        public int? LayerLineWeigh { get; set; }

        public string KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? TubeCanal { get; set; }

        public bool? InImport { get; set; }
    }
}
