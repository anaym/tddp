using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Statistic
{
    public static class Statistic
    {
        public static IEnumerable<string> ExtractWords(this IEnumerable<string> lines, bool onlyLetters)
        {
            var separators = new[]
            {
                '.', ',', '!', '?', ':', ';',
                '[', ']', '{', '}', '(', ')', '<', '>',
                '-', '_', '=', '+', '^',
                '@', '#', '$', '%', '^', '&', '*',
                '\"', '\'', '~', '`',
                '\\', '|', '/',
                '\t', ' ', '\r', '\n'
            };
            var words = lines.SelectMany(l => l.Split(separators, StringSplitOptions.RemoveEmptyEntries));
            if (onlyLetters) words = words.Where(w => w.All(char.IsLetter));
            return words.Where(w => !String.IsNullOrWhiteSpace(w));
        }

        public static IEnumerable<KeyValuePair<string, int>> CreateStatistic(this IEnumerable<string> words, int count, int minLength)
        {
            var stat = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!stat.ContainsKey(word)) stat[word] = 0;
                stat[word]++;
            }
            return stat
                .Where(p => p.Key.Length >= minLength)
                .OrderByDescending(p => p.Value)
                .Take(count);
        }
    }
}