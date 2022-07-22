using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Api.Data.EventMessage
{
    public class EventQueueMessage<TData>
    {
        public string Type { get; set; } = null!;
        public TData Data { get; set; } = default!;
    }

    public class EventQueueMessageBuilder<TData>
    {
        private readonly EventQueueMessage<TData> _message = new();

        public static EventQueueMessageBuilder<TData> Builder()
        {
            return new EventQueueMessageBuilder<TData>();
        }
        public EventQueueMessageBuilder<TData> Type(string type)
        {
            _message.Type = type;
            return this;
        }

        public EventQueueMessageBuilder<TData> Data(TData data)
        {
            _message.Data = data;
            return this;
        }

        public EventQueueMessage<TData> Build()
        {
            return _message;
        }
    }
}
