using Aurora.Api.Entities.Interfaces.Dto;
using Aurora.Api.Entities.Interfaces.Entities;
using Aurora.Turbine.Api.Data.Pagination;

namespace Aurora.Turbine.Api.Interfaces
{
    public interface IRestPaginatorService
    {

        Task<PaginationObject<TEntity>> Paginate<TId, TEntity>(IQueryable<TEntity> resultQueryObjects, int page, int pageSize)
            where TEntity : IBaseEntity<TId>;

        Task<PaginationObject<TEntity>> Paginate<TId, TEntity>(List<TEntity> resultQueryObjects, int page, int pageSize)
            where TEntity : IBaseEntity<TId>;

        Task<PaginationObject<TDto>> Paginate<TId, TEntity, TDto, TDtoMapper>(List<TEntity> resultQueryObjects,
            int page,
            int pageSize,
            TDtoMapper mapper
        ) where TEntity : IBaseEntity<TId>
            where TDto : IBaseDto<TId>
            where TDtoMapper : IDtoMapper<TId, TEntity, TDto>;

        Task<PaginationObject<TDto>> Paginate<TId, TEntity, TDto, TDtoMapper>(IQueryable<TEntity> resultQueryObjects,
            int page,
            int pageSize,
            TDtoMapper mapper
        ) where TEntity : IBaseEntity<TId>
            where TDto : IBaseDto<TId>
            where TDtoMapper : IDtoMapper<TId, TEntity, TDto>;

        Task<PaginationObject<TDto>> PaginateToDto<TId, TEntity, TDto, TDtoMapper>(
            PaginationObject<TEntity> paginationObject, TDtoMapper mapper)
            where TEntity : IBaseEntity<TId>
            where TDto : IBaseDto<TId>
            where TDtoMapper : IDtoMapper<TId, TEntity, TDto>;
    }
}
