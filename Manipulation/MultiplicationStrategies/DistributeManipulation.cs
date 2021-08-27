using System;
using System.Collections.Generic;
using System.Linq;
using LSharp.Symbols;

namespace LSharp.Manipulation.MultiplicationStrategies
{
    public class DistributeManipulation : IManipulationStrategy
    {
        public Expression Manipulate(Expression expression)
        {
            ChainManipulation(expression, expression.GetRoot());

            return expression;
        } 
        public void ChainManipulation(Expression expression, int index)
        {
            foreach (int child in expression.GetChildren(index))
            {
                ChainManipulation(expression, child);
            }
            ManipulationResult manipulation = Distribute(expression, index);

            if (manipulation.change)
            {
                expression.ReplaceNode(index, manipulation.result);
            }
        }  
        public ManipulationResult Distribute(Expression expression, int index)
        {
            if (expression.GetNode(index).IsMultiplication())
            {
                Expression result = new Expression();

                Symbol add = new Operation(true, SymbolType.Summation);

                result.SetRoot(add);

                foreach(Expression multiplication in Distribute(expression, expression.GetChildren(index), 0, new Dictionary<int, int>()))
                {
                    result.AppendNode(0, multiplication);
                }
                return new ManipulationResult(){ result = result, change = true };
            }
            else
            {
                return new ManipulationResult(){ result = expression, change = false };
            }
        }
        public List<Expression> Distribute(Expression expression, List<int> symbols, int currentIndex, Dictionary<int, int> sumMap)
        {
            List<Expression> multiplications = new List<Expression>();

            List<int> children = new List<int>();

            if (expression.GetNode(symbols[currentIndex]).IsSummation())
            {
                children = expression.GetChildren(symbols[currentIndex]);
            }
            else
            {
                children.Add(symbols[currentIndex]);
            }
            for (int i = 0; i < children.Count; i ++)
            {
                sumMap[symbols[currentIndex]] = children[i];

                if (currentIndex != symbols.Count - 1)
                {
                    multiplications.AddRange(Distribute(expression, symbols, currentIndex + 1, sumMap));
                }
                else
                {
                    multiplications.Add(expression.Multiply(sumMap.Values.ToList()));
                }
            }
            return multiplications;
        }
        
    }
}