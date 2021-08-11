using System;
using System.Collections.Generic;

namespace LSharp.Symbols
{
    public class Operation : Symbol
    {
        private readonly Dictionary<SymbolType, char> symbolMap = new Dictionary<SymbolType, char>()
        {
            { SymbolType.Summation, '+' },
            { SymbolType.Multiplication, '*' },
            { SymbolType.Division, '/' },
            { SymbolType.Exponent, '^' },
            { SymbolType.Radical, 'v' }
        };
        public Operation(bool sign, SymbolType type) : base(sign, type){ }
        public override string GetValue()
        {
            return symbolMap[type].ToString();
        }
        public override int? GetNumericValue()
        {
            return null;
        }
        public override Symbol Copy()
        {
            return new Operation(sign, type);
        }
    }
}