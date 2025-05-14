using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Books
{
    public class AuthorsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IList<LibraryCore.Models.Author> authors { get; set; } = new List<LibraryCore.Models.Author>();
        public void OnGet()
        {
            authors = _unitOfWork.AuthorRepository.GetAll().OrderByDescending(x => x.AuthorId).ToList();
        }
    }
}
