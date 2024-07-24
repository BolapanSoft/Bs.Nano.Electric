// Ignore Spelling: Gc

using Nano.Electric.Enums;

/* Unmerged change from project 'Iek.MakeModelStudioCS'
Before:
using System.Collections.Generic;
After:
using System;
using System.Collections.Generic;
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Nano.Electric {
    /// <summary>
    /// Элемент яруса
    /// </summary>
    /// <remarks>Представляет элемент яруса с несущим элементом - консолью, комплектующими для крепления
    /// консоли и лотка. Содержит описание трассы лотка в свойстве Gutter.</remarks>
    [XmlRoot(nameof(DbGcSystemPlain))]
    public partial class DbGcSystemPlain : KitElement, IXmlSerializable {
        ScsGutterBolting? dbShelf;

        public DbGcSystemPlain() {
            Children.Clear();
            Children.Add(new DbGcSystemGutter());
        }
        public int DbProfileLength { get; set; }
        public int LayerNumber { get; set; } = 1;
        public int Number { get; set; } = 1;
        public bool IsEnabled { get; set; } = true;
        public bool IsUse { get; set; } = true;
        public bool IsAutoSelection { get; set; } = true;
        /// <summary>
        /// Описание трассы лотка
        /// </summary>
        public DbGcSystemGutter Gutter {
            get { return (DbGcSystemGutter)Children[0]; }
            set {
                if (value is null)
                    throw new ArgumentNullException(nameof(Gutter));
                Children[0] = value;
            }
        }
        /// <summary>
        /// Консоль
        /// </summary>
        /// <remarks>Представляет элемент "Несущий элемент" в дереве DbGcMountSystem-DbGcSystemPlain.</remarks>
        public ScsGutterBolting? DbShelf {
            get => dbShelf;
            set {
                if (value is null) {
                    dbShelf = value;
                    IsAutoSelection = true;
                }
                else if (value.IsDbShelf()) {
                    dbShelf = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(DbShelf), $"DbShelf must be instance of  ScsGutterBolting width CanalBoltingType = {ScsGutterBoltingTypeEnum.CONSOLE}|{ScsGutterBoltingTypeEnum.CROSSBAR}|{ScsGutterBoltingTypeEnum.CRAMP}|{ScsGutterBoltingTypeEnum.PROFILE}.");
                }
            }
        }
        public IEnumerable<DbUtilityUnit> GetChildren() {
            return base.GetChildren<DbUtilityUnit>().ToArray();
        }
        public void AddChild(DbUtilityUnit child) {
            Throw.IfNull(child);
            Children.Add(child);
        }
        public bool Remove(DbUtilityUnit child) {
            return Children.Remove(child);
        }

        protected override void WriteProperties(XmlWriter writer) {
            writer.WriteElementString(nameof(DbProfileLength), DbProfileLength.ToString());
            if (!(DbShelf is null)) {
                XElement el = new XElement(nameof(DbShelf));
                el.Add(new XAttribute("TableName", nameof(ScsGutterBolting)));
                el.Add(new XAttribute("Id", DbShelf.Id));
                el.WriteTo(writer);
            }
            writer.WriteElementString(nameof(IsAutoSelection), IsAutoSelection.ToString());
            writer.WriteElementString(nameof(IsEnabled), IsEnabled.ToString());
            writer.WriteElementString(nameof(IsUse), IsUse.ToString());
            writer.WriteElementString(nameof(LayerNumber), LayerNumber.ToString());
            writer.WriteElementString(nameof(Number), Number.ToString());

        }
        public override string ToString() {
            return $"{base.ToString()}: Ярус {Number}";
        }
    }
}
