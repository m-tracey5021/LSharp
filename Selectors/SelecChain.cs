using System;
using System.Collections.Generic;
using LSharp.Symbols;
using LSharp.Selectors;

namespace LSharp.Selectors
{
    public class SelectChain
    {
        public List<Symbol> symbols { get; set; }
        public List<Selector> selectors { get; set; }
        public void Execute()
        {
            if (symbols.Count != selectors.Count)
            {
                throw new Exception("Symbol count does not equal selector count");
            }
            else
            {
                int index = 0;

                selectors[0].Chain(symbols, selectors, index);
            }
        }
    }
}