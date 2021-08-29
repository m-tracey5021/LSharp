using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Evaluation
{
    public class EvaluationResult
    {
        public Expression result { get; set; }
        public bool change { get; set; }
    }
}