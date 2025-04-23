using Xunit;
using SportsPro.Models;

namespace SportsProTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("user@example.com", true)]
        [InlineData("invalid-email", false)]
        [InlineData("", false)]
        public void IsValidEmail_ValidatesCorrectly(string input, bool expected)
        {
            var result = Check.IsValidEmail(input);

            Assert.Equal(expected, result);
        }
    }
}
