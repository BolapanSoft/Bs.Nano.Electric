﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Nano.Electric {
    public static class Throw {
        public static void IfNull([NotNull]object? instance, [CallerMemberName] string? paramName = null) {
            if (instance is null) {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
