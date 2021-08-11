using System;
using System.Collections.Generic;
using LSharp.Rules;
using LSharp.Selectors;

namespace LSharp.Symbols
{
    public class Radical : Symbol
    {
        public Radical(){ this.sign = true; this.variable = true; }

        public override void Dispatch(Selector selector){ selector.Select(this); }
        public override int? GetValue(){ return null; }
        public override Symbol Sum(Symbol other)
        {
            return other.Sum(this);
        }
        public override Symbol Sum(Summation other)
        {
            Console.WriteLine("Radical + Summation");
            return null;
        }
        public override Symbol Sum(Multiplication other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Division other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Exponent other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Radical other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Variable other)
        {
            throw new NotImplementedException();
        }
        public override Symbol Sum(Constant other)
        {
            throw new NotImplementedException();
        }
        public override bool IsEqual(Symbol other){ return other.IsEqual(this); }
        public override bool IsEqual(Summation other){ return false; }
        public override bool IsEqual(Multiplication other){ return false; }
        public override bool IsEqual(Division other){ return false; }
        public override bool IsEqual(Exponent other){ return false; }
        public override bool IsEqual(Radical other)
        {
            List<int> children = GetChildren();
            List<int> otherChildren = other.GetChildren();

            if (children.Count == otherChildren.Count)
            {
                for (int i = 0; i < children.Count; i ++)
                {
                    if (!expression.GetNode(children[i]).IsEqual(expression.GetNode(otherChildren[i])))
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
        public override bool IsEqual(Variable other){ return false; }
        public override bool IsEqual(Constant other){ return false; }
        public override Expression SumLikeTerm(Symbol other){ return null; }
        public override bool TestAgainstStage(StructureStage stage)
        {
            if (stage.type == 'v' || stage.type == 'x')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override Symbol Copy()
        {
            return new Radical() { sign = this.sign };
        }
        public override string ToString()
        {
            return Char.ToString('v');
        }
    }
}