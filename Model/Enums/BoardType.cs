using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum BoardType {
        [Description("Универсальный")]
        UNIVERSAL = 0,
        [Description("Модульный")]
        MODULAR = 1,
        [Description("Кабельный отсек")]
        CABLE_BAY = 2
    }
    public enum ElLightingPurpouse {
        [Description("Светильник рабочего освещения")]
        LT_WORK_LIGHT = 0,
        [Description("Светильник аварийного освещения")]
        LT_EMERGENCY_LIGHT = 1,
        [Description("Световой указатель")]
        LT_LIGHT_SIGN = 2
    }
    public enum LampExistance {
        [Description("Съемная лампа")]
        EXIST = 0,
        [Description("Часть конструкции светильника")]
        UNEXIST = 1
    }
    public enum KccTypeEnum {
        [Description("Ies")]
        Ies = 0,
        [Description("Ldt")]
        Ldt = 1
    }
    public enum InstLightingEnum {
        [Description("Потолочный")]
        INST_CEILING = 0,
        [Description("Подвесной")]
        INST_HANGING = 1,
        [Description("Встроенный")]
        INST_BUILD_IN = 2,
        [Description("Настенный")]
        INST_WALL = 3,
        [Description("Консольный")]
        INST_CONSOLE = 4,
        [Description("Переносной")]
        INST_PORTABLE = 5
    }
    public enum ElLightingDimensionType {
        [Description("Параллелепипед")]
        Parallelepiped = 0,
        [Description("Цилиндр")]
        Cylinder = 1,
        [Description("Конус (вершина сверху)")]
        ConeVertexUp = 2,
        [Description("Конус (вершина снизу)")]
        ConeVertexDown = 3,
        [Description("Сфера")]
        Sphere = 4
    }
}
