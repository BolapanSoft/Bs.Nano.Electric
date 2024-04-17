namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElTransformer")]
    public partial class ElTransformer
    {
        public double? ActiveResistanceDirectSequence { get; set; }

        public double? ActiveResistanceNullSequence { get; set; }

        public string Code { get; set; }

        public int? ConnectingScheme { get; set; }

        public int? DbClimate { get; set; }

        public double? DbDepth { get; set; }

        public string DbDocument { get; set; }

        public double? DbHeight { get; set; }

        public double? DbHighVoltage { get; set; }

        public double? DbLowVoltage { get; set; }

        public double? DbPower { get; set; }

        public int? DbSafeDegree { get; set; }

        public double? DbWidth { get; set; }

        public string Description { get; set; }

        public double? FullResistance { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public double? KzLoss { get; set; }

        public double? KzPower { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public string NameFull { get; set; }

        public double? NoLoadCurrent { get; set; }

        public double? NoLoadLoss { get; set; }

        public int? OrderNumber { get; set; }

        public double? ReactiveResistanceDirectSequence { get; set; }

        public double? ReactiveResistanceNullSequence { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

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
