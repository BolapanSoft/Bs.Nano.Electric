// Ignore Spelling: Nano

using System;
using System.IO;
using System.Reflection;
using Cadwise.OpenCascade.Interop;
using Nano.Electric;

namespace Bs.Nano.Electric.Interop {
    public class Service:IDisposable {
        private const string OpenCascadeInteropDll = "Cadwise.OpenCascade.Interop.dll";
        private const string CadwiseGraphicDb_Dll = "Cadwise.Graphic.Db.dll";
        private readonly string cadwisePath;
        private readonly Lazy<Cadwise.OpenCascade.Interop.StepImporterService> lzStepImporterService;
        private readonly IDisposable resolver;
        private bool disposedValue;

        public Service(string cadwisePath) {
            if (cadwisePath is not null) {
                string localPath =Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var _path = System.IO.Path.Combine(localPath, cadwisePath);
                if (System.IO.Directory.Exists(_path)) {
                    this.cadwisePath = _path;
                }
                else {
                    throw new System.Exception($"Не найден каталог {_path}");
                }
            }
            else {
                throw new ArgumentNullException($"Не задан каталог места расположения модулей Cadwise (nanoCAD Электро. Редактор БД)");
            }
            resolver=new Resolver(cadwisePath);
            lzStepImporterService = new Lazy<Cadwise.OpenCascade.Interop.StepImporterService>(GetStepImporter, isThreadSafe: false);
        }
        public Cadwise.Graphic.Db.DbGraphic LoadSTEP(string file) {
            if (disposedValue) { throw new ObjectDisposedException(GetType().Name); }
            if (file is null) {
                throw new ArgumentNullException(nameof(file));
            }
            if (!System.IO.File.Exists(file)) {
                throw new DllNotFoundException($"Не найден файл \"{file}\"");
            }
            var ext = System.IO.Path.GetExtension(file).ToLower();
            if (ext != ".stp" && ext != ".step") {
                throw new FormatException($"Неверный тип  файла \"{file}\". Для импорта необходимо указать файл STEP.");
            }
            Cadwise.OpenCascade.Interop.StepImporterService service = lzStepImporterService.Value;
            string codeBase = typeof(StepImporterService).Assembly.CodeBase;
            Cadwise.Graphic.Interfaces.IGraphicView dbGraphicView = service.Import(file);
            Cadwise.Graphic.Db.DbGraphic dbGraphic = new();
            dbGraphic.GraphicView = dbGraphicView;
            dbGraphic.SaveChanges();
            return dbGraphic;
        }
        private StepImporterService GetStepImporter() {
            string ocDLL = System.IO.Path.Combine(cadwisePath, OpenCascadeInteropDll);
            if (!System.IO.File.Exists(ocDLL)) {
                throw new DllNotFoundException($"Не найден файл \"{ocDLL}\"");
            }
            string cgdDll = System.IO.Path.Combine(cadwisePath, CadwiseGraphicDb_Dll);
            if (!System.IO.File.Exists(cgdDll)) {
                throw new DllNotFoundException($"Не найден файл \"{cgdDll}\"");
            }
            // Load .net assembly
            Assembly assembly = Assembly.LoadFile(ocDLL);
            assembly = Assembly.LoadFile(cgdDll);

            return new Cadwise.OpenCascade.Interop.StepImporterService();
        }
        private string CadwisePath => cadwisePath;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    resolver?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
