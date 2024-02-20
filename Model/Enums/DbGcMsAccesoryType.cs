//using Cadwise.Reflection;
//using Cadwise.ObjectLib.WireCommon.Db.Localization;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    DbGcMsAccesoryType.SWIVEL_V,
    //    WireCommonLocalizationDictionary.ШарнирВертикальный,
    //    DbGcMsAccesoryType.SWIVEL_H,
    //    WireCommonLocalizationDictionary.ШарнирГоризонтальный,
    //    DbGcMsAccesoryType.BRANCH_U,
    //    WireCommonLocalizationDictionary.ОтветвительУниверсальный,
    //    DbGcMsAccesoryType.PASS_V,
    //    WireCommonLocalizationDictionary.ПереходникУниверсальныйВертикальный,
    //    DbGcMsAccesoryType.PASS_H,
    //    WireCommonLocalizationDictionary.ПереходникУниверсальныйГоризонтальный,
    //    DbGcMsAccesoryType.PLAIN_JOINT,
    //    WireCommonLocalizationDictionary.ПластинаСоединительнаяНаБорт,
    //    DbGcMsAccesoryType.PLAIN_RIGED,
    //    WireCommonLocalizationDictionary.ПластинаСоединительнаяНаОснование,
    //    DbGcMsAccesoryType.FOOT_ANGLE,
    //    WireCommonLocalizationDictionary.УголокОпорный,
    //    //DbGcMsAccesoryType.OTHER,
    //    // Cadwise.CAE.Model.Localization.CaeModelLocalizationDictionary.Другой
    //})]
    public enum DbGcMsAccesoryType {
        [Description("Другой")]
        OTHER = 0,
        [Description("Шарнир вертикальный")]
        SWIVEL_V = 1,
        [Description("Шарнир горизонтальный")]
        SWIVEL_H = 2,
        [Description("Ответвитель универсальный")]
        BRANCH_U = 3,
        [Description("Переходник универсальный вертикальный")]
        PASS_V = 4,
        [Description("Переходник универсальный горизонтальный")]
        PASS_H = 5,
        [Description("Пластина соединительная на борт")]
        PLAIN_JOINT = 6,
        [Description("Пластина соединительная на основание")]
        PLAIN_RIGED = 7,
        [Description("Уголок опорный")]
        FOOT_ANGLE = 8
    }
}
