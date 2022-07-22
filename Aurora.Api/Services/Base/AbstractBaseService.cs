using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Api.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Aurora.Api.Services.Base
{
    public class AbstractBaseService<TService> where TService : class
    {
        private readonly IEventBusService _eventBusService;
        protected ILogger Logger { get; set; }

        public AbstractBaseService(IEventBusService eventBusService, ILogger<TService> logger)
        {
            _eventBusService = eventBusService;
            Logger = logger;
        }

        protected Task PublishEvent<TEntity>(TEntity entity)
        {
            return _eventBusService.PublishEvent(entity);
        }
    }
}
