using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BPCalculator.BDDTests.StepDefinitions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.Name ?? enumValue.ToString();
        }
    }
}
