using Explore.Core;
using Explore.Core.Data;
using Explore.Core.Domain.Media;
using Explore.Data;
using Explore.Services.Configuration;
using Explore.Services.Events;
using Explore.Services.Logging;
using Explore.Services.Seo;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Explore.Services.Media
{
    /// <summary>
    /// Picture service
    /// </summary>
    public partial class PictureService : IPictureService
    {
        #region Const

        private const int MULTIPLE_THUMB_DIRECTORIES_LENGTH = 3;

        #endregion

        #region Fields

        private readonly IRepository<Picture> _pictureRepository;
        //private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly MediaSettings _mediaSettings;
        private readonly IDataProvider _dataProvider;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="pictureRepository">Picture repository</param>
        /// <param name="productPictureRepository">Product picture repository</param>
        /// <param name="settingService">Setting service</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="logger">Logger</param>
        /// <param name="dbContext">Database context</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="mediaSettings">Media settings</param>
        /// <param name="dataProvider">Data provider</param>
        public PictureService(IRepository<Picture> pictureRepository,
            //IRepository<ProductPicture> productPictureRepository,
            ISettingService settingService,
            IWebHelper webHelper,
            ILogger logger,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
            MediaSettings mediaSettings,
            IDataProvider dataProvider)
        {
            this._pictureRepository = pictureRepository;
            //this._productPictureRepository = productPictureRepository;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._logger = logger;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;
            this._mediaSettings = mediaSettings;
            this._dataProvider = dataProvider;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="resizeType">Resize type</param>
        /// <param name="ensureSizePositive">A value indicatingh whether we should ensure that size values are positive</param>
        /// <returns></returns>
        protected virtual Size CalculateDimensions(Size originalSize, int targetSize,
            ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true)
        {
            float width, height;

            switch (resizeType)
            {
                case ResizeType.LongestSide:
                    if (originalSize.Height > originalSize.Width)
                    {
                        // portrait
                        width = originalSize.Width * (targetSize / (float)originalSize.Height);
                        height = targetSize;
                    }
                    else
                    {
                        // landscape or square
                        width = targetSize;
                        height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    }
                    break;
                case ResizeType.Width:
                    width = targetSize;
                    height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    break;
                case ResizeType.Height:
                    width = originalSize.Width * (targetSize / (float)originalSize.Height);
                    height = targetSize;
                    break;
                default:
                    throw new Exception("Not supported ResizeType");
            }

            if (ensureSizePositive)
            {
                if (width < 1)
                    width = 1;
                if (height < 1)
                    height = 1;
            }

            //we invoke Math.Round to ensure that no white background is rendered - http://www.nopcommerce.com/boards/t/40616/image-resizing-bug.aspx
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        /// <summary>
        /// 从MIME类型返回文件扩展名。
        /// </summary>
        /// <param name="mimeType">Mime类型</param>
        /// <returns>文件扩展名</returns>
        protected virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            //also see System.Web.MimeMapping for more mime types

            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }

        /// <summary>
        /// 从文件加载图片
        /// </summary>
        /// <param name="pictureId">图片ID</param>
        /// <param name="mimeType">MIME类型</param>
        /// <returns>图片二进制</returns>
        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            var filePath = GetPictureLocalPath(fileName);
            if (!File.Exists(filePath))
                return new byte[0];
            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// 在文件系统上保存图片
        /// </summary>
        /// <param name="pictureId">图片标识符</param>
        /// <param name="pictureBinary">图片二进制</param>
        /// <param name="mimeType">MIME类型</param>
        protected virtual void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            File.WriteAllBytes(GetPictureLocalPath(fileName), pictureBinary);
        }

        /// <summary>
        /// Delete a picture on file system
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string fileName = string.Format("{0}_0.{1}", picture.Id.ToString("0000000"), lastPart);
            string filePath = GetPictureLocalPath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 删除图片缩略图
        /// </summary>
        /// <param name="picture">图片</param>
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            string filter = string.Format("{0}*.*", picture.Id.ToString("0000000"));
            var thumbDirectoryPath = CommonHelper.MapPath("~/content/images/thumbs");
            string[] currentFiles = System.IO.Directory.GetFiles(thumbDirectoryPath, filter, SearchOption.AllDirectories);
            foreach (string currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                File.Delete(thumbFilePath);
            }
        }

        /// <summary>
        /// 获取图片（缩略图）本地路径
        /// </summary>
        /// <param name="thumbFileName">文件名</param>
        /// <returns>本地图片缩略图路径</returns>
        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = CommonHelper.MapPath("~/content/images/thumbs");
            if (_mediaSettings.MultipleThumbDirectories)
            {
                //获取文件名的前两个字母
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > MULTIPLE_THUMB_DIRECTORIES_LENGTH)
                {
                    var subDirectoryName = fileNameWithoutExtension.Substring(0, MULTIPLE_THUMB_DIRECTORIES_LENGTH);
                    thumbsDirectoryPath = Path.Combine(thumbsDirectoryPath, subDirectoryName);
                    if (!System.IO.Directory.Exists(thumbsDirectoryPath))
                    {
                        System.IO.Directory.CreateDirectory(thumbsDirectoryPath);
                    }
                }
            }
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }

        /// <summary>
        /// 获取图片（缩略图）URL
        /// </summary>
        /// <param name="thumbFileName">文件名</param>
        /// <param name="storeLocation">存储位置url；空值用于自动确定当前存储位置</param>
        /// <returns>本地图片缩略图路径</returns>
        protected virtual string GetThumbUrl(string thumbFileName, string storeLocation = null)
        {
            storeLocation = !String.IsNullOrEmpty(storeLocation)
                                    ? storeLocation
                                    : _webHelper.GetStoreLocation();

            var url = storeLocation + "content/images/thumbs/";

            if (_mediaSettings.MultipleThumbDirectories)
            {
                //get the first two letters of the file name
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > MULTIPLE_THUMB_DIRECTORIES_LENGTH)
                {
                    var subDirectoryName = fileNameWithoutExtension.Substring(0, MULTIPLE_THUMB_DIRECTORIES_LENGTH);
                    url = url + subDirectoryName + "/";
                }
            }

            url = url + thumbFileName;
            return url;
        }

        /// <summary>
        /// 获取图片本地路径。存储在文件系统（不在数据库中）上的图像时使用
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>本地图片路径</returns>
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return Path.Combine(CommonHelper.MapPath("~/content/images/"), fileName);
        }

        /// <summary>
        /// 根据图片存储设置获取加载的图片二进制文件
        /// </summary>
        /// <param name="picture">图片</param>
        /// <param name="fromDb">从数据库加载；否则，从文件系统加载</param>
        /// <returns>图片为二进制文件</returns>
        protected virtual byte[] LoadPictureBinary(Picture picture, bool fromDb)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            var result = fromDb
                ? picture.PictureBinary
                : LoadPictureFromFile(picture.Id, picture.MimeType);
            return result;
        }

        /// <summary>
        /// 获取一个值，该值指示某个文件（缩略图）是否已存在
        /// </summary>
        /// <param name="thumbFilePath">缩略图文件路径</param>
        /// <param name="thumbFileName">缩略图文件名</param>
        /// <returns>结果</returns>
        protected virtual bool GeneratedThumbExists(string thumbFilePath, string thumbFileName)
        {
            return File.Exists(thumbFilePath);
        }

        /// <summary>
        /// 保存一个值，该值指示某个文件（缩略图）是否已存在
        /// </summary>
        /// <param name="thumbFilePath">缩略图文件路径</param>
        /// <param name="thumbFileName">缩略图文件名</param>
        /// <param name="mimeType">MIME类型</param>
        /// <param name="binary">图片二进制</param>
        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            File.WriteAllBytes(thumbFilePath, binary);
        }

        #endregion

        #region Getting picture local path/URL methods

        /// <summary>
        /// 根据图片存储设置获取加载的图片二进制文件
        /// </summary>
        /// <param name="picture">图片</param>
        /// <returns>图片二进制</returns>
        public virtual byte[] LoadPictureBinary(Picture picture)
        {
            return LoadPictureBinary(picture, this.StoreInDb);
        }

        /// <summary>
        /// Get picture SEO friendly name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Result</returns>
        public virtual string GetPictureSeName(string name)
        {
            return SeoExtensions.GetSeName(name, true, false);
        }

        /// <summary>
        /// 获取默认图片URL
        /// </summary>
        /// <param name="targetSize">目标图片大小（最长边）</param>
        /// <param name="defaultPictureType">默认图片类型</param>
        /// <param name="storeLocation">存储位置url；空值用于自动确定当前存储位置</param>
        /// <returns>图片URL</returns>
        public virtual string GetDefaultPictureUrl(int targetSize = 0,
            PictureType defaultPictureType = PictureType.Entity,
            string storeLocation = null)
        {
            string defaultImageFileName;
            switch (defaultPictureType)
            {
                case PictureType.Avatar:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.Customer.DefaultAvatarImageName", "default-avatar.jpg");
                    break;
                case PictureType.Entity:
                default:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.DefaultImageName", "default-image.png");
                    break;
            }
            string filePath = GetPictureLocalPath(defaultImageFileName);
            if (!File.Exists(filePath))
            {
                return "";
            }


            if (targetSize == 0)
            {
                string url = (!String.IsNullOrEmpty(storeLocation)
                                 ? storeLocation
                                 : _webHelper.GetStoreLocation())
                                 + "content/images/" + defaultImageFileName;
                return url;
            }
            else
            {
                string fileExtension = Path.GetExtension(filePath);
                string thumbFileName = string.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(filePath),
                    targetSize,
                    fileExtension);
                var thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    using (var b = new Bitmap(filePath))
                    {
                        using (var destStream = new MemoryStream())
                        {
                            var newSize = CalculateDimensions(b.Size, targetSize);
                            ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                            {
                                Width = newSize.Width,
                                Height = newSize.Height,
                                Scale = ScaleMode.Both,
                                Quality = _mediaSettings.DefaultImageQuality
                            });
                            var destBinary = destStream.ToArray();
                            SaveThumb(thumbFilePath, thumbFileName, "", destBinary);
                        }
                    }
                }
                var url = GetThumbUrl(thumbFileName, storeLocation);
                return url;
            }
        }

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="pictureId">图片ID</param>
        /// <param name="targetSize">目标图片大小（最长边）</param>
        /// <param name="showDefaultPicture">指示是否显示默认图片的值</param>
        /// <param name="storeLocation">存储位置url；空值用于自动确定当前存储位置</param>
        /// <param name="defaultPictureType">默认图片类型</param>
        /// <returns>图片URL</returns>
        public virtual string GetPictureUrl(int pictureId,
            int targetSize = 0,
            bool showDefaultPicture = true,
            string storeLocation = null,
            PictureType defaultPictureType = PictureType.Entity)
        {
            var picture = GetPictureById(pictureId);
            return GetPictureUrl(picture, targetSize, showDefaultPicture, storeLocation, defaultPictureType);
        }

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="picture">图片实例</param>
        /// <param name="targetSize">目标图片大小（最长边）</param>
        /// <param name="showDefaultPicture">指示是否显示默认图片的值</param>
        /// <param name="storeLocation">存储位置url；空值用于自动确定当前存储位置</param>
        /// <param name="defaultPictureType">默认图片类型</param>
        /// <returns>图片URL</returns>
        public virtual string GetPictureUrl(Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true,
            string storeLocation = null,
            PictureType defaultPictureType = PictureType.Entity)
        {
            string url = string.Empty;
            byte[] pictureBinary = null;
            if (picture != null)
                pictureBinary = LoadPictureBinary(picture);
            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                {
                    url = GetDefaultPictureUrl(targetSize, defaultPictureType, storeLocation);
                }
                return url;
            }

            if (picture.IsNew)
            {
                DeletePictureThumbs(picture);

                //我们不验证图片二进制文件，以确保不会引发异常（“参数无效”）。
                picture = UpdatePicture(picture.Id,
                    pictureBinary,
                    picture.MimeType,
                    picture.SeoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    false,
                    false);
            }

            var seoFileName = picture.SeoFilename; // = GetPictureSeName(picture.SeoFilename); //just for sure

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;
            if (targetSize == 0)
            {
                thumbFileName = !String.IsNullOrEmpty(seoFileName)
                    ? string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), seoFileName, lastPart)
                    : string.Format("{0}.{1}", picture.Id.ToString("0000000"), lastPart);
            }
            else
            {
                thumbFileName = !String.IsNullOrEmpty(seoFileName)
                    ? string.Format("{0}_{1}_{2}.{3}", picture.Id.ToString("0000000"), seoFileName, targetSize, lastPart)
                    : string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), targetSize, lastPart);
            }
            string thumbFilePath = GetThumbLocalPath(thumbFileName);

            //命名的互斥体有助于避免在不同的线程中创建相同的文件，并且不会显著降低性能，因为代码只针对特定的文件被阻塞。
            using (var mutex = new Mutex(false, thumbFileName))
            {
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    mutex.WaitOne();

                    //check, if the file was created, while we were waiting for the release of the mutex.
                    if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                    {
                        byte[] pictureBinaryResized;

                        //resizing required
                        if (targetSize != 0)
                        {
                            using (var stream = new MemoryStream(pictureBinary))
                            {
                                Bitmap b = null;
                                try
                                {
                                    //try-catch to ensure that picture binary is really OK. Otherwise, we can get "Parameter is not valid" exception if binary is corrupted for some reasons
                                    b = new Bitmap(stream);
                                }
                                catch (ArgumentException exc)
                                {
                                    _logger.Error(string.Format("Error generating picture thumb. ID={0}", picture.Id),
                                        exc);
                                }

                                if (b == null)
                                {
                                    //bitmap could not be loaded for some reasons
                                    return url;
                                }

                                using (var destStream = new MemoryStream())
                                {
                                    var newSize = CalculateDimensions(b.Size, targetSize);
                                    ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                                    {
                                        Width = newSize.Width,
                                        Height = newSize.Height,
                                        Scale = ScaleMode.Both,
                                        Quality = _mediaSettings.DefaultImageQuality
                                    });
                                    pictureBinaryResized = destStream.ToArray();
                                    b.Dispose();
                                }
                            }
                        }
                        else
                        {
                            //create a copy of pictureBinary
                            pictureBinaryResized = pictureBinary.ToArray();
                        }

                        SaveThumb(thumbFilePath, thumbFileName, picture.MimeType, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }

            }
            url = GetThumbUrl(thumbFileName, storeLocation);
            return url;
        }

        /// <summary>
        /// Get a picture local path
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public virtual string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
        {
            string url = GetPictureUrl(picture, targetSize, showDefaultPicture);
            if (String.IsNullOrEmpty(url))
                return String.Empty;

            return GetThumbLocalPath(Path.GetFileName(url));
        }

        #endregion

        #region CRUD methods

        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <returns>Picture</returns>
        public virtual Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _pictureRepository.GetById(pictureId);
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="picture">Picture</param>
        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            //delete thumbs
            DeletePictureThumbs(picture);

            //delete from file system
            if (!this.StoreInDb)
                DeletePictureOnFileSystem(picture);

            //delete from database
            _pictureRepository.Delete(picture);

            //event notification
            _eventPublisher.EntityDeleted(picture);
        }

        /// <summary>
        /// Gets a collection of pictures
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <returns>Paged list of pictures</returns>
        public virtual IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from p in _pictureRepository.Table
                        orderby p.Id descending
                        select p;
            var pics = new PagedList<Picture>(query, pageIndex, pageSize);
            return pics;
        }

        /// <summary>
        /// Gets pictures by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>Pictures</returns>
        //public virtual IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0)
        //{
        //    if (productId == 0)
        //        return new List<Picture>();


        //    var query = from p in _pictureRepository.Table
        //                join pp in _productPictureRepository.Table on p.Id equals pp.PictureId
        //                orderby pp.DisplayOrder, pp.Id
        //                where pp.ProductId == productId
        //                select p;

        //    if (recordsToReturn > 0)
        //        query = query.Take(recordsToReturn);

        //    var pics = query.ToList();
        //    return pics;
        //}

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="pictureBinary">图片二进制</param>
        /// <param name="mimeType">图片MIME类型</param>
        /// <param name="seoFilename">SEO文件名</param>
        /// <param name="altAttribute">“img”html元素的“alt”属性</param>
        /// <param name="titleAttribute">“img”html元素的“title”属性</param>
        /// <param name="isNew">指示图片是否为新图片的值</param>
        /// <param name="validateBinary">指示是否验证提供的图片二进制的值。</param>
        /// <returns>图片</returns>
        public virtual Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = new Picture
            {
                PictureBinary = this.StoreInDb ? pictureBinary : new byte[0],
                MimeType = mimeType,
                SeoFilename = seoFilename,
                AltAttribute = altAttribute,
                TitleAttribute = titleAttribute,
                IsNew = isNew,
            };
            _pictureRepository.Insert(picture);

            if (!this.StoreInDb)
                SavePictureInFile(picture.Id, pictureBinary, mimeType);

            //event notification
            _eventPublisher.EntityInserted(picture);

            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="mimeType">The picture MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided picture binary</param>
        /// <returns>Picture</returns>
        public virtual Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType,
            string seoFilename, string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = GetPictureById(pictureId);
            if (picture == null)
                return null;

            //delete old thumbs if a picture has been changed
            if (seoFilename != picture.SeoFilename)
                DeletePictureThumbs(picture);

            picture.PictureBinary = this.StoreInDb ? pictureBinary : new byte[0];
            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;
            picture.IsNew = isNew;

            _pictureRepository.Update(picture);

            if (!this.StoreInDb)
                SavePictureInFile(picture.Id, pictureBinary, mimeType);

            //event notification
            _eventPublisher.EntityUpdated(picture);

            return picture;
        }

        /// <summary>
        /// Updates a SEO filename of a picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <returns>Picture</returns>
        public virtual Picture SetSeoFilename(int pictureId, string seoFilename)
        {
            var picture = GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            //update if it has been changed
            if (seoFilename != picture.SeoFilename)
            {
                //update picture
                picture = UpdatePicture(picture.Id,
                    LoadPictureBinary(picture),
                    picture.MimeType,
                    seoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    true,
                    false);
            }
            return picture;
        }

        /// <summary>
        /// 验证输入图片尺寸
        /// </summary>
        /// <param name="pictureBinary">图片二进制</param>
        /// <param name="mimeType">MIME类型</param>
        /// <returns>图片二进制或引发异常n</returns>
        public virtual byte[] ValidatePicture(byte[] pictureBinary, string mimeType)
        {
            using (var destStream = new MemoryStream())
            {
                ImageBuilder.Current.Build(pictureBinary, destStream, new ResizeSettings
                {
                    MaxWidth = _mediaSettings.MaximumImageSize,
                    MaxHeight = _mediaSettings.MaximumImageSize,
                    Quality = _mediaSettings.DefaultImageQuality
                });
                return destStream.ToArray();
            }
        }

        /// <summary>
        /// Helper class for making pictures hashes from DB
        /// </summary>
        private class HashItem : IComparable, IComparable<HashItem>
        {
            public int PictureId { get; set; }
            public byte[] Hash { get; set; }

            public int CompareTo(object obj)
            {
                return CompareTo(obj as HashItem);
            }

            public int CompareTo(HashItem other)
            {
                return other == null ? -1 : PictureId.CompareTo(other.PictureId);
            }
        }

        /// <summary>
        /// Get pictures hashes
        /// </summary>
        /// <param name="picturesIds">Pictures Ids</param>
        /// <returns></returns>
        public IDictionary<int, string> GetPicturesHash(int[] picturesIds)
        {
            var supportedLengthOfBinaryHash = _dataProvider.SupportedLengthOfBinaryHash();
            if (supportedLengthOfBinaryHash == 0 || !picturesIds.Any())
                return new Dictionary<int, string>();

            const string strCommand = "SELECT [Id] as [PictureId], HASHBYTES('sha1', substring([PictureBinary], 0, {0})) as [Hash] FROM [Picture] where id in ({1})";
            return _dbContext.SqlQuery<HashItem>(String.Format(strCommand, supportedLengthOfBinaryHash, picturesIds.Select(p => p.ToString()).Aggregate((all, current) => all + ", " + current))).Distinct()
                .ToDictionary(p => p.PictureId, p => BitConverter.ToString(p.Hash).Replace("-", ""));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置一个值，该值指示图像是否应存储在数据库中。
        /// </summary>
        public virtual bool StoreInDb
        {
            get
            {
                return _settingService.GetSettingByKey("Media.Images.StoreInDB", false);
            }
            set
            {
                //检查它是否为新值
                if (this.StoreInDb == value)
                    return;

                //保存新设置值
                _settingService.SetSetting("Media.Images.StoreInDB", value);

                int pageIndex = 0;
                const int pageSize = 400;
                var originalProxyCreationEnabled = _dbContext.ProxyCreationEnabled;
                try
                {
                    //we set this property for performance optimization
                    //it could be critical if you we have several thousand pictures
                    _dbContext.ProxyCreationEnabled = false;

                    while (true)
                    {
                        var pictures = this.GetPictures(pageIndex, pageSize);
                        pageIndex++;

                        //all pictures converted?
                        if (!pictures.Any())
                            break;

                        foreach (var picture in pictures)
                        {
                            var pictureBinary = LoadPictureBinary(picture, !value);

                            //we used the code below before. but it's too slow
                            //let's do it manually (uncommented code) - copy some logic from "UpdatePicture" method
                            /*just update a picture (all required logic is in "UpdatePicture" method)
                            we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown when "moving" pictures
                            UpdatePicture(picture.Id,
                                          pictureBinary,
                                          picture.MimeType,
                                          picture.SeoFilename,
                                          true,
                                          false);*/
                            if (value)
                                //delete from file system. now it's in the database
                                DeletePictureOnFileSystem(picture);
                            else
                                //now on file system
                                SavePictureInFile(picture.Id, pictureBinary, picture.MimeType);
                            //update appropriate properties
                            picture.PictureBinary = value ? pictureBinary : new byte[0];
                            picture.IsNew = true;
                            //raise event?
                            //_eventPublisher.EntityUpdated(picture);
                        }
                        //save all at once
                        _pictureRepository.Update(pictures);
                        //detach them in order to release memory
                        foreach (var picture in pictures)
                        {
                            _dbContext.Detach(picture);
                        }
                    }
                }
                finally
                {
                    _dbContext.ProxyCreationEnabled = originalProxyCreationEnabled;
                }
            }
        }

        #endregion
    }
}
