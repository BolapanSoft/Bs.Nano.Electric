using System;

namespace Nano.Electric {
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property|AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultLocalizeValueAttribute : Attribute {
        public string DefaultLocalizeValue { get; }
        public DefaultLocalizeValueAttribute(string defaultLocalizeValue) {
            DefaultLocalizeValue = defaultLocalizeValue;
        }
    }
}
