using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;
using LSharp.Math.Comparison;

namespace LSharp.Math.Rules
{
    public class ExponentRuleOne : Rule
    {
        public int variableIndex { get; set; }
        public ExponentRuleOne(Expression expression) : base(expression, new Dictionary<int, (SymbolType type, bool recurse)>
        {
            { 0, (SymbolType.Multiplication, true) },
            { 1, (SymbolType.Exponent, true) },
            { 2, (SymbolType.Variable, false) },
            { 3, (SymbolType.Variable, false) },
            { 4, (SymbolType.Exponent, true) },
            { 5, (SymbolType.Variable, false) },
            { 6, (SymbolType.Variable, false) }
            // 6, 
            // new List<char>(){ '*', '^', 'x', 'x', '^', 'x', 'x' },
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
        public override Expression Apply()
        {
            if (AppliesTo())
            {
                int root = expression.GetRoot();

                Expression result = new Expression();

                Symbol exponent = new Operation(true, SymbolType.Exponent);

                Symbol addition = new Operation(true, SymbolType.Summation);

                Expression a = expression.CopySubTree(expression.GetChild(root, new List<int> { 0, 0 }));

                Expression m = expression.CopySubTree(expression.GetChild(root, new List<int> { 0, 1 }));

                Expression n = expression.CopySubTree(expression.GetChild(root, new List<int> { 1, 1 }));

                result.SetRoot(exponent);

                result.AppendNode(0, a);

                int addIndex = result.AppendNode(0, addition);

                result.AppendNode(addIndex, m);

                result.AppendNode(addIndex, n);

                return result;
            }
            else
            {
                return expression;
            }
            
        }
    }
}