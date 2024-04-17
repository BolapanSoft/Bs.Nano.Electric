namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ElLamp")]
    public partial class ElLamp
    {
        public string Code { get; set; }

        public string DbDocument { get; set; }

        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public double? LightFlux { get; set; }

        public string Manufacturer { get; set; }

        public string Mass { get; set; }

        public string Name { get; set; }

        public int? OrderNumber { get; set; }

        public double? Power { get; set; }

        public string Series { get; set; }

        public int? Socle { get; set; }

        public string SpecDescription { get; set; }

        public double? Voltage { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public string EtmCode { get; set; }

        public bool? InImport { get; set; }

        public string Url { get; set; }

        public string LampsType { get; set; }

#if DBNE23
        public string DbClassificatorCode { get; set; }
#endif
    }
}
