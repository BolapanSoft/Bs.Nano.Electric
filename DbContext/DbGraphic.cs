namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("DbGraphic")]
    public partial class DbGraphic : IDbGraphic {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string? Name { get; set; }

        public string? Category { get; set; }

        public bool? AutoSelectSize { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }
        [MaxLength(-1)]
        public byte[] GraphicInBytes { get; set; }
    }
}
