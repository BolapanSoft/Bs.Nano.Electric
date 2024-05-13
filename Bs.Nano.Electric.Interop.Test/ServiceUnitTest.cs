using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace Bs.Nano.Electric.Interop.Test {
    [TestClass]
    public class ServiceUnitTest {
        static ServiceUnitTest() {
          var resolver = new Resolver("C:\\Program Files (x86)\\Nanosoft\\nanoCAD Электро. Редактор БД 8.5");
          var assembly=  Assembly.LoadFile(@"C:\Program Files (x86)\Nanosoft\nanoCAD Электро. Редактор БД 8.5\Cadwise.Graphic.Db.dll");
        }
        [TestMethod]
        [DataRow(@"C:\Program Files (x86)\Nanosoft\nanoCAD Электро. Редактор БД 8.5", @"Resources\screw.step")]
        public void LoadSTEP(string cadwisePath, string stepFile) {
            var service = new Service(cadwisePath);
            Assert.IsNotNull(service);
            var localDir =Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var stepFullName=Path.Combine(localDir, stepFile);
            var stepModel = service.LoadSTEP(stepFullName);
            Assert.IsNotNull (stepModel);
            Assert.IsNotNull(stepModel.Graphic);
        }
    }
}