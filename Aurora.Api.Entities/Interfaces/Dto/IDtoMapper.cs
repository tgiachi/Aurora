using Aurora.Api.Entities.Interfaces.Entities;

namespace Aurora.Api.Entities.Interfaces.Dto
{
    /// <summary>
    ///     Interface for create DTO Mapper
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public interface IDtoMapper<TId, TEntity, TDto> where TEntity : IBaseEntity<TId> where TDto : IBaseDto<TId>
    {
        /// <summary>
        ///     Transform list of entities in DTOs
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<TDto> ToDto(List<TEntity> entities);

        /// <summary>
        ///     Transform single entity in DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TDto ToDto(TEntity entity);

        /// <summary>
        ///     Transform List of DTOs in Entities
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        List<TEntity> ToEntity(List<TDto> dto);

        /// <summary>
        ///     Transform DTO in Entity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        TEntity ToEntity(TDto dto);
    }
}
