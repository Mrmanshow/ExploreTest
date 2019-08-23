using Explore.Core;
using Explore.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Events
{
    public static class EventPublisherExtensions
    {
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityInserted<T>(entity));
        }

        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityUpdated<T>(entity));
        }

        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity
        {
            eventPublisher.Publish(new EntityDeleted<T>(entity));
        }
    }
}
