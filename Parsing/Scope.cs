using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Parsing
{
    public class Scope
    {
        public bool sign { get; set; }
        public SymbolType type { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public List<string> operands { get; set; }
        public Scope()
        {
            operands = new List<string>();
        }
        public void AppendOperand(int start, int end, string expression, bool insert)
        {
            string operand = expression.Substring(start, (end - start) + 1);

            if (insert)
            {
                operands.Insert(0, operand);
            }
            else
            {
                operands.Add(operand);
            }
        }
    }
}