using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Evaluation
{
    public interface IEvaluationStrategy
    {
        Expression Evaluate();
    }
}