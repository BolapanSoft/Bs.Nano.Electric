using System;
using System.Runtime.CompilerServices;

namespace Nano.Electric {
    public static class Throw {
        public static void IfNull(object instance, [CallerMemberName] string? paramName = null) {
            if (instance is null) {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
