using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using Nano.Electric;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Bs.Nano.Electric.Report {
    public static class Assert {
        public static void IsNotNull(object value, string message) {
            if (value == null) {
                HandleFail(message);
            }
        }
        public static void AreEqual(string expected, string actual, string message) {
            if (string.CompareOrdinal(expected, actual) != 0) {
                HandleFail(message);
            }
        }

        internal static void HandleFail(string message) {
            throw new RuleTestException(message);
        }
    }
    public class RuleTestException : Exception {
        public RuleTestException(string message) : base(message) {

        }
        public RuleTestException(string message, Exception ex) : base(message, ex) {

        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestClassAttribute : Attribute {
        //
        // Summary:
        //     Gets a test method attribute that enables running this test.
        //
        // Parameters:
        //   testMethodAttribute:
        //     The test method attribute instance defined on this method.
        //
        // Returns:
        //     The Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute to be used
        //     to run this test.
        //
        // Remarks:
        //     Extensions can override this method to customize how all methods in a class are
        //     run.
        public virtual ReportRuleAttribute GetTestMethodAttribute(ReportRuleAttribute testMethodAttribute) {
            return testMethodAttribute;
        }
    }
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ReportRuleAttribute : Attribute, IComparable {
        public static readonly ReportRuleAttribute Empty = new ReportRuleAttribute();
        private readonly int[] index;
        private readonly Lazy<string> lzListIndex;
        //
        // Summary:
        //     Gets display name for the test.
        public string DisplayName { get; }

        public string ListIndex => lzListIndex.Value;
        //
        // Summary:
        //     Initializes a new instance of the Bs.Nano.Electric.Report.TestMethodAttribute
        //     class.
        public ReportRuleAttribute()
            : this(string.Empty) {
        }

        //
        // Summary:
        //     Initializes a new instance of the Bs.Nano.Electric.Report.TestMethodAttribute
        //     class.
        //
        // Parameters:
        //   displayName:
        //     Display name for the test.
        public ReportRuleAttribute(string displayName, params int[] index) {
            DisplayName = displayName;
            this.index = index;
            lzListIndex = new(() => {
                var index = this.index;
                if (index == null) {
                    return string.Empty;
                }
                return string.Join(".", index);
            });
        }

        public int CompareTo(object obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ReportRuleAttribute tmAttr) {
                for (int i = 0; i < Math.Min(index.Length, tmAttr.index.Length); i++) {
                    int result = index[i].CompareTo(tmAttr.index[i]);
                    if (result != 0) {
                        return result;
                    }
                }
                return index.Length - tmAttr.index.Length;
            }
            else {
                throw new ArgumentException();
            }
        }

        public override string ToString() {
            return $"{ListIndex} \"{DisplayName}\"";
        }
    }
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RuleCategoryAttribute : TestCategoryBaseAttribute {
        private IList<string> testCategories;

        //
        // Summary:
        //     Gets the test categories that has been applied to the test.
        public override IList<string> TestCategories => testCategories;

        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute
        //     class and applies the category to the test.
        //
        // Parameters:
        //   testCategory:
        //     The test Category.
        public RuleCategoryAttribute(string testCategory) {
            testCategories = new List<string>(1) { testCategory };
        }
        public RuleCategoryAttribute(params string[] testCategoryes) {
            testCategories = testCategoryes;
        }
    }
    //
    // Summary:
    //     Base class for the "Category" attribute
    //
    // Remarks:
    //     The reason for this attribute is to let the users create their own implementation
    //     of test categories. - test framework (discovery, etc) deals with TestCategoryBaseAttribute.
    //     - The reason that TestCategories property is a collection rather than a string,
    //     is to give more flexibility to the user. For instance the implementation may
    //     be based on enums for which the values can be OR'ed in which case it makes sense
    //     to have single attribute rather than multiple ones on the same test.
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class TestCategoryBaseAttribute : Attribute {
        //
        // Summary:
        //     Gets the test category that has been applied to the test.
        public abstract IList<string> TestCategories { get; }

        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryBaseAttribute
        //     class. Applies the category to the test. The strings returned by TestCategories
        //     are used with the /category command to filter tests
        protected TestCategoryBaseAttribute() {
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class PriorityAttribute : Attribute {
        //
        // Summary:
        //     Gets the priority.
        public int Priority { get; }

        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.TestTools.UnitTesting.PriorityAttribute
        //     class.
        //
        // Parameters:
        //   priority:
        //     The priority.
        public PriorityAttribute(int priority) {
            Priority = priority;
        }
    }

    public static class RuleTestHelper {
        //public static void RunTests<T>(string testCategory, int? priority = null) where T : class {
        //    var testTasks = new List<Task>();
        //    var testClass = typeof(T);
        //    if (!testClass.GetCustomAttributes<TestClassAttribute>().Any()) {
        //        throw new ArgumentException($"Для типа {nameof(T)} не назначен атрибут TestClassAttribute.");
        //    }
        //    IEnumerable<(int? Priority, MethodInfo TestMethhod)> testMethodsQuery = GetTests<T>(testCategory);
        //    if (priority.HasValue) {
        //        testMethodsQuery = testMethodsQuery.Where(p => p.Priority == priority.Value);
        //    }
        //    else {
        //        testMethodsQuery = testMethodsQuery.Where(p => !p.Priority.HasValue);
        //    }
        //    // bool allRight = true;
        //    InvokeAll<T>(testMethodsQuery.Select(tm => tm.TestMethhod));
        //    return;
        //}

        public static void InvokeAll<T>(this IEnumerable<MethodInfo> testMethodsQuery, Func<T>? ctor) {
            var testClass = typeof(T);
            var instance = (ctor is null) ? Activator.CreateInstance(testClass) : ctor();
            List<Exception> exceptions = new List<Exception>();

            foreach (var testItem in testMethodsQuery) {
                ReportRuleAttribute rule = GetReportRule(testItem);
                try {
                    var miParameters = testItem.GetParameters();
                    if (miParameters.Length == 0) {
                        testItem.Invoke(instance, null);
                    }
                    else
                        throw new NotImplementedException("Допускается вызов только методов без параметров.");
                }
                catch (TargetInvocationException tiEx) {
                    RuleTestException ex = new RuleTestException($"Не пройдено правило проверки {rule}", tiEx.InnerException);
                    exceptions.Add(ex);
                }
                catch (Exception ex) {
                    exceptions.Add(new RuleTestException($"Не пройдено правило проверки {rule}", ex));
                }
            }
            if (exceptions.Count > 0) {
                throw new AggregateException(exceptions);
            }
        }
        public static void CheckRule(this MethodInfo testItem, Checker checker) {
            ReportRuleAttribute rule = GetReportRule(testItem);
            try {
                var miParameters = testItem.GetParameters();
                if (miParameters.Length == 0) {
                    testItem.Invoke(checker, null);
                }
                else
                    throw new NotImplementedException("Допускается вызов только методов без параметров.");
            }
            catch (TargetInvocationException tiEx) {
                throw new RuleTestException($"Не пройдено правило проверки {rule}", tiEx.InnerException);
            }
            catch (Exception ex) {
                throw new RuleTestException($"Не пройдено правило проверки {rule}", ex);
            }
        }
        public static IEnumerable<MethodInfo> GetTests<T>(this string testCategory) where T : class {
            int getPriority(MethodInfo mi) {
                return mi.GetCustomAttribute<PriorityAttribute>()?.Priority ?? 0;
            };
            return typeof(T).GetMethods()
            .Where(m => m.GetCustomAttributes<RuleCategoryAttribute>()
                .Any(a => a.TestCategories.Contains(testCategory)))
            .OrderBy(mi => getPriority(mi));

        }
        public static ReportRuleAttribute GetReportRule(this MethodInfo mi) {
            return mi.GetCustomAttribute<ReportRuleAttribute>() ?? ReportRuleAttribute.Empty;
        }
        public static int GetCount(IQueryable queryable) {
            var countProperty = typeof(Queryable).GetMethods()
                .FirstOrDefault(m => m.Name == "Count" && m.GetParameters().Length == 1)
                ?.MakeGenericMethod(queryable.ElementType);

            if (countProperty != null) {
                return (int)countProperty.Invoke(null, new object[] { queryable });
            }

            return 0;
        }
        public static IEnumerable<(object property, string tableName)> GetKnownTables(this DbContext source) {
            var dbSetProperties = source.GetType().GetProperties()
        .Where(p => p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.PropertyType.GetGenericArguments().Length > 0);

            foreach (PropertyInfo propInfo in dbSetProperties) {
                Type propType = propInfo.PropertyType.GetGenericArguments()[0];
                string tableName = propType.GetCustomAttribute<DefaultLocalizeValueAttribute>()?.DefaultLocalizeValue
                                   ?? propType.Name;
                object dbSetInstance = propInfo.GetValue(source); // Get the instance of the DbSet<T> property
                yield return (dbSetInstance, tableName);
            }
        }
    }
}
