using LibraryCore.Repositories;

namespace LibraryCore.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository BookRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        IUserRepository UserRepository { get; }

        ICartRepository CartRepository { get; }

        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IAuthorRepository AuthorRepository { get; }

        void SaveChange();

    }
}
