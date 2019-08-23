using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Web.Framework.UI
{
    /// <summary>
    /// 页头构造器
    /// </summary>
    public partial interface IPageHeadBuilder
    {
        void AddTitleParts(string part);
        void AppendTitleParts(string part);

        void AddMetaDescriptionParts(string part);
        void AppendMetaDescriptionParts(string part);

        void AddMetaKeywordParts(string part);
        void AppendMetaKeywordParts(string part);

        void AddScriptParts(ResourceLocation location, string part, bool excludeFromBundle, bool isAync);
        void AppendScriptParts(ResourceLocation location, string part, bool excludeFromBundle, bool isAsync);
        string GenerateScripts(UrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null);

        void AddCssFileParts(ResourceLocation location, string part, bool excludeFromBundle = false);
        void AppendCssFileParts(ResourceLocation location, string part, bool excludeFromBundle = false);
        string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null);

        void AddCanonicalUrlParts(string part);
        void AppendCanonicalUrlParts(string part);
        string GenerateCanonicalUrls();

        void AddHeadCustomParts(string part);
        void AppendHeadCustomParts(string part);
        string GenerateHeadCustom();

        void AddPageCssClassParts(string part);
        void AppendPageCssClassParts(string part);
        string GeneratePageCssClasses();

        /// <summary>
        /// Specify "edit page" URL
        /// </summary>
        /// <param name="url">URL</param>
        void AddEditPageUrl(string url);
        /// <summary>
        /// Get "edit page" URL
        /// </summary>
        /// <returns>URL</returns>
        string GetEditPageUrl();

        /// <summary>
        /// 指定应选择的管理菜单项的系统名称（展开）
        /// </summary>
        /// <param name="systemName">系统名称</param>
        void SetActiveMenuItemSystemName(string systemName);
        /// <summary>
        /// 获取应选择的管理菜单项的系统名称（展开）
        /// </summary>
        /// <returns>系统名称</returns>
        string GetActiveMenuItemSystemName();
    }
}
