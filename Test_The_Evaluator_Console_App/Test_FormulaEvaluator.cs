using System;
using FormulaEvaluator;

namespace Test_The_Evaluator_Console_App
{
    /// <summary>
    /// The class tests the Evaluator class
    /// </summary>
    class Test_FormulaEvaluator
    {
        

        static void Main(string[] args)
        {
            
            
            testBasicAddition();
            testSubtraction();
            testmultipcation();
            testDivide();
            testorderofOperations();
            testVariable();
            test_emptyString();
            testExtraAdd();
            testExtraSubract();
            testParenthesis();
            testExtradivision();
            testExtraMultipication();
            testIllegalVariable();
            test_NoNumbers();
            Console.ReadLine();



        }

        //Tests are of legal expressions
        public static void testBasicAddition()
        {
            if (Evaluator.Evaluate("5+5", null) == 10)
                Console.WriteLine("sucess");
            else
                Console.WriteLine("addition failded");
        }

        public static void testSubtraction()
        {
            if (!(Evaluator.Evaluate("5-8", null) == -3))
                Console.WriteLine("fail subtraction");
        }

        public static void testmultipcation()
        {
            if (!(Evaluator.Evaluate("5*8", null) == 40))
                Console.WriteLine("fail multipication");
        }

        public static void testDivide()
        {
            if (!(Evaluator.Evaluate("40/8", null) == 5))
                Console.WriteLine("fail division");
        }

        public static void testorderofOperations()
        {
            if (!(Evaluator.Evaluate("5*8+10/10", (variableValue) => 5) == 41))
                Console.WriteLine("fail order of operations");
        }

        public static void testParenthesis()
        {
            if (!(Evaluator.Evaluate("5*(10+10)/10", (variableValue) => 5) == 10))
                Console.WriteLine("fail Parenthesis");
        }

        public static void testVariable()
        {
            if (!(Evaluator.Evaluate("z1*(10+10)/10", (variableValue) => 5) == 10))
                Console.WriteLine("fail Vairable");
        }

        //Testing illegal input. When evaluated Evaluate must throw ArgumentException

        public static void test_emptyString()
        {
            try
            {
                Evaluator.Evaluate("", (variableValue) => 5);
                Console.WriteLine("empty string fail");

            }
            catch(ArgumentException ex)
            {

            }
            
        }

        public static void testExtraAdd()
        {
            try
            {
                Evaluator.Evaluate("5++5", (variableValue) => 5);
                Console.WriteLine("ExtraAdd fail");

            }
            catch (ArgumentException ex)
            {

            }

            try
            {
                Evaluator.Evaluate("+3-3", (variableValue) => 5);
                Console.WriteLine("ExtraAdd fail");

            }
            catch (ArgumentException ex)
            {

            }

            try
            {
                Evaluator.Evaluate("(+(3-3))", (variableValue) => 5);
                Console.WriteLine("ExtraAdd fail");

            }
            catch (ArgumentException ex)
            {

            }
        }

        public static void testExtraSubract()
        {
            try
            {
                Evaluator.Evaluate("5--5", (variableValue) => 5);
                Console.WriteLine("extra subtract fail");

            }
            catch (ArgumentException ex)
            {

            }

            try
            {
                Evaluator.Evaluate("(5-5)-", (variableValue) => 5);
                Console.WriteLine("extra subtract fail");

            }
            catch (ArgumentException ex)
            {
                
            }
        }

        public static void testExtradivision()
        {
            try
            {
                Evaluator.Evaluate("5/5/", (variableValue) => 5);
                Console.WriteLine("extra divide fail");

            }
            catch (ArgumentException ex)
            {

            }
        }

        public static void testExtraMultipication()
        {
            try
            {
                Evaluator.Evaluate("5/5*", (variableValue) => 5);
                Console.WriteLine("extra multiply fail");

            }
            catch (ArgumentException ex)
            {
                
            }
        }

        public static void testIllegalVariable()
        {
            try
            {
                Evaluator.Evaluate("5/5*2a", (variableValue) => 5);
                Console.WriteLine("illegal variable fail");

            }
            catch (ArgumentException ex)
            {

            }

            try
            {
                Evaluator.Evaluate("5/5*gg", (variableValue) => 5);
                Console.WriteLine("illegal variable fail");

            }
            catch (ArgumentException ex)
            {

            }

            try
            {
                Evaluator.Evaluate("5/5*2^4", (variableValue) => 5);
                Console.WriteLine("illegal variable fail");

            }
            catch (ArgumentException ex)
            {

            }

            try
            {
                Evaluator.Evaluate("5/5*g", (variableValue) => 5);
                Console.WriteLine("illegal variable fail");

            }
            catch (ArgumentException ex)
            {

            }
        }

        public static void test_NoNumbers()
        {
            try
            {
                Evaluator.Evaluate("*)(*", (variableValue) => 5);
                Console.WriteLine("No numbers fail");

            }
            catch (ArgumentException ex)
            {

            }

            try
            {
                Evaluator.Evaluate("*", (variableValue) => 5);
                Console.WriteLine("No numbers fail");

            }
            catch (ArgumentException ex)
            {

            }

            try
            {
                Evaluator.Evaluate("()", (variableValue) => 5);
                Console.WriteLine("No numbers fail");

            }
            catch (ArgumentException ex)
            {

            }
        }
    }
}
