using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Parsing
{
    public class ExpressionParser
    {
        public Dictionary<SymbolType, Dictionary<string, List<char>>> terminatingCharacters = new Dictionary<SymbolType, Dictionary<string, List<char>>>()
        {
            { 
                SymbolType.Summation, 
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ ')', '}', ']' } },
                    { "backwards", new List<char>(){ '(', '{', '[' } }
                }
            },
            { 
                SymbolType.Multiplication, 
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ ')', '}', ']', '+', '/'} },
                    { "backwards", new List<char>(){ '(', '{', '[', '+', '/' } }
                }
            },
            { 
                SymbolType.Division, 
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ ')', '}', ']', '+', '/' } },
                    { "backwards", new List<char>(){ '(', '{', '[', '+', '/' } }
                }
            },
            { 
                SymbolType.Exponent, 
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ ')', '}', ']', '+', '*', '/' } },
                    { "backwards", new List<char>(){ '(', '{', '[', '+', '*', '/' } }
                }
            },
            { 
                SymbolType.Radical, 
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ ')', '}', ']', '+', '*', '/' } },
                    { "backwards", new List<char>(){ '(', '{', '[', '+', '*', '/' } }
                }
            },
        };
        public Dictionary<SymbolType, Dictionary<string, List<char>>> skippingCharacters = new Dictionary<SymbolType, Dictionary<string, List<char>>>()
        {
            {
                SymbolType.Summation,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ '(', '{', '[' } },
                    { "backwards", new List<char>(){ ')', '}', ']' } }
                }
            },
            {
                SymbolType.Multiplication,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ '(', '{', '[' } },
                    { "backwards", new List<char>(){ ')', '}', ']' } }
                }
            },
            {
                SymbolType.Division,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ '(', '{', '[' } },
                    { "backwards", new List<char>(){ ')', '}', ']' } }
                }
            },
            {
                SymbolType.Exponent,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ '(', '{', '[' } },
                    { "backwards", new List<char>(){ ')', '}', ']' } }
                }
            },
            {
                SymbolType.Radical,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ '(', '{', '[' } },
                    { "backwards", new List<char>(){ ')', '}', ']' } }
                }
            }
        };
        public Dictionary<SymbolType, Dictionary<string, List<char>>> addingCharacters = new Dictionary<SymbolType, Dictionary<string, List<char>>>()
        {
            {
                SymbolType.Summation,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ '+' } },
                    { "backwards", new List<char>(){ '+' } }
                }
            },
            {
                SymbolType.Multiplication,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){ '*' } },
                    { "backwards", new List<char>(){ '*' } }
                }
            },
            {
                SymbolType.Division,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){  } },
                    { "backwards", new List<char>(){  } }
                }
            },
            {
                SymbolType.Exponent,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){  } },
                    { "backwards", new List<char>(){  } }
                }
            },
            {
                SymbolType.Radical,
                new Dictionary<string, List<char>>
                {
                    { "forwards", new List<char>(){  } },
                    { "backwards", new List<char>(){  } }
                }
            }
        };
        public SymbolType GetSymbolType(char symbol)
        {
            if (symbol == '+' || symbol == '-')
            {
                return SymbolType.Summation;
            }
            else if (symbol == '*')
            {
                return SymbolType.Multiplication;
            }
            else if (symbol == '/')
            {
                return SymbolType.Division;
            }
            else if (symbol == '^')
            {
                return SymbolType.Exponent;
            }
            else if (symbol == 'v')
            {
                return SymbolType.Radical;
            }
            else if (Char.IsLetter(symbol))
            {
                return SymbolType.Variable;
            }
            else if (Char.IsDigit(symbol))
            {
                return SymbolType.Constant;
            }
            else
            {
                throw new Exception("Unsupported symbol type");
            }
        }
        public int FindMatchingBracket(int i, string expression)
        {
            char opening, closing;

            if (expression[i] == '(' || expression[i] == ')')
            {
                opening = '(';
                closing = ')';
            }
            else if (expression[i] == '{' || expression[i] == '}')
            {
                opening = '{';
                closing = '}';
            }
            else if (expression[i] == '[' || expression[i] == ']')
            {
                opening = '[';
                closing = ']';
            }
            else
            {
                throw new Exception("Character at " + i + " is not a bracket");
            }
            int bracketStack = 0;

            if (expression[i] == opening)
            {
                for (int j = i + 1; j < expression.Length; j ++)
                {
                    if (expression[j] == closing)
                    {
                        bracketStack ++;
                    }
                    else if (expression[j] == opening)
                    {
                        bracketStack --;
                    }
                    if (bracketStack == 1)
                    {
                        return j;
                    }
                }
                return -1;
            }
            else if (expression[i] == closing)
            {
                for (int j = i - 1; j >= 0; j --)
                {
                    if (expression[j] == closing)
                    {
                        bracketStack --;
                    }
                    else if (expression[j] == opening)
                    {
                        bracketStack ++;
                    }
                    if (bracketStack == 1)
                    {
                        return j;
                    }
                }
                return -1;
            }
            else
            {
                throw new Exception("Character at " + i + " is not a bracket");
            }
        }
        public Scope ScopeAtom(int i, SymbolType atomType, string expression)
        {
            if (atomType == SymbolType.Variable)
            {
                bool sign = i - 1 >= 0 ? (expression[i - 1] == '-' ? false : true) : true;

                return new Scope()
                {
                    sign = sign,
                    type = SymbolType.Variable,
                    start = i - 1,
                    end = i + 1,
                    operands = new List<string>(){ expression.Substring(i, 1) }
                };
            }
            else if (atomType == SymbolType.Constant)
            {
                int j = i - 1, k = i + 1;

                bool backwards = j >= 0 ? true : false, forwards = k < expression.Length ? true : false;

                while (backwards)
                {
                    if (j < 0)
                    {
                        backwards = false;
                    }
                    else
                    {
                        if (Char.IsDigit(expression[j]))
                        {
                            j --;
                        }
                        else
                        {
                            backwards = false;
                        }
                    }
                }
                while (forwards)
                {
                    if (k >= expression.Length)
                    {
                        forwards = false;
                    }
                    else
                    {
                        if (Char.IsDigit(expression[k]))
                        {
                            k ++;
                        }
                        else
                        {
                            forwards = false;
                        }
                    }
                }
                bool sign = j >= 0 ? (expression[j] == '-' ? false : true) : true;

                return new Scope()
                {
                    sign = sign,
                    type = SymbolType.Constant,
                    start = j,
                    end = k,
                    operands = new List<string>(){ expression.Substring(j + 1, (k - j) - 1) }
                };
            }
            else 
            {
                throw new Exception("Unsupported symbol type");
            }
        }
        public Scope ScopeOperation(int i, SymbolType operatorType, string expression)
        {
            Scope scope = new Scope()
            {
                type = operatorType
            };
            int j = i - 1, k = i + 1;

            int breakBackwards = j, breakForwards = k; 

            bool backwards = j >= 0 ? true : false, forwards = k < expression.Length ? true : false;

            while (backwards)
            {
                if (j < 0)
                {
                    scope.AppendOperand(0, breakBackwards, expression, true);

                    backwards = false;
                }
                else
                {
                    char symbol = expression[j];

                    if (skippingCharacters[operatorType]["backwards"].Contains(symbol))
                    {
                        j = FindMatchingBracket(j, expression) - 1;
                    }
                    else if (addingCharacters[operatorType]["backwards"].Contains(symbol))
                    {
                        scope.AppendOperand(j + 1, breakBackwards, expression, true);

                        breakBackwards = j - 1;

                        j --;
                    } 
                    else if (terminatingCharacters[operatorType]["backwards"].Contains(symbol))
                    {
                        scope.AppendOperand(j + 1, breakBackwards, expression, true);

                        backwards = false;
                    }
                    else
                    {
                        j --;
                    }
                }
            }
            while (forwards)
            {
                if (k >= expression.Length)
                {
                    scope.AppendOperand(breakForwards, expression.Length - 1, expression, false);

                    forwards = false;
                }
                else
                {
                    char symbol = expression[k];

                    if (skippingCharacters[operatorType]["forwards"].Contains(symbol))
                    {
                        k = FindMatchingBracket(k, expression) + 1;
                    }
                    else if (addingCharacters[operatorType]["forwards"].Contains(symbol))
                    {
                        scope.AppendOperand(breakForwards, k - 1, expression, false);

                        breakForwards = k + 1;

                        k ++;
                    }
                    else if (terminatingCharacters[operatorType]["forwards"].Contains(symbol))
                    {
                        scope.AppendOperand(breakForwards, k - 1, expression, false);

                        forwards = false;
                    }
                    else
                    {
                        k ++;
                    }
                }
            }
            if (scope.operands.Count == 0)
            {
                return null;
            }
            else
            {
                scope.sign = j - 1 >= 0 ? (expression[j - 1] == '-' ? false : true) : true; 

                scope.start = j;

                scope.end = k;

                return scope;
            }
        }
        public Scope FindScope(int i, string expression)
        {
            char symbol = expression[i];

            List<char> operations = new List<char>()
            {
                '+', '*', '/', '^', 'v'
            };

            if (operations.Contains(symbol))
            {
                return ScopeOperation(i, GetSymbolType(symbol), expression);
            }
            else if (Char.IsLetter(symbol) || Char.IsDigit(symbol))
            {
                return ScopeAtom(i, GetSymbolType(symbol), expression);
            }
            else
            {
                return null;
            }
        }
        public Scope FindMainScope(string expression)
        {
            Scope mainScope = new Scope();

            for (int i = 0; i < expression.Length; i ++)
            {
                if (Char.IsLetter(expression[i]) || 
                    Char.IsDigit(expression[i]) || 
                    expression[i] == '+' ||
                    expression[i] == '-' ||
                    expression[i] == '/' ||
                    expression[i] == '^' ||
                    expression[i] == 'v')
                {
                    Scope ithScope = FindScope(i, expression);

                    if ((ithScope.end - ithScope.start) > (mainScope.end - mainScope.start))
                    {
                        mainScope = ithScope;
                    }
                    i = ithScope.end;
                }
            }
            return mainScope;
        }
    }
}