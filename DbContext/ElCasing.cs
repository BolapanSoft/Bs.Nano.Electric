namespace Nano.Electric {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("ElCasing")]
    public partial class ElCasing {
#if !InitDbContextEnums
        public int? IsExplSafe { get; set; }
        public int? MountType { get; set; }
        public int? DbInstType { get; set; }
        public int? CableLeadIn { get; set; }
        public int? RailMountTypeFlagged { get; set; }


#endif
        public string Code { get; set; }

        public int? DbClimate { get; set; }

        public double? DbDepth { get; set; }

        public string DbDocument { get; set; }

        public double? DbHeight { get; set; }

        public int? DbSafeDegree { get; set; }

        public double? DbWidth { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public string SpecDescription { get; set; }

        public string Series { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? ExplodeLevel { get; set; }

        public string EtmCode { get; set; }


        public bool? InImport { get; set; }

        public string Url { get; set; }

        public int? DbGraphicRef { get; set; }

        public string DbClassificatorCode { get; set; }

        public string SafeDegreeIk { get; set; }
    }
}
