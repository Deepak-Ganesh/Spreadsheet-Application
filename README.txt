Author:     [Deepak Ganesh]
Partner:    [Keaton Davis]
Date:       [2/21/2019]
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment 6
Copyright:  CS 3500 and [Deepak Ganesh, Keaton Davis] - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:
    If additional information is needed on the extra feature we had to adjust code in the DrawingPanel.cs, SpreadsheetGridWidget.cs, and the 
    SpreadsheetGridWidget.Designer.cs files in order to get the functionality to work properly.


Hours Estimated/Worked               Assignment                                                   Note
          6     /   6.5           - Assignment 1     - Formula Evaluator     
          7     /   8             - Assignment 2     - Dependency Graph     took extra time to figure out how the assignemnt should be submitted 
          7     /   10            - Assignment 3     - Refactoring the FormulaEvaluator Spent an unexpected amount of time in unit testing 
          7     /   8             - Assignment Four  - Onward to a Spreadsheet
          7.5   /   9             - Assignment 5     - spent an unexpected amount of time on the xml constructor
          8     /   12.5          - Assignment 6     - Spent too much time on extra feature, and did not account for background worker in time estimate

Time Estimate Evaluation:
    Deepak Ganesh:
        Usually my time estimates are very close to how long it actually took me to do the assignment. However this time it took
        much longer due to extra requirements added at the last minute and the extra feture we chose to do was much harder than we
        expeceted it to be.
    Keaton:
        My time estimates are improving and I feel that I'm more properly allocating for time spent learning different techniques that aren't
        directly explained in class or lab. Refer to Deepak's explanation as to why our estimate was off on this particular assignment.

2. Assignment Specific Topic
   Time grouping: 1.5 hours to get spreadsheet GUI to show up on the screen, 4 hours to move code to core and get basic functionality working,
   5 hours spent on the extra feature, 1 hour for testing, and 1 hour for documenting.
    

3. Consulted Peers:
None

4. References:

    1. Array.Exists<T>(T[], Predicate<T>) Method -	https://docs.microsoft.com/en-us/dotnet/api/system.array.exists?view=netframework-4.8
    2. Writing XML with the XmlWriter class -    https://csharp.net-tutorials.com/xml/writing-xml-with-the-xmlwriter-class/
    3. Read XML File Using XMLReader in C#-     https://www.c-sharpcorner.com/UploadFile/167ad2/read-xml-file-using-xml-reader-in-C-Sharp/
    4.Microsoft Doc -   https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog?view=netframework-4.8
    5. How do I create a message box with “Yes”, “No” choices and a DialogResult? - https://stackoverflow.com/questions/3036829/how-do-i-create-a-message-box-with-yes-no-choices-and-a-dialogresult
    6. StackOverflow overriding close button - https://stackoverflow.com/questions/1669318/override-standard-close-x-button-in-a-windows-form

5. Partnership Information:
    Assignment was completed via the pair programming method. Deepak did some work on the basic functionality working properly on his own, and added
    a check box. Otherwise we worked together on the rest of the assigment switching between driver and navigator.

6. Branching:
    We never worked on separate branches, but branched to test features added. No issues with the merge process to report.

7. Extra Feature:
    For the extra feature we added the functionality to type contents "directly" into the cells. This is accomplished by putting a textbox on top
    of the cell selected by the user. The files that were altered for the extra feature are: DrawingPanel.cs, SpreadsheetGridWidget.cs, 
    SpreadsheetGridWidget.Designer.cs, and the SimpleSpreadsheetGUI.cs file.
    To utilize this feature: First, check the extra feature checkbox in the top left of the GUI window, this will populate a textbox on the current
    cell selection. Next, click on the text box to type contents. Finally, press enter to submit the contents into the cell.

8. Best Team Practices:
    We were good at finding time to work together in person so as to ease communications and keep each other on the same page. This also helped prevent
    merge conflicts with gitHub, which helped to reduce time spent on the assignment. We communicated ideas effectively so that coding was terribly
    difficult.

    We can work on spending time more equally between navigator and driver. Though we were both participating we didn't switch roles very often, thus
    not affording the other learning experiences that come with each position. We will work to improve on this in the next assignment.

9. Testing View & Controller of Spreadsheet:
    As we programed we regularly checked the GUI was behaving properly by running the code and testing the functionality previously added. After
    several features had been added we would both pull up the GUI application on our computers and try to break it, keeping track of any errors we
    discovered. We followed this process all the way through to ensure that our GUI worked as intended. For example, a basic test we'd run is entering
    a circular dependcy into a cell and ensure that the proper error message was displayed. We also used formulas with mulitple cells dependent upon
    each other then gave a real value intended to change all FormulaErrors into doubles, and made sure GUI updated properly.
/**************************************************Assignment 5***************************************************************/
Author:     [Deepak Ganesh]
Partner:    [None]
Date:       [2/9/2019]
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment 5
Copyright:  CS 3500 and [Deepak Ganesh] - This work may not be copied for use in Academic Coursework.

2. Assignment Specific Topics
    The 5 assignment brings together all the class that have been implemented so far and is essentially the administrator class for
    all the backend code of the Spreadsheet. Assignment 5 is the Model part of the MVC arcitechture.

/**************************************************Assignment 4***************************************************************/
Date:       [2/7/2019]
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment Four - Onward to a Spreadsheet
Copyright:  CS 3500 and [Deepak Ganesh] - This work may not be copied for use in Academic Coursework.

2. Assignment Specific Topics
    In the fourth assignment I finished the backend of the spreadsheet. The Spreadsheet can add cells with contents of either formulas, numbers or strings.
    I chose to store the entire list of Cells that are non empty in a Dictionary since finding is constant time. I chose to create a Cell class to store the contents
    of an individual Cells to make the code more organized. I prioritized speed over memory in my code.


/************************************************************Assignment 3******************************************************/

Date:       [1/31/2019]
Course:     CS 3500, University of Utah, School of Computing
Assignment: ssignment 3 - Refactoring the FormulaEvaluator
Copyright:  CS 3500 and [Deepak Ganesh] - This work may not be copied for use in Academic Coursework.

2. Assignment Specific Topics
    In the third assignment I created a better version of the Formula Evaluator where the formula would be thrown out if it's form was incorrect.
    Like in the Formula Evaluator the formula class can evaluate expressions. The class also can determined the equality of formulas and get a list
    of the variables within the expressions.


/**********************************************************Assignment #2******************************************************/

Date:       [1/17/2019]
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #2 - Dependency Graph
Copyright:  CS 3500 and [Deepak Ganesh] - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:
    My unit tests are at the top of the Dependency graph labeled as Deepak Ganesh unitTests.
    The Professor written testes are below mine labeled as Professor's unitTests.

2. Assignment Specific Topics
    In the second assignment I implemented an Dependency Graph. The Dependency Graph's job is to build a graph that stores the relationship 
    of cells that depend on other cells for their values. The graph takes in values in pairs of strings eg.("A1","A2"). The second paramater is
    the dependent of the first parameter and the first paramater is the dependee of the second parameter.


/*************************************** Assignment #1***********************************************************************/


Date:       [1/10/2019]
Assignment: Assignment #1 - Formula Evaluator
Copyright:  CS 3500 and [Deepak Ganesh] - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:
Estimated Completion Time:
implementation time: 3hours
Testing time: 3 hours

date:1/16/2019
Actual Completion Time:
implementation time: 3hours
Testing time: 3.5 hours

2. Assignment Specific Topics
The first assignment FormulaEvaluator evaluates the integer values of expressions. For example, it would evaluate the expression "5*3-6" as 9 and would throw an
argumentException if expression was formated incorrectly like having an extra plus like in "5++5".

