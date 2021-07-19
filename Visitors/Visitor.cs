using System;
using LSharp.Symbols;

namespace LSharp.Visitors
{
    public abstract class Visitor
    {
        public abstract void Visit(Summation symbol);
        public abstract void Visit(Multiplication symbol);
        public abstract void Visit(Division symbol);
        public abstract void Visit(Exponent symbol);
        public abstract void Visit(Radical symbol);
        public abstract void Visit(Variable symbol);
        public abstract void Visit(Constant symbol);
    }
}