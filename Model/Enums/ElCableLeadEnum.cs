using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElCableLeadEnum
	{
		[Description("Cверху")]
		LEAD_UP = 0,
		[Description("Cнизу")]
		LEAD_DOWN = 1,
		[Description("Cнизу и сверху")]
		LEAD_UP_DOWN = 2
	}
}
