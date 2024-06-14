using SpellChecker.ConsoleApplication;
using System.Globalization;
namespace SpellChecker.Tests {
    [TestFixture]
    public class SpellCheckerTests {

        [Test]
        public void ParserTest() {
            var input = "rain spain plain plaint pain main mainly\r\nthe in on fall falls his was\r\n===\r\nhte rame in pain fells\r\nmainy oon teh lain\r\nwas hints pliant\r\n===";
            var actual = InputParser.Parse(input);
            var expected = (
                new HashSet<string>() { "rain", "spain", "plain", "plaint", "pain", "main", "mainly", "the", "in", "on", "fall", "falls", "his", "was" },
                new string[] { "hte", "rame", "in", "pain", "fells", "mainy", "oon", "teh", "lain", "was", "hints", "pliant" }
                );
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("the", "hte", 2)] // delete + insert
        [TestCase("fells", "falls", 2)] // delete + insert
        [TestCase("oon", "on", 1)] // delete
        [TestCase("main", "mainy", 1)] // insert
        [TestCase("Word", "word", 0)] // case-insensetivity checking
        public static void EditsCountTest1(string first, string second, int expected) {
            var actual = ConsoleApplication.SpellChecker.GetEditsCount(first, second);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("hints", "his", 2)] // delete + delete
        [TestCase("abc", "abcde", 2)] // insert + insert
        public static void EditsCountTest2(string first, string second, int expected) {
            var actual = ConsoleApplication.SpellChecker.GetEditsCount(first, second);
            Assert.That(actual, !Is.EqualTo(expected));
        }

        [Test]
        public static void SpellCheckerTest() {
            var spellChecker = new ConsoleApplication.SpellChecker(
                new HashSet<string>() { "rain", "spain", "plain", "plaint", "pain", "main", "mainly", "the", "in", "on", "fall", "falls", "his", "was" }
                );
            var text = new string[] { "hte", "rame", "in", "pain", "fells", "mainy", "oon", "teh", "lain", "was", "hints", "pliant" };
            var actualOutput = spellChecker.GetCorrectText(text);
            var expectedOutput = "the {rame?} in pain falls {main mainly} on the plain was {hints?} plaint";
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}