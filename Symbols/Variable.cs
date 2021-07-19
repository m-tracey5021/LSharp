using System;
using System.Collections.Generic;
using LSharp.Visitors;

namespace LSharp.Symbols
{
    public class Variable : Symbol
    {
        public char value { get; set; }
        public Variable(){}
        public Variable(bool sign, char value){ this.symbol = value; this.sign = sign; this.value = value; }
        public override void Dispatch(Visitor visitor)
        {
            visitor.Visit(this);
        }
        public override SymbolFlat Flatten()
        {
            return new SymbolFlat(SymbolType.Variable, value);
        }
        public override void Sanitise()
        {
            foreach (Symbol child in children){
                child.Sanitise();
            }
        }
        public override Nullable<int> GetValue()
        {
            return null;
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