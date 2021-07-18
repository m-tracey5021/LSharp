using System;
using MathSharp.Visitors;

namespace MathSharp.Symbols
{
    public class Constant : Symbol
    {
        public int value { get; set; }
        public Constant(){}
        public Constant(bool sign, int value){ this.symbol = Convert.ToChar(value); this.sign = sign; this.value = value; }
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
            return value;
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