using System;
using System.Collections.Generic;

namespace LSharp.Symbols
{
    public class Exponent : Symbol
    {
        public Exponent(){ this.sign = true; this.symbol = '*'; }

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
        public override bool IsEqual(Exponent other){ if (this == other){ return true; } else { return false; } }
        public override bool IsEqual(Radical other){ return false; }
        public override bool IsEqual(Variable other){ return false; }
        public override bool IsEqual(Constant other){ return false; }
        public override bool CanApplyER1(){ return false; }
        public override bool CanApplyER2()
        {
            int stage = 1;

            List<int> children = GetChildren();

            foreach (int child in children)
            {
                expression.GetNode(child).IsER2Constituent(ref stage);
            }
            if (stage == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void IsER1Constituent(ref int stage)
        {
            if (stage == 1 || stage == 4)
            {
                stage ++;

                List<int> children = GetChildren();

                foreach (int child in children)
                {
                    expression.GetNode(child).IsER1Constituent(ref stage);
                }
                return;
            }
            else if (stage == 2 || stage == 3 || stage == 5 || stage == 6)
            {
                stage ++;
                return;
            }
            else
            {
                return;
            }
        }
        public override Symbol Copy()
        {
            return new Exponent() { sign = this.sign, symbol = this.symbol, index = this.index };
        }
    }
}