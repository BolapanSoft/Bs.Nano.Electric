namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScsOrganaizerPanel")]
    public partial class ScsOrganaizerPanel
    {
#if !InitDbContextEnums
        public string BlockName { get; set; }
   public string BlockFileName { get; set; }

        public string BackBlockFileName { get; set; }

        public string BackBlockName { get; set; }
#endif
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string Name { get; set; }

        public string Series { get; set; }

        public string Description { get; set; }

        public string SpecDescription { get; set; }

        public string DbDocument { get; set; }

        public string Code { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public int? SlotSize { get; set; }

        //[MaxLength(-1)]
        //public string KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

        public string EtmCode { get; set; }

     
        public double? Depth { get; set; }

        public double? Width { get; set; }

        public double? Height { get; set; }

        public int? DbGraphicRef { get; set; }

#if DBNE23
        public string DbClassificatorCode { get; set; }
#endif
    }
}
