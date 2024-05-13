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
        private readonly string path;
        public Resolver(string assembliesPath) {
            this.path = assembliesPath; 
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }
        private Assembly AssemblyResolve( object sender, ResolveEventArgs args) {
            string assemblyName = new AssemblyName(args.Name).Name;
            string assemblyFileName = Path.Combine(path, assemblyName + ".dll");
            if (File.Exists(assemblyFileName)) {
                return Assembly.LoadFrom(assemblyFileName);
            }
            return null;
        }

        public void Dispose() {
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
        }
    }
}
