using System.ComponentModel;

namespace Nano.Electric.Enums {
    public enum ElCableLeadEnum
	{
		[Description("Сверху")]
		LEAD_UP = 0,
		[Description("Снизу")]
		LEAD_DOWN = 1,
		[Description("Снизу и сверху")]
		LEAD_UP_DOWN = 2
	}
}
