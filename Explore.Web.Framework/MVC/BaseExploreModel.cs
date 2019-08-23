using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Web.Framework.MVC
{

    /// <summary>
    /// 基础模型
    /// </summary>
    [ModelBinder(typeof(ExploreModelBinder))]
    public partial class BaseExploreModel
    {
        public BaseExploreModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }
        
        /// <summary>
        /// 开发人员可以在自定义partial classes中重写此方法，以便向构造函数添加一些自定义初始化代码。
        /// </summary>
        protected virtual void PostInitialize()
        {

        }

        /// <summary>
        /// 使用此属性存储模型的任何自定义值。
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    /// <summary>
    /// 基于Web层的实体模型
    /// </summary>
    public partial class BaseExploreEntityModel : BaseExploreModel
    {
        public virtual int Id { get; set; }
    }
}
