namespace Nano.Electric
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DSInformation")]
    public partial class DSInformation
    {
        public string? Description { get; set; }

        public bool? HideEmptyTables { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
            public int Id { get; set; }

        public string? Name { get; set; }

        public string? MLDescription { get; set; }

        public bool? IsEnableEditMultypleObjects { get; set; }

        public string? Category { get; set; }

        public string? ModelHash { get; set; }

        public int? BranchVersion { get; set; }
#if DBNE201

        public string? AppliedConverters { get; set; }
#endif
    }
}
