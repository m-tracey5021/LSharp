using System;
using System.Collections.Generic;
using LSharp.Rules;

namespace LSharp.Symbols
{
    public class Variable : Symbol
    {
        public char value { get; set; }
        public Variable(){}
        public Variable(bool sign, char value){ this.symbol = value; this.sign = sign; this.value = value; }

        public override Symbol Sum(Symbol other)
        {
            return other.Sum(this);
        }
        public override Symbol Sum(Summation other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Multiplication other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Division other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Exponent other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Radical other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Variable other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Constant other)
        {
            throw new NotImplementedException();
        }
        public override bool IsEqual(Symbol other){ return other.IsEqual(this); }
        public override bool IsEqual(Summation other){ return false; }
        public override bool IsEqual(Multiplication other){ return false; }
        public override bool IsEqual(Division other){ return false; }
        public override bool IsEqual(Exponent other){ return false; }
        public override bool IsEqual(Radical other){ return false; }
        public override bool IsEqual(Variable other){ if (this == other){ return true; } else { return false; } }
        public override bool IsEqual(Constant other){ return false; }
        public override bool CanApplyER1(){ return false; }
        public override void IsER1Constituent(ref int stage)
        {
            if (stage == 2 || stage == 3 || stage == 5 || stage == 6)
            {
                stage ++;
                return;
            }
            else
            {
                return;
            }
        }
        public override bool CanApply(Rule rule)
        {
            bool passed = rule.Test(this);
            
            if (passed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override Symbol Copy()
        {
            return new Variable() { sign = this.sign, symbol = this.symbol, index = this.index };
        }


    }
}