using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public class LikeTerm : Rule
    {
        public Structure structure = new Structure
        (
            6, 
            new List<char>(){ '+', '*', 'n', 'x', '*', 'n', 'x' },
            new List<bool>(){ true, true, false, false, true, false, false }
        );
        private static readonly Structure deepLeftStructure = new Structure
        (
            4,
            new List<char>(){ '+', '*', 'n', 'x', 'x' },
            new List<bool>(){ true, true, false, false, false }
        );
        private static readonly Structure deepRightStructure = new Structure
        (
            4,
            new List<char>(){ '+', 'x', '*', 'n', 'x' },
            new List<bool>(){ true, false, true, false, false }
        );
        private static readonly Structure smallStructure = new Structure
        (
            2,
            new List<char>(){ '+', 'x', 'x' },
            new List<bool>(){ true, false, false }
        );
        private List<int> lhsChildren { get; set; }
        private List<int> rhsChildren { get; set; }
        
        public int variableIndex { get; set; }
        public int totalSum { get; set; }
        public LikeTerm() : base(new Structure
        (
            6, 
            new List<char>(){ '+', '*', 'n', 'x', '*', 'n', 'x' },
            new List<bool>(){ true, true, false, false, true, false, false }
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

                }
                else if (stage == 2)
                {

                }
                else
                {
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
        public override bool Test(Symbol symbol)
        {
            StructureStage structureStage = structure.At(stage);

            if (symbol.ToString().Contains(structureStage.type))
            {
                Success();

                return true;
            }
            else if (structureStage.type == 'n' && symbol.variable == false)
            {
                if (symbol.GetValue() != null)
                {
                    totalSum += (int) symbol.GetValue();
                }
                Success();

                return true;
            }
            else if (structureStage.type == 'x' && symbol.variable == true)
            {
                if (stage == 3)
                {
                    variableIndex = symbol.GetIndex();
                }
                else if (stage == 6)
                {
                    if (!symbol.IsEqual(symbol.expression.GetNode(variableIndex)))
                    {
                        return false;
                    }
                }
                Success();

                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Test(Summation summation)
        {
            StructureStage structureStage = structure.At(stage);

            if (structureStage.type == '+')
            {
                Success();

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
                Success();

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
                Success();

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
                    variableIndex = symbol.GetIndex();
                }
                if (stage == 6)
                {
                    if (!symbol.IsEqual(symbol.expression.GetNode(variableIndex)))
                    {
                        return false;
                    }
                }
                Success();

                return true; 
            }
            else
            {
                return false;
            }
        }
    }
}