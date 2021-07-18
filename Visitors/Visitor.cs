using System;
using LSharp.Symbols;

namespace LSharp.Visitors
{
    public abstract class Visitor
    {
        public abstract void visit(Summation symbol);
        public abstract void visit(Multiplication symbol);
        public abstract void visit(Division symbol);
        public abstract void visit(Exponent symbol);
        public abstract void visit(Radical symbol);
        public abstract void visit(Variable symbol);
        public abstract void visit(Constant symbol);
    }
}