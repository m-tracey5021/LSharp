using System;
using System.Collections.Generic;
using System.Linq;
using LSharp.Symbols;

namespace LSharp.Manipulation.DivisionStrategies
{
    public class CancelManipulation : IManipulationStrategy
    {
        public List<Expression> exponents { get ; set; } = new List<Expression>();
        public Expression Manipulate(Expression expression)
        {
            Expression result = expression.CopyTree();

            ChainManipulation(result, result.GetRoot());

            return result;
        } 
        public void ChainManipulation(Expression expression, int index)
        {
            foreach (int child in expression.GetChildren(index))
            {
                ChainManipulation(expression, child);
            }
            ManipulationResult manipulation = Cancel(expression, index);

            if (manipulation.change)
            {
                expression.ReplaceNode(index, manipulation.result);
            }
        }  
        public void AddToExponents(Expression expression, int i, int j)
        {
            if (expression.GetNode(i).IsExponent() && expression.GetNode(j).IsExponent())
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

            if (expression.GetNode(index).IsDivision())
            {
                int num = expression.GetChild(index, 0);

                int denom = expression.GetChild(index, 1);

                List<int> cancelledNums;

                List<int> cancelledDenoms;

                if (expression.GetNode(num).IsMultiplication() && expression.GetNode(denom).IsMultiplication())
                {
                    cancelledNums = new List<int>(expression.GetChildren(num));

                    cancelledDenoms = new List<int>(expression.GetChildren(denom));
                }
                else if (!expression.GetNode(num).IsMultiplication() && expression.GetNode(denom).IsMultiplication())
                {
                    cancelledNums = new List<int>(){ num };

                    cancelledDenoms = new List<int>(expression.GetChildren(denom));
                }
                else if (expression.GetNode(num).IsMultiplication() && !expression.GetNode(denom).IsMultiplication())
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
                        if (expression.IsEqual(cancelledNums[i], cancelledDenoms[j]) || expression.IsEqualByBase(cancelledNums[i], cancelledDenoms[j]))
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