namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElLighting")]
    public partial class ElLighting
    {
#if !InitDbContextEnums
        public int? IsExplSafe { get; set; }       
        public int? DbInstType { get; set; }
        public int? Lamp { get; set; }
        public int? Socle { get; set; }
        public int? LampExistance { get; set; }
        public int? KccPmType { get; set; }
        public int? DimensionType { get; set; }
        public int? LightingPurpouse { get; set; }

        public int? KiTable { get; set; }
#endif

        public int? Climate { get; set; }

        public string Code { get; set; }

        public string DbDocument { get; set; }

        public int? ExplodeLevel { get; set; }

        public string DbName { get; set; }

        public string Description { get; set; }

        public double? Efficiency { get; set; }

        public double? Height { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }


        public int? LampCount { get; set; }

        public double? LightLength { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public double? MaxSectionConductor { get; set; }

        public double? NominalPower { get; set; }

        public int? OrderNumber { get; set; }

        public double? PowerCoef { get; set; }

        public int? SafeDegree { get; set; }

        public string Series { get; set; }


        public string SpecDescription { get; set; }

        public double? Voltage { get; set; }

        public double? Width { get; set; }

        public string UserCategory { get; set; }

        public double? PraPower { get; set; }
 
     public double? DbNominalPower { get; set; }

        public double? LightFlux { get; set; }
        [MaxLength(-1)]
        public string CurvePmContent { get; set; }


        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }



        public string KccFileName { get; set; }

        public string EtmCode { get; set; }


        public double? Diameter { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }


        public double? EmergancyWorkTime { get; set; }

        public double? EmergancySeeDistance { get; set; }

        public double? EmergancyMinLux { get; set; }

        public double? EmergancyAverageLux { get; set; }

        public string DbLighting { get; set; }

        public double? LightEmergencyFlux { get; set; }

        public int? EmergancyLampCount { get; set; }

        public int? DbGraphicRef { get; set; }

#if DBNE23
        public string DbClassificatorCode { get; set; }
#endif

#if DBNE23
        public string SafeDegreeIk { get; set; }
#endif
    }
}
