using System;
using System.Collections.Generic;
using LSharp.Visitors;

namespace LSharp.Symbols
{
    public class Multiplication : Symbol
    {
        public override void Dispatch(Visitor visitor)
        {
            visitor.Visit(this);
        }
        public override SymbolFlat Flatten()
        {
            return new SymbolFlat(SymbolType.Multiplication, symbol);
        }
        public override void Sanitise()
        {
            SanitiseMultiplication sanitise = new SanitiseMultiplication(this);
            for (int i = 0; i < children.Count; i ++){
                children[i].Sanitise();
                children[i].Dispatch(sanitise);
                if (sanitise.childRemoved){
                    i --;
                }
            }
            if (sanitise.coefficient > 1){
                Symbol coefficient = new Constant(true, sanitise.coefficient);
                children.Insert(0, coefficient);
            }
        }
        public override Nullable<int> GetValue()
        {
            return null;
        }
        public override Symbol Evaluate()
        {
            Symbol result = children[0];

            for (int i = 0; i < children.Count; i ++){

                Symbol lhs = result.Evaluate();
                Symbol rhs = children[i + 1].Evaluate();

                result = lhs.Multiply(rhs);

                if (result == null){
                    result = children[i + 1];
                }
            }
            return result;
        }
        public override Symbol Sum(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Multiply(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Divide(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Raise(Symbol other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Floor(Symbol other)
        {
            throw new NotImplementedException();
        }
    }
}