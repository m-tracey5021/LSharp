using System;
using System.Linq;
using System.Collections.Generic;
using LSharp.Manipulation;
using LSharp.Manipulation.SummationStrategies;
using LSharp.Manipulation.MultiplicationStrategies;
using LSharp.Manipulation.DivisionStrategies;

namespace LSharp.Symbols
{
    public class Expression
    {
        private int root { get; set; }
        private Dictionary<int, Symbol> tree { get; set; }
        private Dictionary<int, int> parentMap { get; set; }
        private Dictionary<int, List<int>> childMap { get; set; }
        private Dictionary<Symbol, int> reverseTree { get; set; }
        private IManipulationStrategy manipulationStrategy { get; set; }
        public Expression()
        {
            tree = new Dictionary<int, Symbol>();
            reverseTree = new Dictionary<Symbol, int>();
            parentMap = new Dictionary<int, int>();
            childMap = new Dictionary<int, List<int>>();
        }
        public int GetRoot()
        {
            return root;
        }
        public Dictionary<int, Symbol> GetTree()
        {
            return tree;
        }
        public int? GetNumericValue(int index)
        {
            return tree[index].GetNumericValue();
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
        public int GenerateId()
        {
            int id = 0;

            while (tree.Keys.Contains(id))
            {
                id ++;
            }
            return id;
        }
        public int AddToMap(Symbol node)
        {
            int id = GenerateId();

            try 
            {
                reverseTree.Add(node, id);

                tree.Add(id, node);

                childMap[id] = new List<int>();

                return id;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public int AddToMap(Symbol node, int? parent, List<int> children)
        {
            int id = GenerateId();

            try 
            {
                reverseTree.Add(node, id);

                tree.Add(id, node);

                if (parent != null)
                {
                    parentMap[id] = (int) parent;
                }
                childMap[id] = children;

                return id;
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
            tree = expression.tree;

            parentMap = expression.parentMap;

            childMap = expression.childMap;

            reverseTree = expression.reverseTree;

            root = expression.GetRoot();
        }
        public int AppendNode(int parent, Symbol child)
        {
            int index = AddToMap(child);

            int childIndex = tree.Count - 1; 

            parentMap[childIndex] = parent;

            childMap[parent].Add(childIndex);

            return index;
        }
        public int AppendNode(int parent, Expression expression, int transferIndex = 0)
        {
            Symbol transfer = expression.GetNode(transferIndex);

            int index = AddToMap(transfer);

            parentMap[index] = parent;

            childMap[parent].Add(index); 

            foreach (int child in expression.GetChildren(transferIndex))
            {
                AppendNode(index, expression, child);
            }
            return index;
        }
        public int AppendNode(int parent, int child)
        {
            return AppendNode(parent, CopySubTree(child));
        }
        public int AppendNode(int parent, int child, Expression source)
        {
            return AppendNode(parent, source.CopySubTree(child));
        }
        public void AppendBulkNodes(int parent, List<int> children)
        {
            foreach (int child in children)
            {
                AppendNode(parent, CopySubTree(child));
            }
        }
        public void AppendBulkNodes(int parent, List<int> children, Expression source)
        {
            foreach (int child in children)
            {
                AppendNode(parent, source.CopySubTree(child));
            }
        }
        public void ReplaceNode(int index, Symbol symbol)
        {
            Symbol replaced = tree[index];

            tree[index] = symbol;

            reverseTree[symbol] = index;

            reverseTree.Remove(replaced);
        }
        public void ReplaceNode(int index, Expression expression)
        {
            int? parent = GetParent(index);

            if (parent == null)
            {
                SetRoot(expression);
            }
            else if (parent != null && tree.Count != 0)
            {
                int otherRoot = expression.GetRoot();

                tree[index] = expression.GetNode(otherRoot);

                while (GetChildren(index).Count != 0)
                {
                    RemoveNode(GetChild(index, 0));
                }
                foreach (int otherChild in expression.GetChildren(otherRoot))
                {
                    AppendNode(index, expression, otherChild);
                }                
            }
            else
            {
                throw new Exception("Node does not exist in tree");
            }
        }
        public void RemoveNode(int index, bool startIndex = true)
        {
            foreach (int child in GetChildren(index))
            {
                RemoveNode(child, false);
            }
            if (startIndex && index != GetRoot())
            {
                int? parent = GetParent(index);

                if (parent != null)
                {
                    childMap[(int) parent].Remove(index);
                }
            }
            Symbol removed = tree[index];

            tree.Remove(index);

            parentMap.Remove(index);

            childMap.Remove(index);
            
            reverseTree.Remove(removed);
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

            result.AppendNode(0, CopySubTree(lhs));

            result.AppendNode(0, CopySubTree(rhs));

            return result;
        }
        public Expression Multiply(List<int> children)
        {
            Expression result = new Expression();

            Symbol mul = new Operation(true, SymbolType.Multiplication);

            result.SetRoot(mul);

            foreach (int child in children)
            {
                result.AppendNode(0, CopySubTree(child));
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
        public bool IsEqualByBase(int first, int second)
        {
            Symbol firstSymbol = GetNode(first);

            Symbol secondSymbol = GetNode(second);
            
            if (GetNode(first).GetValue() == GetNode(second).GetValue())
            {
                if (GetNode(first).IsExponent() && GetNode(second).IsExponent())
                {
                    if (!IsEqual(GetChild(first, 0), GetChild(second, 0))) // ignore the exponents
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
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

            foreach (int node in tree.Keys)
            {
                Symbol copy = tree[node].Copy();

                int? parent = GetParent(node);

                List<int> children = childMap[node];

                copiedExpression.AddToMap(copy, parent, children);
            }
            return copiedExpression;
        }
        public Expression CopySubTree(int parent, int? copiedParent = null, Expression copiedExpression = null)
        {
            if (copiedExpression == null)
            {
                copiedExpression = new Expression();

                copiedParent = 0;

                copiedExpression.SetRoot(GetNode(parent).Copy());
            }
            foreach (int child in childMap[parent])
            {
                Symbol copiedChild = GetNode(child).Copy();

                int index = copiedExpression.AppendNode((int) copiedParent, copiedChild);

                CopySubTree(child, index, copiedExpression);
            }
            return copiedExpression;
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
                AppendNode(index, new Constant(true, coefficient));
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
        public Expression Manipulate(ManipulationInstruction instruction)
        {
            if (instruction == ManipulationInstruction.SumLikeTerms)
            {
                manipulationStrategy = new SumLikeTermManipulation();
            }
            else if (instruction == ManipulationInstruction.Distribute)
            {
                manipulationStrategy = new DistributeManipulation();
            }
            else
            {
                return null;
            }
            return manipulationStrategy.Manipulate(this);
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