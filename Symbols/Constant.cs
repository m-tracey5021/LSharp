using System;
using System.Collections.Generic;
using LSharp.Rules;
using LSharp.Selectors;

namespace LSharp.Symbols
{
    public class Constant : Symbol
    {
        public int value { get; set; }
        public Constant(bool sign, int value) : base(sign, SymbolType.Constant){ this.value = value; }
        public override bool IsEqual(Symbol other)
        {
            if (type == other.type && value == other.GetValue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override int? GetValue()
        {
            return value;
        }
        public override string GetVariable()
        {
            return 
        }
        public override Symbol Copy()
        {
            return new Constant(sign, value);
        }
    }
}