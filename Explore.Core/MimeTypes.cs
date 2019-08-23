using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core
{
    /// <summary>
    /// 用于在需要时避免输入错误的mimetype常量 如果有集合mimetype遗漏，请随意添加
    /// </summary>
    public static class MimeTypes
    {
        #region application/*

        public const string ApplicationForceDownload = "application/force-download";

        public const string ApplicationJson = "application/json";

        public const string ApplicationOctetStream = "application/octet-stream";

        public const string ApplicationPdf = "application/pdf";

        public const string ApplicationRssXml = "application/rss+xml";

        public const string ApplicationXWwwFormUrlencoded = "application/x-www-form-urlencoded";

        public const string ApplicationXZipCo = "application/x-zip-co";

        #endregion application/*


        #region image/*

        public const string ImageBmp = "image/bmp";

        public const string ImageGif = "image/gif";

        public const string ImageJpeg = "image/jpeg";

        public const string ImagePJpeg = "image/pjpeg";

        public const string ImagePng = "image/png";

        public const string ImageTiff = "image/tiff";

        #endregion image/*


        #region text/*

        public const string TextCss = "text/css";

        public const string TextCsv = "text/csv";

        public const string TextJavascript = "text/javascript";

        public const string TextPlain = "text/plain";

        public const string TextXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        #endregion text/*
    }
}
