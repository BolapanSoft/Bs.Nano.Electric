
using System.ComponentModel;

namespace Nano.Electric.Enums {
	public enum BoardStructureType
	{
        [Description("Комплектный")]
        GANGED = 0,
        [Description("Некомплектный")]
		NOT_GANGED = 1
	}
}
