using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule1 : Rule
    {
        public Symbol secondStageVariable { get; set; }
        public ExponentRule1() : base(){}
        public override bool Test(Summation summation)
        {
            return GenericPass(summation);
        }
        public override bool Test(Multiplication multiplication)
        {
            if (stage == 0)
            {
                SuccessContinue();

                return true;
            }
            else if (stage == 2)
            {
                secondStageVariable = multiplication;

                SuccessReturn();

                return true;
            }
            else if (stage == 5)
            {
                if (secondStageVariable.IsEqual(multiplication))
                {
                    SuccessReturn();

                    return true;
                }
                else
                {
                    return false;
                }
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
        public override bool Test(Division division)
        {
            return GenericPass(division);
        }
        public override bool Test(Exponent exponent)
        {
            if (stage == 1 || stage == 4)
            {
                SuccessContinue();

                return true;
            }
            else if (stage == 2)
            {
                secondStageVariable = exponent;

                SuccessReturn();

                return true;
            }
            else if (stage == 5)
            {
                if (exponent.IsEqual(secondStageVariable))
                {
                    SuccessReturn();

                    return true;
                }
                else
                {
                    return false;
                }
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

            Symbol exponent = new Exponent();

            Symbol addition = new Summation();

            Symbol a = expression.GetNode(symbol.GetChild(new List<int> { 0, 0 }));

            Symbol m = expression.GetNode(symbol.GetChild(new List<int> { 0, 1 }));

            Symbol n = expression.GetNode(symbol.GetChild(new List<int> { 1, 1 }));

            result.AddNode(exponent);

            result.AddNode(exponent, a);

            result.AddNode(exponent, addition);

            result.AddNode(addition, m);

            result.AddNode(addition, n);

            return result;
        }
        public bool GenericPass(Symbol symbol)
        {
            if (stage == 2)
            {
                secondStageVariable = symbol;

                SuccessReturn();

                return true;
            }
            else if (stage == 5)
            {
                if (symbol.IsEqual(secondStageVariable))
                {
                    SuccessReturn();

                    return true;
                }
                else
                {
                    return false;
                }
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
    }
}