using System;
using System.ComponentModel;
using System.Reflection;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Nano.Electric {
    public static class ReflectionHelper {
        public static string GetDescription<MyEnum>(this MyEnum value) where MyEnum : Enum {
            FieldInfo? field = typeof(MyEnum).GetField(value.ToString());
            if (field is null) {
                throw new InvalidOperationException($"For value = \"{value}\" of type {typeof(MyEnum)} not defined DescriptionAttribute.");
            }
            DescriptionAttribute? attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute?.Description ?? value.ToString();
        }

    }
}
