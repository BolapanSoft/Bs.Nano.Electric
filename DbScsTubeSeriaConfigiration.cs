namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DbScsTubeSeriaConfigiration")]
    public partial class DbScsTubeSeriaConfigiration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string DbName { get; set; }

        public string DbDescription { get; set; }

        public string TubeSeria { get; set; }

        public string AngleSeria { get; set; }

        public string TripleSeria { get; set; }

        public string CrossSeria { get; set; }

        public string KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }
    }
}
