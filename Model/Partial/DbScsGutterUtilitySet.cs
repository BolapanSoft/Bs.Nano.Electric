using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {

    [XmlRoot(nameof(DbScsGutterUtilitySet))]
    public partial class DbScsGutterUtilitySet : KitElement, IXmlSerializable, IProduct {
        /// <summary>
        /// Тип несущего элемента
        /// </summary>
        public DbGcKnotLevelType LevelType { get; set; }
        /// <summary>
        /// Тип крепления ярусов
        /// </summary>
        public DbGcKnotStandType StandType { get; set; }
        /// <summary>
        /// Количество креплений ярусов
        /// </summary>
        public DbGcKnotStandStructureType StructureType { get; set; }
        /// <summary>
        /// Односторонняя/Двухсторонняя
        /// </summary>
        public ScsGcStandType? KnotType { get; set; }
        /// <summary>
        /// Крепление
        /// </summary>
        public DbGcKnotInstallType InstallType { get; set; }
        [NotMapped]
        public DbGcKnotStand Stand {
            get {
                if (Children.Count == 0)
                    return null;
                return Children[0] as DbGcKnotStand;
            }
            set {
                if (Children.Count == 0)
                    Children.Add(value);
                else
                    Children[0] = value;
            }
        }

        string IProduct.Code => DbName;

        int IProduct.Id { get => id; set => id = value; }
        int? IProduct.DbImageRef { get => DbImageRef; set => DbImageRef = value; }
        string IProduct.Name { get => DbDescription; set => DbDescription = value; }
        string IProduct.Manufacturer { get => string.Empty; set => throw new NotImplementedException(); }

        public IEnumerable<DbUtilityUnit> GetChilds() {
            return base.GetChilds<DbUtilityUnit>().ToArray();
        }
        //public void Add(DbUtilityUnit child) {
        //    Children.Add(child);
        //}
        //public  bool Remove(DbUtilityUnit child) {
        //    return Children.Remove(child);
        //}
        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString("LevelType", LevelType.ToString());
            writer.WriteElementString("StandType", StandType.ToString());
            writer.WriteElementString("StructureType", StructureType.ToString());
        }

    }

}
