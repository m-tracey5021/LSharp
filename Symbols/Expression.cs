using System;
using System.Linq;
using System.Collections.Generic;
using LSharp.Comparison;
using LSharp.Comparison.EqualityComparison;
using LSharp.Evaluation;
using LSharp.Evaluation.ConstantEvaluation;
using LSharp.Manipulation;
using LSharp.Manipulation.SummationManipulation;
using LSharp.Manipulation.MultiplicationManipulation;
using LSharp.Manipulation.DivisionManipulation;

namespace LSharp.Symbols
{
    public class Expression
    {
        private int root { get; set; }
        private Dictionary<int, Symbol> tree { get; set; }
        private Dictionary<int, int> parentMap { get; set; }
        private Dictionary<int, List<int>> childMap { get; set; }
        private Dictionary<Symbol, int> reverseTree { get; set; }
        private IComparisonStrategy comparisonStrategy { get; set; }
        private IEvaluationStrategy evaluationStrategy { get; set; }
        private IManipulationStrategy manipulationStrategy { get; set; }
        public Expression()
        {
            tree = new Dictionary<int, Symbol>();
            reverseTree = new Dictionary<Symbol, int>();
            parentMap = new Dictionary<int, int>();
            childMap = new Dictionary<int, List<int>>();
        }

        /*
        =================

            Retrieval

        =================
        */

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

        /*
        ===================

            Identifying

        ===================    
        */

        public bool IsOperation(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Summation ||
                    type == SymbolType.Multiplication || 
                    type == SymbolType.Division ||
                    type == SymbolType.Exponent ||
                    type == SymbolType.Radical)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool IsSummation(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Summation)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool IsMultiplication(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Multiplication)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool IsDivision(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Division)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool IsExponent(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Exponent)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool IsRadical(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Radical)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool IsAtomic(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Variable || type == SymbolType.Constant)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool IsVariable(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Variable)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool IsConstant(int index)
        {
            SymbolType type = GetNode(index).type;

            if (type == SymbolType.Constant)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        /*
        =============================

            Generating and Adding

        =============================
        */

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
        public void AppendBulkNodes(int parent, List<Expression> children)
        {
            foreach (Expression child in children)
            {
                AppendNode(parent, child);
            }
        }

        /*
        ===============================

            Replacing and Removing

        ===============================
        */

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

        /*
        ==================

            Arithmetic

        ==================    
        */

        public void Negate()
        {
            GetNode(root).sign = false;
        }
        public Expression Add()
        {
            throw new NotImplementedException();
        }
        public Expression Subtract(Expression other)
        {
            Expression sub = new Expression();

            sub.SetRoot(new Operation(true, SymbolType.Summation));

            sub.AppendNode(0, CopyTree());

            Expression lhs = other.CopyTree();

            lhs.Negate();

            sub.AppendNode(0, lhs);

            return sub;
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
        // public bool IsEqual(int first, int second, Expression other = null)
        // {
        //     if (other == null)
        //     {
        //         other = this;
        //     }
        //     if (GetNode(first).GetValue() == other.GetNode(second).GetValue())
        //     {
        //         List<int> firstChildren = GetChildren(first);

        //         List<int> secondChildren = other.GetChildren(second);

        //         if (firstChildren.Count != secondChildren.Count)
        //         {
        //             return false;
        //         }
        //         else
        //         {
        //             for (int i = 0; i < firstChildren.Count; i ++)
        //             {
        //                 if (!IsEqual(firstChildren[i], secondChildren[i], other))
        //                 {
        //                     return false;
        //                 }
        //             }
        //             return true;
        //         }
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
        // public bool IsEqual(Expression other)
        // {
        //     return IsEqual(root, other.GetRoot());
        // }
        // public bool IsEqualByBase(int first, int second, Expression other = null)
        // {
        //     if (other == null)
        //     {
        //         other = this;
        //     }
        //     Symbol firstSymbol = GetNode(first);

        //     Symbol secondSymbol = other.GetNode(second);
            
        //     if (GetNode(first).GetValue() == other.GetNode(second).GetValue())
        //     {
        //         if (GetNode(first).IsExponent() && other.GetNode(second).IsExponent())
        //         {
        //             if (!IsEqual(GetChild(first, 0), other.GetChild(second, 0))) // ignore the exponents
        //             {
        //                 return false;
        //             }
        //             else
        //             {
        //                 return true;
        //             }
        //         }
        //         else
        //         {
        //             List<int> firstChildren = GetChildren(first);

        //             List<int> secondChildren = other.GetChildren(second);

        //             if (firstChildren.Count != secondChildren.Count)
        //             {
        //                 return false;
        //             }
        //             else
        //             {
        //                 for (int i = 0; i < firstChildren.Count; i ++)
        //                 {
        //                     if (!IsEqual(firstChildren[i], secondChildren[i]))
        //                     {
        //                         return false;
        //                     }
        //                 }
        //                 return true;
        //             }
        //         }
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
        // public bool IsEqualByBase(Expression other)
        // {
        //     return IsEqualByBase(root, other.GetRoot());
        // }

        /*
        ===============

            Copying

        ===============
        */

        public Expression CopyTree()
        {
            Expression copiedExpression = new Expression();

            copiedExpression.tree = new Dictionary<int, Symbol>(tree);

            copiedExpression.parentMap = new Dictionary<int, int>(parentMap);
            
            copiedExpression.childMap = new Dictionary<int, List<int>>(childMap);

            copiedExpression.reverseTree = new Dictionary<Symbol, int>(reverseTree);

            // foreach (int node in tree.Keys)
            // {
            //     Symbol copy = tree[node].Copy();

            //     int? parent = GetParent(node);

            //     List<int> children = childMap[node];

            //     copiedExpression.AddToMap(copy, parent, children);
            // }
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
            if (IsMultiplication(index))
            {
                foreach (int child in GetChildren(index))
                {
                    if (IsConstant(child))
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
            if (IsMultiplication(index))
            {
                foreach (int child in GetChildren(index))
                {
                    if (IsConstant(child))
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

            if (IsMultiplication(index))
            {
                foreach (int child in GetChildren(index))
                {
                    if (!IsConstant(child))
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
        public bool Compare(ComparisonInstruction instruction, Expression other = null, int? first = null, int? second = null)
        {
            if (instruction == ComparisonInstruction.IsEqual)
            {
                comparisonStrategy = new IsEqualStrategy(CopyTree(), other, first, second);
            }
            else if (instruction == ComparisonInstruction.IsEqualByBase)
            {
                comparisonStrategy = new IsEqualByBaseStrategy(CopyTree(), other, first, second);
            }
            else
            {
                throw new Exception("Instruction not recognised");
            }
            return comparisonStrategy.Compare();
        }
        public Expression Evaluate(EvaluationInstruction instruction)
        {
            if (instruction == EvaluationInstruction.EvaluateConstants)
            {
                evaluationStrategy = new ConstantEvaluationStrategy(CopyTree());
            }
            else
            {
                throw new Exception("Instruction not recognised");
            }
            Expression result = evaluationStrategy.Evaluate();

            return result;
        }
        public Expression Manipulate(ManipulationInstruction instruction)
        {
            if (instruction == ManipulationInstruction.SumLikeTerms)
            {
                manipulationStrategy = new LikeTermStrategy(CopyTree());
            }
            else if (instruction == ManipulationInstruction.Distribute)
            {
                manipulationStrategy = new DistributionStrategy(CopyTree());
            }
            else if (instruction == ManipulationInstruction.Cancel)
            {
                manipulationStrategy = new CancellationStrategy(CopyTree());
            }
            else
            {
                throw new Exception("Instruction not recognised");
            }
            Expression result = manipulationStrategy.Manipulate();

            return result;
        }

        /*
        ================

            Printers

        ================    
        */

        public string ToString(int index)
        {
            Symbol symbol = GetNode(index);

            int? parent = GetParent(index);

            if (IsAtomic(index))
            {
                return symbol.ToString();
            }
            else
            {
                string operation = "";

                List<int> children = childMap[index];

                for (int i = 0; i < children.Count; i ++)
                {
                    if (i == 0)
                    {
                        operation += ToString(children[i]);
                    }
                    else
                    {   
                        operation += symbol.ToString() + ToString(children[i]);
                    }
                }
                if (!symbol.sign)
                {
                    return "-(" + operation + ")";
                }
                else
                {
                    if (parent == null)
                    {
                        return operation;
                    }
                    else
                    {   
                        return "(" + operation + ")";
                    }
                }
            }
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
                return ToString(root);
            }
        }
    }
}