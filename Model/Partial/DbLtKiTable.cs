// Ignore Spelling: Expl

using Nano.Electric.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Linq;

namespace Nano.Electric {
    public partial class DbLtKiTable {
#if InitDbContextEnums
        public KiCurveType CurveType { get; set; }
        private XDocument _curveDb;

        [Column(nameof(CurveDb), TypeName = "ntext")]
        [MaxLength(-1)]
        public string CurveDbString {
            get { return _curveDb?.ToString()??string.Empty; }
            set {
                if (value == null) {
                    _curveDb = null;
                }
                else {
                    var doc = XDocument.Parse(value);
                    _curveDb = doc;
                }
            }
        }

        [NotMapped]
        public XDocument CurveDb {
            get { return _curveDb; }
            set { _curveDb = value; }
        }

#endif
    }
}
