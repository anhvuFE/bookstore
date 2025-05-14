using LibraryCore.Models;

namespace LibraryCore.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IList<Category> GetAllCategories();
    }
}
