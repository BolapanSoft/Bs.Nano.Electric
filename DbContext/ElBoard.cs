// Ignore Spelling: Ik

namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElBoard")]
    public partial class ElBoard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
            public int Id { get; set; }

        public string? Name { get; set; }

        public string? Series { get; set; }

        public string? UserCategory { get; set; }

        public string? Description { get; set; }

        public string? SpecDescription { get; set; }

        public string? DbDocument { get; set; }

        public string? Manufacturer { get; set; }

        public string? Code { get; set; }
#if !InitDbContextEnums
        public int? StructureType { get; set; }
        public int? BoardType { get; set; }
        public int? DbInstType { get; set; }
        public int? IsExplSafe { get; set; }
        public int? CableLeadIn { get; set; }
#endif
#if VER23_1
        public string? DbClassificatorCode { get; set; }
        public string? SafeDegreeIk { get; set; }
#endif
        public double? DbVoltage { get; set; }

        public double? DbInom { get; set; }

        public double? ShockIkz { get; set; }

        public int? DbGraphicRef { get; set; }



        public string? Mass { get; set; }

        public double? DbHeight { get; set; }

        public double? DbWidth { get; set; }

        public double? DbDepth { get; set; }

        public double? DbFlushedHeight { get; set; }

        public double? DbFlushedWidth { get; set; }

        public double? DbFlushedDepth { get; set; }

        public double? DbOutHeight { get; set; }

        public double? DbOutWidth { get; set; }

        public double? DbOutDepth { get; set; }

        public double? LeftMinIndent { get; set; }

        public double? RightMinIndent { get; set; }

        public double? TopMinIndent { get; set; }

        public double? BottomMinIndent { get; set; }

        public int? RowCount { get; set; }

        public int? ModuleCountInRow { get; set; }
        

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }
        [Column("DbClimate")]
        public int? Climate { get; set; }

        public int? ExplodeLevel { get; set; }
        [Column("DbSafeDegree")]
        public int? SafeDegree { get; set; }

        public string? EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string? Url { get; set; }



    }
}
