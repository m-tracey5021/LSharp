using System;
using System.Collections.Generic;
using System.Linq;


namespace LSharp.Symbols
{
    public abstract class Symbol
    {
        public bool sign { get; set; }
        public char symbol { get; set; }
        public Expression expression { get; set; }

        // constructors

        public Symbol(){}
        public Symbol(bool sign, char symbol){ this.sign = sign; this.symbol = symbol; }

        // methods

        public virtual int GetParent()
        {
            int index = expression.Search(this);
            return expression.GetParent(index);
        }
        public virtual List<int> GetChildren()
        {
            int index = expression.Search(this);
            return expression.GetChildren(index);
        }
        public abstract Symbol Sum(Symbol other);
        public abstract Symbol Sum(Summation other);
        public abstract Symbol Sum(Multiplication other);
        public abstract Symbol Sum(Division other);
        public abstract Symbol Sum(Exponent other);
        public abstract Symbol Sum(Radical other);
        public abstract Symbol Sum(Variable other);
        public abstract Symbol Sum(Constant other);

        public abstract bool IsEqual(Symbol other);
        public abstract bool IsEqual(Summation other);
        public abstract bool IsEqual(Multiplication other);
        public abstract bool IsEqual(Division other);
        public abstract bool IsEqual(Exponent other);
        public abstract bool IsEqual(Radical other);
        public abstract bool IsEqual(Variable other);
        public abstract bool IsEqual(Constant other);

        public abstract bool CanApplyER1();
        public abstract void IsER1Constituent(ref int stage);
        
    }
}