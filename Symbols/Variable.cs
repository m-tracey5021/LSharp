using System;
using System.Collections.Generic;
using LSharp.Rules;

namespace LSharp.Symbols
{
    public class Variable : Symbol
    {
        public char value { get; set; }
        public Variable(){}
        public Variable(bool sign, char value){ this.sign = sign; this.value = value; this.variable = true; }

        public override int? GetValue(){ return null; }
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
        public override bool IsEqual(Variable other)
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
        public override bool IsEqual(Constant other){ return false; }
        public override bool TestAgainstStage(StructureStage stage)
        {
            if (stage.type == 'x')
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
            return new Variable() { sign = this.sign };
        }
        public override string ToString()
        {
            return Char.ToString(value);
        }
    }
}