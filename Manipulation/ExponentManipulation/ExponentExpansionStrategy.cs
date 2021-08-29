using System;
using System.Collections.Generic;
using System.Linq;
using LSharp.Symbols;

namespace LSharp.Manipulation.MultiplicationManipulation
{
    public class ExponentExpansionStrategy : IManipulationStrategy
    {
        public Expression expression { get; set; }
        public ExponentExpansionStrategy(Expression expression)
        {
            this.expression = expression;
        }
        public Expression Manipulate()
        {
            ChainManipulation(expression.GetRoot());

            return expression;
        } 
        public void ChainManipulation(int index)
        {
            foreach (int child in expression.GetChildren(index))
            {
                ChainManipulation(child);
            }
            ManipulationResult manipulation = ExpandExponents(index);

            if (manipulation.change)
            {
                expression.ReplaceNode(index, manipulation.result);
            }
        }  
        public ManipulationResult ExpandExponents(int index)
        {
            if (expression.IsExponent(index))
            {
                Symbol exp = expression.GetNode(expression.GetChild(index, 1));
            }
            return null;
        }
    }
}