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
    /// <summary>
    /// Представляет элемент крепления структуры DbCcMountSystem
    /// </summary>
    [XmlRoot(nameof(DbCcMsMount))]
    public partial class DbCcMsMount : KitElement, IXmlSerializable {
        public bool IsUse { get; set; } = true;
        public double PostDistance { get; set; } = 1.0;
        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
            writer.WriteElementString(nameof(PostDistance), PostDistance.ToString(CultureInfo.GetCultureInfo("Ru-ru")));
        }
    }
}
