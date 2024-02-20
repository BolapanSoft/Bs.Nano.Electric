//using System;

/* Unmerged change from project 'Iek.MakeModelStudioCS'
Before:
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
After:
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/
//using Cadwise.Reflection;
//using Cadwise.ObjectLib.WireCommon.Db.Localization;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    ScsVerticalUniversalBendTypeEnum.ANGLE_90,
    //    "+/-90",
    //    ScsVerticalUniversalBendTypeEnum.OTHER,
    //    WireCommonLocalizationDictionary.___Другой
    //})]
    public enum ScsVerticalUniversalBendTypeEnum {
        //"+/-90"
        [Description("+/-90")]
        ANGLE_90 = 0,
        /// <summary>
        /// WireCommonLocalizationDictionary.___Другой
        /// </summary>
        [Description("+/-Другой")]
        OTHER = 1
    }
}
