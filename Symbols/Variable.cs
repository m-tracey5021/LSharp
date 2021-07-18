using System;
using LSharp.Visitors;

namespace LSharp.Symbols
{
    public class Variable : Symbol
    {
        public char value { get; set; }
        public Variable(){}
        public Variable(bool sign, char value){ this.symbol = value; this.sign = sign; this.value = value; }
        public override void dispatch(Visitor visitor)
        {
            visitor.visit(this);
        }
        public override void sanitise()
        {
            foreach (Symbol child in children){
                child.sanitise();
            }
        }
        public override Nullable<int> getValue()
        {
            return null;
        }
        public override Symbol evaluate()
        {
            return this;
        }
        public override Symbol sum(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol multiply(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol divide(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol raise(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol floor(Symbol other)
        {
            throw new NotImplementedException();
        }
    }
}