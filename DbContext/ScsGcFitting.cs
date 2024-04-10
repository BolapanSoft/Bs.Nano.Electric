// Ignore Spelling: Gc Scs

namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ScsGcFitting")]
    public partial class ScsGcFitting
    {
        public double? BendAngle { get; set; }

#if !InitDbContextEnums
        public int? BendType { get; set; } 
#endif

        public string Code { get; set; }

        public string DbDocument { get; set; }

        public string DbOtherName { get; set; }

        public double? Depth { get; set; }

        public string Description { get; set; }


        public double? Height { get; set; }

        public double? Height1Branch { get; set; }

        public double? Height2Branch { get; set; }

        public double? HeightMainBranch { get; set; }

        public double? HeightOutBranch { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public override int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public double? Width1Branch { get; set; }

        public double? Width2Branch { get; set; }

        public double? WidthMainBranch { get; set; }

        public double? WidthOutBranch { get; set; }

        public double? CountPerBox { get; set; }
#if !InitDbContextEnums
        public int? FittingType { get; set; } 
        public int? VerticalBendType { get; set; }
        public int? VerticalUniversalBendType { get; set; } 
        public int? GutterPassageType { get; set; }
#endif


        public bool? IsCovered { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

       // public string DbClassificatorCode { get; set; }
    }
}
