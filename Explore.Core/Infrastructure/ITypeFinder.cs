using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Infrastructure
{
    /// <summary>
    /// 实现此接口的类提供有关引擎中各种服务的类型的信息。
    /// </summary>
    public interface ITypeFinder
    {
        IList<Assembly> GetAssemblies();

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
    }
}
