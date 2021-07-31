using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class LikeTerm : Rule
    {
        private static readonly Structure structure = new Structure
        (
            6, 
            new List<char>(){ '+', '*', 'n', 'x', '*', 'n', 'x',  },
            new List<bool>(){ true, true, false, false, true, false, false }
        );
        public int variableIndex { get; set; }
        public int totalSum { get; set; }
        public LikeTerm() : base(){}
        public override bool Test(Summation summation)
        {
            StructureStage structureStage = structure.At(stage);

            if (structureStage.type == '+')
            {
                Success(structureStage);

                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Test(Multiplication multiplication)
        {
            StructureStage structureStage = structure.At(stage);

            if (structureStage.type == '*')
            {
                Success(structureStage);

                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Test(Division division)
        {
            return GenericPass(division);
        }
        public override bool Test(Exponent exponent)
        {
            return GenericPass(exponent);
        }
        public override bool Test(Radical radical)
        {
            return GenericPass(radical);
        }
        public override bool Test(Variable variable)
        {
            return GenericPass(variable);
        }
        public override bool Test(Constant constant)
        {
            StructureStage structureStage = structure.At(stage);

            if (structureStage.type == 'n')
            {
                if (constant.GetValue() != null)
                {
                    totalSum += (int) constant.GetValue();
                }
                Success(structureStage);

                return true;
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
        public bool GenericPass(Symbol symbol)
        {
            StructureStage structureStage = structure.At(stage);

            if (structureStage.type == 'x')
            {
                if (stage == 3)
                {
                    variableIndex = symbol.index;
                }
                if (stage == 6)
                {
                    if (!symbol.IsEqual(symbol.expression.GetNode(variableIndex)))
                    {
                        return false;
                    }
                }
                Success(structureStage);

                return true; 
            }
            else
            {
                return false;
            }
        }
    }
}