using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Rules
{
    public abstract class Rule
    {
        public int stage { get; set; }
        public bool recurse { get; set; }
        public Rule(){ this.stage = 0; }
        public void Success(StructureStage stage){ this.stage ++; this.recurse = stage.recurse; }
        public void SuccessContinue(){ this.stage ++; this.recurse = true; }
        public void SuccessReturn(){ this.stage ++; this.recurse = false; }
        public abstract bool Test(Summation summation);
        public abstract bool Test(Multiplication multiplication);
        public abstract bool Test(Division division);
        public abstract bool Test(Exponent exponent);
        public abstract bool Test(Radical radical);
        public abstract bool Test(Variable variable);
        public abstract bool Test(Constant constant);
        public abstract Expression Apply(Symbol symbol);
        
    }

    public class Structure
    {
        public Structure()
        {
            structure = new Dictionary<int, StructureStage>();
        }
        public Structure(int stages, List<char> types, List<bool> recurses)
        {
            structure = new Dictionary<int, StructureStage>();
            
            for (int i = 0; i <= stages; i ++)
            {
                StructureStage stage = new StructureStage(){ type = types[i], recurse = recurses[i] };

                structure.Add(i, stage);
            }
        }
        private Dictionary<int, StructureStage> structure;
        public StructureStage At(int index){ return structure[index]; }
    }

    public class StructureStage
    {
        public char type { get; set; }
        public bool recurse { get; set; }
    }
}