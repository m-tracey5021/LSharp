using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule1 : Rule
    {
        public int variableIndex { get; set; }
        public ExponentRule1() : base(new Structure
        (
            6, 
            new List<char>(){ '*', '^', 'x', 'x', '^', 'x', 'x' },
            new List<bool>(){ true, true, false, false, true, false, false }
        ))
        {

        }
        public override bool AppliesTo(Symbol symbol)
        {
            bool passed = symbol.TestAgainstStage(structure.At(stage));

            if (passed)
            {
                if (stage == 0 || stage == 1 || stage == 4)
                {
                    stage ++;

                    foreach (int child in symbol.GetChildren())
                    {
                        if (!AppliesTo(symbol.expression.GetNode(child)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else if (stage == 2)
                {
                    variableIndex = symbol.GetIndex();

                    stage ++;

                    return true;
                }
                else if (stage == 5)
                {
                    if (!symbol.IsEqual(symbol.expression.GetNode(variableIndex)))
                    {
                        return false;
                    }
                    stage ++;

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
        public override Expression Apply(Symbol symbol)
        {
            Expression expression = symbol.expression;
            Expression result = new Expression();

            Symbol exponent = new Exponent();

            Symbol addition = new Summation();

            Symbol a = expression.GetNode(symbol.GetChild(new List<int> { 0, 0 }));

            Symbol m = expression.GetNode(symbol.GetChild(new List<int> { 0, 1 }));

            Symbol n = expression.GetNode(symbol.GetChild(new List<int> { 1, 1 }));

            result.AddNode(exponent);

            result.AddNode(exponent, a);

            result.AddNode(exponent, addition);

            result.AddNode(addition, m);

            result.AddNode(addition, n);

            return result;
        }
    }
}