// Ignore Spelling: Scs

namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ScsGutterBolting")]
    public partial class ScsGutterBolting
    {
        public string Code { get; set; }

        public string DbDocument { get; set; }

        public string DbOtherName { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public override int Id { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public string Series { get; set; }

        public string SpecDescription { get; set; }


        public double? CountPerBox { get; set; }

#if !InitDbContextEnums
        public int? СanalBoltingType { get; set; }

        public int? MountType { get; set; }

        public int? StandType { get; set; }

        public int? ConsoleMountType { get; set; } 
#endif

        public double? MaxLoadingDouble { get; set; }

        public double? MaxMomentumDouble { get; set; }

        public double? Heigth { get; set; }

        public bool? IsHeight { get; set; }

        public double? Length { get; set; }

        public double? ProfileLength { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

       // public string DbClassificatorCode { get; set; }
    }
}
