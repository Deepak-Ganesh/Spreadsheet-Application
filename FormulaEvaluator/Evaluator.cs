/// <summary>
/// author: Deepak Ganesh
/// Date: 1/16/2019
/// I pledge that I wrote this code.
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace FormulaEvaluator
{
    /// <summary>
    /// The class is used to evaluate the value of different expressions
    /// </summary>
    public static class Evaluator
    {
        public delegate int Lookup(String variable_name); 
        private static char[] letters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray();

        

        /// <summary>
        /// The method evaluates the given expression and return an integer if the expression is legal.
        /// If the expression is illegal then the method throws an ArgumentException
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns>method returns the value of the expression</returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] equation = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            Stack<int> valueStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>();
            bool expressionHasDigits = false;

            if (expression.Trim().Length == 0)
                throw new ArgumentException();


            int index = 0;

            while (index < equation.Length)
            {
                string token = equation[index].Trim();
                int numberValue;

                if (token.Equals(""))
                {
                    index++;
                    continue;
                }
                //if the token is an integer
                if (Int32.TryParse(token, out numberValue))
                {
                    expressionHasDigits = true;
                    if (operatorStack.TryPeek(out string operation))
                    {
                        if (operatorStack.Peek().Equals("*"))
                        {
                            if (valueStack.Count == 0)
                                throw new ArgumentException("there is an extra * or /");

                            operatorStack.Pop();
                            valueStack.Push(valueStack.Pop() * numberValue);
                        }

                        else if (operatorStack.Peek().Equals("/"))
                        {
                            if (valueStack.Count == 0)
                                throw new ArgumentException("there is an extra * or /");

                            operatorStack.Pop();
                            if (numberValue == 0)
                                throw new ArgumentException("a division by 0 has occured");
                            valueStack.Push(valueStack.Pop() / numberValue);
                        }

                        else
                            valueStack.Push(numberValue);
                    }

                    else
                        valueStack.Push(numberValue);

                }

                // if the token is a + or -
                else if (token.Equals("+") || token.Equals("-"))
                {
                    if (operatorStack.TryPeek(out string operation))
                    {
                        if (operatorStack.Peek().Equals("+"))
                        {
                            if (valueStack.Count < 2)
                            {
                                throw new ArgumentException("There is an extra + or -");
                            }
                            int number1 = valueStack.Pop();
                            int number2 = valueStack.Pop();

                            operatorStack.Pop();
                            valueStack.Push(number1 + number2);
                        }

                        else if (operatorStack.Peek().Equals("-"))
                        {
                            if (valueStack.Count < 2)
                            {
                                throw new ArgumentException("There is an extra + or -");
                            }

                            int number1 = valueStack.Pop();
                            int number2 = valueStack.Pop();
                            operatorStack.Pop();

                            valueStack.Push(number2 - number1);
                        }

                    }


                    operatorStack.Push(token);
                }

                // if the token is a * or /
                else if (token.Equals("*") || token.Equals("/"))
                {
                    operatorStack.Push(token);
                }

                // if the token is a (
                else if (token.Equals("("))
                {
                    operatorStack.Push(token);
                }

                // if the token is a )
                else if (token.Equals(")"))
                {
                    if (operatorStack.TryPeek(out string operation))
                    {
                        if (operatorStack.Peek().Equals("+"))
                        {
                            if (valueStack.Count < 2)
                            {
                                throw new ArgumentException("There is an extra + or -");
                            }
                            int number1 = valueStack.Pop();
                            int number2 = valueStack.Pop();
                            operatorStack.Pop();

                            valueStack.Push(number1 + number2);
                        }

                        else if (operatorStack.Peek().Equals("-"))
                        {
                            if (valueStack.Count < 2)
                            {
                                throw new ArgumentException("There is an extra + or -");
                            }

                            int number1 = valueStack.Pop();
                            int number2 = valueStack.Pop();
                            operatorStack.Pop();

                            valueStack.Push(number2 - number1);
                        }
                    }

                    if (!operatorStack.TryPeek(out string paranthesisOrNot))
                    {
                        throw new ArgumentException("there is an extra paranthesis");
                    }
                    if (!(operatorStack.Peek().Equals("(")))
                        throw new ArgumentException("The number of opening braces and closing braces aren't equal");

                    operatorStack.Pop();

                    if (operatorStack.TryPeek(out string operations))
                    {
                        if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                        {
                            if (operatorStack.Peek().Equals("*"))
                            {
                                if (valueStack.Count < 2)
                                {
                                    throw new ArgumentException("There is an extra * or / in the expression");
                                }

                                int number1 = valueStack.Pop();
                                int number2 = valueStack.Pop();
                                operatorStack.Pop();

                                valueStack.Push(number1 * number2);
                            }

                            else if (operatorStack.Peek().Equals("/"))
                            {
                                if (valueStack.Count < 2)
                                {
                                    throw new ArgumentException("There is an extra * or / in the expression");
                                }

                                int number1 = valueStack.Pop();
                                int number2 = valueStack.Pop();
                                operatorStack.Pop();

                                if (number2 == 0)
                                    throw new ArgumentException("A division by 0 has occured");

                                valueStack.Push(number2 / number1);
                            }
                        }
                    }
                }

                // If the token is a variable
                else if (VariableIsValid(token))
                {
                    expressionHasDigits = true;
                    int VariableValue = variableEvaluator(token);

                    if (operatorStack.TryPeek(out string operation))
                    {
                        if (operatorStack.Peek().Equals("*"))
                        {
                            if (valueStack.Count == 0)
                                throw new ArgumentException("there is an extra * or /");

                            operatorStack.Pop();
                            valueStack.Push(valueStack.Pop() * VariableValue);
                        }

                        else if (operatorStack.Peek().Equals("/"))
                        {
                            if (valueStack.Count == 0)
                                throw new ArgumentException("there is an extra * or /");

                            operatorStack.Pop();
                            if (VariableValue == 0)
                                throw new ArgumentException("a division by 0 has occured");
                            valueStack.Push(valueStack.Pop() / VariableValue);
                        }

                        else
                            valueStack.Push(VariableValue);
                    }

                    else
                        valueStack.Push(VariableValue);

                }
                index++;
            }

            if (expressionHasDigits == false)
                throw new ArgumentException("expression has no digits");

            if (operatorStack.Count == 0)
            {
                if (valueStack.Count > 1)
                    throw new ArgumentException("there is an extra operator");
                return valueStack.Pop();
            }

            if (operatorStack.Count == 1)
            {
                if (valueStack.Count == 2)
                {
                    int number1 = valueStack.Pop();
                    int number2 = valueStack.Pop();

                    if (operatorStack.Peek().Equals("+"))
                        return number1 + number2;
                    else
                        return number2 - number1;
                }
                else
                    throw new ArgumentException();
            }

            else
                throw new ArgumentException();

        }

        /// <summary>
        /// The method determines wheather or not the variable is legal. The 
        /// rules for a valid variable it must start with a letters and then
        /// followed by a numbers.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>returns true if variable is valid otherwise throw and argumentException</returns>
        private static bool VariableIsValid(string str)
        {
            if (str.Length == 0)
                return false;
            char[] strArray = str.ToCharArray();
            int pos = 0;

            bool hasLetter = false;
            bool hasNumber = false;

            while (pos < str.Length)
            {
                if (Int32.TryParse(str.Substring(pos, 1), out int numberValue))
                {
                    break;
                }
                else if (Array.Exists(letters, element => element == strArray[pos]))
                {
                    hasLetter = true;
                }

                else
                {
                    throw new ArgumentException("variable is of incorrect form");
                }
                pos++;
            }

            while (pos < str.Length)
            {
                if (Int32.TryParse(str.Substring(pos, 1), out int numberValue))
                {
                    hasNumber = true;
                }
                else
                    throw new ArgumentException("variable is of incorrect form");
                pos++;
            }

            if (hasLetter && hasNumber)
                return true;
            else
                throw new ArgumentException("variable is of incorrect form");
        }


    }

    public static class StackExtension
    {
        public static bool valueIsOnTop<T>(this Stack<T> stack, T value)
        {
            //if(stack.TryPeek(out ))
            return true;
        }
    }
}
