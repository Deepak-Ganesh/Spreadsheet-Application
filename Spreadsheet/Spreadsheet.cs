using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;


/// <summary> 
/// Author:    [Deepak Ganesh] 
/// Partner:   [None] 
/// Date:      [2/9/19] 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and [Your Name(s)] - This work may not be copied for use in Academic Coursework. 
/// 
/// I, [Deepak Ganesh], certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// 
///    The spreadsheet brings together Cell class, DependencyGraph, and Formula class and is the entire backend
///    of the Spreadsheet. It is the Model part of the MVC arcitechture.
///    
///     I chose to save all the cell in the Spreadsheet in a dictionary so I would have constant access time
///     and had a seperate DependencyGraph object for organizational purposes.
/// </summary>
namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// private variables
        /// </summary>
        private Dictionary<string, Cell> spreadsheet;
        private DependencyGraph graph = new DependencyGraph();
        private static string _version = "default";
        private bool isChanged;
        

        /// <summary>
        /// The zero argument contructor passes in the a delagate thats always returns true,
        /// a delegate takes in a string and returns that same string and passes back version
        /// default to the AbstractSpreadsheet
        /// </summary>
        public Spreadsheet()
            : base(str => true, str => str, _version)
        {
            isChanged = false;
        }


        /// <summary>
        /// The constructor allows for the outside world to add further restrictions as to what 
        /// can and cannot go into a cell
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            isChanged = false;
        }

        /// <summary>
        /// The method reads the PathtoFile as an xml and parses it into a spreadsheet object that holds 
        /// all the cells. If the file is not of correct form than a SpreadsheetReadWriteException() is 
        /// thrown.
        /// </summary>
        /// <param name="PathToFile"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(string PathToFile, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            isChanged = false;
            if (!File.Exists(PathToFile))
                throw new SpreadsheetReadWriteException("file not found in specified location");


            if (File.ReadAllText(PathToFile).Length < 5 || !File.ReadAllText(PathToFile).Substring(0, 5).Equals("<?xml"))
                throw new SpreadsheetReadWriteException("file is empty");

            // setting of reading the file
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            spreadsheet = new Dictionary<string, Cell>();

            bool hasSpreadsheet = false;
            bool hasCell = false;
            using (XmlReader reader = XmlReader.Create(PathToFile, settings))
            {
                //begin reading file
                while (reader.Read())
                {


                    if (reader.Name.ToString().Equals("spreadsheet"))
                    {
                        //string str = reader.GetAttribute("version");
                        if (!reader.GetAttribute("version").Equals(Version))
                            throw new SpreadsheetReadWriteException("Your spreadsheet is version given doesn't match the version of the spreadsheet");
                        hasSpreadsheet = true;
                        
                        while (reader.Read())
                        {
                            // check if the file has ended
                            if (reader.Name.ToString().Equals("spreadsheet"))
                                break;

                            //grab the contents of the the stuff in the cell tag
                            if (reader.Name.ToString().Equals("cell"))
                            {
                                hasCell = true;
                                reader.Read();

                                
                                string CellName = null;
                                string content = null;

                                //Add a new cell into the Dictionary of cells
                                if (reader.Name.ToString().Equals("name"))
                                {
                                    
                                    CellName = reader.ReadElementContentAsString();
                                    if (reader.Name.ToString().Equals("contents"))
                                    {
                                        content = reader.ReadElementContentAsString();
                                        try
                                        {
                                            SetContentsOfCell(CellName, content);
                                        }
                                        catch(Exception ex)
                                        {
                                            throw new SpreadsheetReadWriteException("In the file" + ex.Message);
                                        }
                                    }
                                    else
                                        throw new SpreadsheetReadWriteException("there was not a content element after a name element");



                                }
                                else
                                    throw new SpreadsheetReadWriteException("there was not a name element after a cell element");


                            }
                            else
                                throw new SpreadsheetReadWriteException("there was not a cell element after a spreadsheet element");

                        }
                    }


                }
            }
            //throw exception only there are no spreadsheet tag and cell tag in the xml file
            if (!hasSpreadsheet)
                throw new SpreadsheetReadWriteException("spreadsheet does not start with a spreadsheet tag");
            isChanged = false;
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get => isChanged; protected set => Changed = isChanged; }




        /// <summary>
        /// <para>
        /// If name is null or invalid, throws an InvalidNameException.
        /// </para>
        /// 
        /// <para>
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns>the contents of Cell name</returns>
        public override object GetCellContents(string name)
        {
            //determine the vailidity of the cell name
            if (name == null || !is_valid(name))
                throw new InvalidNameException();

            name = Normalize(name);
            //get and return the contents of the specified cell
            if (spreadsheet == null)
                return "";

            if (spreadsheet.ContainsKey(name))
            {
                return spreadsheet[name].getContents();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        /// <returns>return all the cells that don't have empty contents</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            if (spreadsheet == null)
                return new List<string>();

            return spreadsheet.Keys;
        }

        /// <summary>
        /// <para></para>
        /// If name is null or invalid, throws an InvalidNameException.
        /// </para>
        /// 
        /// <para>
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// </para>
        /// 
        /// <para>
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns>returns the cell values that need to be recalculated as a result of chaning the 
        /// contents of a cell</returns>
        protected override IList<string> SetCellContents(string name, double number)
        {
            
            if (spreadsheet == null)
                spreadsheet = new Dictionary<string, Cell>();

            if (!GetCellContents(name).Equals(number))
                isChanged = true;

            //set the contents of the cell into the spreadsheet
            if (spreadsheet.ContainsKey(name))
            {
                //If the cell was a formula before then clear its dependencies
                if (this.GetCellContents(name) is Formula)
                {
                    foreach (string variable in ((Formula)GetCellContents(name)).GetVariables())
                    {
                        graph.RemoveDependency(variable, name);
                    }
                }
                spreadsheet[name].setContents(number);
                spreadsheet[name].setValue(number);
                evaluateVarialbes(spreadsheet[name]);
            }
            else
            {
                spreadsheet.Add(name, new Cell(name, number));
                spreadsheet[name].setValue(number);
                evaluateVarialbes(spreadsheet[name]);
            }

            //return the cells that need to be recalculated
            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns>returns the cell values that need to be recalculated as a result of chaning the 
        /// contents of a cell</returns>
        protected override IList<string> SetCellContents(string name, string text)
        {
           

            //Put the text into the specified cell
            if (spreadsheet == null)
                spreadsheet = new Dictionary<string, Cell>();

            if (!GetCellContents(name).Equals(text))
                isChanged = true;

            //if (!text.Equals(""))
            //{
                if (spreadsheet.ContainsKey(name))
                {
                    //If the cell was a formula before then clear its dependencies
                    if (this.GetCellContents(name) is Formula)
                    {
                        foreach (string variable in ((Formula)GetCellContents(name)).GetVariables())
                        {
                            graph.RemoveDependency(variable, name);
                        }
                    }
                    spreadsheet[name].setContents(text);
                    spreadsheet[name].setValue(new FormulaError(""));
                }
                else
                {
                    spreadsheet.Add(name, new Cell(name, text));
                    spreadsheet[name].setValue(new FormulaError(""));
                }
            //}

            return new List<string>(GetCellsToRecalculate(name));


        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        /// <returns>returns the cell values that need to be recalculated as a result of chaning the 
        /// contents of a cell</returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {

            if (spreadsheet == null)
                spreadsheet = new Dictionary<string, Cell>();

            //Clear old dependencies if there Cell previously was a formula
            Object oldValues = GetCellContents(name);
            if (oldValues is Formula)
            {

                oldValues = (Formula)GetCellContents(name);
                foreach (string variable in ((Formula)oldValues).GetVariables())
                {
                    graph.RemoveDependency(variable, name);
                }

            }

            // add new dependencies for the new formula
            foreach (string variable in formula.GetVariables())
            {
                graph.AddDependency(variable, name);
            }

            // determine if a circular dependency is created 
            List<string> output = new List<string>();
            try
            {

                output = new List<string>(GetCellsToRecalculate(name));
            }
            catch (CircularException)
            {
                //remove dependencies which resulted in a circular exception
                foreach (string variable in formula.GetVariables())
                {
                    graph.RemoveDependency(variable, name);
                }

                //add old dependencies back if there was a circular exception
                if (oldValues is Formula)
                {
                    foreach (string variable in ((Formula)oldValues).GetVariables())
                    {
                        graph.AddDependency(variable, name);
                    }
                }
                throw new CircularException();
            }

            //add the formula into the specified Cell
            if (spreadsheet.ContainsKey(name))
            {
                spreadsheet[name].setContents(formula);
                evaluateVarialbes(spreadsheet[name]);
            }
            else
            {
                spreadsheet.Add(name, new Cell(name, formula));
                evaluateVarialbes(spreadsheet[name]);
            }

            if (oldValues is Formula)
            {
                if (!((Formula)(oldValues)).Equals(formula))
                    isChanged = true;
            }

            else
                isChanged = true;

            return output;
        }

        /// <summary>
        /// The method determines the value of every variable in the cell
        /// and determines the what value the formula evaluates to and stores
        /// it in the value variable in the Cell class
        /// </summary>
        /// <param name="_cell"></param>
        private void evaluateVarialbes(Cell _cell)
        {

            IEnumerable<string> output = GetCellsToRecalculate(_cell.getName());
            foreach (string variable in output)
            {
                
                if (spreadsheet[variable].getContents() is Formula)
                {
                    object valueOfCell = ((Formula)spreadsheet[variable].getContents()).Evaluate(Lookup);
                    
                    spreadsheet[variable].setValue(valueOfCell);
                }
            }



        }

        /// <summary>
        /// The implementage of the Lookup delagate in the Formula class.
        /// The method determines the value of the variables
        /// </summary>
        /// <param name="variable"></param>
        /// <returns>It returns the double value of the variable</returns>
        private double Lookup(string variable)
        {
            if (spreadsheet[variable].getValue() is FormulaError)
                throw new Exception();
            return (double)spreadsheet[variable].getValue();
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        /// <param name="name"></param>
        /// <returns>return the direct dependents of name</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return graph.GetDependents(name);
        }

        /// <summary>
        /// The method determines if a Cell Name is valid
        /// </summary>
        /// <param name="name"></param>
        /// <returns>return true if cell is valid</returns>
        private bool is_valid(string name)
        {
            //valid character that can be first in the name of a cell
            string letters = "_qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

            if (!letters.Contains(name[0]))
                return false;

            //determine if the characters in name are a letter, underscore or a number
            foreach (char character in name)
            {
                if (!(letters.Contains(character) || Int32.TryParse(character.ToString(), out int numberValue)))
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        ///   <para>Sets the contents of the named cell to the appropriate value. </para>
        ///   <para>
        ///       First, if the content parses as a double, the contents of the named
        ///       cell becomes that double.
        ///   </para>
        ///
        ///   <para>
        ///       Otherwise, if content begins with the character '=', an attempt is made
        ///       to parse the remainder of content into a Formula.  
        ///       There are then three possible outcomes:
        ///   </para>
        ///
        ///   <list type="number">
        ///       <item>
        ///           If the remainder of content cannot be parsed into a Formula, a 
        ///           SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       </item>
        /// 
        ///       <item>
        ///           If changing the contents of the named cell to be f
        ///           would cause a circular dependency, a CircularException is thrown,
        ///           and no change is made to the spreadsheet.
        ///       </item>
        ///
        ///       <item>
        ///           Otherwise, the contents of the named cell becomes f.
        ///       </item>
        ///   </list>
        ///
        ///   <para>
        ///       Finally, if the content is a string that is not a double and does not
        ///       begin with an "=" (equal sign), save the content as a string.
        ///   </para>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException"> 
        ///   If the content parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name parameter is null or invalid, throw an InvalidNameException
        /// </exception>
        ///
        /// <exception cref="SpreadsheetUtilities.FormulaFormatException"> 
        ///   If the content is "=XYZ" where XYZ is an invalid formula, throw a FormulaFormatException.
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name that is being changed</param>
        /// <param name="content"> The new content of the cell</param>
        /// 
        /// <returns>
        ///       <para>
        ///           This method returns a list consisting of the passed in cell name,
        ///           followed by the names of all other cells whose value depends, directly
        ///           or indirectly, on the named cell. The order of the list MUST BE any
        ///           order such that if cells are re-evaluated in that order, their dependencies 
        ///           are satisfied by the time they are evaluated.
        ///       </para>
        ///
        ///       <para>
        ///           For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///           list {A1, B1, C1} is returned.  If the cells are then evaluate din the order:
        ///           A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
        ///       </para>
        /// </returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
                throw new ArgumentNullException();
            if (name == null || !is_valid(name) || !IsValid(name))
                throw new InvalidNameException();

            name = Normalize(name);
            if (Double.TryParse(content, out double number))
                return this.SetCellContents(name, number);

            if (content.Length != 0)
            {
                if (content[0].Equals('='))
                    return this.SetCellContents(name, new Formula(content.Substring(1, content.Length - 1), Normalize, IsValid));
            }


            return SetCellContents(name, content);


        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override string GetSavedVersion(string filename)
        {
            string versionInFile = "";
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name.ToString().Equals("spreadsheet"))
                        {
                            versionInFile = reader.GetAttribute("version");
                        }

                    }
                }
            }
            return versionInFile;
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename"></param>
        public override void Save(string filename)
        {
            //Space each tag with a tab
            XmlWriterSettings Settings = new XmlWriterSettings();
            Settings.Indent = true;
            Settings.IndentChars = ("\t");


            try
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(filename, Settings))
                {
                    xmlWriter.WriteStartDocument();

                    //start spreadsheet element
                    xmlWriter.WriteStartElement("spreadsheet");
                    xmlWriter.WriteAttributeString("version", Version);

                    foreach (string cellName in GetNamesOfAllNonemptyCells())
                    {


                        //start cell Element
                        xmlWriter.WriteStartElement("cell");

                        //start name
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(cellName);
                        //end name
                        xmlWriter.WriteEndElement();

                        //start contents
                        xmlWriter.WriteStartElement("contents");
                        if (spreadsheet[cellName].getContents() is Formula)
                            xmlWriter.WriteString("=" + spreadsheet[cellName].getContents().ToString());
                        else
                            xmlWriter.WriteString(spreadsheet[cellName].getContents().ToString());
                        //end contents
                        xmlWriter.WriteEndElement();

                        //End cell Element
                        xmlWriter.WriteEndElement();


                    }

                    //end spreadsheet Element
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndDocument();
                    xmlWriter.Close();
                    isChanged = false;
                }
            }
            //Go into catch if the File path does not exist
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File path is not valid");
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetCellValue(string name)
        {
            //determine the validity of a cell
            if (name == null || !is_valid(name))
                throw new InvalidNameException();
            name = Normalize(name);

            //get the value of the cell with the name "name"
            if (GetCellContents(name) is Formula)
            {
                this.evaluateVarialbes(spreadsheet[name]);
                return spreadsheet[name].getValue();
            }
            else
                return GetCellContents(name);
        }


    }
}
