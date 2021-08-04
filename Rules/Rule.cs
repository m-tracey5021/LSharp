using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public abstract class Rule
    {
        public int stage { get; set; }
        public bool recurse { get; set; }
        public Structure structure { get; set; }
        public Rule(Structure structure){ this.stage = 0; this.structure = structure; }
        public void Success(){ this.stage ++; this.recurse = this.structure.At(stage - 1).recurse; }
        public void SuccessContinue(){ this.stage ++; this.recurse = true; }
        public void SuccessReturn(){ this.stage ++; this.recurse = false; }
        public abstract bool Test(Symbol symbol);
        public abstract bool Test(Summation summation);
        public abstract bool Test(Multiplication multiplication);
        public abstract bool Test(Division division);
        public abstract bool Test(Exponent exponent);
        public abstract bool Test(Radical radical);
        public abstract bool Test(Variable variable);
        public abstract bool Test(Constant constant);
        public abstract Expression Apply(Symbol symbol);
        public abstract bool AppliesTo(Symbol symbol);
        
    }
}