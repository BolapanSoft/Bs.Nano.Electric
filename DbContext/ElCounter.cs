namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElCounter")]
    public partial class ElCounter
    {
#if !InitDbContextEnums
        public int? IsExplSafe { get; set; }
        public int? MountType { get; set; }
        public int? InstallationType { get; set; }

        public int? RailMountTypeFlagged { get; set; }

        public int? DimensionType { get; set; }

        public int? FacadeDimensionType { get; set; }
        public int? InnerDimensionType { get; set; }
        public int? CounterType { get; set; }
        public int? InstallType { get; set; }
        public int? PoleCount { get; set; }

#endif
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }



        public double? DbVoltage { get; set; }

        public double? DbInom { get; set; }

        public double? DbSecondaryInom { get; set; }

        public string? Name { get; set; }

        public string? Series { get; set; }

        public string? Description { get; set; }

        public string? SpecDescription { get; set; }

        public string? DbDocument { get; set; }

        public string? Manufacturer { get; set; }

        public string? Code { get; set; }


        public string? accuracyClass { get; set; }

        public string? Mass { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public double? Depth { get; set; }

        public bool? DbIsModule { get; set; }

        public double? DbModuleCount { get; set; }


        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? Climate { get; set; }

        public int? ExplodeLevel { get; set; }

        public int? SafeDegree { get; set; }

        public string? EtmCode { get; set; }

        public double? Diameter { get; set; }


        public double? FacadeHeight { get; set; }

        public double? FacadeWidth { get; set; }

        public double? FacadeDiameter { get; set; }

        public double? FacadeDepth { get; set; }


        public double? InnerHeight { get; set; }

        public double? InnerWidth { get; set; }

        public double? InnerDiameter { get; set; }

        public double? InnerDepth { get; set; }

        public bool? InImport { get; set; }

        public string? Url { get; set; }

        public int? DbGraphicRef { get; set; }

#if DBNE23
        public string? DbClassificatorCode { get; set; }
#endif

#if DBNE23
        public string? SafeDegreeIk { get; set; }
#endif
    }
}
