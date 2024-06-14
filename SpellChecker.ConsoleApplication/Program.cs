namespace SpellChecker.ConsoleApplication {
    public class Program {
        static void Main(string[] args) {
            Console.WriteLine("Input:");
            //var input = InputParser.Parse(Console.ReadLine()!);
            if (InputParser.TryParse(Console.ReadLine()!, out var input)) {
                var spellChecker = new SpellChecker(input.dictionary);
                var output = spellChecker.GetCorrectText(input.text);
                Console.WriteLine($"Output:\n{output}");
            }
            else {
                Console.WriteLine("Parse error, wrong input.");
            }
        }
    }
}
