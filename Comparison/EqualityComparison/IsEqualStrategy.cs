using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Comparison.EqualityComparison
{
    public class IsEqualStrategy : IComparisonStrategy
    {
        public Expression first { get; set; }
        public Expression second { get; set; }
        public int firstStartIndex { get; set; }
        public int secondStartIndex { get; set; }

        public IsEqualStrategy(Expression first, Expression second = null, int? firstStartIndex = null, int? secondStartIndex = null)
        {
            if ((second == null && (firstStartIndex == null || secondStartIndex == null)) ||
                (second != null && ((firstStartIndex == null && secondStartIndex != null) || (firstStartIndex != null && secondStartIndex == null))))
            {
                throw new Exception("Not well formed");
            }
            this.first = first;

            this.second = second == null ? first : second;

            this.firstStartIndex = firstStartIndex == null ? (int) first.GetRoot() : (int) firstStartIndex;

            this.secondStartIndex = secondStartIndex == null ? (int) second.GetRoot() : (int) secondStartIndex;
        }
        public bool Compare()
        {
            return IsEqual();
        }
        public bool IsEqual()
        {
            return IsEqual(firstStartIndex, secondStartIndex);
        }
        public bool IsEqual(int firstIndex, int secondIndex)
        {
            if (first.GetNode(firstIndex).GetValue() == second.GetNode(secondIndex).GetValue())
            {
                List<int> firstChildren = first.GetChildren(firstIndex);

                List<int> secondChildren = second.GetChildren(secondIndex);

                if (firstChildren.Count != secondChildren.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < firstChildren.Count; i ++)
                    {
                        if (!IsEqual(firstChildren[i], secondChildren[i]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}