using System;
using LSharp.Symbols;

namespace LSharp.Selectors
{
    public class IsCoefficient : Selector
    {
        public bool isCoefficient { get; set; }
        public override void Select(Summation summation)
        {
            isCoefficient = false;
        }
        public override void Select(Multiplication multiplication)
        {
            isCoefficient = false;
        }
        public override void Select(Division division)
        {
            isCoefficient = false;
        }
        public override void Select(Exponent exponent)
        {
            isCoefficient = false;
        }
        public override void Select(Radical radical)
        {
            isCoefficient = false;
        }
        public override void Select(Variable variable)
        {
            isCoefficient = false;
        }
        public override void Select(Constant constant)
        {
            isCoefficient = true;
        }
    }
}