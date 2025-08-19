#if NETFRAMEWORK

namespace System.Diagnostics.CodeAnalysis {
    //
    // Summary:
    //     Applied to a method that will never return under any circumstance.
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute {
    }
    //
    // Summary:
    //     Specifies that an output is not null even if the corresponding type allows it.
    //     Specifies that an input argument was not null when the call returns.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
    public sealed class NotNullAttribute : Attribute {
        //
        // Summary:
        //     Initializes a new instance of the System.Diagnostics.CodeAnalysis.NotNullAttribute
        //     class.
        public NotNullAttribute() { }
    }
} 
/// <summary>Specifies that when a method returns <see cref="ReturnValue"/>, the parameter will not be null even if the corresponding type allows it.</summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute : Attribute
    {
        /// <summary>Initializes the attribute with the specified return value condition.</summary>
        /// <param name="returnValue">
        /// The return value condition. If the method returns this value, the associated parameter will not be null.
        /// </param>
        public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

        /// <summary>Gets the return value condition.</summary>
        public bool ReturnValue { get; }
    }
#endif


