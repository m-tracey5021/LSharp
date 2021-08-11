using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule4 : Rule
    {
        public ExponentRule4() : base(new Structure
        (
            4, 
            new List<char>(){ '^', '*', 'x', 'x', 'x' },
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
        public override Expression Apply(Expression expression, int index){ return null; }
    }
}