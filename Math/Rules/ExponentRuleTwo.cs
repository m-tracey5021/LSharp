using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Rules
{
    public class ExponentRuleTwo : Rule
    {
        public ExponentRuleTwo(Expression expression) : base(expression, new Dictionary<int, (SymbolType type, bool recurse)>
        {
            { 0, (SymbolType.Exponent, true) },
            { 1, (SymbolType.Division, true) },
            { 2, (SymbolType.Variable, false) },
            { 3, (SymbolType.Variable, false) },
            { 4, (SymbolType.Variable, false) }
        //     4, 
        //     new List<char>(){ '^', '/', 'x', 'x', 'x' },
        //     new List<bool>(){ true, true, false, false, false }
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
        public override Expression Apply()
        {
            if (AppliesTo())
            {
                int root = expression.GetRoot();

                Expression result = new Expression();

                Symbol division = new Operation(true, SymbolType.Division);

                Symbol exp1 = new Operation(true, SymbolType.Exponent);

                Symbol exp2 = new Operation(true, SymbolType.Exponent);

                Expression a = expression.CopySubTree(expression.GetChild(root, new List<int> { 0, 0 }));

                Expression b = expression.CopySubTree(expression.GetChild(root, new List<int> { 0, 1 }));

                Expression m1 = expression.CopySubTree(expression.GetChild(root, 1));

                Expression m2 = expression.CopySubTree(expression.GetChild(root, 1));

                result.SetRoot(division);

                int lhs = result.AppendNode(0, exp1);

                int rhs = result.AppendNode(0, exp2);

                result.AppendNode(lhs, a);

                result.AppendNode(lhs, m1);

                result.AppendNode(rhs, b);

                result.AppendNode(rhs, m2);

                return result;
            }
            else
            {
                return expression;
            }
            
        }
    }
}