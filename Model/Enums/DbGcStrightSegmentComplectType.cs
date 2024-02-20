//using Cadwise.ObjectLib.WireCommon.Db.Localization;
//using Cadwise.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    // {
    //    DbGcStrightSegmentComplectType.METER,
    //    WireCommonLocalizationDictionary.НаМетр,
    //    DbGcStrightSegmentComplectType.SEGMENT,
    //    WireCommonLocalizationDictionary.НаСегмент
    // })]
    public enum DbGcStrightSegmentComplectType {
        [Description("На метр")]
        METER,
        [Description("На сегмент")]
        SEGMENT
    }
}
