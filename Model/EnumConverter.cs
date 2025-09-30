// Ignore Spelling: Tdest Tsource Tprop dest Tret

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NetConvert = System.Convert;

namespace Nano.Electric {
    public static class EnumConverter<TEnum> where TEnum : struct, Enum {
        private static readonly bool _isFlagsEnum;
        private static readonly uint _allFlags;
        private static readonly string[] _descriptions;
        private static readonly string[] _enumNames;
        private static readonly TEnum[] _enumValues;

        static EnumConverter() {
            var enumType = typeof(TEnum);
            _isFlagsEnum = enumType.GetCustomAttribute<FlagsAttribute>() != null;

            _enumValues = Enum.GetValues(enumType).Cast<TEnum>().OrderBy(e => NetConvert.ToUInt32(e)).ToArray();
            _enumNames = _enumValues.Select(v => v.ToString()).ToArray();
            _descriptions = _enumValues
                .Select(v => {
                    var member = enumType.GetMember(v.ToString()).FirstOrDefault();
                    var attr = member?.GetCustomAttribute<DescriptionAttribute>();
                    return attr?.Description ?? v.ToString();
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
                : _enumValues.Contains(value);

        public static bool IsDefinedName(string value) =>
            _enumNames.Contains(value);

        public static TEnum Convert(string description) =>
            TryConvert(description, out var result)
                ? result
                : throw new ArgumentOutOfRangeException(nameof(description));

        public static bool TryConvert(string description, out TEnum value) {
            for (int i = 0; i < _descriptions.Length; i++) {
                if (string.Equals(_descriptions[i], description, StringComparison.OrdinalIgnoreCase)) {
                    value = _enumValues[i];
                    return true;
                }
            }
            value = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ToUint(TEnum val) =>
            NetConvert.ToUInt32(val);

        private static string GetDescriptionSingleValue(TEnum value) {
            int idx = Array.IndexOf(_enumValues, value);
            return idx >= 0 ? _descriptions[idx] : string.Empty;
        }
    }
}
