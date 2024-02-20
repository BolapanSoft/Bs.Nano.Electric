//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using
/* Unmerged change from project 'Iek.MakeModelStudioCS'
Before:
using System.Linq;
After:
using System.ComponentModel;
using System.Linq;
*/
DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    // [Enum(new object[]
    //{
    //     ScsGcStandMountType.CELL,
    //     WireCommonLocalizationDictionary.CellMountType,
    //     ScsGcStandMountType.WALL,
    //     WireCommonLocalizationDictionary.WallMountType,
    //     ScsGcStandMountType.FLOOR,
    //     WireCommonLocalizationDictionary.FloorMountType,
    //     ScsGcStandMountType.UNIVERSAL,
    //     WireCommonLocalizationDictionary.UniversalMountType
    //})]
    public enum ScsGcStandMountType {
        /// <summary>
        ///  Потолочная
        /// </summary>
        [Description("Потолочная")]
        CELL = 0,
        /// <summary>
        /// Настенная
        /// </summary>
        [Description("Настенная")]
        WALL = 1,
        /// <summary>
        ///  Универсальная
        /// </summary>
        [Description("Универсальная")]
        UNIVERSAL = 2,
        /// <summary>
        /// Напольная
        /// </summary>
        [Description("Напольная")]
        FLOOR = 3
    }
}
