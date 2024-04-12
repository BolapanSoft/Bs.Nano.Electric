using System.ComponentModel;

namespace Nano.Electric.Enums {
	public enum EnvExplSafeBoolEnum
	{
		[Description("Без взрывозащиты")]
		EXPL_SAFE_NO = 0,
		[Description("Со взрывозащитой")]
		EXPL_SAFE_YES = 1
	}
}
