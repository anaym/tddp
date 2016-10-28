using System;

namespace TagsCloudVisualization.Utility
{
    // !CR (krait): Ну нет, так не пойдёт. Теперь что ли на каждый новый тип аргументов по перегрузке писать?
    public static class LazyHash
    {
        public static int GetHashCode<TA, TB>(TA a, TB b) => GetHashCode(a.GetHashCode(), b.GetHashCode());

        public static int GetHashCode(int a, int b)
        {
            unchecked
            {
                return (a << 16) ^ b;
            }
        }
    }
}
