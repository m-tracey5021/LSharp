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
        public abstract bool AppliesTo(Symbol symbol);
        public abstract Expression Apply(Symbol symbol);
        
        
    }
}