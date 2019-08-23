using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Web.Framework.MVC
{
    public class ExploreModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            if (model is BaseExploreModel)
            {
                ((BaseExploreModel)model).BindModel(controllerContext, bindingContext);
            }
            return model;
        }

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            //检查值的数据类型是否为System.String
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                //开发人员可以使用[notrim]属性标记要从剪裁中排除的属性。
                if (propertyDescriptor.Attributes.Cast<object>().All(a => a.GetType() != typeof(NoTrimAttribute)))
                {
                    var stringValue = (string)value;
                    value = string.IsNullOrEmpty(stringValue) ? stringValue : stringValue.Trim();
                }
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
