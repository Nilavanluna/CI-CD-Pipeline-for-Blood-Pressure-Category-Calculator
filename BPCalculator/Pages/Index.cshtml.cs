using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        private readonly TelemetryClient _telemetry;

        public BloodPressureModel(TelemetryClient telemetry)
        {
            _telemetry = telemetry;
        }

        [BindProperty]
        public BloodPressure BP { get; set; }

        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 120, Diastolic = 80 };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!BP.IsValid())
            {
                ModelState.AddModelError(string.Empty,
                    "Systolic pressure must be greater than Diastolic pressure.");
                return Page();
            }

            // Track telemetry event
            _telemetry.TrackEvent("BPCalculated", new Dictionary<string, string>
            {
                { "Category",  BP.Category.ToString() },
                { "Systolic",  BP.Systolic.ToString() },
                { "Diastolic", BP.Diastolic.ToString() }
            });

            _telemetry.TrackMetric("SystolicValue",  BP.Systolic);
            _telemetry.TrackMetric("DiastolicValue", BP.Diastolic);

            return Page();
        }
    }
}
