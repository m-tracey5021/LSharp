using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Parsing
{
    public class ExpressionParser
    {
        public Dictionary<SymbolType, List<char>> skippingCharacters = new Dictionary<SymbolType, List<char>>()
        {

        };
        public Dictionary<SymbolType, List<char>> terminatingCharacters = new Dictionary<SymbolType, List<char>>()
        {
            { SymbolType.Summation, new List<char>(){  } },
            { SymbolType.Multiplication, new List<char>(){ '+', '-', '/' } },
            { SymbolType.Division, new List<char>(){  } },
            { SymbolType.Exponent, new List<char>(){  } },
            { SymbolType.Radical, new List<char>(){  } },
        };
        public bool IsOpeningBracket(char bracket)
        {
            if (bracket == '(' ||
                bracket == '{' || 
                bracket == '[')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsClosingBracket(char bracket)
        {
            if (bracket == ')' ||
                bracket == '}' || 
                bracket == ']')
            {
                return true;
            }
            else
            {
                return false;
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
        public bool IsMultiplied(int start, int end, string expression)
        {
            if (end - start != 1 || start < 0 || end >= expression.Length)
            {
                return false;
            }
            else
            {
                char a = expression[start], b = expression[end];

                return
                (
                    (IsClosingBracket(a) && ((Char.IsLetter(b) && b != 'v') || Char.IsDigit(b))) || 
                    (IsOpeningBracket(b) && ((Char.IsLetter(a) && a != 'v') || Char.IsDigit(a))) || 
                    (IsClosingBracket(a) && IsOpeningBracket(b)) || 
                    (Char.IsLetter(a) && Char.IsLetter(b) && a != 'v' && b != 'v') || 
                    (Char.IsDigit(a) && Char.IsLetter(b) && b != 'v') || 
                    (Char.IsDigit(b) && Char.IsLetter(a) && a != 'v')
                );
            }
        }
        public Scope ScopeSummation(int i, string expression)
        {
            Scope scope = new Scope()
            {
                type = SymbolType.Summation
            };
            int breakForwards = expression[i] == '+' ? i + 1 : i, breakBackwards = expression[i] == '+' ? i + 1 : i - 1, j = i + 1, k = i - 1;

            bool forwards = true, backwards = true;

            while (true)
            {
                if (forwards)
                {
                    if (expression[j] == '+' || expression[j] == '-')
                    {
                        scope.AppendOperand(breakForwards, j - 1, expression, false);

                        breakForwards = expression[j] == '+' ? j + 1 : j;

                        j ++;
                    }
                    else if (IsOpeningBracket(expression[j]))
                    {
                        j = FindMatchingBracket(j, expression) + 1;
                    }
                    else if (IsClosingBracket(expression[j]) || j >= expression.Length)
                    {
                        scope.AppendOperand(breakForwards, j - 1, expression, false);

                        forwards = false;
                    }
                    else
                    {
                        j ++;
                    }
                }
                if (backwards)
                {
                    if (expression[k] == '+' || expression[k] == '-')
                    {
                        int start = expression[k] == '+' ? k + 1 : k;

                        scope.AppendOperand(start, breakBackwards, expression, true);

                        breakBackwards = k - 1;

                        k --;
                    }
                    else if (IsClosingBracket(expression[k]))
                    {
                        k = FindMatchingBracket(k, expression) - 1;
                    }
                    // else if (IsOpeningBracket(expression[k]) || k <= -1)
                    // {
                    //     if (k != breakBackwards){ scope.AppendOperand(k + 1, breakBackwards, expression, true); }

                    //     scope.sign = k != 0 ? (expression[k - 1] == '-' ? false : true) : false;

                    //     backwards = false;
                    // }
                    else if (IsOpeningBracket(expression[k]))
                    {
                        scope.AppendOperand(k + 1, breakBackwards, expression, true);

                        scope.sign = expression[k - 1] == '-' ? false : true;

                        backwards = false;
                    }
                    else if (k < 0)
                    {
                        if (breakBackwards >= 0){ scope.AppendOperand(0, breakBackwards, expression, true); }

                        backwards = false;
                    }
                    else
                    {
                        k --;
                    }
                }
                if (!forwards && !backwards)
                {
                    break;
                }
            }
            if (scope.operands.Count == 1 || scope.operands.Count == 0)
            {
                return null;
            }
            else
            {
                scope.start = k;

                scope.end = j;

                return scope;
            }
        }
        public Scope ScopeMultiplication(int lhs, int rhs, string expression)
        {
            Scope scope = new Scope()
            {
                type = SymbolType.Multiplication
            };

            int breakForwards = rhs, breakBackwards = lhs, j = rhs, k = lhs;

            bool forwards = true, backwards = true;

            while (true)
            {
                if (forwards)
                {
                    if (IsMultiplied(j, j + 1, expression))
                    {
                        scope.AppendOperand(breakForwards, j, expression, false);

                        breakForwards = j + 1;

                        j ++;
                    }
                    else
                    {
                        if (IsOpeningBracket(expression[j]))
                        {
                            j = FindMatchingBracket(j, expression);

                            if (expression[j + 1] == 'v')
                            {
                                j = expression[j + 2] == '(' ? FindMatchingBracket(j + 2, expression) : j + 2;
                            }
                            else if (expression[j + 1] == '^')
                            {
                                j = FindMatchingBracket(j + 2, expression);
                            }
                            else
                            {
                                throw new Exception("Expression is not well formed");
                            }
                        }
                        else if (terminatingCharacters[SymbolType.Multiplication].Contains(expression[j]) || j >= expression.Length)
                        {
                            scope.AppendOperand(breakForwards, j - 1, expression, false);

                            forwards = false;
                        }
                        else
                        {
                            if (IsClosingBracket(expression[j + 1]))
                            {
                                scope.AppendOperand(breakForwards, j, expression, false);

                                forwards = false;
                            }
                            j ++;
                        }
                    }
                }
                if (backwards)
                {
                    if (IsMultiplied(k - 1, k, expression))
                    {
                        scope.AppendOperand(k, breakBackwards, expression, true);

                        breakBackwards = k - 1;

                        k --;
                    }
                    else
                    {
                        if (IsClosingBracket(expression[k]))
                        {
                            k = FindMatchingBracket(k, expression);

                            if (expression[k - 1] == '^')
                            {
                                k = expression[k - 2] == ')' ? FindMatchingBracket(k - 2, expression) : k - 2;
                            }
                            else if (expression[k - 1] == 'v')
                            {
                                k = FindMatchingBracket(k - 2, expression);
                            }
                            else
                            {
                                throw new Exception("Expression is not well formed");
                            }
                        }
                        else if (terminatingCharacters[SymbolType.Multiplication].Contains(expression[k]) || k < 0)
                        {
                            scope.AppendOperand(k + 1, breakBackwards, expression, true);

                            backwards = false;
                        }
                        else
                        {
                            if (IsOpeningBracket(expression[k - 1]))
                            {
                                scope.AppendOperand(k, breakBackwards, expression, true);

                                backwards = false;
                            }
                            k --;
                        }
                    }
                }
                if (!forwards && !backwards)
                {
                    break;
                }
            }
            scope.sign = expression[k] == '-' ? false : true;

            scope.start = k;

            scope.end = j;

            return scope;
        }
        public Scope ScopeDivision(int i, string expression)
        {
            Scope scope = new Scope()
            {
                type = SymbolType.Division
            };
            int breakForwards = i + 1, breakBackwards = i - 1, j = i + 1, k = i - 1;

            bool forwards = true, backwards = true;

            while(true)
            {
                if (forwards)
                {
                    if (Char.IsLetter(expression[j]) || Char.IsDigit(expression[j]))
                    {
                        j ++;
                    }
                    else if ((new List<char>(){ '(', '[', '^' }).Contains(expression[j]))
                    {
                        int startOffset = expression[j] == '^' ? j + 1 : j, endOffset = expression[j] == '[' ? 2 : 1;

                        j = FindMatchingBracket(startOffset, expression) + endOffset;
                    }
                    else if (j >= expression.Length)
                    {

                    }
                    else
                    {
                        scope.AppendOperand(breakForwards, j - 1, expression, false);

                        forwards = false;
                    }
                }
                if (backwards)
                {
                    if (Char.IsLetter(expression[k]) || Char.IsDigit(expression[k]))
                    {
                        k --;
                    }
                    else if ((new List<char>(){ ')', '}', 'v' }).Contains(expression[k]))
                    {
                        int startOffset = expression[k] == 'v' ? k - 1 : k, endOffset = expression[k] == '}' ? -2 : -1;

                        k = FindMatchingBracket(startOffset, expression) + endOffset;
                    }
                    else if (k < 0)
                    {
                        scope.AppendOperand(0, breakBackwards, expression, true);

                        backwards = false;
                    }
                    else
                    {
                        scope.AppendOperand(k + 1, breakBackwards, expression, true);

                        backwards = false;
                    }
                }
                if (!backwards && !forwards)
                {
                    break;
                }   
            }
            if (expression[k] == '-' && expression[i + 1] == '-')
            {
                scope.sign = true;
            }
            else if (expression[k] == '-' || expression[i + 1] == '-')
            {
                scope.sign = false;
            }
            scope.start = k;

            scope.end = j;

            return scope;
        }
        public Scope ScopeAuxillary(int i, string expression)
        {
            Scope scope = new Scope();

            int breakForwards = i + 1, breakBackwards = i - 1, j = i + 1, k = i - 1;

            if (expression[i] == '^')
            {
                scope.type = SymbolType.Exponent;

                scope.AppendOperand(breakForwards + 1, j - 2, expression, false);

                if (Char.IsLetter(expression[k]))
                {
                    k --;

                    scope.AppendOperand(k + 1, breakBackwards, expression, true);
                }
                else if (Char.IsDigit(expression[k]))
                {
                    while (Char.IsDigit(expression[k]))
                    {
                        k --;
                    }
                    scope.AppendOperand(k + 1, breakBackwards, expression, true);
                }
                else if (expression[k] == ')')
                {
                    k = FindMatchingBracket(k, expression) - 1;

                    scope.AppendOperand(k + 2, breakBackwards, expression, true);
                }
                else
                {
                    throw new Exception("Expression is not well formed");
                }
            }
            else if (expression[i] == 'v')
            {
                scope.type = SymbolType.Radical;

                k = FindMatchingBracket(k, expression) - 1;

                scope.AppendOperand(k + 2, breakBackwards - 1, expression, true);

                if (Char.IsLetter(expression[j]))
                {
                    j ++;

                    scope.AppendOperand(breakForwards, j - 1, expression, false);
                }
                else if (Char.IsDigit(expression[j]))
                {
                    while (Char.IsDigit(expression[j]))
                    {
                        j ++;
                    }
                    scope.AppendOperand(breakForwards, j - 1, expression, false);
                }
                else if (expression[j] == '(')
                {   
                    j = FindMatchingBracket(j, expression) + 1;

                    scope.AppendOperand(breakForwards + 1, j - 2, expression, false);
                }
                else
                {
                    throw new Exception("Expression is not well formed");
                }
            }
            else
            {
                throw new Exception("Expression is not well formed");
            }
            scope.sign = expression[k] == '-' ? false : true;

            scope.start = k;

            scope.end = j;

            return scope;
        }
        // public Scope ScopeConstant(int i, string expression)
        // {
        //     Scope scope = new Scope()
        //     {
        //         type = SymbolType.Constant
        //     };
        //     int j = i + 1, k = i - 1;

        //     bool forwards = j < expression.Length ? true : false, backwards = k >= 0 ? true : false;

        //     while (true)
        //     {
        //         if (forwards)
        //         {
        //             if (Char.IsDigit(expression[j]))
        //             {
        //                 j ++;
        //             }
        //             else
        //             {
        //                 forwards = false;
        //             }
        //         }
        //         if (backwards)
        //         {
        //             if (Char.IsDigit(expression[k]))
        //             {
        //                 k --;
        //             }
        //             else
        //             {
        //                 backwards = false;
        //             }
        //         }
        //         if (!forwards && !backwards)
        //         {
        //             break;
        //         }
        //     }
        //     scope.AppendOperand(k + 1, j - 1, expression, false);

        //     scope.sign = expression[k] == '-' ? false : true;

        //     scope.start = k;

        //     scope.end = j;

        //     return scope;
        // }
        public Scope ScopeConstant(int i, string expression)
        {
            Scope scope = new Scope()
            {
                type = SymbolType.Constant
            };

            int start = i - 1, end = i + 1;

            bool forwards = end < expression.Length ? true : false, backwards = start >= 0 ? true : false;

            while (forwards)
            {
                if (Char.IsDigit(expression[end]))
                {
                    end ++;
                }
                else
                {
                    forwards = false;
                }
                forwards = end < expression.Length ? true : false;
            }
            while (backwards)
            {
                if (Char.IsDigit(expression[start]))
                {
                    start --;
                }
                else
                {
                    backwards = false;
                }
                backwards = start >= 0 ? true : false;
            }
            scope.AppendOperand(start + 1, end - 1, expression, false);

            scope.sign = expression[scope.start] == '-' ? false : true;

            scope.start = start;

            scope.end = end;

            return scope;
        }
        public Scope FindScope(int i, string expression)
        {
            char symbol = expression[i];

            if (symbol == '+' || symbol == '-')
            {
                return ScopeSummation(i, expression);
            }   
            else if (IsMultiplied(i, i + 1, expression))
            {
                return ScopeMultiplication(i, i + 1, expression);
            }
            else if (IsMultiplied(i - 1, i, expression))
            {
                return ScopeMultiplication(i - 1, i, expression);
            }
            else if (symbol == '/')
            {
                return ScopeDivision(i, expression);
            }
            else if (symbol == '^' || symbol == 'v')
            {
                return ScopeAuxillary(i, expression);
            }
            else if (!IsMultiplied(i, i + 1, expression) && !IsMultiplied(i - 1, i, expression) && Char.IsLetter(symbol) && symbol != 'v')
            {
                return new Scope()
                {
                    sign = expression[i - 1] == '-' ? false : true,
                    type = SymbolType.Variable,
                    start = i - 1,
                    end = i + 1,
                    operands = new List<string>()
                    {
                        symbol.ToString()
                    }
                };
            }
            else if (!IsMultiplied(i, i + 1, expression) && !IsMultiplied(i - 1, i, expression) && Char.IsDigit(symbol))
            {
                return ScopeConstant(i, expression);
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
                }
            }
            return mainScope;
        }
    }
}