using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum EnvExplosionTypeEnum {
        [Description("d (взрывонепроницаемая оболочка)")]
        EXPL_D = 0,
        [Description("e (защита вида «е»)")]
        EXPL_E = 1,
        [Description("ia (искробезопасная электрическая цепь)")]
        EXPL_IA = 2,
        [Description("ib (искробезопасная электрическая цепь)")]
        EXPL_IB = 3,
        [Description("ic (искробезопасная электрическая цепь)")]
        EXPL_IC = 4,
        [Description("h (герметическая изоляция)")]
        EXPL_H = 5,
        [Description("m (герметизация компаундом)")]
        EXPL_M = 6,
        [Description("o (масляное заполнение оболочки)")]
        EXPL_O = 7,
        [Description("p (метод повышенного давления)")]
        EXPL_P = 8,
        [Description("q (кварцевое заполнение оболочки)")]
        EXPL_Q = 9,
        [Description("s (спецзащита)")]
        EXPL_S = 10
    }
}
