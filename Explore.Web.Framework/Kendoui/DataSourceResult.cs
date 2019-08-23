using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Web.Framework.Kendoui
{
    public class DataSourceResult
    {
        public object ExtraData { get; set; }

        public IEnumerable Data { get; set; }

        public object Errors { get; set; }

        public int Total { get; set; }
    }
}
