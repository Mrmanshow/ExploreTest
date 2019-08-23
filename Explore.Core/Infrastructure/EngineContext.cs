using Explore.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Infrastructure
{
    public class EngineContext
    {
        #region Methods

        /// <summary>
        /// 初始化出静态工厂实例
        /// </summary>
        /// <param name="forceRecreate">强制重新初始化工厂实例</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = new NopEngine();

                var config = ConfigurationManager.GetSection("NopConfig") as NopConfig;
                Singleton<IEngine>.Instance.Initialize(config);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// 将静态引擎实例设置为提供的引擎。使用此方法提供您自己的引擎实现。
        /// </summary>
        /// <param name="engine">要使用的引擎。</param>
        /// <remarks>只有当你知道自己在做什么的时候才使用这个方法。</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton Nop engine used to access Nop services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}
