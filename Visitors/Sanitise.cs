using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Visitors
{
    public class SanitiseMultiplication : Visitor
    {
        public Symbol parent { get; set; }
        public int coefficient { get; set; } = 1;
        public bool childRemoved { get; set; } = false;
        public SanitiseMultiplication(Symbol parent){ this.parent = parent; }
        public override void Visit(Summation child){ childRemoved = false; }
        public override void Visit(Multiplication child)
        { 
            foreach (Symbol grandchild in child.children){
                parent.children.Add(grandchild);
            }
            child.parentExpression.RemoveNode(child);
            childRemoved = true;
        }
        public override void Visit(Division child){ childRemoved = false; }
        public override void Visit(Exponent child){ childRemoved = false; }
        public override void Visit(Radical child){ childRemoved = false; }
        public override void Visit(Variable child){ childRemoved = false; }
        public override void Visit(Constant child)
        {
            if (child.GetValue() != null){
                coefficient *= (int) child.GetValue();
            }
            child.parentExpression.RemoveNode(child);
            childRemoved = true;
        }
    }
}