using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bs.Nano.Electric.Interop {
    public class Resolver:IDisposable {
        private bool disposed;
        private readonly string path= "C:\\Program Files (x86)\\Nanosoft\\nanoCAD BIM Электро. Редактор БД 23.1";
        private readonly AppDomain domain;
        // Переменная потока, чтобы отслеживать, какие сборки уже разрешаются в этом потоке
        [ThreadStatic]
        private static HashSet<string>? _resolving;
        public Resolver(string assembliesPath):this(AppDomain.CurrentDomain, assembliesPath) {
            //this.path = assembliesPath; 

            //AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }
        public Resolver(AppDomain domain, string assembliesPath) {
            this.path = assembliesPath; 
            this.domain = domain;
            this.domain.AssemblyResolve += AssemblyResolve;
        }
        private Assembly? AssemblyResolve( object sender, ResolveEventArgs args) {
            if (string.IsNullOrEmpty(args?.Name))
                return null;

            var requested = new AssemblyName(args.Name);
            string simpleName = requested.Name ?? args.Name;
            try {
                //Console.Write($"Resolving assembly: {simpleName} ");
                _resolving ??= new(StringComparer.OrdinalIgnoreCase);
                if (_resolving.Contains(requested.FullName ?? simpleName)) {
                    // В лог можно писать, но возвращаем null, чтобы не зациклиться
                    // //Console.WriteLine($"Resolver: recursion detected for {simpleName}");
                    return null;
                }
                _resolving.Add(requested.FullName ?? simpleName);
                // 1) Попытаться загрузить из заданного каталога
                string assemblyFileName = Path.Combine(path, simpleName + ".dll");
                if (File.Exists(assemblyFileName)) {
                    try {
                        //Console.WriteLine($"- found at {assemblyFileName} (loading from path)");
                        return Assembly.LoadFrom(assemblyFileName);
                    }
                    catch (Exception ex) {
                        //Console.WriteLine($"- found but failed to load from path: {ex.Message}");
                        // Продолжаем попытки разрешить другим способом
                    }
                }
                else {
                    //Console.WriteLine("- not found in path");
                }

                // 2) Попытаться загрузить через глобальные/системные механизмы (GAC / Default Load Context)
                try {
                    // Assembly.Load с полным именем попросит рантайм загрузить сборку из GAC или default-контекста
                    var loaded = Assembly.Load(requested);
                    if (loaded != null) {
                        //Console.WriteLine($"- loaded by Assembly.Load from default context / GAC: {loaded.FullName}");
                        return loaded;
                    }
                }
                catch (FileLoadException fle) {
                    // Если манифест не совпадает (версия) — сообщим и вернём null
                    //Console.WriteLine($"- Assembly.Load failed (FileLoad): {fle.Message}");
                }
                catch (Exception ex) {
                    //Console.WriteLine($"- Assembly.Load failed: {ex.Message}");
                }

                //// 3) В качестве дополнительной попытки — пробуем LoadFrom путём поиска файла по пути (расширенные поиски)
                //// (например, когда в path есть подпапки или нестандартные имена)
                //try {
                //    var candidate = Directory.EnumerateFiles(path, assemblySimpleName + ".dll", SearchOption.AllDirectories).FirstOrDefault();
                //    if (!string.IsNullOrEmpty(candidate) && File.Exists(candidate)) {
                //        //Console.WriteLine($"- found in subfolder: {candidate} (loading)");
                //        return Assembly.LoadFrom(candidate);
                //    }
                //}
                //catch (Exception ex) {
                //    //Console.WriteLine($"- searching subfolders failed: {ex.Message}");
                //}

                //Console.WriteLine("- not resolved");
                return null;
            }
            catch (Exception ex) {
                // Защищаемся от неожиданного исключения в резолвере
                //Console.WriteLine($"Resolver exception: {ex}");
                return null;
            }
        }

        public void Dispose() {
            if (!disposed) {
                try {
                    domain.AssemblyResolve -= AssemblyResolve;
                }
                catch {
                    // ignore
                }
                disposed = true;
            }
        }
    }
}

