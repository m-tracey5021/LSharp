using System;
using LSharp.Symbols;

namespace LSharp.Visitors
{
    public class Append : Visitor
    {
        public Symbol child { get; set; }
        public Append(Symbol child){ this.child = child; }
        public override void Visit(Summation parent){ child.Dispatch(new AppendToSumOp(parent)); }
        public override void Visit(Multiplication parent){ child.Dispatch(new AppendToMulOp(parent)); }
        public override void Visit(Division parent){ AppendUtility.Append(parent, child); }
        public override void Visit(Exponent parent){ AppendUtility.Append(parent, child); }
        public override void Visit(Radical parent){ AppendUtility.Append(parent, child); }
        public override void Visit(Variable parent){ AppendUtility.Append(parent, child); }
        public override void Visit(Constant parent){ AppendUtility.Append(parent, child); }

    }
    public class AppendToSumOp : Visitor
    {
        public Symbol parent { get; set; }
        public AppendToSumOp(Symbol parent){ this.parent = parent; }
        public override void Visit(Summation child){ AppendUtility.AppendEach(parent, child); }
        public override void Visit(Multiplication child){ AppendUtility.Append(parent, child); }
        public override void Visit(Division child){ AppendUtility.Append(parent, child); }
        public override void Visit(Exponent child){ AppendUtility.Append(parent, child); }
        public override void Visit(Radical child){ AppendUtility.Append(parent, child); }
        public override void Visit(Variable child){ AppendUtility.Append(parent, child); }
        public override void Visit(Constant child){ AppendUtility.Append(parent, child); }

    }
    public class AppendToMulOp : Visitor
    {
        public Symbol parent { get; set; }
        public AppendToMulOp(Symbol parent){ this.parent = parent; }
        public override void Visit(Summation child){ AppendUtility.Append(parent, child); }
        public override void Visit(Multiplication child){ AppendUtility.AppendEach(parent, child); }
        public override void Visit(Division child){ AppendUtility.Append(parent, child); }
        public override void Visit(Exponent child){ AppendUtility.Append(parent, child); }
        public override void Visit(Radical child){ AppendUtility.Append(parent, child); }
        public override void Visit(Variable child){ AppendUtility.Append(parent, child); }
        public override void Visit(Constant child){ AppendUtility.Append(parent, child); }

    }
    public static class AppendUtility 
    {
        public static void Append(Symbol parent, Symbol child){
            parent.children.Add(child);
        }
        public static void AppendEach(Symbol parent, Symbol child){
            foreach (Symbol grandchild in child.children){
                parent.children.Add(grandchild);
            }
        }
    }
}