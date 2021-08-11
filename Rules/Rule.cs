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
        public virtual bool Test(Symbol symbol)
        {
            if (structure.At(stage).type.ToString() == symbol.GetValue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public abstract bool AppliesTo(Expression expression, int index);
        public abstract Expression Apply(Expression expression, int index);
        
        
    }
}