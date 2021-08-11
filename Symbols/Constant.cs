using System;
using System.Collections.Generic;
using LSharp.Rules;
using LSharp.Selectors;

namespace LSharp.Symbols
{
    public class Constant : Symbol
    {
        public int value { get; set; }
        public Constant(){}
        public Constant(bool sign, int value){ this.sign = sign; this.value = value; this.variable = false; }

        public override void Dispatch(Selector selector){ selector.Select(this); }
        public override int? GetValue(){ return value; }
        public override Symbol Sum(Symbol other)
        {
            return other.Sum(this);
        }
        public override Symbol Sum(Summation other)
        {
            throw new NotImplementedException();
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
        public override bool IsEqual(Radical other){ return false; }
        public override bool IsEqual(Variable other){ return false; }
        public override bool IsEqual(Constant other)
        {
            if (value == other.value)
            {
                return true;
            }   
            else
            {
                return false;
            }
        }
        public override Expression SumLikeTerm(Symbol other){ return null; }

        public override bool TestAgainstStage(StructureStage stage)
        {
            if (stage.type == 'n')
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
            return new Constant() { sign = this.sign };
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }
}