// Ignore Spelling: Scs

namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ScsGutterCanal")]
    public partial class ScsGutterCanal
    {
        public string Code { get; set; }

        public string DbDocument { get; set; }

        public string Description { get; set; }

        public double? GutterDepth { get; set; }

        public double? GutterHeight { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public double? SegLength { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public double? CountPerBox { get; set; }

#if !InitDbContextEnums
        public int? BoxMeasureUnit { get; set; }
        public int? GutterType { get; set; }

#endif
        public bool? IsCovered { get; set; }

        public string GraphLoadingPrp { get; set; }

        public double? GutterUsefullHeight { get; set; }

        public double? GutterUsefullDepth { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

#if DBNE23
        public string DbClassificatorCode { get; set; }
#endif
    }
}
