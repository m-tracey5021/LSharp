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
        public int GetParent(int index)
        {
            return parentMap[index];
        }
        public int GetParent(int index, int depth)
        {
            int nextParent = parentMap[index];

            for (int i = 0; i < depth; i ++)
            {
                nextParent = parentMap[nextParent];
            }

            return nextParent;
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
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void AddNode(Symbol node)
        {
            if (tree.Count == 0)
            {
                AddToMap(node);

                // node.expression = this;
                // node.index = 0;

                root = 0;

                childMap[root] = new List<int>();
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

                // child.expression = this;
                // child.index = 0;

                root = 0;

                childMap[root] = new List<int>();
            }
            else if (tree.Count > 0 && parent != null)
            {
                AddToMap(child);

                int parentIndex = reverseTree[parent];

                int childIndex = tree.Count - 1; 

                // child.expression = this;
                // child.index = childIndex;

                parentMap[childIndex] = parentIndex;

                childMap[parentIndex].Add(childIndex);

                childMap[childIndex] = new List<int>();
            }
            else
            {
                throw new Exception("Incorrect format supplied");
            }
        }
        public void AddNode(Symbol parent, Expression children)
        {
            foreach (KeyValuePair<int, Symbol> child in children.tree)
            {
                int offset = tree.Count;

                parentMap.Add(offset, children.parentMap[child.Key] + offset);
                childMap.Add(offset, children.childMap[child.Key].Select(x => x + offset).ToList());

                AddToMap(child.Value);
            }
        }
        public bool IsEqual(Expression other)
        {
            if (parentMap == other.parentMap && childMap == other.childMap)
            {
                if (tree.Count == other.tree.Count)
                {
                    for (int i = 0; i < tree.Count; i ++)
                    {
                        if (tree[i].GetValue() != other.tree[i].GetValue())
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsEqualSubTree(int first, int second)
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
                        if (!IsEqualSubTree(firstChildren[i], secondChildren[i]))
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
        public Expression CopyTree()
        {
            Expression copiedExpression = new Expression();

            copiedExpression.parentMap = new Dictionary<int, int>(parentMap);
            copiedExpression.childMap = new Dictionary<int, List<int>>(childMap);

            foreach (Symbol symbol in tree.Values)
            {
                Symbol copy = symbol.Copy();

                // copy.expression = copiedExpression;

                copiedExpression.AddToMap(copy);
            }
            return copiedExpression;
        }

        public Expression CopySubTree(int index)
        {
            Expression copiedExpression = new Expression();

            Symbol copiedRoot = GetNode(index).Copy();

            copiedExpression.AddNode(copiedRoot);

            foreach (int child in childMap[index])
            {
                Symbol copiedChild = GetNode(child).Copy();

                copiedExpression.AddNode(copiedRoot, copiedChild);  

                CopySubTree(child, copiedChild, ref copiedExpression);              
            }
            return copiedExpression;
        }
        public Expression CopySubTree(int parent, Symbol copiedParent, ref Expression copiedExpression)
        {
            foreach (int child in childMap[parent])
            {
                Symbol copiedChild = GetNode(child).Copy();

                copiedExpression.AddNode(copiedParent, copiedChild);

                CopySubTree(child, copiedChild, ref copiedExpression);
            }
            return copiedExpression;
        }

        public Expression SumLikeTerms(int first, int second)
        {
            List<int> firstTerms = GetTerms(first);

            List<int> secondTerms = GetTerms(second);

            int totalSum = GetCoefficient(first) + GetCoefficient(second);

            Expression result = new Expression();

            if (firstTerms.Count == secondTerms.Count)
            {
                for (int i = 0; i < firstTerms.Count; i ++)
                {
                    if (!IsEqualSubTree(firstTerms[i], secondTerms[i]))
                    {
                        return null;
                    }
                }
                Symbol multiplication = new Operation(true, SymbolType.Multiplication);

                Symbol coefficient = new Constant(true, totalSum);

                result.AddNode(multiplication);

                if (totalSum > 1)
                {
                    result.AddNode(multiplication, coefficient);
                }
                foreach (int term in firstTerms)
                {
                    result.AddNode(multiplication, CopySubTree(term));
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
                        if (GetNode(index).GetNumericValue() != null)
                        {
                            return (int) GetNode(index).GetNumericValue();
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