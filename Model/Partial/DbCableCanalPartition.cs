using Nano.Electric.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nano.Electric {
    public partial class DbCableCanalPartition {
#if InitDbContextEnums
        public ScsBoxMeasureUnit? BoxMeasureUnit { get; set; } = ScsBoxMeasureUnit.IN_SEGMENTS;

#endif
    }
}
