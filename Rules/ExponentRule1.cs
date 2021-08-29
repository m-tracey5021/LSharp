using System;
using System.Collections.Generic;
using LSharp.Symbols;
using LSharp.Comparison;

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
        public override bool AppliesTo(Expression expression, int index)
        {
            bool passed = Test(expression.GetNode(index));

            if (passed)
            {
                if (stage == 0 || stage == 1 || stage == 4)
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
                else if (stage == 2)
                {
                    variableIndex = index;

                    stage ++;

                    return true;
                }
                else if (stage == 5)
                {
                    if (!expression.Compare(ComparisonInstruction.IsEqual, first: index, second: variableIndex))
                    // if (!expression.IsEqual(index, variableIndex))
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
        public override Expression Apply(Expression expression, int index)
        {
            Expression result = new Expression();

            Symbol exponent = new Operation(true, SymbolType.Exponent);

            Symbol addition = new Operation(true, SymbolType.Summation);

            Symbol a = expression.GetNode(expression.GetChild(index, new List<int> { 0, 0 }));

            Symbol m = expression.GetNode(expression.GetChild(index, new List<int> { 0, 1 }));

            Symbol n = expression.GetNode(expression.GetChild(index, new List<int> { 1, 1 }));

            result.SetRoot(exponent);

            result.AppendNode(0, a);

            int addIndex = result.AppendNode(0, addition);

            result.AppendNode(addIndex, m);

            result.AppendNode(addIndex, n);

            return result;
        }
    }
}