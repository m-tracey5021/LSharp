using System;
using LSharp.Symbols;

namespace LSharp.Comparison
{
    public interface IComparisonStrategy
    {
        bool Compare();
    }
}