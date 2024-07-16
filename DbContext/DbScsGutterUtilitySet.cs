namespace Nano.Electric {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DbScsGutterUtilitySet")]
    public partial class DbScsGutterUtilitySet {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")] public int Id { get; set; }

        public string DbName { get; set; }

        public string DbDescription { get; set; }

        public string Description { get; set; }

        public string DbCatalog { get; set; }

#if !InitDbContextEnums
        public int? StandType { get; set; }

        public int? StructureType { get; set; }

        public int? LevelType { get; set; }
  public int? KnotType { get; set; }

        public int? InstallType { get; set; }

#endif
        public int? LevelCount { get; set; }



        public string Seria { get; set; }
        [MaxLength(-1)]
        public string KitStructure { get; set; }

        public int? OrderNumber { get; set; }

        public int? CwPCode { get; set; }

        public int? DbImageRef { get; set; }

        public bool? InImport { get; set; }
    }
}
