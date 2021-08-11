using System;
using System.Collections.Generic;
using LSharp.Rules;
using LSharp.Selectors;

namespace LSharp.Symbols
{
    public class Operation : Symbol
    {
        public Operation(bool sign, SymbolType type) : base(sign, type){ }
        public override int? GetValue()
        {
            return null;
        }
        public override Symbol Copy()
        {
            return new Operation(sign, type);
        }
    }
}