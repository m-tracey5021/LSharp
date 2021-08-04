using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class LikeTerm : Rule
    {
        public int variableIndex { get; set; }
        public int totalSum { get; set; }
        public LikeTerm() : base(new Structure
        (
            6, 
            new List<char>(){ '+', '*', 'n', 'x', '*', 'n', 'x' },
            new List<bool>(){ true, true, false, false, true, false, false }
        ))
        {
            this.totalSum = 0;
        }

        public override bool AppliesTo(Symbol symbol)
        {
            bool passed = symbol.TestAgainstStage(structure.At(stage));

            if (passed)
            {
                if (stage == 3)
                {
                    variableIndex = symbol.GetIndex();

                    stage ++;

                    return true;
                }
                else if (stage == 6)
                {
                    if (!symbol.IsEqual(symbol.expression.GetNode(variableIndex)))
                    {
                        return false;
                    }
                    stage ++;

                    return true;
                }
                else if (stage == 0 || stage == 1 || stage == 4)
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
                else
                {
                    if (symbol.GetValue() != null)
                    {
                        totalSum += (int) symbol.GetValue();
                    }
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
            Expression expression = new Expression();

            Symbol mul = new Multiplication();

            Symbol total = new Constant(true, totalSum);

            Expression variable = symbol.expression.CopySubTree(variableIndex);

            expression.AddNode(mul);

            expression.AddNode(mul, total);
            expression.AddNode(mul, variable);

            return expression;
        }
    }
}