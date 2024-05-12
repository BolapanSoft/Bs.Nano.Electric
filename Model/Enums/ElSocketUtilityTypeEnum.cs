using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElSocketUtilityTypeEnum {
		[Description("Установочные коробки")]
        INSTALL_CASE = 0,
        [Description("Суппорты")]
        SUPPORT = 1,
        [Description("Рамки")]
        FRAME = 2,
        [Description("Другое")]
        OTHER = 100
    }
}
