using System;
using System.Linq;
using System.Collections.Generic;

namespace LSharp.Symbols
{
    public class Expression
    {
        private int root { get; set; }
        private Dictionary<int, Symbol> tree { get; set; }
        private Dictionary<int, int> parentMap { get; set; }
        private Dictionary<int, List<int>> childMap { get; set; }
        private Dictionary<Symbol, int> reverseTree { get; set; }
        public Expression()
        {
            tree = new Dictionary<int, Symbol>();
            reverseTree = new Dictionary<Symbol, int>();
            parentMap = new Dictionary<int, int>();
            childMap = new Dictionary<int, List<int>>();
        }
        public int? GetNumericValue(int index)
        {
            return tree[index].GetNumericValue();
        }
        public int GetRoot()
        {
            return root;
        }
        public Symbol GetNode(int index)
        {
            return tree[index];
        }
        public int GetNode(Symbol symbol)
        {
            return reverseTree[symbol];
        }
        public int? GetParent(int index)
        {
            if (root == index)
            {
                return null;
            }
            else
            {
                return parentMap[index];
            }
        }
        public int? GetParent(int index, int depth)
        {
            if (root == index)
            {
                return null;
            }
            else
            {
                int nextParent = parentMap[index];

                for (int i = 0; i < depth; i ++)
                {
                    nextParent = parentMap[nextParent];
                }
                return nextParent;
            }
        }
        public List<int> GetChildren(int index)
        {
            return childMap[index];
        }
        public int GetChild(int index, int breadth)
        {
            return childMap[index][breadth];
        }
        public int GetChild(int index, List<int> path)
        {
            int nextChild = childMap[index][path[0]];

            for (int i = 1; i < path.Count; i ++)
            {
                nextChild = childMap[nextChild][path[i]];
            }

            return nextChild;
        }
        public void AddToMap(Symbol node)
        {
            int size = tree.Count;

            try 
            {
                reverseTree.Add(node, size);

                tree.Add(size, node);

                childMap[size] = new List<int>();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void SetRoot(Symbol node)
        {
            if (tree.Count == 0)
            {
                AddToMap(node);

                root = 0;
            }
            else
            {
                throw new Exception("Tree is not empty");
            }
        }
        public void SetRoot(Expression expression)
        {
            if (tree.Count == 0)
            {
                tree = expression.tree;

                parentMap = expression.parentMap;

                childMap = expression.childMap;

                reverseTree = expression.reverseTree;

                root = expression.GetRoot();
            }
            else
            {
                throw new Exception("Tree is not empty");
            }
        }
        public void AddNode(Symbol parent, Symbol child)
        {
            if (tree.Count == 0 && parent == null)
            {
                AddToMap(child);

                root = 0;
            }
            else if (tree.Count > 0 && parent != null)
            {
                AddToMap(child);

                int parentIndex = reverseTree[parent];

                int childIndex = tree.Count - 1; 

                parentMap[childIndex] = parentIndex;

                childMap[parentIndex].Add(childIndex);
            }
            else
            {
                throw new Exception("Incorrect format supplied");
            }
        }
        public void AddNode(Symbol parent, Expression expression, int transferIndex = 0)
        {
            Symbol transfer = expression.GetNode(transferIndex);

            AddToMap(transfer);

            int parentIndex = reverseTree[parent];

            parentMap[tree.Count - 1] = parentIndex;

            childMap[parentIndex].Add(reverseTree[transfer]); 

            foreach (int child in expression.GetChildren(transferIndex))
            {
                AddNode(transfer, expression, child);
            }
        }
        public void AddNode(Symbol parent, int child)
        {
            AddNode(parent, CopySubTree(child));
        }
        public void AddNode(Symbol parent, int child, Expression source)
        {
            AddNode(parent, source.CopySubTree(child));
        }
        public void AddBulkNodes(Symbol parent, List<int> children)
        {
            foreach (int child in children)
            {
                AddNode(parent, CopySubTree(child));
            }
        }
        public void AddBulkNodes(Symbol parent, List<int> children, Expression source)
        {
            foreach (int child in children)
            {
                AddNode(parent, source.CopySubTree(child));
            }
        }
        public Expression Sum()
        {
            throw new NotImplementedException();
        }
        public Expression Multiply(int lhs, int rhs)
        {
            Expression result = new Expression();

            Symbol mul = new Operation(true, SymbolType.Multiplication);

            result.SetRoot(mul);

            result.AddNode(mul, CopySubTree(lhs));

            result.AddNode(mul, CopySubTree(rhs));

            return result;
        }
        public Expression Multiply(List<int> children)
        {
            Expression result = new Expression();

            Symbol mul = new Operation(true, SymbolType.Multiplication);

            result.SetRoot(mul);

            foreach (int child in children)
            {
                result.AddNode(mul, CopySubTree(child));
            }
            return result;
        }
        public Expression Divide()
        {
            throw new NotImplementedException();
        }
        public Expression Raise()
        {
            throw new NotImplementedException();
        }
        public Expression Root()
        {
            throw new NotImplementedException();
        }
        public bool IsEqual(int first, int second)
        {
            if (GetNode(first).GetValue() == GetNode(second).GetValue())
            {
                List<int> firstChildren = GetChildren(first);

                List<int> secondChildren = GetChildren(second);

                if (firstChildren.Count != secondChildren.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < firstChildren.Count; i ++)
                    {
                        if (!IsEqual(firstChildren[i], secondChildren[i]))
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
        public bool IsEqual(int first, int second, Expression other)
        {
            if (GetNode(first).GetValue() == GetNode(second).GetValue())
            {
                List<int> firstChildren = GetChildren(first);

                List<int> secondChildren = other.GetChildren(second);

                if (firstChildren.Count != secondChildren.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < firstChildren.Count; i ++)
                    {
                        if (!IsEqual(firstChildren[i], secondChildren[i], other))
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
        public bool IsEqual(Expression other)
        {
            return IsEqual(root, other.GetRoot());
        }
        public Expression CopyTree()
        {
            Expression copiedExpression = new Expression();

            copiedExpression.parentMap = new Dictionary<int, int>(parentMap);
            
            copiedExpression.childMap = new Dictionary<int, List<int>>(childMap);

            foreach (Symbol symbol in tree.Values)
            {
                Symbol copy = symbol.Copy();

                copiedExpression.AddToMap(copy);
            }
            return copiedExpression;
        }
        public Expression CopySubTree(int parent, Symbol copiedParent = null, Expression copiedExpression = null)
        {
            if (copiedExpression == null)
            {
                copiedExpression = new Expression();

                copiedParent = GetNode(parent).Copy();

                copiedExpression.SetRoot(copiedParent);
            }
            foreach (int child in childMap[parent])
            {
                Symbol copiedChild = GetNode(child).Copy();

                copiedExpression.AddNode(copiedParent, copiedChild);

                CopySubTree(child, copiedChild, copiedExpression);
            }
            return copiedExpression;
        }
        public bool IsLikeTerm(int first, int second)
        {
            if (GetNode(first).IsMultiplication() && GetNode(second).IsMultiplication())
            {
                List<int> firstTerms = GetTerms(first);

                List<int> secondTerms = GetTerms(second);

                if (firstTerms.Count != secondTerms.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < firstTerms.Count; i ++)
                    {
                        if (!IsEqual(firstTerms[i], secondTerms[i]))
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
        public Expression SumLikeTerms(int index)
        {
            Expression result = new Expression();

            Symbol add = new Operation(true, SymbolType.Summation);

            result.SetRoot(add);

            int totalSum = 0;

            if (GetNode(index).IsSummation())
            {
                List<int> children = GetChildren(index);

                List<int> visited = new List<int>();

                for (int i = 0; i < children.Count; i ++)
                {
                    if (!visited.Contains(i))
                    {
                        int compared = children[i];

                        visited.Add(i);

                        totalSum += GetCoefficient(children[i]);

                        for (int j = i + 1; j < children.Count; j ++)
                        {
                            if (!visited.Contains(j))
                            {
                                if (IsLikeTerm(compared, children[j]))
                                {
                                    visited.Add(j);

                                    totalSum += GetCoefficient(children[j]);
                                }
                            }
                        }
                        Expression summed = CopySubTree(children[i]);

                        if (totalSum > 1)
                        {
                            summed.SetCoefficient(0, totalSum);    
                        }

                        result.AddNode(add, summed);
                    }
                    
                } 
                return result;
            }
            else
            {
                return this;
            }
        }
        public List<Expression> Distribute(List<int> symbols, int currentIndex, Dictionary<int, int> sumMap)
        {
            List<Expression> multiplications = new List<Expression>();

            List<int> children = new List<int>();

            if (GetNode(symbols[currentIndex]).IsSummation())
            {
                children = GetChildren(symbols[currentIndex]);
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
                    multiplications.AddRange(Distribute(symbols, currentIndex + 1, sumMap));
                }
                else
                {
                    multiplications.Add(Multiply(sumMap.Values.ToList()));
                }
            }
            return multiplications;
        }
        public Expression Distribute(int index)
        {
            if (GetNode(index).IsMultiplication())
            {
                Expression result = new Expression();

                Symbol add = new Operation(true, SymbolType.Summation);

                result.SetRoot(add);

                foreach(Expression multiplication in Distribute(GetChildren(index), 0, new Dictionary<int, int>()))
                {
                    result.AddNode(add, multiplication);
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        public Expression Cancel(int index)
        {
            // factor first

            if (GetNode(index).IsDivision())
            {
                int num = GetChild(index, 0);

                int denom = GetChild(index, 1);

                List<int> numChildren = new List<int>();

                List<int> denomChildren = new List<int>();

                if (GetNode(num).IsMultiplication())
                {
                    numChildren = GetChildren(num);
                }
                else
                {
                    numChildren.Add(num);
                }
                if (GetNode(denom).IsMultiplication())
                {
                    denomChildren = GetChildren(denom);
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
                        if (IsEqual(numChildren[i], denomChildren[j]))
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
                        if (IsEqual(denomChildren[i], numChildren[j]))
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
                    result.SetRoot(CopySubTree(cancelledNums[0]));
                    
                }
                else if (cancelledNums.Count == 0 && cancelledDenoms.Count == 1)
                {
                    result.SetRoot(CopySubTree(cancelledDenoms[0]));
                }
                else
                {
                    Symbol div = new Operation(true, SymbolType.Division);

                    Symbol numMul = new Operation(true, SymbolType.Multiplication);

                    Symbol denomMul = new Operation(true, SymbolType.Multiplication);

                    result.SetRoot(div);

                    result.AddNode(div, numMul);

                    result.AddNode(div, denomMul);

                    result.AddBulkNodes(numMul, cancelledNums, this);

                    result.AddBulkNodes(denomMul, cancelledDenoms, this);
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        public int GetCoefficient(int index)
        {
            if (GetNode(index).IsMultiplication())
            {
                foreach (int child in GetChildren(index))
                {
                    if (GetNode(child).IsConstant())
                    {
                        if (GetNode(child).GetNumericValue() != null)
                        {
                            return (int) GetNode(child).GetNumericValue();
                        }
                    }
                }
                return 1;
            }
            else 
            {
                return 1;
            }
        }
        public void SetCoefficient(int index, int coefficient)
        {
            if (GetNode(index).IsMultiplication())
            {
                foreach (int child in GetChildren(index))
                {
                    if (GetNode(child).IsConstant())
                    {
                        GetNode(child).SetNumericValue(coefficient);

                        return;
                    }
                }
                AddNode(GetNode(index), new Constant(true, coefficient));
            }
        }
        public List<int> GetTerms(int index)
        {
            List<int> terms = new List<int>();

            if (GetNode(index).IsMultiplication())
            {
                foreach (int child in GetChildren(index))
                {
                    if (!GetNode(child).IsConstant())
                    {
                        terms.Add(child);
                    }
                }
                return terms;
            }
            else 
            {
                return null;
            }
        }
        public string ToString(string result, int index)
        {
            Symbol symbol = GetNode(index);

            string symbolStr = symbol.ToString();

            if (symbol.IsOperation())
            {
                result += '(';

                List<int> children = childMap[index];

                for (int i = 0; i < children.Count; i ++)
                {
                    if (i == 0)
                    {
                        result = ToString(result, children[i]);
                    }
                    else
                    {
                        result = ToString(result + symbolStr, children[i]);
                    }
                }
                result += ')';
            }
            else
            {
                result += symbolStr;
            }
            return result;
        }

        public override string ToString()
        {
            string result = "";

            if (tree.Count == 0)
            {
                return result;
            }
            else
            {
                return ToString(result, root);
            }
        }
    }
}