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

            result.AddNode(division);

            result.AddNode(division, exp1);

            result.AddNode(division, exp2);

            result.AddNode(exp1, a);
            result.AddNode(exp1, m1);

            result.AddNode(exp2, b);
            result.AddNode(exp2, m2);

            return result;
        }
    }
}