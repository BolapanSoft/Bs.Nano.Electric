namespace Nano.Electric {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("ElAutomat")]
    public partial class ElAutomat {
#if !InitDbContextEnums
        public int? DbPoleCountEnum { get; set; }
        public int? CurrentChoice { get; set; }
        public int? IsExplSafe { get; set; }
        public int? ContactType { get; set; }
        public int? MountType { get; set; }
        public int? KzInstantCurrentChoice { get; set; }
        public int? VoltageType { get; set; }
       public int? RailMountTypeFlagged { get; set; }

#endif  
        public bool? IsHeatR { get; set; }

        public bool? IsElMagR { get; set; }

        public bool? IsElectronicR { get; set; }

        public bool? HasUzo { get; set; }
        public double? ActiveResistance { get; set; }
        public int? Climate { get; set; }

        public string Code { get; set; }


        public string DbDocument { get; set; }

        public double? Depth { get; set; }

        public string Description { get; set; }

        public double? Height { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public double? InductiveResistance { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public double? MaxCommutation { get; set; }

        public double? MaxCordS { get; set; }

        public double? MaxSensivity { get; set; }

        public double? MinSensivity { get; set; }

        public string Name { get; set; }

        public string NameNcu { get; set; }

        public double? NominalCurrent { get; set; }

        public int? OrderNumber { get; set; }
        public int? SafeDegree { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }

        public double? Width { get; set; }


        public bool? DbIsModule { get; set; }

        public double? DbModuleCount { get; set; }

        public double? DynResistance { get; set; }



        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public int? ExplodeLevel { get; set; }

        public string UserCategory { get; set; }



        public string CurrentScale { get; set; }

        public string TimeReleaseIr { get; set; }

        public double? TimeOeReleaseIr { get; set; }

        public string MultiplScale { get; set; }

        public string CurrReleaseScale { get; set; }

        public string UnlinkTimeElectronicScale { get; set; }

        public bool? IsMultiplicityOfCurrentForTmTime { get; set; }

        public double? MultiplicityOfCurrentForTmTime { get; set; }


        public string KzKiScale { get; set; }

        public string KzIiScale { get; set; }

        public string CurrentScaleUzo { get; set; }


        public string EtmCode { get; set; }


        public bool? InImport { get; set; }

        public string Purpose { get; set; }


        public double? MaxCommutation660 { get; set; }

        public double? DynResistance660 { get; set; }

        public string AutomatCharReleaseType { get; set; }

        public double? AutomatReleaseMinCoef { get; set; }

        public double? AutomatReleaseMaxCoef { get; set; }

        public string UnlinkTimeScale { get; set; }

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
