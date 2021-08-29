using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Manipulation
{
    public interface IManipulationStrategy
    {
        Expression Manipulate();
    }
}