namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class DbImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
            public int Id { get; set; }
        /// <summary>
        /// Образ изображения в памяти.
        /// </summary>
        /// <remarks>Внутренний формат хранения изображений в nanoCad Elecnro - .png</remarks>
        [MaxLength(-1)]
        public byte[]? Image { get; set; }

        public string? Text { get; set; }

        public string? Description { get; set; }

        public string? Category { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }
    }
}
