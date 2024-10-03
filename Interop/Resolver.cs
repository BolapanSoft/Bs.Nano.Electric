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
        private readonly string path= "C:\\Program Files (x86)\\Nanosoft\\nanoCAD BIM Электро. Редактор БД 23.1";
        private readonly AppDomain domain;
        public Resolver(string assembliesPath):this(AppDomain.CurrentDomain, assembliesPath) {
            this.path = assembliesPath; 

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }
        public Resolver(AppDomain domain, string assembliesPath) {
            this.path = assembliesPath; 
            this.domain = domain;
            this.domain.AssemblyResolve += AssemblyResolve;
        }
        private Assembly? AssemblyResolve( object sender, ResolveEventArgs args) {
            string assemblyName = new AssemblyName(args.Name).Name;
            string assemblyFileName = Path.Combine(path, assemblyName + ".dll");
            if (File.Exists(assemblyFileName)) {
                return Assembly.LoadFrom(assemblyFileName);
            }
            return null;
        }

        public void Dispose() {
            domain.AssemblyResolve -= AssemblyResolve;
        }
    }
}
