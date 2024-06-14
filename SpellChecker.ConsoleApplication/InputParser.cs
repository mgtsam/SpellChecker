using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker.ConsoleApplication {
    public static class InputParser {
        public static (HashSet<string> dictionary, string[] text) Parse(string input) {
            var splitedInput = input.Split("===", StringSplitOptions.RemoveEmptyEntries);
            if (splitedInput.Length > 2 || splitedInput.Length < 1)
                throw new ArgumentException($"Некорректное количество блоков текста: {splitedInput.Length}.");
            return (ConvertToHashSet(splitedInput[0]), ConvertToArray(splitedInput[1]));
        }

        public static bool TryParse(string input, out (HashSet<string> dictionary, string[] text) dictionaryTextPair) {
            try {
                dictionaryTextPair = Parse(input);
            }
            catch (Exception) {
                dictionaryTextPair = default;
                return false;
            }
            return true;
        }

        private static readonly char[] _separators = { ' ', '\r', '\n' };

        public static string[] ConvertToArray(string text) {
            if (string.IsNullOrEmpty(text)) return Array.Empty<string>();
            return text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static HashSet<string> ConvertToHashSet(string text) {
            if (string.IsNullOrEmpty(text)) return new();
            return text.Split(_separators, StringSplitOptions.RemoveEmptyEntries).ToHashSet();
        }
    }
}
