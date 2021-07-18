using System;
using LSharp.Symbols;

namespace LSharp.Visitors
{
    public class Append : Visitor
    {
        public Symbol child { get; set; }
        public Append(Symbol child){ this.child = child; }
        public override void visit(Summation parent){ child.dispatch(new AppendToSumOp(parent)); }
        public override void visit(Multiplication parent){ child.dispatch(new AppendToMulOp(parent)); }
        public override void visit(Division parent){ AppendUtility.append(parent, child); }
        public override void visit(Exponent parent){ AppendUtility.append(parent, child); }
        public override void visit(Radical parent){ AppendUtility.append(parent, child); }
        public override void visit(Variable parent){ AppendUtility.append(parent, child); }
        public override void visit(Constant parent){ AppendUtility.append(parent, child); }

    }
    public class AppendToSumOp : Visitor
    {
        public Symbol parent { get; set; }
        public AppendToSumOp(Symbol parent){ this.parent = parent; }
        public override void visit(Summation child){ AppendUtility.appendEach(parent, child); }
        public override void visit(Multiplication child){ AppendUtility.append(parent, child); }
        public override void visit(Division child){ AppendUtility.append(parent, child); }
        public override void visit(Exponent child){ AppendUtility.append(parent, child); }
        public override void visit(Radical child){ AppendUtility.append(parent, child); }
        public override void visit(Variable child){ AppendUtility.append(parent, child); }
        public override void visit(Constant child){ AppendUtility.append(parent, child); }

    }
    public class AppendToMulOp : Visitor
    {
        public Symbol parent { get; set; }
        public AppendToMulOp(Symbol parent){ this.parent = parent; }
        public override void visit(Summation child){ AppendUtility.append(parent, child); }
        public override void visit(Multiplication child){ AppendUtility.appendEach(parent, child); }
        public override void visit(Division child){ AppendUtility.append(parent, child); }
        public override void visit(Exponent child){ AppendUtility.append(parent, child); }
        public override void visit(Radical child){ AppendUtility.append(parent, child); }
        public override void visit(Variable child){ AppendUtility.append(parent, child); }
        public override void visit(Constant child){ AppendUtility.append(parent, child); }

    }
    public static class AppendUtility 
    {
        public static void append(Symbol parent, Symbol child){
            parent.children.Add(child);
        }
        public static void appendEach(Symbol parent, Symbol child){
            foreach (Symbol grandchild in child.children){
                parent.children.Add(grandchild);
            }
        }
    }
}