using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Rules
{
    public class ExponentRuleSix : Rule
    {
        public ExponentRuleSix(Expression expression) : base(expression, new Dictionary<int, (SymbolType type, bool recurse)>
        {
            { 0, (SymbolType.Exponent, true) },
            { 1, (SymbolType.Variable, true) },
            { 2, (SymbolType.Variable, false) }

            // 2,
            // new List<char>(){ '^', 'x', 'x' },
            // new List<bool>(){ true, false, false }
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
                if (currentStage == 0)
                {
                    foreach (int child in expression.GetChildren((int) index))
                    {
                        if (!AppliesTo(child))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else if (currentStage == 1)
                {
                    currentStage ++;

                    return true;
                }
                else
                {
                    if (!expression.GetNode((int) index).sign)
                    {
                        currentStage ++;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
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

                Symbol div = new Operation(true, SymbolType.Division);

                Symbol one = new Constant(true, 1);

                Symbol exp = new Operation(true, SymbolType.Exponent);

                Expression a = expression.CopySubTree(expression.GetChild(root, 0));

                Expression n = expression.CopySubTree(expression.GetChild(root, 1));

                result.SetRoot(div);

                int numIndex = result.AppendNode(0, one);

                int denomIndex = result.AppendNode(0, exp);

                result.AppendNode(numIndex, a);

                result.AppendNode(denomIndex, n);

                return result;
            }
            else
            {
                return expression;
            }
            
        }
    }
}