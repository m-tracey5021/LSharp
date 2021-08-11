using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Selectors
{
    public abstract class Selector
    {
        public abstract void Select(Summation sum);
        public abstract void Select(Multiplication sum);
        public abstract void Select(Division sum);
        public abstract void Select(Exponent sum);
        public abstract void Select(Radical sum);
        public abstract void Select(Variable sum);
        public abstract void Select(Constant sum);
        public virtual void Chain(List<Symbol> symbols, List<Selector> selectors, int index)
        {
            symbols[index].Dispatch(selectors[index]);

            index ++;

            if (!(index > symbols.Count))
            {
                selectors[0].Chain(symbols, selectors, index);
            }

            
        }
    }
}