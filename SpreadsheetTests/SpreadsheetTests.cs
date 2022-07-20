using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Threading;

namespace SpreadsheetTests
{
    [TestClass]
    public class UnitTestsOfSpreadsheet
    {

        [TestMethod]
        public void TestAddingNumbers()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("bb", "90");
            Assert.AreEqual(90, (double)sheet.GetCellContents("bb"));
            sheet.SetContentsOfCell("bb", "89");
            Assert.AreEqual(89, (double)sheet.GetCellContents("bb"));
        }

        [TestMethod]
        public void TestAddingText()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("bb", "");
            Assert.IsTrue("".Equals(sheet.GetCellContents("bb")));
            sheet.SetContentsOfCell("bb", "d");
            Assert.IsTrue("d".Equals(sheet.GetCellContents("bb")));
        }

        [TestMethod]
        public void TestAddiingFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("bb", "=4");
            Assert.IsTrue(new Formula("4").Equals((Formula)sheet.GetCellContents("bb")));
            sheet.SetContentsOfCell("bb", "=5");
            Assert.IsTrue(new Formula("5").Equals((Formula)sheet.GetCellContents("bb")));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestIllegalCellName()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("892740575tdsf", "=4");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestCellNameWithIllegalCharacter()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("h&&", "=4");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testGetCellContentsWithNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testGetCellContentsWithIllegalName()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("*");
        }

        [TestMethod]
        public void testGetCellContentsWithEmptyCells()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsTrue("".Equals(sheet.GetCellContents("j")));
            sheet.SetContentsOfCell("j", "g");
            Assert.IsTrue("".Equals(sheet.GetCellContents("d")));


        }

        [TestMethod]
        public void testGetNamesOfAllNonEmptyCells_Basic()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            CollectionAssert.AreEqual(new List<string>(), new List<string>(sheet.GetNamesOfAllNonemptyCells()));
            sheet.SetContentsOfCell("d9", "90");
            CollectionAssert.AreEqual(new List<string> { "d9" }, new List<string>(sheet.GetNamesOfAllNonemptyCells()));

        }

        [TestMethod]
        public void testSetContentsOfCellWithNullNames()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            try
            {
                sheet.SetContentsOfCell(null, "3");
                Assert.IsTrue(false);
            }
            catch (InvalidNameException)
            {
                Assert.IsTrue(true);
            }

            try
            {
                sheet.SetContentsOfCell(null, "lkjfds");
                Assert.IsTrue(false);
            }
            catch (InvalidNameException)
            {
                Assert.IsTrue(true);
            }

            try
            {
                sheet.SetContentsOfCell(null, "=4");
                Assert.IsTrue(false);
            }
            catch (InvalidNameException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void testSetContentsOfCellWithNullValues()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            try
            {
                string nullstring = null;
                sheet.SetContentsOfCell("j", nullstring);
                Assert.IsTrue(false);
            }
            catch (ArgumentNullException)
            {
                Assert.IsTrue(true);
            }


        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void testCircularDependencies()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("G1", "=A1");
            sheet.SetContentsOfCell("A1", "=t1");
            sheet.SetContentsOfCell("A1", "=G1");
        }

        [TestMethod]
        public void testBasicDependency()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=9");
            sheet.SetContentsOfCell("B1", "=3");
            sheet.SetContentsOfCell("C1", "=A1+B1");
            sheet.SetContentsOfCell("D1", "=C1+A1");
            sheet.SetContentsOfCell("G1", "=D1+D1");
            CollectionAssert.AreEqual(new List<string> { "A1", "C1", "D1", "G1" }, new List<string>(sheet.SetContentsOfCell("A1", "=40")));
        }

        [TestMethod]
        public void testClearingDependencyWithNumber()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=9");
            sheet.SetContentsOfCell("B1", "=3");
            sheet.SetContentsOfCell("C1", "=D1+B1");
            sheet.SetContentsOfCell("D1", "=G1+A1");
            sheet.SetContentsOfCell("G1", "=D0i");
            CollectionAssert.AreEqual(new List<string> { "D1", "C1" }, new List<string>(sheet.SetContentsOfCell("D1", "5")));
        }

        [TestMethod]
        public void testClearingDependencyWithFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=9");
            sheet.SetContentsOfCell("B1", "=3");
            sheet.SetContentsOfCell("C1", "=D1+B1");
            sheet.SetContentsOfCell("D1", "=G1+A1");
            sheet.SetContentsOfCell("G1", "=D0i");
            CollectionAssert.AreEqual(new List<string> { "D1", "C1" }, new List<string>(sheet.SetContentsOfCell("D1", "=40")));
        }

        [TestMethod]
        public void testClearingDependencyWithText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=9");
            sheet.SetContentsOfCell("B1", "=3");
            sheet.SetContentsOfCell("C1", "=D1+B1");
            sheet.SetContentsOfCell("D1", "=G1+A1");
            sheet.SetContentsOfCell("G1", "=D100");
            CollectionAssert.AreEqual(new List<string> { "D1", "C1" }, new List<string>(sheet.SetContentsOfCell("D1", "K")));
        }


        // New TEST?????????????????????????????????????????????????????????????????????????????????????????????????


        [TestMethod]
        public void testgetCellValueBasic()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A6", "gg");

            Assert.IsTrue("gg".Equals(spreadsheet.GetCellValue("A6")));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testgetCellValueWhenNull()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.GetCellValue(null);
        }

        [TestMethod]
        public void testZeroArgumentConstructor()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void test_EvaluatingformulasWithVariables()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "= a2 + a3");
            sheet.SetContentsOfCell("a2", "= b1 + 3");
            Assert.IsInstanceOfType(sheet.GetCellValue("a1"), typeof(FormulaError));
            Assert.IsInstanceOfType(sheet.GetCellValue("a2"), typeof(FormulaError));
            sheet.SetContentsOfCell("a3", "5.0");
            sheet.SetContentsOfCell("b1", "2.0");


           

            Assert.AreEqual(10, (double)sheet.GetCellValue("a1"));

        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testFileNotExist()
        {
            AbstractSpreadsheet sheet = new Spreadsheet("testy.txt", str => true, str => str, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testFilePathNotExist()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.Save("..//..//..//ldklfjsklfjdklsfjklds//test.txt");
        }

        [TestMethod]
        public void test_xmlConstructorBasic()
        {
            XmlWriterSettings Settings = new XmlWriterSettings();
            Settings.Indent = true;
            Settings.IndentChars = ("\t");
            using (XmlWriter xmlWriter = XmlWriter.Create("test.txt", Settings))
            {
                xmlWriter.WriteStartDocument();


                xmlWriter.WriteStartElement("spreadsheet");
                xmlWriter.WriteAttributeString("version", "j");

                xmlWriter.WriteStartElement("cell");

                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString("A1");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("contents");
                xmlWriter.WriteString("0");
                xmlWriter.WriteEndElement();

                //end of cell
                xmlWriter.WriteEndElement();

                //end of Spreadsheet
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }


            AbstractSpreadsheet sheet = new Spreadsheet("test.txt", str => true, str => str, "j");
            Assert.AreEqual(0, (double)sheet.GetCellValue("A1"));
        }

        [TestMethod]
        public void test_ChangedAfterReadingXml()
        {
            XmlWriterSettings Settings = new XmlWriterSettings();
            Settings.Indent = true;
            Settings.IndentChars = ("\t");
            using (XmlWriter xmlWriter = XmlWriter.Create("test.txt", Settings))
            {
                xmlWriter.WriteStartDocument();


                xmlWriter.WriteStartElement("spreadsheet");
                xmlWriter.WriteAttributeString("version", "j");

                xmlWriter.WriteStartElement("cell");

                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString("A1");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("contents");
                xmlWriter.WriteString("=0");
                xmlWriter.WriteEndElement();

                //end of cell
                xmlWriter.WriteEndElement();

                //end of Spreadsheet
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }


            AbstractSpreadsheet sheet = new Spreadsheet("test.txt", str => true, str => str, "j");
            Assert.IsFalse(sheet.Changed);
        }

        [TestMethod]
        public void test_xmlSave()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=9");
            sheet.SetContentsOfCell("GG", "925430string");


            sheet.Save("test.txt");
            Assert.AreEqual(9, (double)sheet.GetCellValue("A1"));

        }

        [TestMethod]
        public void test_xmlreadingConstructor()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(str => true, str => str, "1");
            sheet.SetContentsOfCell("A1", "=9");
            sheet.SetContentsOfCell("GG", "925430string");

            sheet.Save("test.txt");
            AbstractSpreadsheet sheet2 = new Spreadsheet("test.txt", str => true, str => str, "1");

            Assert.IsTrue("1".Equals(sheet2.GetSavedVersion("test.txt")));
        }



        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void test_IncorretctlyFormatedXML()
        {
            using (XmlWriter xmlWriter = XmlWriter.Create("test.txt"))
            {
                xmlWriter.WriteStartDocument();


                xmlWriter.WriteStartElement("spreadshet");
                xmlWriter.WriteAttributeString("version", "j");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }


            AbstractSpreadsheet sheet = new Spreadsheet("test.txt", str => true, str => str, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void test_EmptyFile()
        {
            using (XmlWriter xmlWriter = XmlWriter.Create("test.txt"))
            {
                xmlWriter.Close();
            }


            AbstractSpreadsheet sheet = new Spreadsheet("test.txt", str => true, str => str, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void test_xmlWithTooManyNames()
        {
            using (XmlWriter xmlWriter = XmlWriter.Create("test.txt"))
            {
                xmlWriter.WriteStartDocument();


                xmlWriter.WriteStartElement("spreadsheet");
                xmlWriter.WriteAttributeString("version", "j");
                xmlWriter.WriteStartElement("cell");
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString("A1");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString("A2");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("content");
                xmlWriter.WriteString("=00");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }


            AbstractSpreadsheet sheet = new Spreadsheet("test.txt", str => true, str => str, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void test_xmlWithCellTagafterCellTag()
        {
            using (XmlWriter xmlWriter = XmlWriter.Create("test.txt"))
            {
                xmlWriter.WriteStartDocument();


                xmlWriter.WriteStartElement("spreadsheet");
                xmlWriter.WriteAttributeString("version", "j");
                xmlWriter.WriteStartElement("cell");

                xmlWriter.WriteStartElement("cell");
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString("A1");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("content");
                xmlWriter.WriteString("=00");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }


            AbstractSpreadsheet sheet = new Spreadsheet("test.txt", str => true, str => str, "1");
        }


        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void test_xmlWithIllegaltagAfterSpreadsheetTag()
        {
            using (XmlWriter xmlWriter = XmlWriter.Create("test.txt"))
            {
                xmlWriter.WriteStartDocument();


                xmlWriter.WriteStartElement("spreadsheet");
                xmlWriter.WriteAttributeString("version", "j");
                xmlWriter.WriteStartElement("celljkldfs");

                xmlWriter.WriteStartElement("cell");
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString("A1");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("content");
                xmlWriter.WriteString("=00");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }


            AbstractSpreadsheet sheet = new Spreadsheet("test.txt", str => true, str => str, "1");
        }




















        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        // Verifies cells and their values, which must alternate.
        public void VV(AbstractSpreadsheet sheet, params object[] constraints)
        {
            for (int i = 0; i < constraints.Length; i += 2)
            {
                if (constraints[i + 1] is double)
                {
                    Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
                }
                else
                {
                    Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
                }
            }
        }


        // For setting a spreadsheet cell.
        public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
        {
            List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
            return result;
        }

        // Tests IsValid
        [TestMethod()]
        public void IsValidTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "x");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void IsValidTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
            ss.SetContentsOfCell("A1", "x");
        }

        [TestMethod()]
        public void IsValidTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "= A1 + C1");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void IsValidTest4()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
            ss.SetContentsOfCell("B1", "= A1 + C1");
        }

        // Tests Normalize
        [TestMethod()]
        public void NormalizeTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("", s.GetCellContents("b1"));
        }

        [TestMethod()]
        public void NormalizeTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
            ss.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("hello", ss.GetCellContents("b1"));
        }

        [TestMethod()]
        public void NormalizeTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("A1", "6");
            s.SetContentsOfCell("B1", "= a1");
            Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
        }

        [TestMethod()]
        public void NormalizeTest4()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
            ss.SetContentsOfCell("a1", "5");
            ss.SetContentsOfCell("A1", "6");
            ss.SetContentsOfCell("B1", "= a1");
            Assert.AreEqual(6.0, (double)ss.GetCellValue("B1"), 1e-9);
        }

        // Simple tests
        [TestMethod()]
        public void EmptySheet()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            VV(ss, "A1", "");
        }


        [TestMethod()]
        public void OneString()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            OneString(ss);
        }

        public void OneString(AbstractSpreadsheet ss)
        {
            Set(ss, "B1", "hello");
            VV(ss, "B1", "hello");
        }


        [TestMethod()]
        public void OneNumber()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            OneNumber(ss);
        }

        public void OneNumber(AbstractSpreadsheet ss)
        {
            Set(ss, "C1", "17.5");
            VV(ss, "C1", 17.5);
        }


        [TestMethod()]
        public void OneFormula()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            OneFormula(ss);
        }

        public void OneFormula(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "5.2");
            Set(ss, "C1", "= A1+B1");
            VV(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
        }


        [TestMethod()]
        public void Changed()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Assert.IsFalse(ss.Changed);
            Set(ss, "C1", "17.5");
            Assert.IsTrue(ss.Changed);
        }


        [TestMethod()]
        public void DivisionByZero1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            DivisionByZero1(ss);
        }

        public void DivisionByZero1(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "0.0");
            Set(ss, "C1", "= A1 / B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }

        [TestMethod()]
        public void DivisionByZero2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            DivisionByZero2(ss);
        }

        public void DivisionByZero2(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "5.0");
            Set(ss, "A3", "= A1 / 0.0");
            Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
        }



        [TestMethod()]
        public void EmptyArgument()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            EmptyArgument(ss);
        }

        public void EmptyArgument(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }


        [TestMethod()]
        public void StringArgument()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            StringArgument(ss);
        }

        public void StringArgument(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "hello");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }


        [TestMethod()]
        public void ErrorArgument()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ErrorArgument(ss);
        }

        public void ErrorArgument(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "");
            Set(ss, "C1", "= A1 + B1");
            Set(ss, "D1", "= C1");
            Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
        }


        [TestMethod()]
        public void NumberFormula1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            NumberFormula1(ss);
        }

        public void NumberFormula1(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + 4.2");
            VV(ss, "C1", 8.3);
        }


        [TestMethod()]
        public void NumberFormula2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            NumberFormula2(ss);
        }

        public void NumberFormula2(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "= 4.6");
            VV(ss, "A1", 4.6);
        }


        // Repeats the simple tests all together
        [TestMethod()]
        public void RepeatSimpleTests()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "17.32");
            Set(ss, "B1", "This is a test");
            Set(ss, "C1", "= A1+B1");
            OneString(ss);
            OneNumber(ss);
            OneFormula(ss);
            DivisionByZero1(ss);
            DivisionByZero2(ss);
            StringArgument(ss);
            ErrorArgument(ss);
            NumberFormula1(ss);
            NumberFormula2(ss);
        }

        // Four kinds of formulas
        [TestMethod()]
        public void Formulas()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formulas(ss);
        }

        public void Formulas(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.4");
            Set(ss, "B1", "2.2");
            Set(ss, "C1", "= A1 + B1");
            Set(ss, "D1", "= A1 - B1");
            Set(ss, "E1", "= A1 * B1");
            Set(ss, "F1", "= A1 / B1");
            VV(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
        }

        [TestMethod()]
        public void Formulasa()
        {
            Formulas();
        }

        [TestMethod()]
        public void Formulasb()
        {
            Formulas();
        }


        // Are multiple spreadsheets supported?
        [TestMethod()]
        public void Multiple()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            AbstractSpreadsheet s2 = new Spreadsheet();
            Set(s1, "X1", "hello");
            Set(s2, "X1", "goodbye");
            VV(s1, "X1", "hello");
            VV(s2, "X1", "goodbye");
        }

        [TestMethod()]
        public void Multiplea()
        {
            Multiple();
        }

        [TestMethod()]
        public void Multipleb()
        {
            Multiple();
        }

        [TestMethod()]
        public void Multiplec()
        {
            Multiple();
        }


        // Reading/writing spreadsheets
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet("q:\\missing\\save.txt", s => true, s => s, "");
        }

        [TestMethod()]
        public void SaveTest3()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            Set(s1, "A1", "hello");
            s1.Save("save1.txt");
            s1 = new Spreadsheet("save1.txt", s => true, s => s, "default");
            Assert.AreEqual("hello", s1.GetCellContents("A1"));
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest4()
        {
            using (StreamWriter writer = new StreamWriter("save2.txt"))
            {
                writer.WriteLine("This");
                writer.WriteLine("is");
                writer.WriteLine("a");
                writer.WriteLine("test!");
            }
            AbstractSpreadsheet ss = new Spreadsheet("save2.txt", s => true, s => s, "");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest5()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.Save("save3.txt");
            ss = new Spreadsheet("save3.txt", s => true, s => s, "version");
        }

        [TestMethod()]
        public void SaveTest6()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "hello");
            ss.Save("save4.txt");
            Assert.AreEqual("hello", new Spreadsheet().GetSavedVersion("save4.txt"));
        }

        [TestMethod()]
        public void SaveTest7()
        {
            using (XmlWriter writer = XmlWriter.Create("save5.txt"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A2");
                writer.WriteElementString("contents", "5.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A3");
                writer.WriteElementString("contents", "4.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A4");
                writer.WriteElementString("contents", "= A2 + A3");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet("save5.txt", s => true, s => s, "");
            VV(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
        }

        [TestMethod()]
        public void SaveTest8()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "hello");
            Set(ss, "A2", "5.0");
            Set(ss, "A3", "4.0");
            Set(ss, "A4", "= A2 + A3");
            ss.Save("save6.txt");
            using (XmlReader reader = XmlReader.Create("save6.txt"))
            {
                int spreadsheetCount = 0;
                int cellCount = 0;
                bool A1 = false;
                bool A2 = false;
                bool A3 = false;
                bool A4 = false;
                string name = null;
                string contents = null;

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                Assert.AreEqual("default", reader["version"]);
                                spreadsheetCount++;
                                break;

                            case "cell":
                                cellCount++;
                                break;

                            case "name":
                                reader.Read();
                                name = reader.Value;
                                break;

                            case "contents":
                                reader.Read();
                                contents = reader.Value;
                                break;
                        }
                    }
                    else
                    {
                        switch (reader.Name)
                        {
                            case "cell":
                                if (name.Equals("A1")) { Assert.AreEqual("hello", contents); A1 = true; }
                                else if (name.Equals("A2")) { Assert.AreEqual(5.0, Double.Parse(contents), 1e-9); A2 = true; }
                                else if (name.Equals("A3")) { Assert.AreEqual(4.0, Double.Parse(contents), 1e-9); A3 = true; }
                                else if (name.Equals("A4")) { contents = contents.Replace(" ", ""); Assert.AreEqual("=A2+A3", contents); A4 = true; }
                                else Assert.Fail();
                                break;
                        }
                    }
                }
                Assert.AreEqual(1, spreadsheetCount);
                Assert.AreEqual(4, cellCount);
                Assert.IsTrue(A1);
                Assert.IsTrue(A2);
                Assert.IsTrue(A3);
                Assert.IsTrue(A4);
            }
        }


        // Fun with formulas
        [TestMethod()]
        public void Formula1()
        {
            Formula1(new Spreadsheet());
        }
        public void Formula1(AbstractSpreadsheet ss)
        {
            Set(ss, "a1", "= a2 + a3");
            Set(ss, "a2", "= b1 + b2");
            Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
            Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
            Set(ss, "a3", "5.0");
            Set(ss, "b1", "2.0");
            Set(ss, "b2", "3.0");
            VV(ss, "a1", 10.0, "a2", 5.0);
            Set(ss, "b2", "4.0");
            VV(ss, "a1", 11.0, "a2", 6.0);
        }

        [TestMethod()]
        public void Formula2()
        {
            Formula2(new Spreadsheet());
        }
        public void Formula2(AbstractSpreadsheet ss)
        {
            Set(ss, "a1", "= a2 + a3");
            Set(ss, "a2", "= a3");
            Set(ss, "a3", "6.0");
            VV(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
            Set(ss, "a3", "5.0");
            VV(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
        }

        [TestMethod()]
        public void Formula3()
        {
            Formula3(new Spreadsheet());
        }
        public void Formula3(AbstractSpreadsheet ss)
        {
            Set(ss, "a1", "= a3 + a5");
            Set(ss, "a2", "= a5 + a4");
            Set(ss, "a3", "= a5");
            Set(ss, "a4", "= a5");
            Set(ss, "a5", "9.0");
            VV(ss, "a1", 18.0);
            VV(ss, "a2", 18.0);
            Set(ss, "a5", "8.0");
            VV(ss, "a1", 16.0);
            VV(ss, "a2", 16.0);
        }

        [TestMethod()]
        public void Formula4()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula1(ss);
            Formula2(ss);
            Formula3(ss);
        }

        [TestMethod()]
        public void Formula4a()
        {
            Formula4();
        }


        [TestMethod()]
        public void MediumSheet()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            MediumSheet(ss);
        }

        public void MediumSheet(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "1.0");
            Set(ss, "A2", "2.0");
            Set(ss, "A3", "3.0");
            Set(ss, "A4", "4.0");
            Set(ss, "B1", "= A1 + A2");
            Set(ss, "B2", "= A3 * A4");
            Set(ss, "C1", "= B1 + B2");
            VV(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
            Set(ss, "A1", "2.0");
            VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
            Set(ss, "B1", "= A1 / A2");
            VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
        }

        [TestMethod()]
        public void MediumSheeta()
        {
            MediumSheet();
        }


        [TestMethod()]
        public void MediumSave()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            MediumSheet(ss);
            ss.Save("save7.txt");
            ss = new Spreadsheet("save7.txt", s => true, s => s, "default");
            VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
        }

        [TestMethod()]
        public void MediumSavea()
        {
            MediumSave();
        }


        // A long chained formula.  If this doesn't finish within 60 seconds, it fails.
        [TestMethod()]
        public void LongFormulaTest()
        {
            object result = "";
            Thread t = new Thread(() => LongFormulaHelper(out result));
            t.Start();
            t.Join(60 * 1000);
            if (t.IsAlive)
            {
                t.Abort();
                Assert.Fail("Computation took longer than 60 seconds");
            }
            Assert.AreEqual("ok", result);
        }

        public void LongFormulaHelper(out object result)
        {
            try
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("sum1", "= a1 + a2");
                int i;
                int depth = 100;
                for (i = 1; i <= depth * 2; i += 2)
                {
                    s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                    s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
                }
                s.SetContentsOfCell("a" + i, "1");
                s.SetContentsOfCell("a" + (i + 1), "1");
                Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);
                s.SetContentsOfCell("a" + i, "0");
                Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
                s.SetContentsOfCell("a" + (i + 1), "0");
                Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
                result = "ok";
            }
            catch (Exception e)
            {
                result = e;
            }
        }

    }

}


