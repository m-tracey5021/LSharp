using System;
using System.Collections.Generic;
using System.Linq;
using LSharp.Symbols;

namespace LSharp.Manipulation.DivisionStrategies
{
    public class CancelManipulation : IManipulationStrategy
    {
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
        public ManipulationResult Cancel(Expression expression, int index)
        {
            // factor first

            if (expression.GetNode(index).IsDivision())
            {
                int num = expression.GetChild(index, 0);

                int denom = expression.GetChild(index, 1);

                List<int> numChildren = new List<int>();

                List<int> denomChildren = new List<int>();

                if (expression.GetNode(num).IsMultiplication())
                {
                    numChildren = expression.GetChildren(num);
                }
                else
                {
                    numChildren.Add(num);
                }
                if (expression.GetNode(denom).IsMultiplication())
                {
                    denomChildren = expression.GetChildren(denom);
                }
                else
                {
                    denomChildren.Add(denom);
                }
                List<int> cancelledNums = new List<int>();

                List<int> cancelledDenoms = new List<int>();
                
                for (int i = 0; i < numChildren.Count; i ++)
                {
                    bool canCancel = false;

                    for (int j = 0; j < denomChildren.Count; j ++)
                    {
                        if (expression.IsEqual(numChildren[i], denomChildren[j]))
                        {
                            canCancel = true;        
                        }
                    }
                    if(!canCancel)
                    {
                        cancelledNums.Add(numChildren[i]);
                    }
                }
                for (int i = 0; i < denomChildren.Count; i ++)
                {
                    bool canCancel = false;

                    for (int j = 0; j < numChildren.Count; j ++)
                    {
                        if (expression.IsEqual(denomChildren[i], numChildren[j]))
                        {
                            canCancel = true;
                        }
                    }
                    if (!canCancel)
                    {
                        cancelledDenoms.Add(denomChildren[i]);
                    }
                }
                Expression result = new Expression();

                if (cancelledNums.Count == 0 && cancelledDenoms.Count == 0)
                {
                    result.SetRoot(new Constant(true, 1));
                }
                else if (cancelledNums.Count == 1 && cancelledDenoms.Count == 0)
                {
                    result.SetRoot(expression.CopySubTree(cancelledNums[0]));
                    
                }
                else if (cancelledNums.Count == 0 && cancelledDenoms.Count == 1)
                {
                    result.SetRoot(expression.CopySubTree(cancelledDenoms[0]));
                }
                else
                {
                    Symbol div = new Operation(true, SymbolType.Division);

                    Symbol numMul = new Operation(true, SymbolType.Multiplication);

                    Symbol denomMul = new Operation(true, SymbolType.Multiplication);

                    result.SetRoot(div);

                    int numIndex = result.AppendNode(0, numMul);

                    int denumIndex = result.AppendNode(0, denomMul);

                    result.AppendBulkNodes(numIndex, cancelledNums, expression);

                    result.AppendBulkNodes(denumIndex, cancelledDenoms, expression);
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