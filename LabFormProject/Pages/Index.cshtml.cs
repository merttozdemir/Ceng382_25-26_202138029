using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using LabFormProject.Data;
using LabFormProject.Models;

//I took lots of the code from Chatgpt
namespace LabFormProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        public int TotalPages { get; set; }

        [BindProperty]
        public ClassInformationModel ClassInfo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterClassName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public List<ClassInformationModel> FilteredTableData { get; set; } = new();



        public async Task<IActionResult> OnGetAsync(string? filterClassName, int pageNumber = 1, int pageSize = 10)
        {

            FilterClassName = filterClassName ?? string.Empty;
            PageNumber = pageNumber;
            PageSize = pageSize;

            var query = _context.ClassInformationTable.AsQueryable();

            if (!string.IsNullOrWhiteSpace(FilterClassName))
            {
                query = query.Where(c => c.ClassName.Contains(FilterClassName));
            }

            var totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            FilteredTableData = await query
                .OrderBy(c => c.ID)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ClassInformationTable.Add(ClassInfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int ID)
        {
            var item = await _context.ClassInformationTable.FindAsync(ID);
            if (item != null)
            {
                _context.ClassInformationTable.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostExportJsonAsync(string mode, List<string>? SelectedColumns)
        {
            IQueryable<ClassInformationModel> query = _context.ClassInformationTable;

            if (mode == "filtered" && !string.IsNullOrWhiteSpace(FilterClassName))
            {
                query = query.Where(c => c.ClassName.Contains(FilterClassName));
            }

            var exportData = await query.ToListAsync();

            var tableFormat = exportData.Select(c => new ClassInformationModel
            {
                ID = c.ID,
                ClassName = c.ClassName,
                StudentCount = c.StudentCount,
                Description = c.Description,
                IsActive = c.IsActive
            }).ToList();

            string json = Utils.Instance.ExportToJson(tableFormat, SelectedColumns);
            return File(Encoding.UTF8.GetBytes(json), "application/json", "export.json");
        }
    }
}