using System;
using MathSharp.Visitors;

namespace MathSharp.Symbols
{
    public class Constant : Symbol
    {
        public int value { get; set; }
        public override void dispatch(Visitor visitor)
        {
            throw new NotImplementedException();
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