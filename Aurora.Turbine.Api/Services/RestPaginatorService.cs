using Aurora.Api.Entities.Interfaces.Dto;
using Aurora.Api.Entities.Interfaces.Entities;
using Aurora.Api.Interfaces.Services;
using Aurora.Api.Services.Base;
using Aurora.Turbine.Api.Data.Pagination;
using Aurora.Turbine.Api.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aurora.Turbine.Api.Services
{
    public class RestPaginatorService : AbstractBaseService<RestPaginatorService>, IRestPaginatorService
    {
        public RestPaginatorService(IEventBusService eventBusService, ILogger<RestPaginatorService> logger) : base(eventBusService, logger)
        {

        }

        public Task<PaginationObject<TEntity>> Paginate<TId, TEntity>(IQueryable<TEntity> resultQueryObjects, int page, int pageSize) where TEntity : IBaseEntity<TId>
        {
            return Paginate<TId, TEntity>(resultQueryObjects.ToList(), page, pageSize);
        }

        public Task<PaginationObject<TEntity>> Paginate<TId, TEntity>(List<TEntity> resultQueryObjects, int page,
            int pageSize) where TEntity : IBaseEntity<TId>
        {
            var totalCount = resultQueryObjects.Count();
            page = page < 1 ? 1 : page;

            resultQueryObjects = resultQueryObjects.Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return Task.FromResult(PaginationObjectBuilder<TEntity>
                .Create()
                .Size(pageSize)
                .Count(totalCount)
                .Page(page)
                .Results(resultQueryObjects)
                .Build());
        }

        public async Task<PaginationObject<TDto>> Paginate<TId, TEntity, TDto, TDtoMapper>(List<TEntity> resultQueryObjects,
            int page,
            int pageSize,
            TDtoMapper mapper
            ) where TEntity : IBaseEntity<TId>
            where TDto : IBaseDto<TId>
            where TDtoMapper : IDtoMapper<TId, TEntity, TDto>
        {
            var result = await Paginate<TId, TEntity>(resultQueryObjects, page, pageSize);
            return await PaginateToDto<TId, TEntity, TDto, TDtoMapper>(result, mapper);
        }

        public Task<PaginationObject<TDto>> Paginate<TId, TEntity, TDto, TDtoMapper>(IQueryable<TEntity> resultQueryObjects,
            int page,
            int pageSize,
            TDtoMapper mapper) where TEntity : IBaseEntity<TId> where TDto : IBaseDto<TId> where TDtoMapper : IDtoMapper<TId, TEntity, TDto>
        {
            return Paginate<TId, TEntity, TDto, TDtoMapper>(resultQueryObjects.ToList(), page, pageSize, mapper);
        }


        public Task<PaginationObject<TDto>> PaginateToDto<TId, TEntity, TDto, TDtoMapper>(
            PaginationObject<TEntity> paginationObject, TDtoMapper mapper)
            where TEntity : IBaseEntity<TId>
            where TDto : IBaseDto<TId>
            where TDtoMapper : IDtoMapper<TId, TEntity, TDto>
        {
            return Task.FromResult(new PaginationObject<TDto>()
            {
                TotalPages = paginationObject.TotalPages,
                Size = paginationObject.Size,
                Count = paginationObject.Count,
                Result = mapper.ToDto(paginationObject.Result),
                Page = paginationObject.Page
            });
        }
    }
}
