using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Evaluation
{
    public class EvaluationResult
    {
        public Expression result { get; set; }
        public bool change { get; set; }
    }
}