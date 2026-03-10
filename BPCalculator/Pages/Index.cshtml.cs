using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        [BindProperty]
        public BloodPressure BP { get; set; }

        // setup initial data on page load
        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 120, Diastolic = 80 };
        }

        // handle form submission
        public IActionResult OnPost()
        {
            // check range validation from [Range] attributes first
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // extra validation — systolic must be greater than diastolic
            if (!BP.IsValid())
            {
                ModelState.AddModelError(string.Empty,
                    "Systolic pressure must be greater than Diastolic pressure.");
                return Page();
            }

            return Page();
        }
    }
}