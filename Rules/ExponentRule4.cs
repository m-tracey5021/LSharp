using System;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule4 : Rule
    {
        public ExponentRule4() : base(){}
        public override bool Test(Summation summation)
        {
            return GenericPass();
        }
        public override bool Test(Multiplication multiplication)
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
        public override bool Test(Division division)
        {
            return GenericPass();
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
        public override Expression Apply(Symbol symbol){ return null; }
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