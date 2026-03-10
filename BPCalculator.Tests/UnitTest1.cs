using Moq;
using BPCalculator;
using BPCalculator.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {
        // ── CATEGORY TESTS ──────────────────────────────────────────

        [Theory]
        [InlineData(85, 55, BPCategory.Low)]      // normal low
        [InlineData(89, 59, BPCategory.Low)]      // boundary — just below ideal
        [InlineData(80, 60, BPCategory.Low)]      // systolic low even if diastolic ok
        public void Category_ReturnsLow_WhenBPIsLow(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            bp.Category.Should().Be(expected);
        }

        [Theory]
        [InlineData(90,  60,  BPCategory.Ideal)]  // boundary — lower limit inclusive
        [InlineData(110, 70,  BPCategory.Ideal)]  // normal ideal
        [InlineData(119, 79,  BPCategory.Ideal)]  // boundary — just below pre-high
        public void Category_ReturnsIdeal_WhenBPIsIdeal(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            bp.Category.Should().Be(expected);
        }

        [Theory]
        [InlineData(120, 80,  BPCategory.PreHigh)] // boundary — lower limit inclusive
        [InlineData(130, 85,  BPCategory.PreHigh)] // normal pre-high
        [InlineData(139, 89,  BPCategory.PreHigh)] // boundary — just below high
        public void Category_ReturnsPreHigh_WhenBPIsPreHigh(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            bp.Category.Should().Be(expected);
        }

        [Theory]
        [InlineData(140, 90,  BPCategory.High)]   // boundary — lower limit inclusive
        [InlineData(160, 95,  BPCategory.High)]   // normal high
        [InlineData(180, 100, BPCategory.High)]   // maximum valid values
        public void Category_ReturnsHigh_WhenBPIsHigh(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            bp.Category.Should().Be(expected);
        }

        // ── ISVALID TESTS ────────────────────────────────────────────

        [Theory]
        [InlineData(120, 80)]   // normal valid
        [InlineData(90,  60)]   // boundary valid
        [InlineData(150, 95)]   // high but valid
        public void IsValid_ReturnsTrue_WhenSystolicGreaterThanDiastolic(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            bp.IsValid().Should().BeTrue();
        }

        [Theory]
        [InlineData(80,  90)]   // diastolic higher — invalid
        [InlineData(70,  70)]   // equal — invalid
        public void IsValid_ReturnsFalse_WhenSystolicNotGreaterThanDiastolic(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            bp.IsValid().Should().BeFalse();
        }

        // ── RECOMMENDATION TESTS ─────────────────────────────────────

        [Fact]
        public void Recommendation_IsNotEmpty_ForAllCategories()
        {
            var readings = new[]
            {
                new BloodPressure { Systolic = 85,  Diastolic = 55 },
                new BloodPressure { Systolic = 110, Diastolic = 70 },
                new BloodPressure { Systolic = 130, Diastolic = 85 },
                new BloodPressure { Systolic = 150, Diastolic = 95 },
            };

            foreach (var bp in readings)
            {
                bp.Recommendation.Should().NotBeNullOrEmpty(
                    because: $"category {bp.Category} should always have a recommendation");
            }
        }

        [Fact]
        public void Recommendation_IsDifferent_ForEachCategory()
        {
            var low     = new BloodPressure { Systolic = 85,  Diastolic = 55 };
            var ideal   = new BloodPressure { Systolic = 110, Diastolic = 70 };
            var preHigh = new BloodPressure { Systolic = 130, Diastolic = 85 };
            var high    = new BloodPressure { Systolic = 150, Diastolic = 95 };

            var recommendations = new[]
            {
                low.Recommendation,
                ideal.Recommendation,
                preHigh.Recommendation,
                high.Recommendation
            };

            recommendations.Should().OnlyHaveUniqueItems(
                because: "each category should return a distinct recommendation");
        }

        [Theory]
        [InlineData(85,  55,  "low")]
        [InlineData(110, 70,  "ideal")]
        [InlineData(130, 85,  "elevated")]
        [InlineData(150, 95,  "high")]
        public void Recommendation_ContainsExpectedKeyword(int systolic, int diastolic, string keyword)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            bp.Recommendation.ToLower().Should().Contain(keyword);
        }

        // ── PAGE MODEL TESTS ──────────────────────────────────────────

        private BloodPressureModel CreateModel()
{
    var model       = new BloodPressureModel();
    var httpContext = new DefaultHttpContext();
    var modelState  = new ModelStateDictionary();
    var actionContext = new ActionContext(
        httpContext,
        new RouteData(),
        new PageActionDescriptor(),
        modelState);
    var modelMetadata = new EmptyModelMetadataProvider();
    var viewData    = new ViewDataDictionary(modelMetadata, modelState);
    var tempData    = new TempDataDictionary(httpContext,
                        Mock.Of<ITempDataProvider>());

    model.PageContext = new PageContext(actionContext)
    {
        ViewData = viewData
    };
    model.TempData = tempData;

    return model;
}


        [Fact]
        public void OnGet_SetsDefaultBPValues()
        {
            var model = CreateModel();
            model.OnGet();

            model.BP.Should().NotBeNull();
            model.BP.Systolic.Should().Be(120);
            model.BP.Diastolic.Should().Be(80);
        }

        [Fact]
        public void OnPost_ReturnsPage_WhenBPIsValid()
        {
            var model = CreateModel();
            model.BP = new BloodPressure { Systolic = 120, Diastolic = 80 };

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public void OnPost_AddsModelError_WhenSystolicNotGreaterThanDiastolic()
        {
            var model = CreateModel();
            model.BP = new BloodPressure { Systolic = 80, Diastolic = 90 };

            model.OnPost();

            model.ModelState.IsValid.Should().BeFalse();
            model.ModelState[string.Empty]!.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void OnPost_AddsModelError_WhenSystolicEqualsDiastolic()
        {
            var model = CreateModel();
            model.BP = new BloodPressure { Systolic = 80, Diastolic = 80 };

            model.OnPost();

            model.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public void OnPost_ReturnsPage_WhenHighBP()
        {
            var model = CreateModel();
            model.BP = new BloodPressure { Systolic = 160, Diastolic = 95 };

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.BP.Category.Should().Be(BPCategory.High);
        }

        [Fact]
        public void OnPost_ReturnsPage_WhenLowBP()
        {
            var model = CreateModel();
            model.BP = new BloodPressure { Systolic = 85, Diastolic = 55 };

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.BP.Category.Should().Be(BPCategory.Low);
        }
    }
}
