namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElResistReactor")]
    public partial class ElResistReactor
    {
        public string Code { get; set; }

        public int? DbClimate { get; set; }

        public double? DbDepth { get; set; }

        public double? DbDinamicCurrentStability { get; set; }

        public string DbDocument { get; set; }

        public double? DbHeight { get; set; }

        public double? DbInom { get; set; }

        public double? DbMaxSectionConductor { get; set; }

        public int? DbSafeDegree { get; set; }

        public double? DbThermalCurrentStability { get; set; }

        public int? DbTimeThermalStability { get; set; }

        public double? DbVoltage { get; set; }

        public double? DbWidth { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public string NameConsistingOfNcu { get; set; }

        public int? OrderNumber { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public double? PowerLoss { get; set; }

        public double? Ractive { get; set; }

        public double? Xreactive { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

        public int? DbGraphicRef { get; set; }

        public string DbClassificatorCode { get; set; }

        public string SafeDegreeIk { get; set; }
    }
}
