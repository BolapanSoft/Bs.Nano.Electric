// Ignore Spelling: Gc
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Nano.Electric {
    public abstract class DbMsMount : KitElement, IXmlSerializable {
        public bool IsUse { get; set; } = true;
        public double PostDistance { get; set; } = 1.0;
        public void Add(DbUtilityUnit child) {
            Children.Add(child);
        }
        public bool Remove(DbUtilityUnit child) {
            return Children.Remove(child);
        }
        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
            writer.WriteElementString(nameof(PostDistance), PostDistance.ToString(CultureInfo.GetCultureInfo("Ru-ru")));
        }
    }
    /// <summary>
    /// Представляет элемент крепления структуры DbCcMountSystem
    /// </summary>
    [XmlRoot(nameof(DbCcMsMount))]
    public class DbCcMsMount : DbMsMount {
    }
    /// <summary>
    /// Представляет элемент крепления структуры DbTbMountSystem
    /// </summary>
    [XmlRoot(nameof(DbTbMsMount))]
    public class DbTbMsMount : DbMsMount {
    }
}
