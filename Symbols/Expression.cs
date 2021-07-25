using System;
using System.Collections.Generic;

namespace LSharp.Symbols
{
    public class Expression
    {
        private int root { get; set; }
        private List<Symbol> tree { get; set; }
        private Dictionary<int, int> parentMap { get; set; }
        private Dictionary<int, List<int>> childMap { get; set; }
        public Expression()
        {
            tree = new List<Symbol>();
            parentMap = new Dictionary<int, int>();
            childMap = new Dictionary<int, List<int>>();
        }
        public int Search(Symbol node)
        {
            for (int i = 0; i < tree.Count; i ++)
            {
                if (tree[i] == node)
                {
                    return i;
                }
            }
            return -1;
        }
        public Symbol GetNode(int index)
        {
            return tree[index];
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
        public void AddNode(Symbol node)
        {
            if (tree.Count == 0)
            {
                tree.Add(node);

                node.expression = this;

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
                tree.Add(child);

                child.expression = this;

                root = 0;

                childMap[root] = new List<int>();
            }
            else if (tree.Count > 0 && parent != null)
            {
                tree.Add(child);

                child.expression = this;

                int parentIndex = Search(parent);

                int childIndex = tree.Count - 1; 

                parentMap[childIndex] = parentIndex;

                childMap[parentIndex].Add(childIndex);

                childMap[childIndex] = new List<int>();
            }
            else
            {
                throw new Exception("Incorrect format supplied");
            }
        }
    }
}