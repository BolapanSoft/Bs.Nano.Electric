namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElPushButtonStation")]
    public partial class ElPushButtonStation
    {
        public int? ButtonCount { get; set; }

        public string Code { get; set; }

        public string DbDocument { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public int? IsExplSafe { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? Climate { get; set; }

        public int? SafeDegree { get; set; }

        public int? ExplodeLevel { get; set; }

        public string EtmCode { get; set; }

        public double? DbHeight { get; set; }

        public double? DbWidth { get; set; }

        public double? DbDepth { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

        public int? DbGraphicRef { get; set; }

#if DBNE23
        public string DbClassificatorCode { get; set; }
#endif

#if DBNE23
        public string SafeDegreeIk { get; set; }
#endif
    }
}
