﻿using Explore.Core;
using Explore.Core.Caching;
using Explore.Core.Data;
using Explore.Core.Domain.Common;
using Explore.Core.Domain.Localization;
using Explore.Data;
using Explore.Services.Events;
using Explore.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Localization
{
    /// <summary>
    /// 提供有关本地化的信息
    /// </summary>
    public partial class LocalizationService : ILocalizationService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string LOCALSTRINGRESOURCES_ALL_KEY = "Nop.lsr.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : resource key
        /// </remarks>
        private const string LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY = "Nop.lsr.{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string LOCALSTRINGRESOURCES_PATTERN_KEY = "Nop.lsr.";

        #endregion

        #region Fields

        private readonly IRepository<LocaleStringResource> _lsrRepository;
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly CommonSettings _commonSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="logger">Logger</param>
        /// <param name="workContext">Work context</param>
        /// <param name="lsrRepository">Locale string resource repository</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="localizationSettings">Localization settings</param>
        /// <param name="eventPublisher">Event published</param>
        public LocalizationService(ICacheManager cacheManager,
            ILogger logger, IWorkContext workContext,
            IRepository<LocaleStringResource> lsrRepository,
            IDataProvider dataProvider, IDbContext dbContext, CommonSettings commonSettings,
            LocalizationSettings localizationSettings, IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._logger = logger;
            this._workContext = workContext;
            this._lsrRepository = lsrRepository;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._commonSettings = commonSettings;
            this._localizationSettings = localizationSettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void DeleteLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException("localeStringResource");

            _lsrRepository.Delete(localeStringResource);

            //cache
            _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(localeStringResource);
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="localeStringResourceId">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceById(int localeStringResourceId)
        {
            if (localeStringResourceId == 0)
                return null;

            return _lsrRepository.GetById(localeStringResourceId);
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName)
        {
            //if (_workContext.WorkingLanguage != null)
            //    return GetLocaleStringResourceByName(resourceName, _workContext.WorkingLanguage.Id);
            return GetLocaleStringResourceByName(resourceName, 0);

            return null;
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId,
            bool logIfNotFound = true)
        {
            var query = from lsr in _lsrRepository.Table
                        orderby lsr.ResourceName
                        where lsr.LanguageId == languageId && lsr.ResourceName == resourceName
                        select lsr;
            var localeStringResource = query.FirstOrDefault();

            if (localeStringResource == null && logIfNotFound)
                _logger.Warning(string.Format("Resource string ({0}) not found. Language ID = {1}", resourceName, languageId));
            return localeStringResource;
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resources</returns>
        public virtual IList<LocaleStringResource> GetAllResources(int languageId)
        {
            var query = from l in _lsrRepository.Table
                        orderby l.ResourceName
                        where l.LanguageId == languageId
                        select l;
            var locales = query.ToList();
            return locales;
        }

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void InsertLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException("localeStringResource");

            _lsrRepository.Insert(localeStringResource);

            //cache
            _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(localeStringResource);
        }

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void UpdateLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException("localeStringResource");

            _lsrRepository.Update(localeStringResource);

            //cache
            _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(localeStringResource);
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resources</returns>
        public virtual Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId)
        {
            string key = string.Format(LOCALSTRINGRESOURCES_ALL_KEY, languageId);
            return _cacheManager.Get(key, () =>
            {
                //we use no tracking here for performance optimization
                //anyway records are loaded only for read-only operations
                var query = from l in _lsrRepository.TableNoTracking
                            orderby l.ResourceName
                            where l.LanguageId == languageId
                            select l;
                var locales = query.ToList();
                //format: <name, <id, value>>
                var dictionary = new Dictionary<string, KeyValuePair<int, string>>();
                foreach (var locale in locales)
                {
                    var resourceName = locale.ResourceName.ToLowerInvariant();
                    if (!dictionary.ContainsKey(resourceName))
                        dictionary.Add(resourceName, new KeyValuePair<int, string>(locale.Id, locale.ResourceValue));
                }
                return dictionary;
            });
        }

        /// <summary>
        /// 获取基于指定的ResourceKey属性的资源字符串。
        /// </summary>
        /// <param name="resourceKey">表示ResourceKey的字符串。</param>
        /// <returns>表示请求的资源字符串的字符串。</returns>
        public virtual string GetResource(string resourceKey)
        {
            //if (_workContext.WorkingLanguage != null)
            //    return GetResource(resourceKey, _workContext.WorkingLanguage.Id);
            return GetResource(resourceKey, 1);
        }

        /// <summary>
        /// 获取基于指定的ResourceKey属性的资源字符串。
        /// </summary>
        /// <param name="resourceKey">表示ResourceKey的字符串。</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="logIfNotFound">指示在找不到区域设置字符串资源时是否记录错误的值。</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="returnEmptyIfNotFound">一个值，指示在找不到资源且默认值设置为空字符串时是否返回空字符串。</param>
        /// <returns>表示请求的资源字符串的字符串。</returns>
        public virtual string GetResource(string resourceKey, int languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            string result = string.Empty;
            if (resourceKey == null)
                resourceKey = string.Empty;
            resourceKey = resourceKey.Trim().ToLowerInvariant();
            if (_localizationSettings.LoadAllLocaleRecordsOnStartup)
            {
                //load all records (we know they are cached)
                var resources = GetAllResourceValues(languageId);
                if (resources.ContainsKey(resourceKey))
                {
                    result = resources[resourceKey].Value;
                }
            }
            else
            {
                //gradual loading
                string key = string.Format(LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY, languageId, resourceKey);
                string lsr = _cacheManager.Get(key, () =>
                {
                    var query = from l in _lsrRepository.Table
                                where l.ResourceName == resourceKey
                                && l.LanguageId == languageId
                                select l.ResourceValue;
                    return query.FirstOrDefault();
                });

                if (lsr != null)
                    result = lsr;
            }
            if (String.IsNullOrEmpty(result))
            {
                if (logIfNotFound)
                    _logger.Warning(string.Format("Resource string ({0}) is not found. Language ID = {1}", resourceKey, languageId));

                if (!String.IsNullOrEmpty(defaultValue))
                {
                    result = defaultValue;
                }
                else
                {
                    if (!returnEmptyIfNotFound)
                        result = resourceKey;
                }
            }
            return result;
        }

        /// <summary>
        /// Export language resources to xml
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Result in XML format</returns>
        //public virtual string ExportResourcesToXml(Language language)
        //{
        //    if (language == null)
        //        throw new ArgumentNullException("language");
        //    var sb = new StringBuilder();
        //    var stringWriter = new StringWriter(sb);
        //    var xmlWriter = new XmlTextWriter(stringWriter);
        //    xmlWriter.WriteStartDocument();
        //    xmlWriter.WriteStartElement("Language");
        //    xmlWriter.WriteAttributeString("Name", language.Name);
        //    xmlWriter.WriteAttributeString("SupportedVersion", NopVersion.CurrentVersion);


        //    var resources = GetAllResources(language.Id);
        //    foreach (var resource in resources)
        //    {
        //        xmlWriter.WriteStartElement("LocaleResource");
        //        xmlWriter.WriteAttributeString("Name", resource.ResourceName);
        //        xmlWriter.WriteElementString("Value", null, resource.ResourceValue);
        //        xmlWriter.WriteEndElement();
        //    }

        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Close();
        //    return stringWriter.ToString();
        //}

        /// <summary>
        /// Import language resources from XML file
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="xml">XML</param>
        /// <param name="updateExistingResources">A value indicating whether to update existing resources</param>
        //public virtual void ImportResourcesFromXml(Language language, string xml, bool updateExistingResources = true)
        //{
        //    if (language == null)
        //        throw new ArgumentNullException("language");

        //    if (String.IsNullOrEmpty(xml))
        //        return;
        //    if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
        //    {
        //        //SQL 2005 insists that your XML schema incoding be in UTF-16.
        //        //Otherwise, you'll get "XML parsing: line 1, character XXX, unable to switch the encoding"
        //        //so let's remove XML declaration
        //        var inDoc = new XmlDocument();
        //        inDoc.LoadXml(xml);
        //        var sb = new StringBuilder();
        //        using (var xWriter = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true }))
        //        {
        //            inDoc.Save(xWriter);
        //            xWriter.Close();
        //        }
        //        var outDoc = new XmlDocument();
        //        outDoc.LoadXml(sb.ToString());
        //        xml = outDoc.OuterXml;

        //        //stored procedures are enabled and supported by the database.
        //        var pLanguageId = _dataProvider.GetParameter();
        //        pLanguageId.ParameterName = "LanguageId";
        //        pLanguageId.Value = language.Id;
        //        pLanguageId.DbType = DbType.Int32;

        //        var pXmlPackage = _dataProvider.GetParameter();
        //        pXmlPackage.ParameterName = "XmlPackage";
        //        pXmlPackage.Value = xml;
        //        pXmlPackage.DbType = DbType.Xml;

        //        var pUpdateExistingResources = _dataProvider.GetParameter();
        //        pUpdateExistingResources.ParameterName = "UpdateExistingResources";
        //        pUpdateExistingResources.Value = updateExistingResources;
        //        pUpdateExistingResources.DbType = DbType.Boolean;

        //        //long-running query. specify timeout (600 seconds)
        //        _dbContext.ExecuteSqlCommand("EXEC [LanguagePackImport] @LanguageId, @XmlPackage, @UpdateExistingResources",
        //            false, 600, pLanguageId, pXmlPackage, pUpdateExistingResources);
        //    }
        //    else
        //    {
        //        //stored procedures aren't supported
        //        var xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(xml);

        //        var nodes = xmlDoc.SelectNodes(@"//Language/LocaleResource");
        //        foreach (XmlNode node in nodes)
        //        {
        //            string name = node.Attributes["Name"].InnerText.Trim();
        //            string value = "";
        //            var valueNode = node.SelectSingleNode("Value");
        //            if (valueNode != null)
        //                value = valueNode.InnerText;

        //            if (String.IsNullOrEmpty(name))
        //                continue;

        //            //do not use "Insert"/"Update" methods because they clear cache
        //            //let's bulk insert
        //            var resource = language.LocaleStringResources.FirstOrDefault(x => x.ResourceName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        //            if (resource != null)
        //            {
        //                if (updateExistingResources)
        //                {
        //                    resource.ResourceValue = value;
        //                }
        //            }
        //            else
        //            {
        //                language.LocaleStringResources.Add(
        //                    new LocaleStringResource
        //                    {
        //                        ResourceName = name,
        //                        ResourceValue = value
        //                    });
        //            }
        //        }
        //        _languageService.UpdateLanguage(language);
        //    }

        //    //clear cache
        //    _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        //}

        #endregion
    }
}
