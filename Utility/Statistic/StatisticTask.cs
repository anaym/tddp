namespace Utility.Statistic
{
    public class StatisticTask
    {
        public string FileSource { get; set; }
        public string FolderSource { get; set; }
        public string XmlSource { get; set; }
        public string CodePage { get; set; }
        public string AvaibleTypes { get; set; }
        public int Count { get; set; }
        public int MinWordLength { get; set; }
        public bool OnlyLetters { get; set; }
    }
}