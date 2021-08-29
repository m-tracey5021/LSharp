using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Rules
{
    public class ExponentRuleThree : Rule
    {
        public ExponentRuleThree(Expression expression) : base(expression, new Dictionary<int, (SymbolType type, bool recurse)>
        {
            { 0, (SymbolType.Exponent, true) },
            { 1, (SymbolType.Exponent, true) },
            { 2, (SymbolType.Variable, false) },
            { 3, (SymbolType.Variable, false) },
            { 4, (SymbolType.Variable, false) }

            // 4, 
            // new List<char>(){ '^', '^', 'x', 'x', 'x' },
            // new List<bool>(){ true, true, false, false, false }
        })
        {

        }
        public override bool AppliesTo(int? index = null)
        {
            if (index == null)
            {
                index = expression.GetRoot();
            }
            bool passed = Test(expression.GetSymbolType((int) index));

            if (passed)
            {
                if (currentStage == 0 || currentStage == 1)
                {
                    currentStage ++;

                    foreach (int child in expression.GetChildren((int) index))
                    {
                        if (!AppliesTo(child))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    currentStage ++;

                    return true;
                }
            }
            else
            {
                return false;
            }
        
        }
        public override Expression Apply(){ return null; }
    }
}