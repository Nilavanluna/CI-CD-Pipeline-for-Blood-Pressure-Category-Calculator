using System;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    public enum BPCategory
    {
        [Display(Name = "Low Blood Pressure")]      Low,
        [Display(Name = "Ideal Blood Pressure")]    Ideal,
        [Display(Name = "Pre-High Blood Pressure")] PreHigh,
        [Display(Name = "High Blood Pressure")]     High
    };

    public class BloodPressure
    {
        public const int SystolicMin  = 70;
        public const int SystolicMax  = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax,
            ErrorMessage = "Systolic must be between 70 and 190")]
        public int Systolic { get; set; }

        [Range(DiastolicMin, DiastolicMax,
            ErrorMessage = "Diastolic must be between 40 and 100")]
        public int Diastolic { get; set; }

        // returns true only when systolic is strictly greater than diastolic
        public bool IsValid() => Systolic > Diastolic;

        // calculate BP category
        public BPCategory Category
        {
            get
            {
                if (Systolic < 90 || Diastolic < 60)
                    return BPCategory.Low;

                if (Systolic < 120 && Diastolic < 80)
                    return BPCategory.Ideal;

                if (Systolic < 140 && Diastolic < 90)
                    return BPCategory.PreHigh;

                return BPCategory.High;
            }
        }

        // NEW FEATURE — health recommendation (max 30 lines)
        public string Recommendation
        {
            get
            {
                return Category switch
                {
                    BPCategory.Low     => "Your BP is low. Consider increasing fluid and salt intake. Consult your GP if you feel dizzy or faint.",
                    BPCategory.Ideal   => "Your BP is ideal. Keep up your healthy lifestyle — regular exercise and a balanced diet.",
                    BPCategory.PreHigh => "Your BP is slightly elevated. Reduce salt intake, exercise regularly, and monitor it closely.",
                    BPCategory.High    => "Your BP is high. Please consult a doctor as soon as possible.",
                    _                  => string.Empty
                };
            }
        }
    }
}