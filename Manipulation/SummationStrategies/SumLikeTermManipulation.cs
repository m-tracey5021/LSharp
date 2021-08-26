using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Manipulation.SummationStrategies
{
    public class SumLikeTermManipulation : IManipulationStrategy
    {
        public Expression Manipulate(Expression expression)
        {
            Expression result = expression.CopyTree();

            ChainManipulation(result, result.GetRoot());

            return result;
        } 
        public void ChainManipulation(Expression expression, int index)
        {
            List<int> children = expression.GetChildren(index);

            for (int i = 0; i < children.Count; i ++)
            {
                ChainManipulation(expression, children[i]);
            }
            ManipulationResult manipulation = SumLikeTerms(expression, index);

            if (manipulation.change)
            {
                expression.ReplaceNode(index, manipulation.result);
            }
        }  
        public ManipulationResult SumLikeTerms(Expression expression, int index)
        {
            Expression result = new Expression();

            List<Expression> terms = new List<Expression>();

            if (expression.GetNode(index).IsSummation())
            {
                List<int> children = expression.GetChildren(index);

                List<int> visited = new List<int>();

                for (int i = 0; i < children.Count; i ++)
                {
                    int totalSum = 0;

                    if (!visited.Contains(i))
                    {
                        int compared = children[i];

                        visited.Add(i);

                        totalSum += expression.GetCoefficient(children[i]);

                        for (int j = i + 1; j < children.Count; j ++)
                        {
                            if (!visited.Contains(j))
                            {
                                if (IsLikeTerm(expression, compared, children[j]))
                                {
                                    visited.Add(j);

                                    totalSum += expression.GetCoefficient(children[j]);
                                }
                            }
                        }
                        Expression summed = expression.CopySubTree(children[i]);

                        if (totalSum > 1)
                        {
                            summed.SetCoefficient(0, totalSum);    
                        }
                        terms.Add(summed);

                        result.AppendNode(0, summed);
                    }
                } 
                if (terms.Count > 1)
                {
                    Symbol add = new Operation(true, SymbolType.Summation);

                    result.SetRoot(add);

                    foreach (Expression term in terms)
                    {
                        result.AppendNode(0, term);
                    }
                }
                else
                {
                    result.SetRoot(terms[0]);
                }
                return new ManipulationResult(){ result = result, change = true };
            }
            else
            {
                return new ManipulationResult(){ result = expression, change = false };
            }
        }
        public bool IsLikeTerm(Expression expression, int first, int second)
        {
            if (expression.GetNode(first).IsMultiplication() && expression.GetNode(second).IsMultiplication())
            {
                List<int> firstTerms = expression.GetTerms(first);

                List<int> secondTerms = expression.GetTerms(second);

                if (firstTerms.Count != secondTerms.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < firstTerms.Count; i ++)
                    {
                        if (!expression.IsEqual(firstTerms[i], secondTerms[i]))
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
    }
}