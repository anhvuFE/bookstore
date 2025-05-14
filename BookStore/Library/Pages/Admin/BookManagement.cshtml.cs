using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Library.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class BookManagementModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookManagementModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Category> Categories { get; set; }
        public IList<LibraryCore.Models.Book> Books { get; set; }
        public IList<Author> Authors { get; set; }
        

        [BindProperty]
        public LibraryCore.Models.Book Book { get; set; }

        [Required]
        public IFormFile Image { get; set; }
        public void OnGet()
        {
            Categories = _unitOfWork.CategoryRepository.GetAll();
            Books = _unitOfWork.BookRepository.GetAll();
            Authors = _unitOfWork.AuthorRepository.GetAll();
        }

        public IActionResult OnPostEditBook()
        {
            try
            {
                LibraryCore.Models.Book b = _unitOfWork.BookRepository.Find(int.Parse(Request.Form["id"]));
                b.Title = Request.Form["title"];
                b.Description = Request.Form["description"];
                b.CategoryId = int.Parse(Request.Form["category"]);
                b.Publisher = Request.Form["publisher"];
                b.PublicationDate = DateTime.Parse(Request.Form["date"]);
                b.Quantity = int.Parse(Request.Form["quantity"]);
                var file = Request.Form.Files["newimage"];

                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName) + ".png";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "books", fileName);

                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);

                        using (var srcImage = System.Drawing.Image.FromStream(memoryStream))
                        using (var newImage = new Bitmap(500, 500))
                        using (var graphics = Graphics.FromImage(newImage))
                        {
                            graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphics.DrawImage(srcImage, new Rectangle(0, 0, 500, 500));

                            newImage.Save(filePath, ImageFormat.Png);
                        }
                    }

                    b.Image = "/img/books/" + fileName;
                }

                b.Rate = int.Parse(Request.Form["rate"]);
                b.Introduction = Request.Form["introduction"];
                _unitOfWork.BookRepository.Update(b);
                _unitOfWork.SaveChange();

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // Log the exception (this could be replaced with your preferred logging framework)
                Console.WriteLine(ex.Message);
                return RedirectToPage("Error"); // Redirect to an error page or handle accordingly
            }
        }


        public IActionResult OnPostAddBook()
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(Image.FileName) + ".png";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "books", fileName);

                using (var memoryStream = new MemoryStream())
                {
                    Image.CopyTo(memoryStream);

                    using (var srcImage = System.Drawing.Image.FromStream(memoryStream))
                    using (var newImage = new Bitmap(500, 500))
                    using (var graphics = Graphics.FromImage(newImage))
                    {
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(srcImage, new Rectangle(0, 0, 500, 500));

                        newImage.Save(filePath, ImageFormat.Png);
                    }
                }

                Book.Image = "/img/books/" + fileName;
                _unitOfWork.BookRepository.Add(Book);
                _unitOfWork.SaveChange();

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // Log the exception (this could be replaced with your preferred logging framework)
                Console.WriteLine(ex.Message);
                return RedirectToPage("Error"); // Redirect to an error page or handle accordingly
            }
        }

        public IActionResult OnGetExportFile()
        {
            var data = _unitOfWork.BookRepository.GetAll().ToList(); // Lấy dữ liệu từ Backend
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.WriteLine("BookId," + "Title," + "Publisher," + "PublicationDate," + "Quantity," + "Rate," + "Introduction");
            // Ghi dữ liệu vào tập tin
            foreach (var item in data)
            {
                streamWriter.WriteLine(item.BookId + "," + item.Title + "," + item.Publisher + "," + item.PublicationDate + "," + item.Quantity + "," + item.Rate+","+item.Introduction);
            }
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Trả về tập tin đã tạo
            return File(memoryStream, "text/csv", "Books.csv");
        }
    }
}
