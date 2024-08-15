namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElWire")]
    public partial class ElWire
    {
#if !InitDbContextEnums
        public int? ThreadType { get; set; }
        public int? ThreadCondShape { get; set; }
        public int? CableSystemType { get; set; }
        public int? wireMark { get; set; }
#endif
        public string AutoDbSpecDescription { get; set; }

        public int? conductorsNumberL { get; set; }

        public int? conductorsNumberPEN { get; set; }

        public string conductsDesignation { get; set; }

        public double? ContactResistance { get; set; }

        public string Description { get; set; }

        public double? diameter { get; set; }

        public double? FullSpecificResistance { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public double? isolationThickness { get; set; }

        public int? longLoadCurrent { get; set; }

        public string mass { get; set; }

        public string mfrId { get; set; }

        public int? OrderNumber { get; set; }

        public double? SectionalAreaL { get; set; }

        public double? SectionalAreaPEN { get; set; }

        public double? SpecificActiveResistanceDirectSeq { get; set; }

        public double? SpecificActiveResistanceNullSeq { get; set; }

        public double? SpecificReactanceDirectSeq { get; set; }

        public double? SpecificReactanceNullSeq { get; set; }




        public string wireName { get; set; }
        [Column("DbCode")]
        public string Code { get; set; }

        public double? CountPerBox { get; set; }

        public int? CwPCode { get; set; }

        public string ShortName { get; set; }

        public bool? IsAutoSpecDescription { get; set; }

        public string DbSpecDescription { get; set; }

        public int? сonductorsPairNumber { get; set; }

        public int? DbImageRef { get; set; }


        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

        public double? SectionalDiameter { get; set; }

#if DBNE23
        public string DbClassificatorCode { get; set; }
#endif
    }
}
