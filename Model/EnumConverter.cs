// Ignore Spelling: Tdest Tsource Tprop dest Tret

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Nano.Electric {
    public static class EnumConverter<TEnum> where TEnum : struct, Enum {
        private static readonly bool _isFlagsEnum;
        private static readonly uint _allFlags;
        private static readonly string[] _descriptions;
        private static readonly string[] _enumNames;
        private static readonly TEnum[] _enumValues;

        static EnumConverter() {
            _isFlagsEnum = typeof(TEnum).GetCustomAttribute<FlagsAttribute>() is not null;

            _enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().OrderBy(e => e).ToArray();
            _enumNames = _enumValues.Select(v => v.ToString()!).ToArray();
            _descriptions = _enumValues
                .Select(v => {
                    var member = typeof(TEnum).GetMember(v.ToString()!).FirstOrDefault();
                    var attr = member?.GetCustomAttribute<DescriptionAttribute>();
                    return attr?.Description ?? string.Empty;
                })
                .ToArray();

            _allFlags = _isFlagsEnum
                ? _enumValues.Aggregate(0u, (acc, v) => acc | ToUint(v))
                : ToUint(_enumValues.Last());
        }

        public static string GetDescription(TEnum value) {
            if (!_isFlagsEnum)
                return GetDescriptionSingleValue(value);

            var uintValue = ToUint(value);
            var parts = _enumValues
                .Where(v => (ToUint(v) & uintValue) == ToUint(v) && ToUint(v) != 0)
                .Select(GetDescriptionSingleValue)
                .Where(desc => !string.IsNullOrEmpty(desc));

            return string.Join("; ", parts);
        }

        public static bool IsDefinedValue(TEnum value) =>
            _isFlagsEnum
                ? (ToUint(value) & ~_allFlags) == 0
                : Array.BinarySearch(_enumValues, value) >= 0;

        public static bool IsDefinedName(string value) =>
            _enumNames.Contains(value);

        public static TEnum Convert(string description) =>
            TryConvert(description, out var result)
                ? result
                : throw new ArgumentOutOfRangeException(nameof(description));

        public static bool TryConvert(string description, out TEnum value) {
            for (int i = 0; i < _descriptions.Length; i++) {
                if (_descriptions[i] == description) {
                    value = _enumValues[i];
                    return true;
                }
            }
            value = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ToUint(TEnum val) =>
            Unsafe.As<TEnum, uint>(ref val);

        private static string GetDescriptionSingleValue(TEnum value) {
            int idx = Array.BinarySearch(_enumValues, value);
            return idx >= 0 ? _descriptions[idx] : string.Empty;
        }
    }
}
