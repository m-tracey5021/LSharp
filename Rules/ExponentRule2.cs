using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule2 : Rule
    {
        public ExponentRule2() : base(){}
        public override bool Test(Summation summation)
        {
            return GenericPass();
        }
        public override bool Test(Multiplication multiplication)
        {
            return GenericPass();
        }
        public override bool Test(Division division)
        {
            if (stage == 1)
            {
                SuccessContinue();

                return true;
            }
            else if (stage == 2 || stage == 3 || stage == 4)
            {
                SuccessReturn();

                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Test(Exponent exponent)
        {
            if (stage == 0)
            {
                SuccessContinue();

                return true;
            }
            else if (stage == 2 || stage == 3 || stage == 4)
            {
                SuccessReturn();

                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Test(Radical radical)
        {
            return GenericPass();
        }
        public override bool Test(Variable variable)
        {
            return GenericPass();
        }
        public override bool Test(Constant constant)
        {
            return GenericPass();
        }
        public override Expression Apply(Symbol symbol)
        {
            Expression expression = symbol.expression;
            Expression result = new Expression();

            Symbol division = new Division();

            Symbol exp1 = new Exponent();
            Symbol exp2 = new Exponent();

            Symbol a = expression.GetNode(symbol.GetChild(new List<int> { 0, 0 }));

            Symbol b = expression.GetNode(symbol.GetChild(new List<int> { 0, 1 }));

            Symbol m1 = expression.GetNode(symbol.GetChild(1)).Copy();

            Symbol m2 = expression.GetNode(symbol.GetChild(1)).Copy();

            result.AddNode(division);

            result.AddNode(division, exp1);

            result.AddNode(division, exp2);

            result.AddNode(exp1, a);
            result.AddNode(exp1, m1);

            result.AddNode(exp2, b);
            result.AddNode(exp2, m2);

            return result;
        }
        public bool GenericPass()
        {
            if (stage == 2 || stage == 3 || stage == 4)
            {
                SuccessReturn();
                
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}