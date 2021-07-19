using System;
using System.Collections.Generic;
using LSharp.Visitors;

namespace LSharp.Symbols
{
    public class Constant : Symbol
    {
        public int value { get; set; }
        public Constant(){}
        public Constant(bool sign, int value){ this.symbol = Convert.ToChar(value); this.sign = sign; this.value = value; }
        public override void Dispatch(Visitor visitor)
        {
            visitor.Visit(this);
        }
        public override SymbolFlat Flatten()
        {
            return new SymbolFlat(SymbolType.Constant, symbol);
        }
        public override void Sanitise()
        {
            foreach (Symbol child in children){
                child.Sanitise();
            }
        }
        public override Nullable<int> GetValue()
        {
            return value;
        }
        public override Symbol Evaluate()
        {
            return this;
        }
        public override Symbol Sum(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Multiply(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Divide(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Raise(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Floor(Symbol other)
        {
            throw new NotImplementedException();
        }
    }
}