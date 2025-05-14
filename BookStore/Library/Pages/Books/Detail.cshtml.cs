using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages
{
	public class DetailModel : PageModel
	{
		public LibraryCore.Models.Book Book { get; set; }
		public IList<LibraryCore.Models.Book> BooksByCategory { get; set; }
		private readonly IUnitOfWork _unitOfWork;
		public LibraryCore.Models.Category Categories { get; set; }
		public LibraryCore.Models.Author Authors { get; set; }
		public IList<Category> ListCategories { get; set; }
		public int SelectedCategoryIds { get; set; }
		public DetailModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public void OnGet(int id)
		{
			Book = _unitOfWork.BookRepository.Find(id);
			Categories = _unitOfWork.CategoryRepository.Find(Book.CategoryId);
			Authors = _unitOfWork.AuthorRepository.Find(Book.AuthorId);
			ListCategories = _unitOfWork.CategoryRepository.GetAll();
			SelectedCategoryIds = Book.CategoryId;

			if (Book != null)
			{
				BooksByCategory = _unitOfWork.BookRepository.GetAll().Where(x => x.CategoryId == Book.CategoryId
																			&& x.BookId != Book.BookId
																			&& x.Status.Value).OrderByDescending(x => x.BookId).ToList();

				//BooksByCategory = _unitOfWork.BookRepository.GetAll().Where(x => x.AuthorId == Authors.AuthorId).ToList();
				ViewData["Title"] = Book.Title;
			}

		}
	}
}
