using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Selectors
{
    public class SumLikeTerm : Selector
    {
        public bool canExecute { get; set; } = false;
        public Multiplication first { get; set; } = null;
        public Multiplication second { get; set; } = null;
        public Expression parent { get; set; }
        public Expression result { get; set; }
        public SumLikeTerm(Expression parent){ this.parent = parent; }
        public override void Select(Summation summation){ result = null; }
        public override void Select(Multiplication multiplication)
        {
            if (first == null)
            {
                first = multiplication;
            }
            else
            {
                if (second == null)
                {
                    second = multiplication;

                    canExecute = true;
                }
                else
                {
                    throw new Exception("Both already assigned");
                }
            }
        }
        public override void Select(Division division){ result = null; }
        public override void Select(Exponent exponent){ result = null; }
        public override void Select(Radical radical){ result = null; }
        public override void Select(Variable variable){ result = null; }
        public override void Select(Constant constant){ result = null; }
        public Expression Execute()
        {
            if (canExecute)
            {
                SeparateCoefficient separate = new SeparateCoefficient();

                first.Dispatch(separate);

                List<int> firstTerms = separate.terms;
                int firstCoefficient = separate.coefficient;

                second.Dispatch(separate);

                List<int> secondTerms = separate.terms;
                int secondCoefficient = separate.coefficient;

                int totalSum = firstCoefficient + secondCoefficient;

                Expression result = new Expression();

                if (firstTerms.Count == secondTerms.Count)
                {
                    for (int i = 0; i < firstTerms.Count; i ++)
                    {
                        if (!parent.GetNode(firstTerms[i]).IsEqual(parent.GetNode(secondTerms[i])))
                        {
                            return null;
                        }
                    }
                    Symbol multiplication = new Multiplication();

                    Symbol coefficient = new Constant(true, totalSum);

                    result.AddNode(multiplication);

                    if (totalSum > 1)
                    {
                        result.AddNode(multiplication, coefficient);
                    }
                    foreach (int term in firstTerms)
                    {
                        result.AddNode(multiplication, parent.CopySubTree(term));
                    }
                    return result;
                }
                else
                {
                    return null;
                }                
            }
            else
            {
                return null;
            }
        }
    }
}