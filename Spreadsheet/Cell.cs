using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text;
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
///    The class is a representation of the each cell in the spreadsheet.
/// </summary>
namespace SS
{
    /// <summary>
    /// Thhe class stores the data of each individual Cell
    /// </summary>
    public class Cell
    {
        private string _name;
        private object _contents;
        private object _value;
        public Cell(string name, object _contents)
        {
            this._contents =_contents;
            this._name = name;
            _value = new FormulaError("can't add numbers with words");
        }

        public string getName()
        {
            return _name;
        }
        

        public object getContents()
        {
            return _contents;
        }

        public void setContents(object contents)
        {
            _contents = contents;
        }
        

        public object getValue()
        {
            return _value;
        }

        public void setValue(object values)
        {
            _value = values;
        }
    }
}
