using System;
using System.Collections.Generic;
using LSharp.Rules;
using LSharp.Selectors;

namespace LSharp.Symbols
{
    public class Variable : Symbol
    {
        public char value { get; set; }
        public Variable(bool sign, char value) : base(sign, SymbolType.Variable){ this.value = value; }
        public override int? GetValue()
        {
            return null;
        }
        public override Symbol Copy()
        {
            return new Variable(sign, value);
        }
    }
}