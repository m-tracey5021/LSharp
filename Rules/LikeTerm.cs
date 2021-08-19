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

        public override bool AppliesTo(Expression expression, int index)
        {
            bool passed = Test(expression.GetNode(index));

            if (passed)
            {
                if (stage == 3)
                {
                    variableIndex = index;

                    stage ++;

                    return true;
                }
                else if (stage == 6)
                {
                    if (!expression.IsEqual(index, variableIndex))
                    {
                        return false;
                    }
                    stage ++;

                    return true;
                }
                else if (stage == 0 || stage == 1 || stage == 4)
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
                    if (expression.GetNumericValue(index) != null)
                    {
                        totalSum += (int) expression.GetNumericValue(index);
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
        public override Expression Apply(Expression expression, int index)
        {
            Expression result = new Expression();

            Symbol mul = new Operation(true, SymbolType.Multiplication);

            Symbol total = new Constant(true, totalSum);

            Expression variable = expression.CopySubTree(variableIndex);

            result.SetRoot(mul);

            result.AddNode(mul, total);
            // result.AddNode(mul, variable);

            return result;
        }
    }
}