using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IList<Category> Categories { get; set; }
        public IList<LibraryCore.Models.Book> Book { get; set; }
        public void OnGet()
        {
            Categories = _unitOfWork.CategoryRepository.GetAll();
            Book = _unitOfWork.BookRepository.GetAll().Where(x => x.Status.Value).OrderByDescending(x => x.BookId).Take(8).ToList();
        }
    }
}