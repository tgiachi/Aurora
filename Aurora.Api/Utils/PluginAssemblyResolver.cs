using System.Reflection;
using System.Runtime.Loader;

namespace Aurora.Api.Utils
{
    public class PluginAssemblyResolver : AssemblyLoadContext
    {
        private readonly List<string> _assemblyBlackList = new();
        private readonly AssemblyDependencyResolver _assemblyDependencyResolver;

        private readonly string _path;

        public PluginAssemblyResolver(string path)
        {
            _path = path;
            _assemblyDependencyResolver = new AssemblyDependencyResolver(path);
            BuildCache();
        }

        private void BuildCache()
        {
            var files = Directory.GetFiles(_path, "*.dll", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    var ass = Load(AssemblyName.GetAssemblyName(file));
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var assemblyPath = _assemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);
            return assemblyName != null ? LoadFromAssemblyPath(assemblyPath) : null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var libraryPath = _assemblyDependencyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            return libraryPath != null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
        }
    }
}
