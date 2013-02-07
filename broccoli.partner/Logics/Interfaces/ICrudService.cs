namespace BroccoliTrade.Logics.Interfaces
{
    public interface ICrudService<TEntity>
    {
        /// <summary>
        /// Retrieve entity by id
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns></returns>
        TEntity GetById(long id);

        /// <summary>
        /// Marks object for deletion and calls SubmitChanges
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        
        /// <summary>
        /// Inserts new entity into database
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);
    }
}
