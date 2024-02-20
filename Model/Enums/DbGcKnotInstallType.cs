
/* Unmerged change from project 'Iek.MakeModelStudioCS'
Before:
//using Cadwise.ObjectLib.WireCommon.Db.Localization;
After:
using System.ComponentModel;
//using Cadwise.ObjectLib.WireCommon.Db.Localization;
*/
//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using
/* Unmerged change from project 'Iek.MakeModelStudioCS'
Before:
using System.ComponentModel;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
After:
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
*/
DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    DbGcKnotInstallType.CEIL,
    //    WireCommonLocalizationDictionary.Cell,
    //    DbGcKnotInstallType.WALL,
    //    WireCommonLocalizationDictionary.Wall,
    //    DbGcKnotInstallType.GROUND,
    //    WireCommonLocalizationDictionary.Floor
    //})]
    public enum DbGcKnotInstallType {
        [Description("Потолок")]
        CEIL = 0,
        [Description("Стена")]
        WALL = 1,
        [Description("Пол")]
        GROUND = 2
    }
}
