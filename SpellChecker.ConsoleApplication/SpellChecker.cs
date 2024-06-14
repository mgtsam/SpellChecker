using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker.ConsoleApplication {
    public class SpellChecker {
        private readonly HashSet<string> _dictionary = null!;
        public SpellChecker(HashSet<string> dictionary) { 
            _dictionary = dictionary;
        }
        public string GetCorrectText(string[] text) {
            var sb = new StringBuilder();
            foreach (var word in text) {
                if (_dictionary.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase))) // если слово присутствует в словаре
                    sb.Append($"{word} ");

                else {
                    var keysAndEdits = new Dictionary<string, int>();
                    var minEditsCount = 2;
                    foreach (var key in _dictionary) {
                        var editsCount = GetEditsCount(word, key);

                        if (minEditsCount > editsCount)
                            minEditsCount = editsCount;

                        if (editsCount == minEditsCount)
                            keysAndEdits.Add(key, editsCount);
                    }

                    var keys = keysAndEdits
                        .Where(kd => kd.Value == minEditsCount)
                        .Select(kd => kd.Key)
                        .ToList();

                    if (keys.Count == 0) // если в словаре нет достаточно близкого слова
                        sb.Append($"{{{word}?}} ");
                    else if (keys.Count == 1) // если в словаре есть 1 достаточно близкое слово
                        sb.Append($"{keys.Single()} ");
                    else { // если в словаре несколько равноблизких слов
                        sb.Append('{');
                        for (var i = 0; i < keys.Count; i++) {
                            sb.Append($"{keys[i]}");
                            if (i != keys.Count - 1)
                                sb.Append(' ');
                        }
                        sb.Append("} ");
                    }
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static int GetEditsCount(string first, string second) { // нахождение количества необходимых изменений по алгоритму поиска расстояния Левенштейна
            var maxCost = first.Length + second.Length + 1;
            var opt = new int[first.Length + 1, second.Length + 1];
            for (var i = 0; i <= first.Length; ++i)
                opt[i, 0] = i;
            for (var i = 0; i <= second.Length; ++i)
                opt[0, i] = i;
            for (var i = 1; i <= first.Length; ++i)
                for (var j = 1; j <= second.Length; ++j) {
                    if (char.ToLower(first[i - 1]).Equals(char.ToLower(second[j - 1])))
                        opt[i, j] = opt[i - 1, j - 1];
                    else {
                        int deleteCost = 1 + opt[i - 1, j];
                        int insertCost = 1 + opt[i, j - 1];

                        if (i > 1 && opt[i - 1, j] == 1 + opt[i - 2, j]) // если удаление второе подряд
                            deleteCost = maxCost;
                        if (j > 1 && opt[i, j - 1] == 1 + opt[i, j - 2]) // если вставка вторая подряд
                            insertCost = maxCost;

                        opt[i, j] = Math.Min(deleteCost, insertCost);
                    }
                }
            return opt[first.Length, second.Length];
        }
    }
}
