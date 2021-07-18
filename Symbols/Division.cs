using System;
using LSharp.Visitors;

namespace LSharp.Symbols
{
    public class Division : Symbol
    {
        public override void dispatch(Visitor visitor)
        {
            visitor.visit(this);
        }
        public override void sanitise()
        {
            foreach (Symbol child in children){
                child.sanitise();
            }
        }
        public override Nullable<int> getValue()
        {
            return null;
        }
        public override Symbol evaluate()
        {
            Symbol result = children[0];

            for (int i = 0; i < children.Count; i ++){

                Symbol lhs = result.evaluate();
                Symbol rhs = children[i + 1].evaluate();

                result = lhs.divide(rhs);

                if (result == null){
                    result = children[i + 1];
                }
            }
            return result;
        }
        public override Symbol sum(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol multiply(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol divide(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol raise(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol floor(Symbol other)
        {
            throw new NotImplementedException();
        }
    }
}