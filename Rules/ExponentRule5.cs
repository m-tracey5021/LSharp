using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule5 : Rule
    {
        public int variableIndex { get; set; }
        public ExponentRule5() : base(new Structure
        (
            6, 
            new List<char>(){ '/', '^', 'x', 'x', '^', 'x', 'x' },
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
                    if (!expression.IsEqual(index, variableIndex))
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
        public override Expression Apply(Expression expression, int index){ return null; }
    }
}