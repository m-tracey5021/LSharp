using System;
using System.Collections.Generic;

namespace LSharp.Symbols
{
    public class Variable : Symbol
    {
        public char variable { get; set; }
        public Variable(bool sign, char variable) : base(sign, SymbolType.Variable){ this.variable = variable; }
        public override string GetValue()
        {
            return variable.ToString();
        }
        public override int? GetNumericValue()
        {
            return null;
        }
        public override Symbol Copy()
        {
            return new Variable(sign, variable);
        }
    }
}