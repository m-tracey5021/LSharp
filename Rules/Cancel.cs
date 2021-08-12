using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class Cancel : Rule
    {
        public List<int> lhsChildren { get; set; }
        public List<int> rhsChildren { get; set; }
        public Cancel() : base(new Structure
        (
            2, 
            new List<char>(){ '/', '*', '*' },
            new List<bool>(){ true, false, false }
        ))
        {

        }
        public override bool AppliesTo(Expression expression, int index)
        {
            bool passed = Test(expression.GetNode(index));

            if (passed)
            {
                List<int> children = expression.GetChildren(index);

                if (stage == 1)
                {
                    lhsChildren = children;

                    stage ++;

                    return true;
                }
                else if (stage == 2)
                {
                    rhsChildren = children;

                    return true;
                }
                else
                {
                    stage ++;

                    foreach (int child in children)
                    {
                        if (!AppliesTo(expression, child))
                        {
                            return false;
                        }
                    }
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
            throw new NotImplementedException();
        }
    }
}