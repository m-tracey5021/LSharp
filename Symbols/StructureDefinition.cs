using System;
using System.Collections.Generic;

namespace LSharp.Symbols
{
    public static class StructureDefinition
    {
        public static List<List<SymbolType>> canDistribute = new List<List<SymbolType>> 
        {
            new List<SymbolType> { SymbolType.Multiplication, SymbolType.Summation, SymbolType.Summation },
            new List<SymbolType> { SymbolType.Multiplication, SymbolType.Summation, SymbolType.Constant },
            new List<SymbolType> { SymbolType.Multiplication, SymbolType.Constant, SymbolType.Summation },
            new List<SymbolType> { SymbolType.Multiplication, SymbolType.Summation, SymbolType.Variable },
            new List<SymbolType> { SymbolType.Multiplication, SymbolType.Variable, SymbolType.Summation } 
        };
    }
}