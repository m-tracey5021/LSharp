using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Comparison.EqualityComparison
{
    public class IsEqualByBaseStrategy : IComparisonStrategy
    {
        public Expression first { get; set; }
        public Expression second { get; set; }
        public int firstStartIndex { get; set; }
        public int secondStartIndex { get; set; }
        public IsEqualByBaseStrategy(Expression first, Expression second = null, int? firstStartIndex = null, int? secondStartIndex = null)
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
            return IsEqualByBase();
        }
        public bool IsEqualByBase()
        {
            return IsEqualByBase(firstStartIndex, secondStartIndex);
        }
        public bool IsEqualByBase(int firstIndex, int secondIndex)
        {
            Symbol firstSymbol = first.GetNode(firstIndex);

            Symbol secondSymbol = second.GetNode(secondIndex);
            
            if (firstSymbol.GetValue() == secondSymbol.GetValue())
            {
                if (first.IsExponent(firstIndex) && second.IsExponent(secondIndex))
                {
                    if (!first.Compare(ComparisonInstruction.IsEqual, second, first.GetChild(firstIndex, 0), second.GetChild(secondIndex, 0))) // ignore the exponents
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
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
                            if (!first.Compare(ComparisonInstruction.IsEqual, second, firstChildren[i], secondChildren[i]))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        
    }
}