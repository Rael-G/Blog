namespace Blog.Domain;

public interface IBaseRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    void Create(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Deletes an existing entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The retrieved entity, or null if not found.</returns>
    Task<T?> Get(Guid id);

    /// <summary>
    /// Retrieves all entities of type T.
    /// </summary>
    /// <returns>A collection of entities of type T.</returns>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    /// Commits any pending changes to the underlying data store.
    /// </summary>
    Task Commit();
}
