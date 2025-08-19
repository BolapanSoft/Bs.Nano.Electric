using Nano.Electric;

namespace Bs.Nano.Electric.Builder.Internals {
    internal static class ReflectionHelper {
        public static string GetDescription<MyEnum>(this MyEnum value) where MyEnum : struct, Enum {
            return EnumConverter<MyEnum>.GetDescription(value);
        }
    }
}
