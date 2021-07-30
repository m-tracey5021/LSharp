using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class LikeTerm : Rule
    {
        private static readonly Dictionary<int, char> structure1 = new Dictionary<int, char>()
        {
            { 0, '+' },
            { 1, '*' },
            { 2, 'n' },
            { 3, 'x' },
            { 4, '*' },
            { 5, 'n' },
            { 6, 'x' }
        };
        public Symbol variable { get; set; }
        public int totalSum { get; set; }
        public bool noMulLhs { get; set; }
        public bool noMulRhs { get; set; }
        public LikeTerm() : base(){}
        public override bool Test(Summation summation)
        {
            if (stage == 0)
            {
                SuccessContinue();

                return true;
            }
            else if (stage == 3 || stage == 6)
            {
                SuccessReturn();

                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Test(Multiplication multiplication)
        {
            if (stage == 1 || stage == 4)
            {
                SuccessContinue();

                return true;
            }
            else if (stage == 3)
            {
                variable = multiplication;

                SuccessReturn();

                return true;
            }
            else if (stage == 6)
            {
                if (variable.IsEqual(multiplication))
                {
                    SuccessReturn();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public override bool Test(Division division)
        {
            return GenericPass(division);
        }
        public override bool Test(Exponent exponent)
        {
            return GenericPass(exponent);
        }
        public override bool Test(Radical radical)
        {
            return GenericPass(radical);
        }
        public override bool Test(Variable variable)
        {
            return GenericPass(variable);
        }
        public override bool Test(Constant constant)
        {
            if (stage == 2 || stage == 5)
            {
                if (constant.GetValue() != null)
                {
                    totalSum += (int) constant.GetValue();
                }
                SuccessReturn();

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GenericPass(Symbol symbol)
        {
            if (stage == 1)
            {
                variable = symbol;

                noMulLhs = true;

                SuccessReturn();

                return true;
            }
            else if (stage == 3)
            {
                variable = symbol;

                SuccessReturn();

                return true;
            }
            else if (stage == 6)
            {
                if (symbol.IsEqual(variable))
                {
                    SuccessReturn();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}