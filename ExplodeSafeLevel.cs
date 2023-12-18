namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ExplodeSafeLevel")]
    public partial class ExplodeSafeLevel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string SafeLevelName { get; set; }

        public int? ExplLevel { get; set; }

        public int? ExplMixCategory { get; set; }

        public int? ExplGroup { get; set; }

        public int? ExplType { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public bool? InImport { get; set; }
    }
}
