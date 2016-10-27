using System;

namespace TagsCloudVisualization.Utility
{
    public static class LazyHash
    {
        public static int GetHashCode(Object a, Object b) => GetHashCode(a.GetHashCode(), b.GetHashCode());

        // !CR (krait): 
        // Да, но нет. Когда ты передаёшь сюда int, происходит боксинг и создаётся лишний объект. В такой часто вызываемой функции как GetHashCode это не очень здорово.
        // Раз уж написал экстеншен, придумай, как его переделать без боксинга.
        // Про боксинг можно почитать тут: https://msdn.microsoft.com/en-us/library/yz2be5wk.aspx
        public static int GetHashCode(int a, int b)
        {
            unchecked
            {
                return (a << 16) ^ b;
            }
        }
    }
}
