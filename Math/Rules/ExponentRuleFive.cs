using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;
using LSharp.Math.Comparison;

namespace LSharp.Math.Rules
{
    public class ExponentRuleFive : Rule
    {
        public int variableIndex { get; set; }
        public ExponentRuleFive(Expression expression) : base(expression, new Dictionary<int, (SymbolType type, bool recurse)>
        {
            { 0, (SymbolType.Division, true) },
            { 1, (SymbolType.Exponent, true) },
            { 2, (SymbolType.Variable, false) },
            { 3, (SymbolType.Variable, false) },
            { 4, (SymbolType.Exponent, true) },
            { 5, (SymbolType.Variable, false) },
            { 6, (SymbolType.Variable, false) }

            // 6, 
            // new List<char>(){ '/', '^', 'x', 'x', '^', 'x', 'x' },
            // new List<bool>(){ true, true, false, false, true, false, false }
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
                if (currentStage == 0 || currentStage == 1 || currentStage == 4)
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
                else if (currentStage == 2)
                {
                    variableIndex = (int) index;

                    currentStage ++;

                    return true;
                }
                else if (currentStage == 5)
                {
                    if (!expression.Compare(ComparisonInstruction.IsEqual, first: index, second: variableIndex))
                    // if (!expression.IsEqual(index, variableIndex))
                    {
                        return false;
                    }
                    currentStage ++;

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