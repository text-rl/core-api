using System.Reflection;

namespace CoreApi.ApplicationCore
{
    public static class DependencyInjection
    {
        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}