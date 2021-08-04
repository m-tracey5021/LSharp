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
        public override bool AppliesTo(Symbol symbol)
        {
            bool passed = symbol.TestAgainstStage(structure.At(stage));

            if (passed)
            {
                List<int> children = symbol.GetChildren();

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
                        if (!AppliesTo(symbol.expression.GetNode(child)))
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
        public override Expression Apply(Symbol symbol)
        {
            throw new NotImplementedException();
        }
    }
}