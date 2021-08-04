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
        public override bool AppliesTo(Symbol symbol)
        {
            bool passed = symbol.TestAgainstStage(structure.At(stage));

            if (passed)
            {
                if (stage == 0)
                {
                    foreach (int child in symbol.GetChildren())
                    {
                        if (!AppliesTo(symbol.expression.GetNode(child)))
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
                    if (!symbol.sign)
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
        public override Expression Apply(Symbol symbol)
        {
            Expression expression = symbol.expression;
            Expression result = new Expression();

            Symbol div = new Division();

            Symbol one = new Constant(true, 1);

            Symbol exp = new Exponent();

            Symbol a = expression.GetNode(symbol.GetChild(0)).Copy();

            Symbol n = expression.GetNode(symbol.GetChild(1)).Copy();

            result.AddNode(div);

            result.AddNode(div, one);
            result.AddNode(div, exp);

            result.AddNode(exp, a);
            result.AddNode(exp, n);

            return result;
        }
    }
}