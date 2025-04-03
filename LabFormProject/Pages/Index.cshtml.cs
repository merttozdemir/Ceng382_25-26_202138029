using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Database.Models;

namespace LabFormProject.Pages
{
    public class IndexModel : PageModel
    {
        
        [BindProperty]
        public ClassInformationModel ClassInfo { get; set; } = new();
        public static List<ClassInformationModel> ClassList { get; set; } = new();

        public void OnGet() { }

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
                ClassInfo.DeclareID();
            }
            return RedirectToPage();
        }
    }
}
