// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

// <summary>
/// author of Implentation of methods: Deepak Ganesh
/// Date: 1/19/2019
/// I pledge that I wrote this code.
/// </summary>
/// <summary> 
/// Author:    [Your Name] 
/// Partner:   [Partner Name or None] 
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

  /// <summary>
  /// (s1,t1) is an ordered pair of strings
  /// t1 depends on s1; s1 must be evaluated before t1
  /// 
  /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
  /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
  /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
  /// set, and the element is already in the set, the set remains unchanged.
  /// 
  /// Given a DependencyGraph DG:
  /// 
  ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
  ///        (The set of things that depend on s)    
  ///        
  ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
  ///        (The set of things that s depends on) 
  //
  // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
  //     dependents("a") = {"b", "c"}
  //     dependents("b") = {"d"}
  //     dependents("c") = {}
  //     dependents("d") = {"d"}
  //     dependees("a") = {}
  //     dependees("b") = {"a"}
  //     dependees("c") = {"a"}
  //     dependees("d") = {"b", "d"}
  /// </summary>
  public class DependencyGraph
  {

        /// <summary>
        /// Private Variables
        /// </summary>
        private Dictionary<string, HashSet<string>> dependentsOf;
        private Dictionary<string, HashSet<string>> dependeesOf;
        private int numOrderedPairs;



        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            //dependents is asking: "who depends on me?"
            dependentsOf = new Dictionary<string, HashSet<string>>();
            //dependees is asking: "who do I depend upon?"
            dependeesOf = new Dictionary<string, HashSet<string>>();
            numOrderedPairs = 0;
        }
    
    


    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
      get { return numOrderedPairs; }
    }


    /// <summary>
    /// The size of dependees(s).
    /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
    /// invoke it like this:
    /// dg["a"]
    /// It should return the size of dependees("a")
    /// </summary>
    public int this[string s]
    {
      get {
                if (dependeesOf.ContainsKey(s))
                    return dependeesOf[s].Count;
                else
                    return 0;
            }
    }


    /// <summary>
    /// Reports whether dependents(s) is non-empty.
    /// </summary>
    public bool HasDependents(string s)
    {
            if (dependentsOf.ContainsKey(s))
                return dependentsOf[s].Count != 0;
            else
                return false;
    }


    /// <summary>
    /// Reports whether dependees(s) is non-empty.
    /// </summary>
    public bool HasDependees(string s)
    {
            if (dependeesOf.ContainsKey(s))
                return dependeesOf[s].Count != 0;
            else
                return false;
    }


    /// <summary>
    /// Enumerates dependents(s).
    /// </summary>
    public IEnumerable<string> GetDependents(string s)
    {
            if (dependentsOf.ContainsKey(s))
                return dependentsOf[s];
            else 
                return new HashSet<string>();
    }

    /// <summary>
    /// Enumerates dependees(s).
    /// </summary>
    public IEnumerable<string> GetDependees(string s)
    {
            if (dependeesOf.ContainsKey(s))
                return dependeesOf[s];
            else
                return new HashSet<string>();
    }


    /// <summary>
    /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
    /// 
    /// <para>This should be thought of as:</para>   
    /// 
    ///   t depends on s
    ///
    /// </summary>
    /// <param name="s"> s must be evaluated first. T depends on S</param>
    /// <param name="t"> t cannot be evaluated until s is</param>        /// 
    public void AddDependency(string s, string t)
    {
            bool sizeChanged = false;

            //Add s and its dependent t to the dependentsOf dictionary
            if (dependentsOf.ContainsKey(s))
            {
                int sizeOfDependents = dependentsOf[s].Count;
                dependentsOf[s].Add(t);
                if (sizeOfDependents != dependentsOf[s].Count)
                    sizeChanged = true;
            }
            else
            {
                sizeChanged = true;
                dependentsOf.Add(s, new HashSet<string>());
                dependentsOf[s].Add(t);
            }

            //Add t and its dependee s to the dependeesOf dictionary
            if (dependeesOf.ContainsKey(t))
            {
                int sizeDependeeOf = dependeesOf[t].Count;
                dependeesOf[t].Add(s);
                if (sizeDependeeOf != dependeesOf[t].Count)
                    sizeChanged = true;
            }
            else
            {
                sizeChanged = true;
                dependeesOf.Add(t, new HashSet<string>());
                dependeesOf[t].Add(s);
            }

            if (sizeChanged)
                numOrderedPairs++;
        }


    /// <summary>
    /// Removes the ordered pair (s,t), if it exists
    /// </summary>
    /// <param name="s"></param>
    /// <param name="t"></param>
    public void RemoveDependency(string s, string t)
    {
            bool sizeChanged = false;

            if (dependentsOf.ContainsKey(s))
            {
                int sizeOfDependents = dependentsOf[s].Count;
                dependentsOf[s].Remove(t);
                if (sizeOfDependents != dependentsOf[s].Count)
                    sizeChanged = true;
            }
            

            if (dependeesOf.ContainsKey(t))
            {
                int sizeDependeeOf = dependeesOf[t].Count;
                dependeesOf[t].Remove(s);
                if (sizeDependeeOf != dependeesOf[t].Count)
                    sizeChanged = true;
            }
            

            if (sizeChanged)
                numOrderedPairs--;
        }


    /// <summary>
    /// Removes all existing ordered pairs of the form (s,r).  Then, for each
    /// t in newDependents, adds the ordered pair (s,t).
    /// </summary>
    public void ReplaceDependents(string s, IEnumerable<string> newDependents)
    {
            //remove all the pairs from the that have the same dependents as in newDepnedents
            if (dependentsOf.ContainsKey(s))
            {
                HashSet<string> values = new HashSet<string>(dependentsOf[s]);
                foreach(string dependents in values)
                {
                    this.RemoveDependency(s,dependents);
                }
                
            }
            else
                dependentsOf.Add(s,new HashSet<string>());
            
            //add the pairs with s and the newDependents 
            foreach(string dependents in newDependents)
            {
                this.AddDependency(s, dependents);
            }

        }


    /// <summary>
    /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
    /// t in newDependees, adds the ordered pair (t,s).
    /// </summary>
    public void ReplaceDependees(string s, IEnumerable<string> newDependees)
    {
            //remove all the pairs from the that have the same dependees as in newDepnedees
            if (dependeesOf.ContainsKey(s))
            {
                HashSet<string> values = new HashSet<string>(dependeesOf[s]);
                foreach (string dependees in values)
                {
                    this.RemoveDependency(dependees, s);
                }
                
            }
            else
                dependeesOf.Add(s, new HashSet<string>());

            //add the pairs with s and the newDependees 
            foreach (string dependees in newDependees)
            {
                this.AddDependency(dependees, s);
            }
            
        }

  }

}
