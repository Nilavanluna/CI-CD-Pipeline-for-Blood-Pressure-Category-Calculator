using BPCalculator;
using TechTalk.SpecFlow;
using Xunit;

namespace BPCalculator.BDDTests.StepDefinitions
{
    [Binding]
    public class BpSteps
    {
        private int _systolic;
        private int _diastolic;
        private string _category = string.Empty;
        private bool _isValid;

        [Given(@"the patient has a systolic pressure of (.*)")]
        public void GivenSystolic(int value)
        {
            _systolic = value;
        }

        [Given(@"the patient has a diastolic pressure of (.*)")]
        public void GivenDiastolic(int value)
        {
            _diastolic = value;
        }

        [When(@"the blood pressure category is calculated")]
        public void WhenCategoryCalculated()
        {
            var bp = new BloodPressure { Systolic = _systolic, Diastolic = _diastolic };
            _category = bp.Category.GetDisplayName();
        }

        [When(@"the blood pressure validity is checked")]
        public void WhenValidityChecked()
        {
            var bp = new BloodPressure { Systolic = _systolic, Diastolic = _diastolic };
            _isValid = bp.IsValid();
        }

        [Then(@"the category should be ""(.*)""")]
        public void ThenCategory(string expected)
        {
            Assert.Equal(expected, _category);
        }

        [Then(@"the reading should be invalid")]
        public void ThenInvalid()
        {
            Assert.False(_isValid);
        }
    }
}