using LibraryCore.Models;
using LibraryCore.Repositories;

namespace LibraryCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly LibraryDBContext context;

        public UnitOfWork(LibraryDBContext context = null)
        {
            this.context = context ?? new LibraryDBContext();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    Console.WriteLine("Dispose");

                }
            }
            this.disposed = true;
        }

        private ICategoryRepository _categoryRepository;
        private IBookRepository _bookRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(context);
        private IUserRepository _userRepository { get; }
        private IOrderRepository _orderRepository { get; }
        public IBookRepository BookRepository => _bookRepository ?? new BookRepository(context);
        public IUserRepository UserRepository => _userRepository ?? new UserRepository(context);
        public IOrderRepository OrderRepository => _orderRepository ?? new OrderRepository(context);

        private ICartRepository _cartRepository { get; }
        private IOrderDetailRepository _orderDetailRepository { get; }
        private IAuthorRepository _authorRepository { get; }
        public IAuthorRepository AuthorRepository => _authorRepository ?? new AuthorRepository(context);
        public ICartRepository CartRepository => _cartRepository ?? new CartRepository(context);
        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository ?? new OrderDetailRepository(context);
        public void SaveChange()
        {
            context.SaveChanges();
        }

    }
}
