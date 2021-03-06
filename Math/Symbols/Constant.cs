using System;
using System.Collections.Generic;

namespace LSharp.Math.Symbols
{
    public class Constant : Symbol
    {
        public int value { get; set; }
        public Constant(bool sign, int value) : base(sign, SymbolType.Constant){ this.value = value; }
        public override string GetValue()
        {
            return value.ToString();
        }
        public override int? GetNumericValue()
        {
            return value;
        }
        public override void SetValue(char value)
        {
            
        }
        public override void SetNumericValue(int value)
        {
            this.value = value;
        }
        public override Symbol Copy()
        {
            return new Constant(sign, value);
        }
    }
}