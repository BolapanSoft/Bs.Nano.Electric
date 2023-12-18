namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScsWorkPlaceDbKit")]
    public partial class ScsWorkPlaceDbKit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string DbName { get; set; }

        public string Mark { get; set; }

        public bool? DirPortMark { get; set; }

        public double? MountHeight { get; set; }

        public string LayerName { get; set; }

        public string LayerColor { get; set; }

        public bool? LayerIsPrintable { get; set; }

        public int? LayerLineWeigh { get; set; }

        public double? DbHeight { get; set; }

        public double? DbWidth { get; set; }

        public double? DbDepth { get; set; }

        public string KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }

        public int? DbGraphicRef { get; set; }
    }
}
