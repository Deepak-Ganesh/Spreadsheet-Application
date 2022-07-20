using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using FormulaEvaluator;


namespace DevelopmentTests
{
    
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        /// <summary>
        ///  Deepak Ganesh unitTests
        /// </summary>

        /*****************************************************************************************************************/

        /// <summary>
        /// Tests bacic functionality of addDependency()
        /// </summary>
        [TestMethod()]
        public void testadd()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            //Assert.AreEqual(new HashSet<string>() { "b" },graph.GetDependents("a"));
            CollectionAssert.AreEqual(new List<string>() { "b" },new List<string>(graph.GetDependents("a")));
            CollectionAssert.AreEqual(new List<string>() { "a" }, new List<string>(graph.GetDependees("b")));
        }

        

        /// <summary>
        /// Test basic fuctionality of HasDependents
        /// </summary>
        [TestMethod()]
        public void testHasDependents()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            Assert.IsTrue(graph.HasDependents("a"));
            Assert.IsFalse(graph.HasDependents("b"));
        }

        /// <summary>
        /// Tests basic functionality of HasDependees
        /// </summary>
        [TestMethod()]
        public void testHasDependees()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            Assert.IsFalse(graph.HasDependees("a"));
            Assert.IsTrue(graph.HasDependees("b"));
        }

        /// <summary>
        /// Tests if GetDependees works on an empty graph
        /// </summary>
        [TestMethod()]
        public void testGetEmptyDependentsAndEmptyDependees()
        {
            DependencyGraph graph = new DependencyGraph();
            CollectionAssert.AreEqual(new List<string>(), new List<string>(graph.GetDependees("k")));
            
        }

        /// <summary>
        /// Tests if GetDependents works on an empty graph
        /// </summary>
        [TestMethod()]
        public void testGetEmptyDependentsAndEmptyDependents()
        {
            DependencyGraph graph = new DependencyGraph();
            CollectionAssert.AreEqual(new List<string>(), new List<string>(graph.GetDependents("k")));
        }

        /// <summary>
        /// Tests baic futionality of the overloaded operator []
        /// </summary>
        [TestMethod()]
        public void testOverloadOperation()
        {
            DependencyGraph graph = new DependencyGraph();

            Assert.AreEqual(0, graph["f"]);
            graph.AddDependency("f", "g");
            Assert.AreEqual(1, graph["g"]);
        }

        /// <summary>
        /// tests basic functionality of replace dependents.
        /// </summary>
        [TestMethod()]
        public void test_ReplaceDependents()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("b", "a");
            graph.ReplaceDependents("b", new HashSet<string>() { "a", "c" });
            CollectionAssert.AreEqual(new List<string>() { "a", "c" }, new List<string>(graph.GetDependents("b")));
            graph.ReplaceDependents("d", new HashSet<string>());
            CollectionAssert.AreEqual(new List<string>() { }, new List<string>(graph.GetDependents("d")));
        }

        /// <summary>
        /// The method test when a dependent is added with no dependee
        /// </summary>
        [TestMethod()]
        public void test_ReplaceDependentsWithNoKey()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.ReplaceDependents("d", new HashSet<string>());
            CollectionAssert.AreEqual(new List<string>() { }, new List<string>(graph.GetDependents("d")));
            Assert.IsTrue(1 == graph.Size);
        }

        /// <summary>
        /// Test basic replaceDependees()
        /// </summary>
        [TestMethod()]
        public void test_ReplaceDependees()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("a", "bx");
            graph.AddDependency("b", "a");
            graph.ReplaceDependees("a", new HashSet<string>() { "a", "c" });
            CollectionAssert.AreEqual(new List<string>() { "a", "c" }, new List<string>(graph.GetDependees("a")));

        }

        /// <summary>
        /// The method tests when a dependee is added without its dependents.
        /// </summary>
        [TestMethod()]
        public void test_ReplaceDependeesWithNoKey()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.ReplaceDependees("d", new HashSet<string>());
            CollectionAssert.AreEqual(new List<string>() { }, new List<string>(graph.GetDependees("d")));
            Assert.IsTrue(1 == graph.Size);
        }


        /// <summary>
        /// Tests if duplicate pairs can be added.
        /// </summary>
        [TestMethod()]
        public void testAddDuplicates()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("a", "b");
            graph.AddDependency("b", "a");
            Assert.AreEqual(2, graph.Size);
            graph.AddDependency("c", "c");
            Assert.AreEqual(3, graph.Size);
        }

        /// <summary>
        /// Tests Basic functionalty of RemoveDependecy
        /// </summary>
        [TestMethod()]
        public void testRemove()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("b", "a");
            Assert.AreEqual(2, graph.Size);
            graph.RemoveDependency("b", "a");
            Assert.AreEqual(1, graph.Size);
            graph.RemoveDependency("a", "b");
            Assert.AreEqual(0, graph.Size);
        }

        /// <summary>
        /// Tests if removeing a Dependency doesn't change the graph
        /// </summary>
        [TestMethod()]
        public void testRemoveFromEmptyGraph()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.RemoveDependency("f", "f");
            Assert.IsTrue(0 == graph.Size);
        }

        /// <summary>
        /// Tests if graph doesn't change when a pair not in the graph is removed.
        /// </summary>
        [TestMethod()]
        public void testRemoveNonExistantPair()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "z");
            graph.RemoveDependency("f", "f");
            Assert.IsTrue(1 == graph.Size);
        }

        

        /**************************************************************************************************************************/
        /// <summary>
        /// Professor's unitTests
        /// </summary>

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }


        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }



        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }



        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

    }
}
