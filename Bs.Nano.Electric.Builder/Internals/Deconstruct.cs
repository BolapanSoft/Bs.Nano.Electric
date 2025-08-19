using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bs.Nano.Electric.Builder.UtilitySetMakerResources;
using static Bs.Nano.Electric.Builder.UtilitySetsMaker;

namespace Bs.Nano.Electric.Builder.Internals {
    internal static class TypeExtentions {
#if NETFRAMEWORK
        public static void Deconstruct(
            this Tuple<
                string, string, string, string,
                Tuple<string, int>,
                Tuple<bool, double>,
                IEnumerable<UtilityUnit>
            > confJob,
            out string DbName,
            out string DbNaming,
            out string DbDescription,
            out string DbImageRef,
            out string CableCanalCode,
            out int PartitionCount,
            out bool IsUse,
            out double PostDistance,
            out IEnumerable<UtilityUnit> UtilityUnits) {
            DbName = confJob.Item1;
            DbNaming = confJob.Item2;
            DbDescription = confJob.Item3;
            DbImageRef = confJob.Item4;

            CableCanalCode = confJob.Item5.Item1;
            PartitionCount = confJob.Item5.Item2;

            IsUse = confJob.Item6.Item1;
            PostDistance = confJob.Item6.Item2;

            UtilityUnits = confJob.Item7;
        } 
#endif
    }
}
