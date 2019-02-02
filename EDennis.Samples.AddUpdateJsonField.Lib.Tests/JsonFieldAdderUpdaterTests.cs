using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json.Linq;
using System.Text;

namespace EDennis.Samples.AddUpdateJsonField.Lib.Tests {
    public class JsonFieldAdderUpdaterTests {

        private readonly ITestOutputHelper _output;

        public JsonFieldAdderUpdaterTests(ITestOutputHelper output) {
            _output = output;
        }

        [Theory]
        [InlineData(1, "UserId", "admin")]
        [InlineData(2, "UserId", "admin")]
        public async Task AddUpdateFieldAsync(int testCase, string fieldName, string fieldValue) {

            var folder = $"TestCase{testCase}";
            var inputJson = File.ReadAllText($"{folder}\\input.json");

            var expectedJson = File.ReadAllText($"{folder}\\expectedOutput.json");
            var expectedJToken = JToken.Parse(expectedJson);
            var expected = expectedJToken.ToString();

            var actualJson = await JsonFieldAdderUpdater.AddUpdateFieldAsync(inputJson, fieldName, fieldValue);
            var actualJToken = JToken.Parse(actualJson);
            var actual = actualJToken.ToString();
            _output.WriteLine(expected);
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);

        }


        [Theory]
        [InlineData(1, "UserId", "admin")]
        [InlineData(2, "UserId", "admin")]
        public async Task AddUpdateFieldAsync_Stream(int testCase, string fieldName, string fieldValue) {

            var folder = $"TestCase{testCase}";
            var inputJson = File.ReadAllText($"{folder}\\input.json");

            var expectedJson = File.ReadAllText($"{folder}\\expectedOutput.json");
            var expectedJToken = JToken.Parse(expectedJson);
            var expected = expectedJToken.ToString();

            using (var output = new MemoryStream())
            using (var reader = new StreamReader(output))
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes(inputJson))) {
                await JsonFieldAdderUpdater.AddUpdateFieldAsync(output, input, fieldName, fieldValue);

                output.Position = 0; //rewind
                var actualJson = await reader.ReadToEndAsync();
                var actualJToken = JToken.Parse(actualJson);
                var actual = actualJToken.ToString();
                _output.WriteLine(expected);
                _output.WriteLine(actual);

                Assert.Equal(expected, actual);
            }



        }


    }
}
