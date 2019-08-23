using Explore.Core.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping.Configuration
{
    public partial class SettingMap : ExploreEntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            this.ToTable("Setting");
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired().HasMaxLength(200);
            this.Property(s => s.Value).IsRequired().HasMaxLength(2000);
        }
    }
}
