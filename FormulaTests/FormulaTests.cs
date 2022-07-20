using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {

        public bool isValid(string str)
        {
            string letters = "qwertyuiopasdfghjklzxcvbnm";

            if (letters.Contains(str))
                return true;
            return false;
        }
        /// <summary>
        /// Test is used as a base check to determine if my code worked as I was writing it.
        /// </summary>
        [TestMethod()]
        public void initialTest()
        {
            Formula formula = new Formula("(8+k-2)", s => s, isValid);
        }

        [TestMethod()]
        public void TestBasicSubtraction()
        {
            Formula formula = new Formula("8-2");
        }

        /// <summary>
        /// The tests would be run every time whenever something new was added to make sure that I didn't brake the code.
        /// </summary>
        [TestMethod()]
        public void TestInitialStuff()
        {
            Formula formula = new Formula("8*(5+3)");
        }

        [TestMethod]
        public void IllegalFormual()
        {
            try
            {
                Formula formula = new Formula("8*)5-3)");
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testExtraParenthesis()
        {
            Formula formula = new Formula("(8+3))");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithAddition()
        {
            Formula formula = new Formula("+4-6");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void test_WithExtraAddition()
        {
            Formula formula = new Formula("4++6");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithDivision()
        {
            Formula formula = new Formula("/4-6");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithEmptyString()
        {
            Formula formula = new Formula("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithSingleParenthesis()
        {
            Formula formula = new Formula(")");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithSinglePlus()
        {
            Formula formula = new Formula("+");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithTooManyClosingParenthesis()
        {
            Formula formula = new Formula("((4)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithSingleIllegal()
        {
            Formula formula = new Formula("&", s => s, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithLastIllegal()
        {
            Formula formula = new Formula("5+&", s => s, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void startingWithLastPlus()
        {
            Formula formula = new Formula("5++");
        }

        [TestMethod]

        public void startingWithTooManyPlus()
        {
            try
            {
                Formula formula = new Formula("5++5", s => s, isValid);
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }

            try
            {
                Formula formula = new Formula("5+^5");
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void startingWithSingleDigit()
        {
            try
            {
                Formula formula = new Formula("5");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsFalse(true);
            }
        }

        [TestMethod]
        public void startingWithBasicVariable()
        {
            try
            {
                Formula formula = new Formula("5+a");
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsFalse(true);
            }
        }





        /*****************************Test Evaluator**************************************************************/

        //Tests are of legal expressions
        [TestMethod]
        public void testBasicAddition()
        {
            Formula formula = new Formula("5+5.5");
            if (formula.Evaluate(null).ToString().Equals("10.5"))
                Assert.IsTrue(true);
            else
                Assert.IsTrue(false);
        }

        [TestMethod]
        public void testSubtraction()
        {
            Formula formula = new Formula("5-8");
            if (!(formula.Evaluate(null).ToString().Equals("-3")))
                Assert.IsTrue(false);
        }

        [TestMethod]
        public void testmultipcation()
        {
            Formula formula = new Formula("5*8");
            if (!(formula.Evaluate(null).ToString().Equals("40")))
                Assert.IsTrue(false);

        }


        [TestMethod]
        public void testDivide()
        {
            Formula formula = new Formula("40/100");
            if (!(formula.Evaluate(null).ToString().Equals("0.4")))
                Assert.IsTrue(false);

        }

        [TestMethod]
        public void testorderofOperations()
        {
            Formula formula = new Formula("5*8+10/10-3+3");
            if (!(formula.Evaluate(null).ToString().Equals("41")))
                Assert.IsTrue(false);
            formula = new Formula("(5*((10+10)/10)-3+3+s2)-(1-1)/(2/2)");
            Assert.AreEqual(11.0, formula.Evaluate((variableValue) => 1));
        }

        [TestMethod()]
        public void testExcpetionLookup()
        {
            Formula formula = new Formula("5*gf3");
            Assert.IsInstanceOfType(formula.Evaluate((s) => throw new Exception()), typeof(FormulaError));
        }

        [TestMethod]
        public void testParenthesis()
        {
            Formula formula = new Formula("5*(10+10)/1000");
            if (!(formula.Evaluate(null).ToString().Equals("0.1")))
                Assert.IsTrue(false);

        }

        [TestMethod]
        public void testVariable()
        {
            Formula formula = new Formula("z1*(10+10)/5");
            if (!(formula.Evaluate((variableValue) => 5).ToString().Equals("20")))
                Assert.IsTrue(false);
            formula = new Formula("5*gf3");
            Assert.AreEqual(5.0, formula.Evaluate((variableValue) => 1));
        }

        //Testing illegal input. When evaluated Evaluate must throw ArgumentException

        [TestMethod]
        public void testDivisionByZero()
        {
            Formula formula = new Formula("5/0");
            Assert.IsInstanceOfType(formula.Evaluate((s) => 77), typeof(FormulaError));
            FormulaError error = (FormulaError)formula.Evaluate((s) => 77);
            Assert.AreEqual("a division by 0 has occured", error.Reason);
        }

        [TestMethod]
        public void test_zerDivsionwithParenthesis()
        {
            //Formula formula = new Formula("(5/0)");
            //Assert.IsInstanceOfType(formula.Evaluate((s) => 77), typeof(FormulaError));
            Formula formulra = new Formula("(5+2)/(df3-okljdfs3)");
            Assert.IsInstanceOfType(formulra.Evaluate((s) => 0), typeof(FormulaError));
            Formula formula = new Formula("((5/4)/aj4)-5");
            Assert.IsInstanceOfType(formula.Evaluate((s) => 0), typeof(FormulaError));

        }

        [TestMethod]
        public void testIllegalVariable()
        {
            Formula formula = new Formula("((5/4)/aj3)-jj");
            Assert.IsTrue(formula.Evaluate((s) => 77) is FormulaError);

            formula = new Formula("((5/4)/aj3)-jj4f");
            Assert.IsTrue(formula.Evaluate((s) => 77) is FormulaError);


            //formula = new Formula("4++");
            //Assert.IsTrue(formula.Evaluate((s) => 77) is FormulaError);
        }


        /**********************************TestEquals***************************************/

        [TestMethod]
        public void testBasicEquals()
        {
            Formula form = new Formula("5.3");
            Assert.IsTrue(form.Equals(new Formula("5.3")));
            Assert.IsFalse(form.Equals(new Formula("5")));
        }

        [TestMethod]
        public void testBasicEqualsWithNull()
        {
            Formula form = new Formula("5.3");
            Assert.IsFalse(form.Equals(null));
        }

        [TestMethod]
        public void testBasicGetHashCode()
        {
            Formula form = new Formula("5.3");
            Assert.AreEqual(form.GetHashCode(), new Formula("5.3").GetHashCode());
        }
        [TestMethod]
        public void testBasicDoubleEquals()
        {
            Formula form = new Formula("5.3");
            Assert.IsTrue(form == new Formula("5.3"));
            Assert.IsFalse(form == new Formula("5"));
        }

        [TestMethod]
        public void testNullDoubleEquals()
        {
            Formula form = null;
            Assert.IsTrue(null == form);
            Assert.IsFalse(null == new Formula("5"));
        }

        /**********************************************************NotEquals****************/
        [TestMethod]
        public void testBasicDoubleNotEquals()
        {
            Formula form = new Formula("5.3");
            Assert.IsFalse(form != new Formula("5.3"));
            Assert.IsTrue(form != new Formula("5"));
        }

        [TestMethod]
        public void testNullNotDoubleEquals()
        {
            Formula form = null;
            Assert.IsFalse(null != form);
            Assert.IsTrue(null != new Formula("5"));
        }

        /********************************GetVariables********************************************************/

        [TestMethod]
        public void testBasicGetVariables()
        {
            Formula form = new Formula("5+g5+s3");
            CollectionAssert.AreEqual(new List<string>(form.GetVariables()), new List<string> { "g5", "s3" });

        }

        [TestMethod]
        public void testGetVariablesWithNoVariables()
        {
            Formula form = new Formula("5+5+3");
            CollectionAssert.AreEqual(new List<string>(form.GetVariables()), new List<string> { });

        }


    }
}
