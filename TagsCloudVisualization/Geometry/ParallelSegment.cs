﻿using System;

namespace TagsCloudVisualization.Geometry
{
    // CR (krait): 
    // Стоит сделать ParallelSegment структурой:
    // 1. Все поля immutable.
    // 2. Суммарный размер полей всего 8 байт, копировать её будет дёшево.

    public class ParallelSegment : IEquatable<ParallelSegment>
    {
        public readonly int Left;
        public readonly int Right;

        public ParallelSegment(int left, int right)
        {
            this.Left = Math.Min(left, right);
            this.Right = Math.Max(left, right);
        }

        public bool Contains(int n, bool includeBorders = true)
        {
            return includeBorders ? (n >= Left && n <= Right) : (n > Left && n < Right);
        }

        public bool IsIntersected(ParallelSegment other, bool includeBorder = true)
        {
            // CR (krait): Зачем проверять эти условия до проверки Equals(other)? Если она пройдёт, считать их будет без надобности.
            var a = Contains(other.Left, includeBorder) || Contains(other.Right, includeBorder);
            var b = other.Contains(Left, includeBorder) || other.Contains(Right, includeBorder);
            return Equals(other) || a || b;
        }
        
        public override int GetHashCode() => Left ^ Right;
        public bool Equals(ParallelSegment other) => other != null && Left == other.Left && Right == other.Right;
        public override bool Equals(object obj) => Equals(obj as ParallelSegment);
        public override string ToString() => $"[{Left}, {Right}]";
    }
}
