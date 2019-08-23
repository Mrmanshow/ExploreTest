using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Web.Framework.Menu
{
    public static class Extensions
    {
        /// <summary>
        /// 检查此节点或子节点是否具有指定的系统名称
        /// </summary>
        /// <param name="node"></param>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public static bool ContainsSystemName(this SiteMapNode node, string systemName)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (String.IsNullOrWhiteSpace(systemName))
                return false;

            if (systemName.Equals(node.SystemName, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return node.ChildNodes.Any(cn => ContainsSystemName(cn, systemName));
        }
    }
}
