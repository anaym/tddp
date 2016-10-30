using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Utility.Statistic
{
    public class Statistic : ReadOnlyDictionary<string, int>
    {
        public static Statistic FromTask(StatisticTask task)
        {
            IEnumerable<string> src = null;
            if (task.XmlSource != null) return FromXml(File.OpenText(task.XmlSource));
            if (task.FileSource != null) src = GetLines.FromFile(task.FileSource, task.CodePage);
            else if (task.FolderSource != null) src = GetLines.FromFolder(task.FolderSource, task.AvaibleTypes, task.CodePage);
            else src = GetLines.FromInputStream();
            var words = ExtractWords(src, task.OnlyLetters);
            var statistic = CreateStatistic(words, task.MinWordLength);
            return new Statistic(statistic, task.Count);
        }
        
        public static Statistic FromXml(StreamReader input)
        {
            var serializer = new XmlSerializer(typeof(Item[]));
            var src = (Item[])serializer.Deserialize(new XmlTextReader(input));
            return new Statistic(src.Select(Item.ToKeyValuePair), int.MaxValue);
        }

        public Statistic(IEnumerable<KeyValuePair<string, int>> source, int count)  
            : base(source.OrderByDescending(p => p.Value).Take(count).ToDictionary())
        { }

        public void ToXml(StreamWriter outputStream)
        {
            var serializer = new XmlSerializer(typeof(Item[]));
            serializer.Serialize(outputStream, this.Select(Item.FromKeyValuePair).ToArray());
        }

        private static IEnumerable<string> ExtractWords(IEnumerable<string> lines, bool onlyLetters)
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

        private static IEnumerable<KeyValuePair<string, int>> CreateStatistic(IEnumerable<string> words, int minLength)
        {
            var stat = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!stat.ContainsKey(word)) stat[word] = 0;
                stat[word]++;
            }
            return stat.Where(p => p.Key.Length >= minLength);
        }
    }
    public class Item
    {
        [XmlElement("Key")]
        public string Key;
        [XmlElement("Value")]
        public int Value;

        public Item()
            : this(null, 0)
        { }

        public Item(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public static KeyValuePair<string, int> ToKeyValuePair(Item item) => new KeyValuePair<string, int>(item.Key, item.Value);
        public static Item FromKeyValuePair(KeyValuePair<string, int> pair) => new Item(pair.Key, pair.Value);
    }
} 