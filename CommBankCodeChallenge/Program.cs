using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace CommBankCodeChallenge
{
    class Program
    {
        // for this exercise, the source is static but we could easily implement a file reader or a XML parser that would be able to load different sources to run the program.
        private static List<string> source = new List<string>() { "Tata", "Coucou", "Arbre", "Blablabla", "Toto", "bete", "betetetetetetetete", "tata"
                ,"Casper", "Anaconda" };

        private static List<string> functionNameList = new List<string>() { "CountAverageLengthOfWordsBeginningWith", "CountSpecialLetterInWordsBeginningWith", "LongestWordBeginningWith", "CountSequenceStartingByFirstLetterAndContinuingWith" };

        static void Main(string[] args)
        {
            //Set the source to ToLower to normalise string comparison
            source = source.Select(x => x.ToLower()).ToList();

            //Case no arguments are given, the functions are applied by default
            if (args.Length == 0)
            {
                //Applying the rules
                CountAverageLengthOfWordsBeginningWith(source);
                CountSpecialLetterInWordsBeginningWith(source);
                LongestWordBeginningWith(source, new List<string> { "a", "b", "c" });
                CountSequenceStartingByFirstLetterAndContinuingWith(source);
            }

            //else we load the XML file and apply the rules according to the parameters file
            else if (args[0].EndsWith(".xml"))
            {
                //extracting the parameters from the XML file
                var functionParametersDict = ExtractParameters(args[0]);

                //Applying the funtions
                //1. CountAverageLengthOfWordsBeginningWith
                var paramsDict = functionParametersDict["CountAverageLengthOfWordsBeginningWith"];
                CountAverageLengthOfWordsBeginningWith(source, paramsDict["firstLetter"]);

                //2. CountSpecialLetterInWordsBeginningWith
                paramsDict = functionParametersDict["CountSpecialLetterInWordsBeginningWith"];
                CountSpecialLetterInWordsBeginningWith(source, paramsDict["firstLetter"], paramsDict["letterToCount"]);

                //3. LongestWordBeginningWith
                paramsDict = functionParametersDict["LongestWordBeginningWith"];
                LongestWordBeginningWith(source, paramsDict["firstLetters"].Split(',').ToList());

                //4. CountSequenceStartingByFirstLetterAndContinuingWith
                paramsDict = functionParametersDict["CountSequenceStartingByFirstLetterAndContinuingWith"];
                CountSequenceStartingByFirstLetterAndContinuingWith(source, paramsDict["firstLetter"], paramsDict["secondLetter"]);
            }
        }

        //function that counts the number of occurences of a word beginning with a firstletter (C by default) followed by a word beginning with a second letter (A by default)
        private static void CountSequenceStartingByFirstLetterAndContinuingWith(IEnumerable<string> source, string firstLetter = "c", string secondLetter = "a")
        {
            int index = 0;
            int count = 0;
            //change the IEnumerable to use an index
            var sourceList = source.ToList();

            foreach(var word in source)
            {
                if (word.StartsWith(firstLetter))
                    if (sourceList[index + 1].StartsWith(secondLetter))
                        count++;
                index++;
            }

            WriteInFile(string.Format("Count_of_sequence_of_words_starting_withs_{0}_and_{1}.txt", firstLetter, secondLetter),
                 string.Format("The number of sequence of words starting withs {0} and {1} is {2}.", firstLetter, secondLetter, count));
        }

        private static void LongestWordBeginningWith(IEnumerable<string> source, IEnumerable<string> firstLetters)
        {
            var maxLength = 0;

            //For each of the first letter, get the max length of the word
            foreach(var letter in firstLetters)
            {
                var maxLetter = source.Where(x => x.StartsWith(letter.Trim())).Max(x => x.Length);
                maxLength = maxLength > maxLetter ? maxLength : maxLetter;
            }

            WriteInFile(string.Format("longest_word_starting_with_{0}.txt", string.Join(string.Empty, firstLetters)),
                string.Format("The longest word starting with {0} has {1} characters.", string.Join(string.Empty, firstLetters), maxLength));
        }

        //function that will count the occurences of one letter in the world beginning by a given letter, "b" by default
        private static void CountSpecialLetterInWordsBeginningWith(IEnumerable<string> source, string firstLetter = "b", string letterToCount = "e")
        {
            //get all the words that beging with the first letter and count the number of letters
            // using a count of a group by char for each word and then sum the result
            var countLetter = source.Where(w => w.StartsWith(firstLetter)).Sum(x => x.Count(l => l == letterToCount[0]));

            //Write the result into a file
            WriteInFile(string.Format("count_of_{0}_in_words_starting_with_{1}.txt", letterToCount, firstLetter),
                string.Format("The number of {0} in the words starting with {1} is {2}.", letterToCount, firstLetter, countLetter));
        }

        //Function that will count the average length of all the words beginning by a given letter, "a" by default 
        private static void CountAverageLengthOfWordsBeginningWith(IEnumerable<string> source, string firstLetter = "a")
        {
            //get all the words that begings with a A or a and get the average length
            var wordsAverage = source.Where(w => w.StartsWith(firstLetter))
                                     .Average(w => w.Length);

            //Write the result to a file
            WriteInFile(string.Format("average_length_of_words_starting_with_{0}.txt", firstLetter),
                string.Format("The average length of the words beginning with {0} is {1}.", firstLetter, wordsAverage));
        }

        //Function that opens a file and write the content parameter inside. 
        //By default, this function will write in the application directory unless specified otherwise.
        private static void WriteInFile(string filename, string content)
        {
            using (var file = new StreamWriter(filename))
            {
                file.WriteLine(content);
            }
        }

        //this function extract the parameters from the XML file
        private static Dictionary<string, Dictionary<string, string>> ExtractParameters(string file)
        {
            var functionParameterDict = new Dictionary<string, Dictionary<string, string>>();

            //reading the XML config file
            using (XmlReader xmlReader = XmlReader.Create(file))
            {
                XDocument xDocument = XDocument.Load(xmlReader);

                foreach (var functionName in functionNameList)
                {
                    //extracting a dictionary of parameters with Key = paramName and Value = paramName value for each function
                    var paramsDict = xDocument.Descendants("function").Where(x => x.FirstAttribute.Value == functionName).Elements("param")
                        .ToDictionary(x => x.Attribute("paramName").Value, x => x.Value);

                    functionParameterDict.Add(functionName, paramsDict);
                }
            }

            return functionParameterDict;
        }
    }
}
