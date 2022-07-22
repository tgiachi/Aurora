using Aurora.Api.Entities.Interfaces.Dto;
using Aurora.Api.Entities.Interfaces.Entities;
using AutoMapper;

namespace Aurora.Api.Entities.Impl.Dto
{
    public class AbstractDtoMapper<TId, TEntity, TDto> : IDtoMapper<TId, TEntity, TDto>
        where TEntity : IBaseEntity<TId> where TDto : IBaseDto<TId>
    {
        private readonly IMapper _mapper;

        public AbstractDtoMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<TDto> ToDto(List<TEntity> entities)
        {
            return entities.Select(ToDto).ToList();
        }

        public TDto ToDto(TEntity entity)
        {
            return _mapper.Map<TEntity, TDto>(entity);
        }

        public List<TEntity> ToEntity(List<TDto> dto)
        {
            return dto.Select(ToEntity).ToList();
        }

        public TEntity ToEntity(TDto dto)
        {
            return _mapper.Map<TEntity>(dto);
        }
    }
}
