using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Api.Entities.Interfaces.Dao;
using Aurora.Api.Entities.Interfaces.Dto;
using Aurora.Api.Entities.Interfaces.Entities;
using Aurora.Turbine.Api.Data.Pagination;
using Aurora.Turbine.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;

namespace Aurora.Turbine.Api.Controllers
{
    public class BaseRestController<TId, TEntity, TDto, TDao, TDtoMapper>
        where TEntity : class, IBaseEntity<TId>
        where TDto : class, IBaseDto<TId>
        where TDao : IDataAccess<TId, TEntity>
        where TDtoMapper : IDtoMapper<TId, TEntity, TDto>
    {
        protected TDao Dao { get; }
        protected TDtoMapper DtoMapper { get; }
        protected ILogger Logger { get; }

        protected IRestPaginatorService RestPaginator { get; }

        public BaseRestController(ILogger<BaseRestController<TId, TEntity, TDto, TDao, TDtoMapper>> logger, TDao dao, TDtoMapper mapper, IRestPaginatorService restPaginatorService)
        {
            Logger = logger;
            Dao = dao;
            DtoMapper = mapper;
            RestPaginator = restPaginatorService;
        }

        public virtual async Task<TDto> Insert([FromBody] TDto dto)
        {
            var entity = await Dao.Insert(DtoMapper.ToEntity(dto));
            return DtoMapper.ToDto(entity);
        }

        public virtual async Task<bool> DeleteById(TId id)
        {
            return await Dao.Delete(id);
        }

        public virtual async Task<PaginationObject<TDto>> ListPaginate([FromQuery] int page, [FromQuery] int pageSize = 20)
        {
            return await RestPaginator.Paginate<TId, TEntity, TDto, TDtoMapper>(await Dao.FindAll(), page,
                 pageSize, DtoMapper);
        }

        public virtual async Task<TDto> Update([FromBody] TDto dto)
        {
            var entity = await Dao.Update(DtoMapper.ToEntity(dto));

            return DtoMapper.ToDto(entity);
        }

        public virtual async Task<List<TDto>> ListAll()
        {
            return DtoMapper.ToDto(await Dao.FindAll());
        }
    }
}
