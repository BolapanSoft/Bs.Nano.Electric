using Nano.Electric.Enums;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    /// <summary>
    /// Аксессуар
    /// </summary>
    /// <remarks>Представляет дочерний элемент конфигурации соединительных элементов.</remarks>
    [XmlRoot(nameof(DbGcMsAcceesory))]
    public class DbGcMsAcceesory : KitElement, IXmlSerializable {
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
        public DbGcMsAccesoryType AccessoryType { get; set; }
        public DbGcMsAcceesory() {
            AccessoryType = DbGcMsAccesoryType.OTHER;
            Count = 1;
            //IsUse = true;
        }
        public static DbGcMsAcceesory From(DbScsGcAccessoryUnit accessoryUnit) {
            Throw.IfNull(accessoryUnit);
            return new DbGcMsAcceesory {
                AccessoryType = accessoryUnit.AccessoryType!.Value,
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
