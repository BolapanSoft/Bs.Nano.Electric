// Ignore Spelling: Iek

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Bs.Nano.Electric.Builder.Internals {
    internal static class Throw {
        
        internal static T NotNull<T>([NotNull] this T? argument, FormattableString errMessage, [CallerArgumentExpression("argument")] string? paramName = "")
            where T : class {
            if (argument is null) {
                throw new ArgumentNullException(paramName, errMessage.ToString());
            }
            return argument;
        }
        internal static T NotNull<T>([NotNull] this T? argument, Func<FormattableString> gerErrMessage, [CallerArgumentExpression("argument")] string? paramName = "")
            where T : class {
            if (argument is null) {
                throw new ArgumentNullException(paramName, gerErrMessage().ToString());
            }
            return argument;
        }
        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        internal static T NotNull<T>([NotNull] this T? argument, [CallerArgumentExpression("argument")] string? paramName = "") where T : class {
            if (argument is null) {
                throw new ArgumentNullException(paramName);
            }
            return argument;
        }
        internal static T NotNull<T>([NotNull] this T? argument, Func<Exception> throwEx) where T : class {
            if (argument is null) {
                throw throwEx();
            }
            return argument;
        }
        internal static void IfIsTrue(bool argument, Func<Exception> throwEx) {
            if (argument) {
                throw throwEx();
            }
        }
        internal static void IfIsFalse(bool argument, Func<Exception> throwEx) {
            if (!argument) {
                throw throwEx();
            }
        }

    }
}