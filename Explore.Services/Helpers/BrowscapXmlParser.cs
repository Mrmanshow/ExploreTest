using Explore.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Explore.Services.Helpers
{
    /// <summary>
    /// 用于处理浏览器功能的XML文件的帮助程序类 (http://browscap.org/)
    /// </summary>
    public class BrowscapXmlHelper
    {
        private readonly List<string> _crawlerUserAgentsRegexp;

        public BrowscapXmlHelper(string userAgentStringsPath, string crawlerOnlyUserAgentStringsPath)
        {
            _crawlerUserAgentsRegexp = new List<string>();

            Initialize(userAgentStringsPath, crawlerOnlyUserAgentStringsPath);
        }

        private void Initialize(string userAgentStringsPath, string crawlerOnlyUserAgentStringsPath)
        {
            List<XElement> crawlerItems = null;

            if (!string.IsNullOrEmpty(crawlerOnlyUserAgentStringsPath) && File.Exists(crawlerOnlyUserAgentStringsPath))
            {
                //尝试仅从爬虫程序文件加载爬虫程序列表
                using (var sr = new StreamReader(crawlerOnlyUserAgentStringsPath))
                {
                    crawlerItems = XDocument.Load(sr).Root.Return(x => x.Elements("browscapitem").ToList(), null);
                }
            }

            if (crawlerItems == null)
            {
                //尝试从完整的用户代理文件加载爬虫程序列表
                using (var sr = new StreamReader(userAgentStringsPath))
                {
                    crawlerItems = XDocument.Load(sr).Root.Return(x => x.Element("browsercapitems"), null)
                        //只有爬虫
                        .Return(x => x.Elements("browscapitem").Where(IsBrowscapItemIsCrawler).ToList(), null);
                }
            }

            if (crawlerItems == null)
                throw new Exception("Incorrect file format");

            _crawlerUserAgentsRegexp.AddRange(crawlerItems
                //仅获取用户代理名称
                .Select(e => e.Attribute("name"))
                .Where(e => e != null && !string.IsNullOrEmpty(e.Value))
                .Select(e => e.Value)
                .Select(ToRegexp));

            if (string.IsNullOrEmpty(crawlerOnlyUserAgentStringsPath) || File.Exists(crawlerOnlyUserAgentStringsPath))
                return;

            //尝试写入爬虫文件
            using (var sw = new StreamWriter(crawlerOnlyUserAgentStringsPath))
            {
                var root = new XElement("browsercapitems");

                foreach (var crawler in crawlerItems)
                {
                    foreach (var element in crawler.Elements().ToList())
                    {
                        if (element.Attribute("name").Return(x => x.Value.ToLower(), string.Empty) == "crawler")
                            continue;
                        element.Remove();
                    }

                    root.Add(crawler);
                }
                root.Save(sw);
            }
        }

        private static bool IsBrowscapItemIsCrawler(XElement browscapItem)
        {
            var el = browscapItem.Elements("item").FirstOrDefault(e => e.Attribute("name").Return(a => a.Value, "") == "Crawler");

            return el != null && el.Attribute("value").Return(a => a.Value.ToLower() == "true", false);
        }

        private static string ToRegexp(string str)
        {
            var sb = new StringBuilder(Regex.Escape(str));
            sb.Replace("&amp;", "&").Replace("\\?", ".").Replace("\\*", ".*?");
            return string.Format("^{0}$", sb);
        }

        /// <summary>
        /// 确定用户代理是否为爬虫程序
        /// </summary>
        /// <param name="userAgent">用户代理字符串</param>
        /// <returns>如果用户代理是爬虫，则为true，否则为-false</returns>
        public bool IsCrawler(string userAgent)
        {
            return _crawlerUserAgentsRegexp.Any(p => Regex.IsMatch(userAgent, p));
        }
    }
}
