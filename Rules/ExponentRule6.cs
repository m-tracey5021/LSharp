using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule6 : Rule
    {
        public ExponentRule6() : base(new Structure
        (
            2,
            new List<char>(){ '^', 'x', 'x' },
            new List<bool>(){ true, false, false }
        ))
        {

        }
        public override bool AppliesTo(Expression expression, int index)
        {
            bool passed = Test(expression.GetNode(index));

            if (passed)
            {
                if (stage == 0)
                {
                    foreach (int child in expression.GetChildren(index))
                    {
                        if (!AppliesTo(expression, child))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else if (stage == 1)
                {
                    stage ++;

                    return true;
                }
                else
                {
                    if (!expression.GetNode(index).sign)
                    {
                        stage ++;

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
        public override Expression Apply(Expression expression, int index)
        {
            Expression result = new Expression();

            Symbol div = new Operation(true, SymbolType.Division);

            Symbol one = new Constant(true, 1);

            Symbol exp = new Operation(true, SymbolType.Exponent);

            Symbol a = expression.GetNode(expression.GetChild(index, 0)).Copy();

            Symbol n = expression.GetNode(expression.GetChild(index, 1)).Copy();

            result.AddNode(div);

            result.AddNode(div, one);
            result.AddNode(div, exp);

            result.AddNode(exp, a);
            result.AddNode(exp, n);

            return result;
        }
    }
}