using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;
using LSharp.Math.Comparison;

namespace LSharp.Math.Manipulation.SummationManipulation
{
    public class LikeTermStrategy : IManipulationStrategy
    {
        public Expression expression { get; set; }
        public LikeTermStrategy(Expression expression)
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
            List<int> children = expression.GetChildren(index);

            for (int i = 0; i < children.Count; i ++)
            {
                ChainManipulation(children[i]);
            }
            ManipulationResult manipulation = SumLikeTerms(index);

            if (manipulation.change)
            {
                expression.ReplaceNode(index, manipulation.result);
            }
        }  
        public ManipulationResult SumLikeTerms(int index)
        {
            Expression result = new Expression();

            List<Expression> terms = new List<Expression>();

            if (expression.IsSummation(index))
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
                                if (IsLikeTerm(compared, children[j]))
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
        public bool IsLikeTerm(int first, int second)
        {
            if (expression.IsMultiplication(first) && expression.IsMultiplication(second))
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
                        // if (!expression.IsEqual(firstTerms[i], secondTerms[i]))
                        if (!expression.Compare(ComparisonInstruction.IsEqual, first: firstTerms[i], second: secondTerms[i]))
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