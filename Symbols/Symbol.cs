using System;
using System.Collections.Generic;
using System.Linq;
using LSharp.Visitors;


namespace LSharp.Symbols
{
    public abstract class Symbol
    {
        public char symbol { get; set; }
        public bool sign { get; set; }
        public Symbol parent { get; set; }
        public List<Symbol> children { get; set; }
        public Expression parentExpression { get; set; }

        // constructors

        public Symbol(){}
        public Symbol(char value, bool sign){ this.symbol = value; this.sign = sign; }

        // methods

        public abstract void Dispatch(Visitor visitor);
        public abstract SymbolFlat Flatten();
        public abstract void Sanitise();
        public abstract Nullable<int> GetValue();
        public virtual List<SymbolFlat> GetStructure()
        {
            List<SymbolFlat> flats = new List<SymbolFlat>();
            GetStructure(flats);
            return flats;
        }
        public virtual void GetStructure(List<SymbolFlat> flats)
        {
            flats.Add(Flatten());
            foreach (Symbol child in children)
            {
                child.GetStructure(flats);
            }
        }
        public virtual Structure Identify()
        {
            List<SymbolFlat> flats = GetStructure();
            List<SymbolType> types = flats.Select(x => x.type).ToList();

            if (StructureDefinition.canDistribute.Contains(types))
            {
                return Structure.CanDistribute;
            }
            else
            {
                return Structure.CanApplyER1;
            }
        }
        
        public abstract Symbol Evaluate();
        public abstract Symbol Sum(Symbol other);
        public abstract Symbol Multiply(Symbol other);
        public abstract Symbol Divide(Symbol other);
        public abstract Symbol Raise(Symbol other);
        public abstract Symbol Floor(Symbol other); // rename this
        
    }
}