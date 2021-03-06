using System;
using System.Collections.Generic;

namespace LSharp.Math.Rules
{
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
}