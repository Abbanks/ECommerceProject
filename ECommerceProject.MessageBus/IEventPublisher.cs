using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceProject.EventBridge
{
    public interface IEventPublisher
    {
        Task PublishEvent(object detail, string detailType, string source);
    }
}
