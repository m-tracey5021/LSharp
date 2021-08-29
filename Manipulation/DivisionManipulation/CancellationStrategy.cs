using System;
using System.Collections.Generic;
using System.Linq;
using LSharp.Symbols;
using LSharp.Comparison;

namespace LSharp.Manipulation.DivisionManipulation
{
    public class CancellationStrategy : IManipulationStrategy
    {   
        public Expression expression { get; set; }
        public List<Expression> exponents { get ; set; }
        public CancellationStrategy(Expression expression)
        {
            this.expression = expression;

            this.exponents = new List<Expression>();
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
            ManipulationResult manipulation = Cancel(expression, index);

            if (manipulation.change)
            {
                expression.ReplaceNode(index, manipulation.result);
            }
        }  
        public void AddToExponents(Expression expression, int i, int j)
        {
            if (expression.IsExponent(i) && expression.IsExponent(j))
            {
                Expression sub = expression.CopySubTree(expression.GetChild(i, 1)).Subtract(expression.CopySubTree(expression.GetChild(j, 1)));

                Expression exp = new Expression();

                exp.SetRoot(new Operation(true, SymbolType.Exponent));

                exp.AppendNode(0, expression.CopySubTree(expression.GetChild(i, 0)));

                exp.AppendNode(0, sub);

                exponents.Add(exp);
            }
        }
        public ManipulationResult Cancel(Expression expression, int index)
        {
            // factor first

            if (expression.IsDivision(index))
            {
                int num = expression.GetChild(index, 0);

                int denom = expression.GetChild(index, 1);

                List<int> cancelledNums;

                List<int> cancelledDenoms;

                if (expression.IsMultiplication(num) && expression.IsMultiplication(denom))
                {
                    cancelledNums = new List<int>(expression.GetChildren(num));

                    cancelledDenoms = new List<int>(expression.GetChildren(denom));
                }
                else if (!expression.IsMultiplication(num) && expression.IsMultiplication(denom))
                {
                    cancelledNums = new List<int>(){ num };

                    cancelledDenoms = new List<int>(expression.GetChildren(denom));
                }
                else if (expression.IsMultiplication(num) && !expression.IsMultiplication(denom))
                {
                    cancelledNums = new List<int>(expression.GetChildren(num));

                    cancelledDenoms = new List<int>(){ denom };
                }
                else
                {
                    return new ManipulationResult(){ result = expression, change = false };
                }  
                for (int i = 0; i < cancelledNums.Count; i ++)
                {
                    bool removed = false;

                    for (int j = 0; j < cancelledDenoms.Count; j ++)
                    {
                        if (expression.Compare(ComparisonInstruction.IsEqualByBase, first: cancelledNums[i], second: cancelledDenoms[j]))
                        {
                            AddToExponents(expression, cancelledNums[i], cancelledDenoms[j]);

                            cancelledNums.RemoveAt(i);
  
                            cancelledDenoms.RemoveAt(j);

                            removed = true;

                            i --;

                            j = -1;
                        }
                    }
                    i = removed ? i - 1 : i;
                }
                Expression result = new Expression();

                List<Expression> finalNums = cancelledNums.Select(x => expression.CopySubTree(x)).ToList();

                List<Expression> finalDenoms = cancelledDenoms.Select(x => expression.CopySubTree(x)).ToList();

                finalNums.AddRange(exponents);

                if (finalNums.Count == 0 && finalDenoms.Count == 0)
                {
                    result.SetRoot(new Constant(true, 1));
                }
                else if (finalNums.Count == 1 && finalDenoms.Count == 0)
                {
                    result.SetRoot(finalNums[0]);
                }
                else if (finalNums.Count == 0 && finalDenoms.Count == 1)
                {
                    result.SetRoot(finalDenoms[0]);
                }
                else
                {
                    Symbol div = new Operation(true, SymbolType.Division);

                    result.SetRoot(div);

                    Symbol numMul = new Operation(true, SymbolType.Multiplication);

                    Symbol denomMul = new Operation(true, SymbolType.Multiplication);

                    if (finalNums.Count == 1 && finalDenoms.Count == 1)
                    {
                        result.AppendNode(0, finalNums[0]);

                        result.AppendNode(0, finalDenoms[0]);
                    }
                    else if (finalNums.Count > 1 && finalDenoms.Count == 1)
                    {
                        int numIndex = result.AppendNode(0, numMul);

                        result.AppendBulkNodes(numIndex, finalNums);

                        result.AppendNode(0, finalDenoms[0]);
                    }
                    else if (finalNums.Count == 1 && finalDenoms.Count > 1)
                    {
                        result.AppendNode(0, finalNums[0]);

                        int denumIndex = result.AppendNode(0, denomMul);

                        result.AppendBulkNodes(denumIndex, finalDenoms);
                    }
                    else
                    {
                        int numIndex = result.AppendNode(0, numMul);

                        int denumIndex = result.AppendNode(0, denomMul);

                        result.AppendBulkNodes(numIndex, finalNums);

                        result.AppendBulkNodes(denumIndex, finalDenoms);
                    } 
                }
                return new ManipulationResult(){ result = result, change = true };
            }
            else
            {
                return new ManipulationResult(){ result = expression, change = false };
            }
        }
    }
}