using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Evaluation
{
    public interface IEvaluationStrategy
    {
        Expression Evaluate();
    }
}