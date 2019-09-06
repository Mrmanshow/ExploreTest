using Explore.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Localization
{
    /// <summary>
    /// 本地化管理器接口
    /// </summary>
    public partial interface ILocalizationService
    {
        /// <summary>
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        void DeleteLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="localeStringResourceId">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        LocaleStringResource GetLocaleStringResourceById(int localeStringResourceId);

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <returns>Locale string resource</returns>
        LocaleStringResource GetLocaleStringResourceByName(string resourceName);

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <returns>Locale string resource</returns>
        LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId,
            bool logIfNotFound = true);

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resources</returns>
        IList<LocaleStringResource> GetAllResources(int languageId);

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        void InsertLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        void UpdateLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resources</returns>
        Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId);

        /// <summary>
        /// 获取基于指定的ResourceKey属性的资源字符串。
        /// </summary>
        /// <param name="resourceKey">表示ResourceKey的字符串。</param>
        /// <returns>表示请求的资源字符串的字符串。</returns>
        string GetResource(string resourceKey);

        /// <summary>
        /// 获取基于指定的ResourceKey属性的资源字符串。
        /// </summary>
        /// <param name="resourceKey">表示ResourceKey的字符串。</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="logIfNotFound">指示在找不到区域设置字符串资源时是否记录错误的值。</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="returnEmptyIfNotFound">一个值，指示在找不到资源且默认值设置为空字符串时是否返回空字符串。</param>
        /// <returns>表示请求的资源字符串的字符串。</returns>
        string GetResource(string resourceKey, int languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false);

        /// <summary>
        /// Export language resources to xml
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Result in XML format</returns>
        //string ExportResourcesToXml(Language language);

        /// <summary>
        /// Import language resources from XML file
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="xml">XML</param>
        /// <param name="updateExistingResources">A value indicating whether to update existing resources</param>
        //void ImportResourcesFromXml(Language language, string xml, bool updateExistingResources = true);
    }
}
