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

                node.expression = this;
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

                child.expression = this;
                // child.index = 0;

                root = 0;

                childMap[root] = new List<int>();
            }
            else if (tree.Count > 0 && parent != null)
            {
                AddToMap(child);

                int parentIndex = reverseTree[parent];

                int childIndex = tree.Count - 1; 

                child.expression = this;
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
                        if (!tree[i].IsEqual(other.tree[i]))
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
        public Expression CopyTree()
        {
            Expression copiedExpression = new Expression();

            copiedExpression.parentMap = new Dictionary<int, int>(parentMap);
            copiedExpression.childMap = new Dictionary<int, List<int>>(childMap);

            foreach (Symbol symbol in tree.Values)
            {
                Symbol copy = symbol.Copy();

                copy.expression = copiedExpression;

                copiedExpression.AddToMap(copy);
            }
            return copiedExpression;
        }

        public Expression CopySubTree(int index)
        {
            Expression copiedExpression = new Expression();

            Symbol rootCopy = GetNode(index).Copy();

            rootCopy.expression = copiedExpression;

            copiedExpression.AddNode(rootCopy);

            List<int> children = childMap[index];

            foreach (int childIndex in children)
            {
                Symbol childCopy = GetNode(childIndex).Copy();

                copiedExpression.AddNode(rootCopy, childCopy);

                CopySubTree(childIndex, copiedExpression.GetNode(childCopy), ref copiedExpression);
            }
            return copiedExpression;
        }
        public Expression CopySubTree(int indexInParent, int indexInCopy, ref Expression copiedExpression)
        {
            Symbol parentCopy = copiedExpression.GetNode(indexInCopy);

            List<int> children = childMap[indexInParent];

            foreach (int childIndex in children)
            {
                Symbol childCopy = GetNode(childIndex).Copy();

                copiedExpression.AddNode(parentCopy, childCopy);

                CopySubTree(childIndex, copiedExpression.GetNode(childCopy), ref copiedExpression);
            }
            return copiedExpression;
        }

        public override string ToString()
        {
            string result = "";

            foreach (Symbol symbol in tree.Values)
            {
                result += symbol.ToString();
            }
            return result;
        }
    }
}