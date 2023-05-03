namespace RestaurantManager.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T?> GetById(int id);
        Task<IEnumerable<T>> GetAll();

        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);

        Task<bool> SaveChanges();
    }
}