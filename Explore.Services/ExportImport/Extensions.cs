using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Explore.Services.ExportImport
{
    public static partial class Extensions
    {
        public static void WriteString(this XmlWriter xmlWriter, string nodeName, object nodeValue, bool ignore = false, string defaulValue = "")
        {
            if (ignore) return;
            xmlWriter.WriteElementString(nodeName, nodeValue == null ? defaulValue : nodeValue.ToString());
        }
    }
}
