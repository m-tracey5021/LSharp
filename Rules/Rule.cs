using System;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public abstract class Rule
    {
        public int stage { get; set; }
        public bool recurse { get; set; }
        public Rule(){ this.stage = 0; }
        public void SuccessContinue(){ this.stage ++; this.recurse = true; }
        public void SuccessReturn(){ this.stage ++; this.recurse = false; }
        public abstract bool Test(Summation summation);
        public abstract bool Test(Multiplication multiplication);
        public abstract bool Test(Division division);
        public abstract bool Test(Exponent exponent);
        public abstract bool Test(Radical radical);
        public abstract bool Test(Variable variable);
        public abstract bool Test(Constant constant);
        public abstract Expression Apply(Symbol symbol);
        
    }
}