//using Cadwise.Reflection;
//using Cadwise.ObjectLib.WireCommon.Db.Localization;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric.Enums {
    //[Enum(new object[]
    //{
    //    DbGcMsAccessoryType.SWIVEL_V,
    //    WireCommonLocalizationDictionary.ШарнирВертикальный,
    //    DbGcMsAccessoryType.SWIVEL_H,
    //    WireCommonLocalizationDictionary.ШарнирГоризонтальный,
    //    DbGcMsAccessoryType.BRANCH_U,
    //    WireCommonLocalizationDictionary.ОтветвительУниверсальный,
    //    DbGcMsAccessoryType.PASS_V,
    //    WireCommonLocalizationDictionary.ПереходникУниверсальныйВертикальный,
    //    DbGcMsAccessoryType.PASS_H,
    //    WireCommonLocalizationDictionary.ПереходникУниверсальныйГоризонтальный,
    //    DbGcMsAccessoryType.PLAIN_JOINT,
    //    WireCommonLocalizationDictionary.ПластинаСоединительнаяНаБорт,
    //    DbGcMsAccessoryType.PLAIN_RIGED,
    //    WireCommonLocalizationDictionary.ПластинаСоединительнаяНаОснование,
    //    DbGcMsAccessoryType.FOOT_ANGLE,
    //    WireCommonLocalizationDictionary.УголокОпорный,
    //    //DbGcMsAccessoryType.OTHER,
    //    // Cadwise.CAE.Model.Localization.CaeModelLocalizationDictionary.Другой
    //})]
    public enum DbGcMsAccessoryType {
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
