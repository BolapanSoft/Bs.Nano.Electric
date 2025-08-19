namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElCompensatorReactivePower")]
    public partial class ElCompensatorReactivePower
    {
        public int? Climate { get; set; }

        public string? Code { get; set; }

        public int? DbCountStageOfRegulation { get; set; }

        public string? DbDocument { get; set; }

        public string? DbName { get; set; }

        public double? DbPower { get; set; }

        public double? DbStageOfRegulationPower { get; set; }

        public double? DbVoltage { get; set; }

        public double? Depth { get; set; }

        public string? Description { get; set; }

        public double? Height { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? Manufacturer { get; set; }

        public string? Mass { get; set; }

        public int? OrderNumber { get; set; }

        public int? SafeDegree { get; set; }

        public string? Series { get; set; }

        public string? SpecDescription { get; set; }

        public double? Width { get; set; }

        public int? IsExplSafe { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? ExplodeLevel { get; set; }

        public string? EtmCode { get; set; }

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
