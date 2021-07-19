using System;

namespace LSharp.Symbols
{
    public class SymbolFlat
    {
        public SymbolType type { get ; set; }
        public Nullable<char> value { get; set; }
        public SymbolFlat(SymbolType type, Nullable<char> value){ this.type = type; this.value = value; }
    }
}