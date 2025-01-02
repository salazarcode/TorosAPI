namespace Domain.Core.Interfaces.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Crea y persiste la entidad.
        /// </summary>
        /// <param name="entity">Entidad a crear.</param>
        /// <returns>La entidad creada, o null si algo falla.</returns>
        Task<TEntity?> Create(TEntity entity);

        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        Task<TEntity?> Get(string id);

        /// <summary>
        /// Obtiene todas las entidades de este tipo.
        /// </summary>
        /// <returns>Un enumerable con todas las entidades.</returns>
        Task<IEnumerable<TEntity>> GetAll();

        /// <summary>
        /// Actualiza la entidad.
        /// </summary>
        /// <param name="entity">Entidad con los datos a actualizar.</param>
        /// <returns>La entidad actualizada o null si no se encuentra.</returns>
        Task<TEntity?> Update(TEntity entity);

        /// <summary>
        /// Elimina una entidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>True si se elimina satisfactoriamente, false en caso contrario.</returns>
        Task<bool> Delete(string id);

        /// <summary>
        /// Elimina la entidad suministrada.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        /// <returns>True si se elimina satisfactoriamente, false en caso contrario.</returns>
        Task<bool> Delete(TEntity entity);
    }
}
