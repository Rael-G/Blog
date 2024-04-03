namespace Blog.Application;

public interface IBaseService<TDto> 
{
    /// <summary>
    /// Creates a new <see cref="TDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="TDto"/> to create.</param>
    void Create(TDto entity);

    /// <summary>
    /// Updates an existing <see cref="TDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="TDto"/> to update.</param>
    void Update(TDto entity);

    /// <summary>
    /// Deletes an existing <see cref="TDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="TDto"/> to delete.</param>
    void Delete(TDto entity);

    /// <summary>
    /// Retrieves a <see cref="TDto"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="TDto"/> to retrieve.</param>
    /// <returns>The retrieved <see cref="TDto"/>, or null if not found.</returns>
    Task<TDto?> Get(Guid id);

    /// <summary>
    /// Retrieves all <see cref="TDto"/>s.
    /// </summary>
    /// <returns>A collection of <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>> GetAll();

    /// <summary>
    /// Save Changes
    /// </summary>
    Task Commit();
}
