// Ignore Spelling: Scs Gc

namespace Nano.Electric
{
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbScsGcAccessoryUnit")]
    public partial class DbScsGcAccessoryUnit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public override int Id { get; set; }

        public double? CountPerBox { get; set; }
#if !InitDbContextEnums

        public int? AccessorySelectType { get; set; }

        public int? AccessoryType { get; set; } 
#endif

        public string? DbOtherName { get; set; }

        public string? Mass { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public double? PassWidth { get; set; }

        public string? Name { get; set; }

        public string? Series { get; set; }

        public string? Description { get; set; }

        public string? SpecDescription { get; set; }

        public string? DbDocument { get; set; }

        public string? Code { get; set; }

        public string? Manufacturer { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string? EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string? Url { get; set; }

#if DBNE23
        public string? DbClassificatorCode { get; set; }
#endif
    }
}
