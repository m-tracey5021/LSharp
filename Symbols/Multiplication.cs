using System;
using System.Collections.Generic;
using LSharp.Rules;

namespace LSharp.Symbols
{
    public class Multiplication : Symbol
    {
        public Multiplication(){ this.sign = true; this.symbol = '*';}
        
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
        public override bool IsEqual(Multiplication other){ if (this == other){ return true; } else { return false; } }
        public override bool IsEqual(Division other){ return false; }
        public override bool IsEqual(Exponent other){ return false; }
        public override bool IsEqual(Radical other){ return false; }
        public override bool IsEqual(Variable other){ return false; }
        public override bool IsEqual(Constant other){ return false; }
        public override bool CanApplyER1()
        {
            int stage = 1;

            List<int> children = GetChildren();

            foreach (int child in children)
            {
                expression.GetNode(child).IsER1Constituent(ref stage);
            }
            if (stage == 7)
            {
                int index = expression.Search(this);

                Symbol ll = expression.GetNode(expression.GetChild(index, new List<int> { 0, 0 }));
                Symbol rl = expression.GetNode(expression.GetChild(index, new List<int> { 1, 0 }));

                if (ll.IsEqual(rl))
                {
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
                if (rule.recurse)
                {
                    List<int> children = GetChildren();

                    foreach (int child in children)
                    {
                        if (!expression.GetNode(child).CanApply(rule))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public override Symbol Copy()
        {
            return new Multiplication() { sign = this.sign, symbol = this.symbol, index = this.index };
        }
    }
}