using System;
using System.Collections.Generic;

namespace LSharp.Symbols
{
    public abstract class Symbol
    {
        public bool sign { get; set; }
        public SymbolType type { get; set; }
        public Symbol(bool sign, SymbolType type){ this.sign = sign; this.type = type; }
        // public virtual bool IsOperation()
        // {
        //     if (type == SymbolType.Summation ||
        //             type == SymbolType.Multiplication || 
        //             type == SymbolType.Division ||
        //             type == SymbolType.Exponent ||
        //             type == SymbolType.Radical)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        // public virtual bool IsSummation()
        // {
        //     if (type == SymbolType.Summation)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        // public virtual bool IsMultiplication()
        // {
        //     if (type == SymbolType.Multiplication)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        // public virtual bool IsDivision()
        // {
        //     if (type == SymbolType.Division)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        // public virtual bool IsExponent()
        // {
        //     if (type == SymbolType.Exponent)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        // public virtual bool IsRadical()
        // {
        //     if (type == SymbolType.Radical)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        // public virtual bool IsAtomic()
        // {
        //     if (type == SymbolType.Variable || type == SymbolType.Constant)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        // public virtual bool IsVariable()
        // {
        //     if (type == SymbolType.Variable)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        // public virtual bool IsConstant()
        // {
        //     if (type == SymbolType.Constant)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }
        public abstract string GetValue();
        public abstract int? GetNumericValue();
        public abstract void SetValue(char value);
        public abstract void SetNumericValue(int value);
        public abstract Symbol Copy();

        public override string ToString()
        {
            if (!sign && (type == SymbolType.Variable || type == SymbolType.Constant))
            {
                return "-" + GetValue();
            }
            else
            {
                return GetValue();
            }
            
        }
    }
}