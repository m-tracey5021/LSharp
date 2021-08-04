using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class ExponentRule3 : Rule
    {
        public ExponentRule3() : base(new Structure
        (
            4, 
            new List<char>(){ '^', '^', 'x', 'x', 'x' },
            new List<bool>(){ true, true, false, false, false }
        ))
        {

        }
        public override bool AppliesTo(Symbol symbol)
        {
            bool passed = symbol.TestAgainstStage(structure.At(stage));

            if (passed)
            {
                if (stage == 0 || stage == 1)
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
                    stage ++;

                    return true;
                }
            }
            else
            {
                return false;
            }
        
        }
        public override Expression Apply(Symbol symbol){ return null; }
    }
}