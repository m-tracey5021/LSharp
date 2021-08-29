using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Manipulation
{
    public interface IManipulationStrategy
    {
        Expression Manipulate();
    }
}