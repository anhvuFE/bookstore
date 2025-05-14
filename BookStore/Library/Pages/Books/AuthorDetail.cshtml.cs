using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Books
{
    public class AuthorDetailModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorDetailModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public LibraryCore.Models.Author author { get; set; } = new LibraryCore.Models.Author();
        public void OnGet(int id = 0)
        {
            author = _unitOfWork.AuthorRepository.Find(id);
            if (author != null)
            {
                // Assuming there's a method to get the number of books written by the author
                author.Books = _unitOfWork.BookRepository.GetAll().Where(x=>x.AuthorId == id).ToList();
            }
        }
    }
}
