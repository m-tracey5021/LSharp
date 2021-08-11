using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Selectors
{
    public class SeparateCoefficient : Selector
    {
        public int coefficient { get; set; }
        public List<int> terms { get; set; }
        public override void Select(Summation summation)
        {
            terms = summation.GetChildren();
        }
        public override void Select(Multiplication multiplication)
        {
            List<int> duplicates = new List<int>();

            foreach (int child in multiplication.GetChildren())
            {
                IsCoefficient select = new IsCoefficient();

                multiplication.expression.GetNode(child).Dispatch(select);

                if (!select.isCoefficient)
                {
                    duplicates.Add(child);
                }
            }
            terms = duplicates;
        }
        public override void Select(Division division)
        {
            terms = division.GetChildren();
        }
        public override void Select(Exponent exponent)
        {
            terms = exponent.GetChildren();
        }
        public override void Select(Radical radical)
        {
            terms = radical.GetChildren();
        }
        public override void Select(Variable variable)
        {
            terms = variable.GetChildren();
        }
        public override void Select(Constant constant)
        {
            terms = constant.GetChildren();
        }
    }
}