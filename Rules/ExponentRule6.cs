using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule6 : Rule
    {
        public ExponentRule6() : base(){}
        public override bool Test(Summation summation)
        {
            return GenericPass(summation);
        }
        public override bool Test(Multiplication multiplication)
        {
            return GenericPass(multiplication);
        }
        public override bool Test(Division division)
        {
            return GenericPass(division);
        }
        public override bool Test(Exponent exponent)
        {
            if (stage == 0)
            {
                SuccessContinue();

                return true;
            }
            else if (stage == 1)
            {
                SuccessReturn();

                return true;
            }
            else if (stage == 2)
            {
                if (!exponent.sign)
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
            return GenericPass(constant);
        }
        public override Expression Apply(Symbol symbol)
        {
            Expression expression = symbol.expression;
            Expression result = new Expression();

            Symbol div = new Division();

            Symbol one = new Constant(true, 1);

            Symbol exp = new Exponent();

            Symbol a = expression.GetNode(symbol.GetChild(0)).Copy();

            Symbol n = expression.GetNode(symbol.GetChild(1)).Copy();

            result.AddNode(div);

            result.AddNode(div, one);
            result.AddNode(div, exp);

            result.AddNode(exp, a);
            result.AddNode(exp, n);

            return result;
        }
        public bool GenericPass(Symbol symbol)
        {
            if (stage == 1)
            {
                SuccessReturn();

                return true;
            }
            else if (stage == 2)
            {
                if (!symbol.sign)
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