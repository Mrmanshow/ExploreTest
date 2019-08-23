using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Data.Mapping
{
    public abstract class ExploreEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected ExploreEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}
