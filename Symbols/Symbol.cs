using System;
using System.Collections.Generic;
using System.Linq;


namespace LSharp.Symbols
{
    public abstract class Symbol
    {
        public bool sign { get; set; }
        public char symbol { get; set; }
        public int index { get; set; }
        public Expression expression { get; set; }

        // constructors

        public Symbol(){}
        public Symbol(bool sign, char symbol){ this.sign = sign; this.symbol = symbol; }

        // methods

        public virtual int GetParent()
        {
            return expression.GetParent(index);
        }
        public virtual int GetParent(int depth)
        {
            return expression.GetParent(index, depth);
        }
        public virtual List<int> GetChildren()
        {
            return expression.GetChildren(index);
        }
        public virtual int GetChild(int breadth)
        {
            return expression.GetChild(index, breadth);
        }
        public virtual int GetChild(List<int> path)
        {
            return expression.GetChild(index, path);
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
        public virtual bool CanApplyER2(){ return false; }
        public abstract void IsER1Constituent(ref int stage);
        public virtual void IsER2Constituent(ref int stage)
        {
            if (stage == 2 || stage == 3 || stage == 4)
            {
                stage ++;
                return;
            }
            else
            {
                return;
            }
        }
        public virtual Expression ApplyER1()
        {
            Expression result = new Expression();

            Symbol exponent = new Exponent();

            Symbol addition = new Summation();

            Symbol a = expression.GetNode(GetChild(new List<int> { 0, 0 }));

            Symbol m = expression.GetNode(GetChild(new List<int> { 0, 1 }));

            Symbol n = expression.GetNode(GetChild(new List<int> { 1, 1 }));

            result.AddNode(exponent);

            result.AddNode(exponent, a);

            result.AddNode(exponent, addition);

            result.AddNode(addition, m);

            result.AddNode(addition, n);

            return result;
        }   
        public virtual Expression ApplyER2()
        {
            Expression result = new Expression();

            Symbol division = new Division();

            Symbol exp1 = new Exponent();
            Symbol exp2 = new Exponent();

            Symbol a = expression.GetNode(GetChild(new List<int> { 0, 0 }));

            Symbol b = expression.GetNode(GetChild(new List<int> { 0, 1 }));

            Symbol m = expression.GetNode(GetChild(1));

            result.AddNode(division);

            result.AddNode(division, exp1);

            result.AddNode(division, exp2);

            result.AddNode(exp1, a);
            result.AddNode(exp1, m);

            result.AddNode(exp2, b);
            result.AddNode(exp2, m);

            return result;
        }
        public abstract Symbol Copy();

        public virtual void CopyToSubTree(Expression parentExpression)
        {
            Symbol copy = Copy();


        }

        public override string ToString()
        {
            return symbol.ToString();
        }
    }
}