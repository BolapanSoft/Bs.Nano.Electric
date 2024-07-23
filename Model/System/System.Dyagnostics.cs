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
	internal sealed class NotNullAttribute : Attribute {
		//
		// Summary:
		//     Initializes a new instance of the System.Diagnostics.CodeAnalysis.NotNullAttribute
		//     class.
		public NotNullAttribute() { }
	}
} 
#endif