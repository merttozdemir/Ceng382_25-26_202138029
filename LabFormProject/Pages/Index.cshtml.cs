using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Database.Models;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace LabFormProject.Pages
{
    /*I took most of the parts of this class from the chat gpt.*/
    public class IndexModel : PageModel
    {
        private void GenerateSampleData(int count)
        {
            var random = new Random();
            var subjects = new[] { "Math", "Science", "History", "English", "Art", "Music", "Physics", "Biology", "Chemistry", "Geography" };

            for (int i = 0; i < count; i++)
            {
                var className = $"{subjects[random.Next(subjects.Length)]} {random.Next(1, 10)}";
                var studentCount = random.Next(10, 35);
                var description = $"This is class {className} with {studentCount} students.";

                var newClass = new ClassInformationModel
                {
                    ClassName = className,
                    StudentCount = studentCount,
                    Description = description
                };
                newClass.SetID();
                ClassList.Add(newClass);
            }
        }
        
        public int TotalPages { get; set; }
        [BindProperty]
        public ClassInformationModel ClassInfo { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? FilterClassName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public List<ClassInformationTable> FilteredTableData { get; set; } = new();

        public static List<ClassInformationModel> ClassList { get; set; } = new();
        public void OnGet(string? filterClassName, int pageNumber = 1, int pageSize = 10) 
        {
            if (ClassList.Count == 0)
            {
                GenerateSampleData(100); 
            }

            FilterClassName = filterClassName ?? string.Empty;
            PageNumber = pageNumber;
            PageSize = pageSize;

            var filtered = string.IsNullOrWhiteSpace(FilterClassName)
                ? ClassList
                : ClassList.Where(c => c.ClassName != null && c.ClassName.Contains(FilterClassName, StringComparison.OrdinalIgnoreCase)).ToList();

            TotalPages = (int)Math.Ceiling(filtered.Count / (double)PageSize);

            FilteredTableData = filtered
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new ClassInformationTable
                {
                    ID = c.ID,
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                })
                .ToList();
        }

       public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ClassInfo.SetID();
            ClassList.Add(ClassInfo);

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int ID)
        {
            var item = ClassList.Find(x => x.ID == ID);
            if (item != null)
            {
                ClassList.Remove(item);
                ClassInfo.DeclareID(item.ID);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostExportJson(string mode, List<string>? SelectedColumns)
        {
            List<ClassInformationTable> exportData;

            if (mode == "filtered")
            {
                // Filtreyi yeniden uygula çünkü POST'ta FilteredTableData boş olur!
                var filtered = string.IsNullOrWhiteSpace(FilterClassName)
                    ? ClassList
                    : ClassList.Where(c => c.ClassName != null && c.ClassName.Contains(FilterClassName, StringComparison.OrdinalIgnoreCase)).ToList();

                exportData = filtered
                    .Select(c => new ClassInformationTable
                    {
                        ID = c.ID,
                        ClassName = c.ClassName,
                        StudentCount = c.StudentCount,
                        Description = c.Description
                    }).ToList();
            }
            else
            {
                exportData = ClassList
                    .Select(c => new ClassInformationTable
                    {
                        ID = c.ID,
                        ClassName = c.ClassName,
                        StudentCount = c.StudentCount,
                        Description = c.Description
                    }).ToList();
            }

            string json = Utils.Instance.ExportToJson(exportData, SelectedColumns);
            return File(System.Text.Encoding.UTF8.GetBytes(json), "application/json", "export.json");
        }
    }
}

