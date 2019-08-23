using Explore.Core;
using Explore.Core.Infrastructure;
using Explore.Services.Localization;
using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Web.Framework
{
    public class ExploreResourceDisplayName : System.ComponentModel.DisplayNameAttribute, IModelAttribute
    {
        private string _resourceValue = string.Empty;
        private bool _resourceValueRetrived;

        public ExploreResourceDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                //do not cache resources because it causes issues when you have multiple languages
                if (!_resourceValueRetrived)
                {
                    var langId = 1;
                    _resourceValue = EngineContext.Current
                        .Resolve<ILocalizationService>()
                        .GetResource(ResourceKey, langId, true, ResourceKey);
                    _resourceValueRetrived = true;
                }
                return _resourceValue;
            }
        }

        public string Name
        {
            get { return "NopResourceDisplayName"; }
        }
    }
}
