using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule2 : Rule
    {
        public ExponentRule2() : base(new Structure
        (
            4, 
            new List<char>(){ '^', '/', 'x', 'x', 'x' },
            new List<bool>(){ true, true, false, false, false }
        ))
        {

        }
        public override bool AppliesTo(Expression expression, int index)
        {
            bool passed = Test(expression.GetNode(index));

            if (passed)
            {
                if (stage == 0 || stage == 1)
                {
                    stage ++;

                    foreach (int child in expression.GetChildren(index))
                    {
                        if (!AppliesTo(expression, child))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    stage ++;

                    return true;
                }
            }
            else
            {
                return false;
            }
        
        }
        public override Expression Apply(Expression expression, int index)
        {
            Expression result = new Expression();

            Symbol division = new Operation(true, SymbolType.Division);

            Symbol exp1 = new Operation(true, SymbolType.Exponent);
            Symbol exp2 = new Operation(true, SymbolType.Exponent);

            Symbol a = expression.GetNode(expression.GetChild(index, new List<int> { 0, 0 }));

            Symbol b = expression.GetNode(expression.GetChild(index, new List<int> { 0, 1 }));

            Symbol m1 = expression.GetNode(expression.GetChild(index, 1)).Copy();

            Symbol m2 = expression.GetNode(expression.GetChild(index, 1)).Copy();

            result.SetRoot(division);

            int lhs = result.AppendNode(0, exp1);

            int rhs = result.AppendNode(0, exp2);

            result.AppendNode(lhs, a);
            result.AppendNode(lhs, m1);

            result.AppendNode(rhs, b);
            result.AppendNode(rhs, m2);

            return result;
        }
    }
}