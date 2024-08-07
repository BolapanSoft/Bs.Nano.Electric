using Nano.Electric.Enums;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    /// <summary>
    /// Аксессуар
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot("DbGcMsAcceesory")]
    public class DbGcMsAccessory : KitElement, IXmlSerializable {
        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Использовать типовую секцию/элемент
        /// </summary>
        //public bool IsUse { get; set; }
        public string? DbOtherName { get; set; }
        public string? DbSeria { get; set; }
        /// <summary>
        /// Тип аксессуара
        /// </summary>
        public DbGcMsAccessoryType AccessoryType { get; set; }
        public DbGcMsAccessory() {
            AccessoryType = DbGcMsAccessoryType.OTHER;
            Count = 1;
            //IsUse = true;
        }
        public static DbGcMsAccessory From(DbScsGcAccessoryUnit accessoryUnit) {
            Throw.IfNull(accessoryUnit, nameof(accessoryUnit));
            //Throw.IfNull(accessoryUnit.AccessoryType, nameof(accessoryUnit.AccessoryType));
            return new DbGcMsAccessory {
                AccessoryType = accessoryUnit.AccessoryType,
                DbSeria = accessoryUnit.Series,
                DbOtherName = accessoryUnit.DbOtherName
            };
        }
        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString(nameof(AccessoryType), AccessoryType.ToString());
            writer.WriteElementString(nameof(Count), Count.ToString());
            writer.WriteElementString(nameof(DbOtherName), DbOtherName);
            writer.WriteElementString(nameof(DbSeria), DbSeria);
        }
    }
}
