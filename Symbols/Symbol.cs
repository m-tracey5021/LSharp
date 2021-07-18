using System;
using System.Collections.Generic;
using MathSharp.Visitors;


namespace MathSharp.Symbols
{
    public abstract class Symbol
    {
        public char value { get; set; }
        public bool sign { get; set; }
        public Symbol parent { get; set; }
        public List<Symbol> children { get; set; }
        public Expression parentExpression { get; set; }

        // constructors

        public Symbol(){}
        public Symbol(char value, bool sign){ this.value = value; this.sign = sign; }

        // methods

        public abstract void dispatch(Visitor visitor);
        public abstract Symbol evaluate();
        public abstract Symbol sum(Symbol other);
        public abstract Symbol multiply(Symbol other);
        public abstract Symbol divide(Symbol other);
        public abstract Symbol raise(Symbol other);
        public abstract Symbol floor(Symbol other); // rename this
        
    }
}