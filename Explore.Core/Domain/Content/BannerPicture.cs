using Explore.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Content
{
    public partial class BannerPicture : BaseEntity
    {
        public int BannerId { set; get; }

        public int PictureId { set; get; }

        public virtual Picture Picture { set; get; }

        public virtual Banner Banner { set; get; }

    }
}
