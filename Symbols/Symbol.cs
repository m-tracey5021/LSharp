using System;
using System.Collections.Generic;
using System.Text;
using LSharp.Rules;


namespace LSharp.Symbols
{
    public abstract class Symbol
    {
        public bool sign { get; set; }
        public bool variable { get; set; }
        public Expression expression { get; set; }

        // constructors

        public Symbol(){}
        public Symbol(bool sign){ this.sign = sign; }

        // methods

        public virtual int GetIndex()
        {
            return expression.GetNode(this);
        }
        public virtual int GetParent()
        {
            return expression.GetParent(GetIndex());
        }
        public virtual int GetParent(int depth)
        {
            return expression.GetParent(GetIndex(), depth);
        }
        public virtual List<int> GetChildren()
        {
            return expression.GetChildren(GetIndex());
        }
        public virtual int GetChild(int breadth)
        {
            return expression.GetChild(GetIndex(), breadth);
        }
        public virtual int GetChild(List<int> path)
        {
            return expression.GetChild(GetIndex(), path);
        }
        public abstract int? GetValue();
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
        public abstract bool TestAgainstStage(StructureStage stage);
        public abstract Symbol Copy();
        public virtual void CopyToSubTree(Expression parentExpression)
        {
            Symbol copy = Copy();


        }
        public new abstract string ToString();
    }
}