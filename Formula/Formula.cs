// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens
//@author: Deepak Ganesh


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary> 
/// Author:    [Deepak Ganesh] 
/// Partner:   [None] 
/// Date:      [Date of Creation] 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and [Your Name(s)] - This work may not be copied for use in Academic Coursework. 
/// 
/// I, [your name], certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// 
///    [... and of course you should describe the contents of the file in broad terms here ...] 
/// </summary>
namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        /// <summary>
        /// private variables
        /// </summary>
        private string formulaAsString;
        private HashSet<string> variables = new HashSet<string>();
        private static char[] letters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray();

        /// <summary>
        /// The delegate is used to determine the values of the variables.
        /// </summary>
        /// <param name="variable_name"></param>
        /// <returns>the double value of variable_name</returns>
        // public delegate double Lookup(String variable_name);


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
        this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //holds the normalized version of the formula
            StringBuilder stringBuilderFormula = new StringBuilder();
            //Paren variables used to determine that there are the same number of opening parenthesis and closing parenthesis.
            int leftParen = 0;
            int rightParen = 0;

            List<string> tokens = new List<string>(GetTokens(formula));


            if (tokens.Count < 1)
                throw new FormulaFormatException("formula has nothing in it");


            //checking first token is valid
            string prevToken = "";
            bool firstToken = false;
            if (double.TryParse(tokens[0], out double value))
            {
                firstToken = true;
                prevToken = tokens[0];
                stringBuilderFormula.Append(value);
            }
            else if (tokens[0].Equals("("))
            {
                firstToken = true;
                prevToken = tokens[0];
                leftParen++;
                stringBuilderFormula.Append(tokens[0]);
            }
            else if (isValid(normalize(tokens[0])))
            {
                firstToken = true;
                prevToken = tokens[0];
                stringBuilderFormula.Append(normalize(tokens[0]));
                variables.Add(normalize(tokens[0]));
            }
            if (tokens[0].Equals("+") || tokens[0].Equals("-") || tokens[0].Equals("*") || tokens[0].Equals("/") || tokens[0].Equals(")"))
                throw new FormulaFormatException("The value in the expression isn't number, ( or variable");

            tokens.RemoveAt(0);
            //throw an FormumalFormatException if the first token is illegal
            if (!firstToken)
                throw new FormulaFormatException("The first token in the expression isn't a number, paranthesis or a variable ");

            if (tokens.Count == 0)
            {
                formulaAsString = stringBuilderFormula.ToString();
                return;
            }

            //checking if last token is valid
            bool lastToken = false;
            if (double.TryParse(tokens[tokens.Count - 1], out double value2))
            {
                lastToken = true;
            }
            else if (tokens[tokens.Count - 1].Equals(")"))
            {
                lastToken = true;
            }
            else if (isValid(normalize(tokens[tokens.Count - 1])))
            {
                lastToken = true;
            }

            if (tokens[tokens.Count - 1].Equals("+") || tokens[tokens.Count - 1].Equals("-") || tokens[tokens.Count - 1].Equals("*") || tokens.Last().Equals("/") || tokens.Last().Equals("("))
                throw new FormulaFormatException("The last value in the expression isn't number, ) or variable");


            if (!lastToken)
                throw new FormulaFormatException("The last token in the expression isn't a number, paranthesis or a variable ");

            //The foreach loop determines if the all the tokens in the expressions are legal with respect to eachother and if only legal tokens are used
            foreach (string token in tokens)
            {
                double number = 0;
                //Check if the token is valid
                if (token.Equals("("))
                {
                    leftParen++;
                    stringBuilderFormula.Append(token);
                }
                else if (token.Equals(")"))
                {
                    rightParen++;
                    stringBuilderFormula.Append(token);
                }
                else if (token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/"))
                {
                    stringBuilderFormula.Append(token);
                }

                else if (double.TryParse(token, out number))
                {
                    stringBuilderFormula.Append(number);
                }

                else if (isValid(normalize(token)))
                {
                    stringBuilderFormula.Append(normalize(token));
                    variables.Add(normalize(token));
                }


                //Check if the token is valid with respect to the previous token
                if (prevToken.Equals("+") || prevToken.Equals("-") || prevToken.Equals("*") || prevToken.Equals("/") || prevToken.Equals("("))
                {
                    if (!double.TryParse(token, out number))
                        if (!(token.Equals("(")))
                            if (!(isValid(normalize(token))))
                                throw new FormulaFormatException("a number, opening paranthesis or a variable isn't followed by operator or opening parenthesis");
                    if (token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/") || token.Equals(")"))
                        throw new FormulaFormatException("There is an extra operator or closing parenthesis");
                }

                else if (double.TryParse(prevToken, out number) || prevToken.Equals(")") || isValid(normalize(prevToken)))
                {
                    if (!(token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/") || token.Equals(")")))
                        throw new FormulaFormatException("An operation or opening parenthesis doesn't followed by a number, variable or a closing parenthesis");
                }

                prevToken = token;

                if (leftParen < rightParen)
                    throw new FormulaFormatException("There are to many closing parenthesis");

            }
            if (leftParen != rightParen)
                throw new FormulaFormatException("There are to many opening parenthesis");
            formulaAsString = stringBuilderFormula.ToString();
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            string[] equation = Regex.Split(formulaAsString, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
           


            int index = 0;
            // either solve the expression or simplifies it to 2 digits and an operation
            while (index < equation.Length)
            {
                string token = equation[index].Trim();
                double numberValue;

                if (token.Equals(""))
                {
                    index++;
                    continue;
                }
                //if the token is an integer
                if (Double.TryParse(token, out numberValue))
                {

                    if (operatorStack.valueIsOnTop("*"))
                    {
                       

                        operatorStack.Pop();
                        valueStack.Push(valueStack.Pop() * numberValue);
                    }

                    else if (operatorStack.valueIsOnTop("/"))
                    {
                       

                        operatorStack.Pop();
                        if (numberValue == 0)
                            return new FormulaError("a division by 0 has occured");
                        valueStack.Push(valueStack.Pop() / numberValue);
                    }

                    else
                        valueStack.Push(numberValue);


                }

                // if the token is a + or - then do add or subtract the digits from the value stack
                else if (token.Equals("+") || token.Equals("-"))
                {

                    if (operatorStack.valueIsOnTop("+"))
                    {
                        
                        double number1 = valueStack.Pop();
                        double number2 = valueStack.Pop();

                        operatorStack.Pop();
                        valueStack.Push(number1 + number2);
                    }

                    else if (operatorStack.valueIsOnTop("-"))
                    {
                       

                        double number1 = valueStack.Pop();
                        double number2 = valueStack.Pop();
                        operatorStack.Pop();

                        valueStack.Push(number2 - number1);
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


                    if (operatorStack.valueIsOnTop("+"))
                    {
                        
                        double number1 = valueStack.Pop();
                        double number2 = valueStack.Pop();
                        operatorStack.Pop();

                        valueStack.Push(number1 + number2);
                    }

                    else if (operatorStack.valueIsOnTop("-"))
                    {
                       
                        double number1 = valueStack.Pop();
                        double number2 = valueStack.Pop();
                        operatorStack.Pop();

                        valueStack.Push(number2 - number1);
                    }


                    
                    //Revemove the opening parenthesis from the operator stack
                    operatorStack.Pop();


                    if (operatorStack.valueIsOnTop("*") || operatorStack.valueIsOnTop("/"))
                    {
                        if (operatorStack.Peek().Equals("*"))
                        {
                            

                            double number1 = valueStack.Pop();
                            double number2 = valueStack.Pop();
                            operatorStack.Pop();

                            valueStack.Push(number1 * number2);
                        }

                        else if (operatorStack.valueIsOnTop("/"))
                        {
                            

                            double number1 = valueStack.Pop();
                            double number2 = valueStack.Pop();
                            operatorStack.Pop();

                            if (number1 == 0)
                                return new FormulaError("A division by 0 has occured");

                            valueStack.Push(number2 / number1);
                        }
                    }

                }
                // Only go into the elseif if the variable is incorrectly formatted
                else if (!(VariableIsValid(token) is bool))
                {
                    return new FormulaError("The variable is of incorrect form");
                }
                // If the token is a variable treat lookup  it double value and treat it like a double in the first case
                else if ((bool)VariableIsValid(token))
                {
                    //Find the value of the variable
                    double VariableValue;
                    try
                    {
                        VariableValue = lookup(token);
                    }
                    catch (Exception)
                    {
                        return new FormulaError("no value for variable");
                    }


                    if (operatorStack.valueIsOnTop("*"))
                    {

                        operatorStack.Pop();
                        valueStack.Push(valueStack.Pop() * VariableValue);
                    }

                    else if (operatorStack.valueIsOnTop("/"))
                    {
                       

                        operatorStack.Pop();
                        if (VariableValue == 0)
                            return new FormulaError("a division by 0 has occured");
                        valueStack.Push(valueStack.Pop() / VariableValue);
                    }

                    else
                        valueStack.Push(VariableValue);


                }
                index++;
            }
            
            //return the final value as to what the expesssion simplifies to
            if (operatorStack.Count == 0)
            {
                
                return valueStack.Pop();
            }

            //return the the simplification  of the last 2 numbers in the value stack
            else 
            {
                if (valueStack.Count == 2)
                {
                    double number1 = valueStack.Pop();
                    double number2 = valueStack.Pop();

                    if (operatorStack.valueIsOnTop("+"))
                        return number1 + number2;
                    else
                        return number2 - number1;
                }
                else
                    return new FormulaError();
            }

            
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return formulaAsString;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return obj.ToString().Equals(formulaAsString);
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (object.ReferenceEquals(f1, null) && object.ReferenceEquals(f2, null))
                return true;
            if (object.ReferenceEquals(f1, null) || object.ReferenceEquals(f2, null))
                return false;

            return f1.Equals(f2);

        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {

            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return formulaAsString.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// The method determines wheather or not the variable is legal. The 
        /// rules for a valid variable it must start with a letters and then
        /// followed by a numbers.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>returns true if variable is valid otherwise throw and argumentException</returns>
        private static object VariableIsValid(string str)
        {

            char[] strArray = str.ToCharArray();
            int pos = 0;

            bool hasLetter = false;
            bool hasNumber = false;
            //Keep reading the varaible until a double is reached
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


                pos++;
            }
            //keep reading the variable's digits until the last character or a character isn't a double in which case return a Formula Error
            while (pos < str.Length)
            {
                if (Int32.TryParse(str.Substring(pos, 1), out int numberValue))
                {
                    hasNumber = true;
                }
                else
                    return new FormulaError("variable is of incorrect form");
                pos++;
            }

            if (hasLetter && hasNumber)
                return true;
            else
                return new FormulaError("variable is of incorrect form");
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }

    /// <summary>
    /// The class is extension for the stack class
    /// </summary>
    public static class StackExtension
    {
        /// <summary>
        /// The method determines if the value is on top of a stack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool valueIsOnTop<T>(this Stack<T> stack, T value)
        {
            if (stack.Count() == 0)
                return false;

            if (stack.Peek().Equals(value))
                return true;
            else
                return false;
        }
    }
}

