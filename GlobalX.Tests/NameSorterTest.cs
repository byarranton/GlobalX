using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Moq;
using System.Collections.Generic;

namespace GlobalX.Tests
{
    /// <summary>
    /// Class to test the NameSorter class
    /// </summary>
    [TestClass]
    public class NameSorterTest
    {
        [TestMethod]
        public void TestMain()
        {
            //Main method is not tested
            Assert.AreEqual(1, 1);
        }

        /// <summary>
        /// Test the class constructor with a file that contains a line which doesn't have a surname (only has one name).
        /// Assert the constructor raises an InvalidDataException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Instantiate_with_file_containing_one_line_with_no_surname()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("Jason Jay Derulo")
                      .Returns("Beyonce")
                      .Returns("Jay Z");

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
        }

        /// <summary>
        /// Test the class constructor with a file that contains a blank line.
        /// Assert the constructor raises an InvalidDataException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Instantiate_with_file_containing_blank_line()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("Jason Jay Derulo")
                      .Returns("")
                      .Returns("Jay Z");

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
        }

        /// <summary>
        /// Test the class constructor with an empty input file.
        /// Assert the instance is created successfully.
        /// </summary>
        [TestMethod]
        public void Instantiate_with_emtpy_file()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns(null);

            //Act
            NameSorter ns = new NameSorter(textReader.Object);

            //Assert an instance is created
            Assert.IsInstanceOfType(ns, typeof(NameSorter));
            Assert.IsNotNull(ns);
        }

        /// <summary>
        /// Test the class constructor with an input file containing one line.
        /// Assert the instance is created successfully.
        /// </summary>
        [TestMethod]
        public void Instantiate_with_one_line_input()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("Valid Girl");

            //Act
            NameSorter ns = new NameSorter(textReader.Object);

            //Assert an instance is created
            Assert.IsInstanceOfType(ns, typeof(NameSorter));
            Assert.IsNotNull(ns);
        }

        /// <summary>
        /// Test the GivenNamesFirst method with an empty names list.
        /// Assert the returned list is an empty array.
        /// </summary>
        [TestMethod]
        public void Test_GivenNamesFirst_with_empty_list()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns(null);

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            List<string> actualResult = ns.GivenNamesFirst();

            //Assert
            Assert.AreEqual(0, actualResult.Count);
        }

        /// <summary>
        /// Test the GivenNamesFirst method with a single-name list.
        /// Assert the returned list contains the name.
        /// </summary>
        [TestMethod]
        public void Test_GivenNamesFirst_with_single_entry()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("The Only Person");
            List<string> expectedResult = new List<string> { "The Only Person" };

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            List<string> actualResult = ns.GivenNamesFirst();

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Test the GivenNamesFirst method with a two-name list.
        /// Assert the returned list contains the two names.
        /// </summary>
        [TestMethod]
        public void Test_GivenNamesFirst_with_two_entries()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("Jackson The Pollock")
                      .Returns("Leonardo DaVinci");
            List<string> expectedResult = new List<string> {
                "Jackson The Pollock", "Leonardo DaVinci" };

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            List<string> actualResult = ns.GivenNamesFirst();

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Test the SortBySurname method with an empty names list.
        /// Assert no error.
        /// </summary>
        [TestMethod]
        public void Test_SortBySurname_with_empty_list()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns(null);

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            ns.SortBySurname();
        }

        /// <summary>
        /// Test the SortBySurname method with a single-name list.
        /// Assert no error.
        /// </summary>
        [TestMethod]
        public void Test_SortBySurname_with_single_entry()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("The Only Person");
            List<string> expectedResult = new List<string> { "The Only Person" };

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            ns.SortBySurname();
            List<string> actualResult = ns.GivenNamesFirst();

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Test the SortBySurname method with a multi-name list.
        /// Assert the returned list is correctly sorted.
        /// </summary>
        [TestMethod]
        public void Test_SortBySurname_with_multi_entries()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("Alpha Jerry Beta")
                      .Returns("Alpha Yiotta")
                      .Returns("Zeta Aak")
                      .Returns("Barry Beta")
                      .Returns("Alpha Beta");
            List<string> expectedResult = new List<string> {
                "Zeta Aak", "Alpha Beta", "Alpha Jerry Beta", "Barry Beta", "Alpha Yiotta" };

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            ns.SortBySurname();
            List<string> actualResult = ns.GivenNamesFirst();

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Test the SortByGivenNames method with an empty names list.
        /// Assert no error.
        /// </summary>
        [TestMethod]
        public void Test_SortByGivenNames_with_empty_list()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns(null);

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            ns.SortByGivenNames();
        }

        /// <summary>
        /// Test the SortByGivenNames method with a single-name list.
        /// Assert no error.
        /// </summary>
        [TestMethod]
        public void Test_SortByGivenNames_with_single_entry()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("The Only Person");
            List<string> expectedResult = new List<string> { "The Only Person" };

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            ns.SortByGivenNames();
            List<string> actualResult = ns.GivenNamesFirst();

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Test the SortByGivenNames method with a multi-name list.
        /// Assert the returned list is correctly sorted.
        /// </summary>
        [TestMethod]
        public void Test_SortByGivenNames_with_multi_entries()
        {
            //Arrange (Mock the TextReader)
            var textReader = new Mock<TextReader>();
            textReader.SetupSequence(r => r.ReadLine())
                      .Returns("Alpha Jerry Beta")
                      .Returns("Alpha Yiotta")
                      .Returns("Zeta Aak")
                      .Returns("Barry Beta")
                      .Returns("Alpha Beta");
            List<string> expectedResult = new List<string> {
                "Alpha Beta", "Alpha Yiotta", "Alpha Jerry Beta", "Barry Beta", "Zeta Aak" };

            //Act
            NameSorter ns = new NameSorter(textReader.Object);
            ns.SortByGivenNames();
            List<string> actualResult = ns.GivenNamesFirst();

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }
    }
}
