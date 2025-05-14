using System.ComponentModel.DataAnnotations;
using System.Text;
using Humanizer.Localisation.TimeToClockNotation;
using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class CategoryManagementModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManagementModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string mess { get; set; }
        public IList<Category> Categories { get; set; }
        [BindProperty]
        public Category Category { get; set; }

        [Required]
        public IFormFile Image { get; set; }
        public void OnGet()
        {
            Categories = _unitOfWork.CategoryRepository.GetAll().ToList();

        }

        public IActionResult OnPostEditCategory()
        {
            Category c = _unitOfWork.CategoryRepository.Find(int.Parse(Request.Form["id"]));
            c.CategoryName = Request.Form["name"];
            var file = Request.Form.Files["newimage"];

            if (file != null && file.Length > 0)
            {
                var fileName = file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "category", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                c.Image = "/img/category/" + fileName;

            }
            _unitOfWork.CategoryRepository.Update(c);
            _unitOfWork.SaveChange();
            return RedirectToPage();
        }

        public IActionResult OnPostAddCategory()
        {
            var fileName = Image.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "category", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Image.CopyTo(stream);
            }
            Category.Image = "/img/category/" + fileName;
            _unitOfWork.CategoryRepository.Add(Category);
            _unitOfWork.SaveChange();
            return RedirectToPage();
        }

        public IActionResult OnGetDeleteCategory(int categoryId)
        {
            try
            {
                _unitOfWork.CategoryRepository.Delete(categoryId);
                _unitOfWork.SaveChange();
                return new JsonResult("1");

            }
            catch (Exception e)
            {
                return new JsonResult("0");

            }

        }
        public IActionResult OnGetExportFile()
        {
            var data = _unitOfWork.CategoryRepository.GetAll().ToList(); // Lấy dữ liệu từ Backend
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            streamWriter.WriteLine("CategoryId," + "CategoryName");
            // Ghi dữ liệu vào tập tin
            foreach (var item in data)
            {
                streamWriter.WriteLine(item.CategoryId + "," + item.CategoryName);
            }
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Trả về tập tin đã tạo
            return File(memoryStream, "text/csv", "categories.csv");
        }
        public string ImportMessage { get; set; }

        public async Task<IActionResult> OnPostImportFile(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ImportMessage = "No file selected.";
                return RedirectToPage();
            }

            using (var reader = new StreamReader(csvFile.OpenReadStream(), Encoding.UTF8))
            {
                // Đọc dữ liệu từ tập tin CSV
                var csvData = await reader.ReadToEndAsync();
                List<Category> chiatay = new List<Category>();
                List<Category> all = _unitOfWork.CategoryRepository.GetAll().ToList();
                var lines = csvData.Split("\n");
                for (int i = 1; i < lines.Length; i++)
                {
                    Category c;
                    var line = lines[i].Trim();
                    if (line == "")
                    {
                        break;
                    }
                    var value = line.Split(',');
                    if (all.FirstOrDefault(x => x.CategoryId == int.Parse(value[0])) == null)
                    {
                        c = new Category
                        {
                            CategoryName = value[1],
                            Image = ""
                        };
                        _unitOfWork.CategoryRepository.Add(c);
                        _unitOfWork.SaveChange();
                    }
                }

                ImportMessage = "File imported successfully.";
            }

            return RedirectToPage();
        }
    }
}
