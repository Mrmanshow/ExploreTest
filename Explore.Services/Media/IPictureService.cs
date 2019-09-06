using Explore.Core;
using Explore.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Media
{
    /// <summary>
    /// Picture service interface
    /// </summary>
    public partial interface IPictureService
    {
        /// <summary>
        /// 根据图片存储设置获取加载的图片二进制文件
        /// </summary>
        /// <param name="picture">图片</param>
        /// <returns>图片二进制</returns>
        byte[] LoadPictureBinary(Picture picture);

        /// <summary>
        /// Get picture SEO friendly name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Result</returns>
        string GetPictureSeName(string name);

        /// <summary>
        /// 获取默认图片URL
        /// </summary>
        /// <param name="targetSize">目标图片大小（最长边）</param>
        /// <param name="defaultPictureType">默认图片类型</param>
        /// <param name="storeLocation">存储位置url；空值用于自动确定当前存储位置</param>
        /// <returns>图片URL</returns>
        string GetDefaultPictureUrl(int targetSize = 0,
            PictureType defaultPictureType = PictureType.Entity,
            string storeLocation = null);

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="pictureId">图片ID</param>
        /// <param name="targetSize">目标图片大小（最长边）</param>
        /// <param name="showDefaultPicture">指示是否显示默认图片的值</param>
        /// <param name="storeLocation">存储位置url；空值用于自动确定当前存储位置</param>
        /// <param name="defaultPictureType">默认图片类型</param>
        /// <returns>图片URL</returns>
        string GetPictureUrl(int pictureId,
            int targetSize = 0,
            bool showDefaultPicture = true,
            string storeLocation = null,
            PictureType defaultPictureType = PictureType.Entity);

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="picture">图片实例</param>
        /// <param name="targetSize">目标图片大小（最长边）</param>
        /// <param name="showDefaultPicture">指示是否显示默认图片的值</param>
        /// <param name="storeLocation">存储位置url；空值用于自动确定当前存储位置</param>
        /// <param name="defaultPictureType">默认图片类型</param>
        /// <returns>图片URL</returns>
        string GetPictureUrl(Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true,
            string storeLocation = null,
            PictureType defaultPictureType = PictureType.Entity);

        /// <summary>
        /// Get a picture local path
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true);

        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <returns>Picture</returns>
        Picture GetPictureById(int pictureId);

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="picture">Picture</param>
        void DeletePicture(Picture picture);

        /// <summary>
        /// Gets a collection of pictures
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <returns>Paged list of pictures</returns>
        IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets pictures by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>Pictures</returns>
        //IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0);

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
        Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true);

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
        Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType,
            string seoFilename, string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true);

        /// <summary>
        /// Updates a SEO filename of a picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <returns>Picture</returns>
        Picture SetSeoFilename(int pictureId, string seoFilename);

        /// <summary>
        /// 验证输入图片尺寸
        /// </summary>
        /// <param name="pictureBinary">图片二进制</param>
        /// <param name="mimeType">MIME类型</param>
        /// <returns>图片二进制或引发异常n</returns>
        byte[] ValidatePicture(byte[] pictureBinary, string mimeType);

        /// <summary>
        /// 获取或设置一个值，该值指示图像是否应存储在数据库中。
        /// </summary>
        bool StoreInDb { get; set; }

        /// <summary>
        /// Get pictures hashes
        /// </summary>
        /// <param name="picturesIds">Pictures Ids</param>
        /// <returns></returns>
        IDictionary<int, string> GetPicturesHash(int[] picturesIds);
    }
}
