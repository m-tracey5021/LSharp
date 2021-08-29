using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Rules
{
    public abstract class Rule
    {
        public int currentStage { get; set; }
        public bool currentRecurse { get; set; }
        public Expression expression { get; set; }
        public Dictionary<int, (SymbolType type, bool recurse)> structure { get; set; }
        public Rule(Expression expression, Dictionary<int, (SymbolType type, bool recurse)> structure)
        {
            this.currentStage = 0;

            this.currentRecurse = false;

            this.expression = expression;
            
            this.structure = structure; 
        }
        public virtual bool Test(SymbolType type)
        {
            if (structure[currentStage].type == type || (structure[currentStage].type == SymbolType.Variable && type != SymbolType.Constant))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public abstract bool AppliesTo(int? index = null);
        public abstract Expression Apply();
        
        
    }
}