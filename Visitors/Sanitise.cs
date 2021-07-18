using System;
using System.Collections.Generic;
using MathSharp.Symbols;

namespace MathSharp.Visitors
{
    public class SanitiseMultiplication : Visitor
    {
        public Symbol parent { get; set; }
        public int coefficient { get; set; } = 1;
        public bool childRemoved { get; set; } = false;
        public SanitiseMultiplication(Symbol parent){ this.parent = parent; }
        public override void visit(Summation child){ childRemoved = false; }
        public override void visit(Multiplication child)
        { 
            foreach (Symbol grandchild in child.children){
                parent.children.Add(grandchild);
            }
            child.parentExpression.removeNode(child);
            childRemoved = true;
        }
        public override void visit(Division child){ childRemoved = false; }
        public override void visit(Exponent child){ childRemoved = false; }
        public override void visit(Radical child){ childRemoved = false; }
        public override void visit(Variable child){ childRemoved = false; }
        public override void visit(Constant child)
        {
            if (child.getValue() != null){
                coefficient *= (int) child.getValue();
            }
            child.parentExpression.removeNode(child);
            childRemoved = true;
        }
    }
}