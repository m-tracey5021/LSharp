using System;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule5 : Rule
    {
        public Symbol secondStageVariable { get; set; }
        public ExponentRule5() : base(){}
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
            if (stage == 0)
            {
                SuccessContinue();

                return true;
            }
            else if (stage == 2)
            {
                secondStageVariable = division;

                SuccessReturn();

                return true;
            }
            else if (stage == 5)
            {
                if (secondStageVariable.IsEqual(division))
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
                if (secondStageVariable.IsEqual(exponent))
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
        public override Expression Apply(Symbol symbol){ return null; }
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
                if (secondStageVariable.IsEqual(symbol))
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