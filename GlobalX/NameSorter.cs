using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalX
{
    /// <summary>
    /// Class to Read names from a .txt file and perform sorting and writing operations
    /// </summary>
    public class NameSorter
    {
        //List of names as read from the names file (given names are first)
        private List<string> names;

        /// <summary>
        /// Reads the names from the file given in args. Sorts the names by surname then given names; saves the sorted list to another file; and prints sorted list to console.
        /// </summary>
        /// <remarks>
        /// Usage:  name-sorter [input_file_path]
        /// </remarks>
        /// <param name="args">The filepath to the file containing the names. The file is a .txt file. Each line in the file has the form "given_names surname", and can have up to 3 given names.
        /// </param>
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("ERROR: No input file was specified");
                return;
            }

            //Read the file path from the input argument
            string inputFile = args[0];

            //Check file exists
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("ERROR: " + inputFile + " not found");
                Console.ReadLine();
                return;
            }

            //Create a StreamReader
            TextReader textReader = new StreamReader(inputFile);

            //Instantiate the class
            try
            {
                NameSorter ns = new NameSorter(textReader);

                //Sort the names by surname
                ns.SortBySurname();

                //File to write to
                string outFile = "sorted-names-list.txt";

                //Write the sorted names to file
                ns.WriteToFile(outFile);

                //Get the sorted names
                List<string> sortedNames = ns.GivenNamesFirst();

                //Print sorted names to console
                foreach (string name in sortedNames)
                {
                    Console.WriteLine(name);
                }
                Console.ReadLine();
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
                return;
            }
            finally
            {
                textReader.Close();
            }
        }

        /// <summary>
        /// Class constructor. Reads the names from file and puts them into givennames_surnames
        /// </summary>
        /// <param name="textReader">TextReader pointing to the file containing the unsorted names
        /// </param>
        /// <exception cref="InvalidDataException">If fileName contains invalid formatting</exception>
        public NameSorter(TextReader textReader)
        {
            //Create the names list
            names = new List<string>();

            //Current line being read
            string line;

            //Current line count
            int lineCount = 1;

            //Read line at a time
            while ((line = textReader.ReadLine()) != null)
            {
                //Check the line contains at least two strings (surname and one given name)
                if (!line.Trim().Contains(" "))
                {
                    throw new InvalidDataException("All lines must contain a surname and at least one given name (error on line " + lineCount + ")");
                }

                //Remove leading/trailing whitespace and add to names list
                names.Add(line.Trim());

                lineCount++;
            }
        }

        /// <summary>
        /// Returns the list of names, where each element of the list has form "given_names surname". List has whatever order the current instance has. 
        /// </summary>
        /// <returns>
        /// list of name strings where each element has form "given_names surnames"
        /// </returns>
        public List<string> GivenNamesFirst()
        {
            return names;
        }

        /// <summary>
        /// Sorts the list of names, first by surname, then by any given names the person may have.
        /// </summary>
        public void SortBySurname()
        {
            //Use delegate to sort by surnames and then by given names
            names.Sort(delegate (string p1, string p2)
            {
                //Get surnames for p1 and p2
                string p1surname = p1.Trim().Split(' ').Last();
                string p2surname = p2.Trim().Split(' ').Last();

                //Get given names for p1 and p2
                string p1givenNames = p1.Replace(p1surname, "");
                string p2givenNames = p2.Replace(p2surname, "");

                if (p1surname.Equals(p2surname))
                {
                    //Then can't separate by surnames, so sort by given names
                    return p1givenNames.CompareTo(p2givenNames);
                }
                //Sort by surnames
                return p1surname.CompareTo(p2surname);
            });
        }

        /// <summary>
        /// Sorts the list of names, first by given names, then by surname.
        /// </summary>
        public void SortByGivenNames()
        {
            //Use delegate to sort by given names and then by surname
            names.Sort(delegate (string p1, string p2)
            {
                //Get surnames for p1 and p2
                string p1surname = p1.Trim().Split(' ').Last();
                string p2surname = p2.Trim().Split(' ').Last();

                //Get given names for p1 and p2
                string p1givenNames = p1.Replace(p1surname, "");
                string p2givenNames = p2.Replace(p2surname, "");

                if (p1givenNames.Equals(p2givenNames))
                {
                    //Then can't separate by given names, so sort by surnames
                    return p1surname.CompareTo(p2surname);
                }
                //Sort by given names
                return p1givenNames.CompareTo(p2givenNames);
            });
        }

        /// <summary>
        /// Writes the names list to file (in whatever the current order of the instance is).
        /// </summary>
        /// <param name="filepath">
        ///  string giving the filepath of the .txt file to write to.
        /// </param>
        public void WriteToFile(string filepath)
        {
            File.WriteAllLines(filepath, names);
        }
    }
}
