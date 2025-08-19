using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#if NET48
using System.Data.Entity; // EF6
#else
using Microsoft.EntityFrameworkCore; // EF Core
#endif

namespace Bs.Nano.Electric.Builder.Internals {
//    public static class EfLikeExtensions {
//#if NET48
//    /// <summary>
//    /// EF6 + SQL Server Compact: будет транслироваться в SQL через LIKE.
//    /// </summary>
//    public static Expression<Func<string, bool>>  Like(this string field, string value)
//    {
//        return s => DbFunctions.Like(s, value);
//    }
//#else
//        /// <summary>
//        /// EF Core + SQLite: простое равенство, транслируется в "=".
//        /// </summary>
//        public static Expression<Func<string, bool>> Like(this string field, string value) {
//            return  s => s == value;
//        }
//#endif
//    }
    public static class EfFactoryExtensions {
        public static TEntity CreateEntity<TEntity>(this DbSet<TEntity> set) where TEntity : class, new() {
#if NET48
            return set.Create();
#else
            return new TEntity();
#endif
        }
    }
}
